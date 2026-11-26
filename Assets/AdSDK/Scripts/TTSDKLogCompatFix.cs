using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// TTSDK（抖音小游戏 SDK 6.9.7）在 WebGL 真机上的兼容补丁。
///
/// 现象：控制台被同一条异常刷屏（计数从十几次涨到上千次）——
///   ArgumentNullException: Value cannot be null. Parameter name: obj
///   at System.Threading.Monitor.ReliableEnterTimeout (System.Object obj, ...)
/// 这就是 C# 里 lock(空对象) 抛的异常（lock 在 IL 里就是 Monitor.Enter(obj, ref lockTaken)，
/// 该方法第一句就校验 obj 是否为 null）。
///
/// 反编译 ttsdk.dll 后确认，SDK 里会在“每条日志 / 每次 JS→C# 桥接回调”上执行的加锁点只有几个：
///   * TTSDK.TTSDKLog.OnEnable() -> TTPrintFile.BeginRecord()，把 RecordLog 挂到
///     Application.logMessageReceivedThreaded（每条 Unity 日志回调一次）；
///     RecordLog 第一件事就是 lock (TTPrintFile.m_Locker)。
///   * BeginRecord 只有在 TTSDKLog.m_PrintTarget 带 FILE(2) 标志时才会真的挂回调，
///     并尝试在 temporaryCachePath 建日志文件——WebGL 没有可写文件系统，这条路径本来就无意义。
///   * TTSDK.TTHostEventImpl.AddHostEventListener / RemoveHostEventListener 里 lock (this._mutex)，
///     这是每次插屏/激励视频 JS 回调都会走到的路径。
///   * 其余静态锁：JavaScriptHandle.ManagedRefCountsLock、TTAudioManager._audioIdLock 等。
/// 只要其中任意一个是 null，就会出现上面那条异常。
///
/// 注意：TTPrintFile 是【全局命名空间】的类型（不是 TTSDK.TTSDKLog 的嵌套类），
/// 所以这里按“遍历程序集 + 按简单类名匹配”的方式找类型，不写死命名空间。
///
/// 补丁行为（全部包在 try/catch 里，失败也不影响游戏）：
///   1. 强制跑一遍相关类型的静态构造函数；
///   2. 把 SDK 里仍为 null 的静态锁对象补成 new object()；
///   3. 清掉 TTSDKLog.m_PrintTarget 的 FILE 标志，并调用 TTPrintFile.EndRecord() 注销日志落盘回调
///      （控制台日志不受影响，引擎照常输出）；
///   4. 修正 TTHostEventImpl 单例的 _mutex（桥接回调路径）。
/// 启动后 12 秒内会重复执行几次，覆盖 SDK 延迟创建的对象。
///
/// 【重要】本类刻意不写任何静态字段初始化器（没有 .cctor）：混淆器 OPS 的
/// AntiTampering / AddRandomCode 会往静态构造器里注入“读自身程序集文件做哈希”的代码，
/// WebGL 下路径是假的 /Assembly-CSharp.dll，注入代码一抛异常，整个类型就永久
/// TypeInitializationException —— 补丁从此完全空转，lock(null) 照样刷屏。
/// 所有状态改为方法内惰性创建，类型本身没有 cctor 可供注入。
/// </summary>
public static class TTSDKLogCompatFix
{
    private const string SdkAssemblyName = "ttsdk";

    /// <summary>FILE 标志位（TTSDK.PrintTarget：None=0, UI=1, FILE=2, WWW=4）</summary>
    private const int PrintTargetFileFlag = 2;

    private static bool s_firstRunDone;
    private static bool s_analyzed;

    private static Type s_printFileType;
    private static Type s_hostEventType;
    private static FieldInfo s_hostEventInstanceField;
    private static MethodInfo s_hostEventInstanceGetter;
    private static FieldInfo s_hostEventMutexField;

    // 不用 static readonly 初始化器，改在 EnsureLists() 里按需创建（见类注释）
    private static List<FieldInfo> s_printTargetFields;
    private static List<FieldInfo> s_lockFields;

    private static void EnsureLists()
    {
        if (s_printTargetFields == null)
        {
            s_printTargetFields = new List<FieldInfo>();
        }

        if (s_lockFields == null)
        {
            s_lockFields = new List<FieldInfo>();
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Install()
    {
        if (!IsRuntimeTarget())
        {
            // 编辑器 / 其他平台下 TTSDK 是 mock，不需要也不应该动它
            return;
        }

        Run();
    }

    /// <summary>是否是需要打补丁的运行环境（非编辑器的 WebGL / Android 小游戏包）</summary>
    private static bool IsRuntimeTarget()
    {
#if (UNITY_WEBGL || UNITY_ANDROID) && !UNITY_EDITOR
        return true;
#else
        return false;
#endif
    }

    /// <summary>补丁入口，可重复调用（幂等）</summary>
    public static void Run()
    {
        try
        {
            EnsureLists();

            if (!s_analyzed)
            {
                Analyze();
            }

            // 1. 关掉日志落盘（RecordLog 里 lock(m_Locker) 是每条日志都会跑的路径）
            for (int i = 0; i < s_printTargetFields.Count; i++)
            {
                ClearFileFlag(s_printTargetFields[i]);
            }

            if (s_printFileType != null)
            {
                EndRecord(s_printFileType);
            }

            // 2. 补齐为 null 的静态锁对象
            int repaired = 0;
            System.Text.StringBuilder repairedNames = null;
            for (int i = 0; i < s_lockFields.Count; i++)
            {
                FieldInfo field = s_lockFields[i];
                try
                {
                    if (field.GetValue(null) == null)
                    {
                        field.SetValue(null, new object());
                        repaired++;

                        if (repairedNames == null)
                        {
                            repairedNames = new System.Text.StringBuilder();
                        }
                        else
                        {
                            repairedNames.Append(", ");
                        }

                        repairedNames.Append(field.DeclaringType != null ? field.DeclaringType.Name : "?")
                                     .Append('.').Append(field.Name);
                    }
                }
                catch (Exception)
                {
                }
            }

            // 3. 桥接回调单例的实例锁
            RepairHostEventMutex();

            if (!s_firstRunDone)
            {
                s_firstRunDone = true;

                // 这一行是排查用的“补丁确实跑起来了”的证据：
                // 若控制台看不到它，说明补丁类型根本没被加载（被混淆 / 类型初始化失败），
                // 这时看到的 lock(null) 与补丁逻辑无关，先查 OPS 的 AntiTampering 开关。
                Debug.Log($"[TTSDK] 静态锁补丁已生效：扫描锁字段 {s_lockFields.Count} 个，本次补全 {repaired} 个"
                          + (repaired > 0 ? "（" + repairedNames + "）" : string.Empty));

                // SDK 组件可能在 InitSDK 之后才创建，挂一个短命驱动器重复兜底
                CreateDriver();
            }
        }
        catch (Exception)
        {
            // 补丁本身绝不能影响游戏启动 / 广告流程
        }
    }

    #region 分析一次，之后只做便宜的重复检查

    private static void Analyze()
    {
        s_analyzed = true;

        try
        {
            Assembly sdk = FindSdkAssembly();
            if (sdk == null)
            {
                return;
            }

            Type[] types = SafeGetTypes(sdk);
            if (types == null || types.Length == 0)
            {
                return;
            }

            s_printFileType = FindBySimpleName(types, "TTPrintFile");
            if (s_printFileType != null)
            {
                RunCctor(s_printFileType);
            }

            for (int i = 0; i < types.Length; i++)
            {
                Type type = types[i];
                if (type == null)
                {
                    continue;
                }

                FieldInfo[] fields;
                try
                {
                    fields = type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                }
                catch (Exception)
                {
                    continue;
                }

                if (fields == null || fields.Length == 0)
                {
                    continue;
                }

                bool needCctor = false;

                for (int j = 0; j < fields.Length; j++)
                {
                    FieldInfo field = fields[j];

                    // PrintTarget：BeginRecord 读它决定是否挂日志回调
                    // （不同小版本可能写成 m_PrintTarget / m_printTarget，统一按名字片段匹配）
                    if (field.Name.IndexOf("printtarget", StringComparison.OrdinalIgnoreCase) >= 0
                        && !s_printTargetFields.Contains(field))
                    {
                        s_printTargetFields.Add(field);
                    }

                    if (field.FieldType == typeof(object) && IsLockFieldName(field.Name))
                    {
                        s_lockFields.Add(field);
                        needCctor = true;
                    }
                }

                if (needCctor)
                {
                    // 锁对象就是静态构造函数里 new 的，先把 .cctor 跑一遍
                    RunCctor(type);
                }
            }

            s_hostEventType = FindBySimpleName(types, "TTHostEventImpl");
            if (s_hostEventType != null)
            {
                RunCctor(s_hostEventType);
                s_hostEventInstanceField = s_hostEventType.GetField("_instance",
                    BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                s_hostEventInstanceGetter = s_hostEventType.GetProperty("Instance",
                    BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)?.GetGetMethod(true);
                s_hostEventMutexField = s_hostEventType.GetField("_mutex",
                    BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            }
        }
        catch (Exception)
        {
        }
    }

    #endregion

    #region 日志落盘

    /// <summary>把 TTSDKLog.m_PrintTarget 里的 FILE 位清掉，BeginRecord 就不会再挂日志回调</summary>
    private static void ClearFileFlag(FieldInfo field)
    {
        try
        {
            object raw = field.GetValue(null);
            if (raw == null)
            {
                return;
            }

            int value = Convert.ToInt32(raw);
            if ((value & PrintTargetFileFlag) == 0)
            {
                return;
            }

            field.SetValue(null, Enum.ToObject(field.FieldType, value & ~PrintTargetFileFlag));
        }
        catch (Exception)
        {
        }
    }

    /// <summary>调用 TTPrintFile.EndRecord()，注销挂在 Application.logMessageReceivedThreaded 上的回调</summary>
    private static void EndRecord(Type printFile)
    {
        try
        {
            MethodInfo endRecord = printFile.GetMethod("EndRecord",
                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (endRecord == null)
            {
                return;
            }

            ParameterInfo[] args = endRecord.GetParameters();
            if (args != null && args.Length > 0)
            {
                return;
            }

            endRecord.Invoke(null, null);
        }
        catch (Exception)
        {
        }
    }

    #endregion

    #region 锁对象修复

    /// <summary>TTHostEventImpl 的 _mutex（AddHostEventListener 会 lock 它）。
    /// 反编译确认它是 private **实例**字段（不是 static），这里仍兼容两种形态，避免换版本后失效。</summary>
    private static void RepairHostEventMutex()
    {
        try
        {
            if (s_hostEventType == null || s_hostEventMutexField == null)
            {
                return;
            }

            if (s_hostEventMutexField.FieldType != typeof(object))
            {
                return;
            }

            // 静态锁：直接补在类型上
            if (s_hostEventMutexField.IsStatic)
            {
                if (s_hostEventMutexField.GetValue(null) == null)
                {
                    s_hostEventMutexField.SetValue(null, new object());
                }

                return;
            }

            object instance = null;
            if (s_hostEventInstanceGetter != null)
            {
                instance = s_hostEventInstanceGetter.Invoke(null, null);
            }

            if (instance == null && s_hostEventInstanceField != null)
            {
                instance = s_hostEventInstanceField.GetValue(null);
            }

            if (instance == null)
            {
                return;
            }

            if (s_hostEventMutexField.GetValue(instance) == null)
            {
                s_hostEventMutexField.SetValue(instance, new object());
            }
        }
        catch (Exception)
        {
        }
    }

    private static bool IsLockFieldName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return false;
        }

        return name.IndexOf("lock", StringComparison.OrdinalIgnoreCase) >= 0
            || name.IndexOf("sync", StringComparison.OrdinalIgnoreCase) >= 0
            || name.IndexOf("mutex", StringComparison.OrdinalIgnoreCase) >= 0;
    }

    #endregion

    #region 反射辅助

    private static Assembly FindSdkAssembly()
    {
        try
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                Assembly assembly = assemblies[i];
                if (assembly == null)
                {
                    continue;
                }

                string name = assembly.GetName().Name;
                if (string.Equals(name, SdkAssemblyName, StringComparison.OrdinalIgnoreCase))
                {
                    return assembly;
                }
            }
        }
        catch (Exception)
        {
        }

        return null;
    }

    private static Type[] SafeGetTypes(Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException e)
        {
            return e.Types ?? new Type[0];
        }
        catch (Exception)
        {
            return new Type[0];
        }
    }

    private static Type FindBySimpleName(Type[] types, string simpleName)
    {
        if (types == null)
        {
            return null;
        }

        for (int i = 0; i < types.Length; i++)
        {
            Type type = types[i];
            if (type != null && type.Name == simpleName)
            {
                return type;
            }
        }

        return null;
    }

    private static void RunCctor(Type type)
    {
        try
        {
            RuntimeHelpers.RunClassConstructor(type.TypeHandle);
        }
        catch (Exception)
        {
        }
    }

    private static void CreateDriver()
    {
        try
        {
            GameObject go = new GameObject("~TTSDKLogFix");
            UnityEngine.Object.DontDestroyOnLoad(go);
            go.AddComponent<TTSDKLogFixDriver>();
        }
        catch (Exception)
        {
        }
    }

    #endregion
}

/// <summary>启动后 12 秒内每隔 0.5 秒重跑一次补丁，覆盖 InitSDK 之后才创建的 SDK 对象，然后自毁。</summary>
public class TTSDKLogFixDriver : MonoBehaviour
{
    private float _elapsed;
    private float _nextRun;

    private void Update()
    {
        _elapsed += Time.unscaledDeltaTime;
        if (_elapsed >= _nextRun)
        {
            _nextRun = _elapsed + 0.5f;
            TTSDKLogCompatFix.Run();
        }

        if (_elapsed >= 12f)
        {
            Destroy(gameObject);
        }
    }
}
