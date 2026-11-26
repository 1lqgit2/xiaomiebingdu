using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_WEBGL && !UNITY_EDITOR
using TTSDK;
#endif

// 广告平台枚举
public enum AdPlatformType
{
    [InspectorName("抖音小游戏")]
    TickTok,
    [InspectorName("TopOn聚合")]
    TopOn,
    [InspectorName("微信小游戏")]
    WebChat,
}


/// <summary>
/// 广告管理
/// </summary>
public class AdManager : MonoBehaviour
{
   // public GameObject aaa;

    private static AdManager instance;

    /// <summary>
    /// 全局唯一实例。
    /// 这里用属性而不是裸静态字段：初始场景 Loadscreen 是「additive 加载 Master → 再卸载自己」的流程，
    /// 一旦静态字段指向的物体被场景卸载销毁，属性会自动重新找一个，避免 Instance 悄悄变成 null。
    /// 场景里完全没有挂载时也会自动创建一个常驻实例，保证广告调用不会拿到 null。
    /// </summary>
    public static AdManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AdManager>();
            }

            if (instance == null)
            {
                GameObject go = new GameObject("TikTokAdManager");
                instance = go.AddComponent<AdManager>();
                Debug.Log("[广告] 场景中没有 AdManager，已自动创建常驻实例");
            }

            return instance;
        }
    }

    [Header("平台")]
    public AdPlatformType adPlatformType = AdPlatformType.TickTok;
    [Header("小游戏ID")]
    public string AppId;
    [Header("激励视频广告位")]
    public List<string> videoAdIdList = new();
    [Header("插屏广告位")]

    public List<string> interAdIdList = new();
    private IAdPlatform platFormIns = null;
    private float lastAdClickTime;

    // 激励视频看护：抖音侧极少情况下会既不回调 OnClose 也不回调 OnError（广告位异常等），
    // 那样调用方永远等不到结果、游戏还会停在 timeScale = 0。超时后按失败收尾。
    private const float RewardTimeoutSeconds = 180f;
    private bool rewardPending;
    private float rewardRequestTime;
    private Action<string> rewardPendingFail;

     void Awake()
    {
        if (instance != null && instance != this)
        {
            // 已经有一个实例了（通常是初始场景里的那个），本场景内的重复实例直接销毁，
            // 避免广告平台被初始化两次。
            // 先 enabled=false：Destroy 是延迟到帧末执行的，不禁用的话本帧 Start() 仍会跑，
            // 会把 TT.InitSDK 和广告平台再初始化一遍。
            enabled = false;
            Destroy(gameObject);
            return;
        }

        instance = this;

        // 关键：LoadscreenScene 用 LoadSceneAsync(1, Additive) 加载 Master，随后 UnloadSceneAsync(0)
        // 卸载初始场景。AdManager 如果留在初始场景里，会跟着一起被销毁，静态 Instance 变成
        // 「已销毁对象」（Unity 里 == null 为 true），点广告时就报「场景里没有 AdManager」。
        // 所以这里把它移到根节点并设为跨场景常驻。
        if (transform.parent != null)
        {
            transform.SetParent(null, true);
        }

        if (transform.root == transform)
        {
            DontDestroyOnLoad(gameObject);
            Debug.Log("[广告] AdManager 已设为跨场景常驻");
        }
        else
        {
            Debug.LogWarning("[广告] AdManager 不在根节点，无法跨场景常驻，切换场景后广告可能失效");
        }
    }

    private void Start()
    {
        // 抖音小游戏 SDK 初始化（WebGL 真机环境；编辑器下 TTSDK 为 mock，不需要初始化）
        InitTTSdk();
        // TTSDKLog 也可能在 InitSDK 回调之后才创建，延迟再关一次兜底
        Invoke(nameof(DisableSdkFileLogFallback), 2f);

        if (this.AppId == "")
        {
            Debug.LogWarning("请填写APPID");
        }
        
        if (this.videoAdIdList == null || this.videoAdIdList.Count == 0)
        {
            Debug.LogWarning("视频广告位ID列表为空, 将使用默认广告位");
        }

        if (this.interAdIdList == null || this.interAdIdList.Count == 0)
        {
            Debug.LogWarning("插屏广告位ID列表为空, 将使用默认广告位");
        }

        switch (adPlatformType)
        {
            case AdPlatformType.TickTok:
                platFormIns = new TikTokPlatform(this.AppId, this.GetRandVideoAdId(), this.GetRandScreenAdId());

                break;
            case AdPlatformType.TopOn:
                Debug.LogWarning("TopOn广告平台尚未实现");
                break;
        }
        
        // 添加广告平台初始化检查
        if (this.platFormIns == null)
        {
            Debug.LogError($"未初始化广告平台：{this.adPlatformType}");
        }
        else
        {
            Debug.Log($"[广告] 平台已就绪：抖音小游戏，激励视频={this.GetRandVideoAdId()}，插屏={this.GetRandScreenAdId()}");
        }
        
        this.lastAdClickTime = Time.unscaledTime;
    }

    private void InitTTSdk()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        try
        {
            // OnTTContainerInitCallback 的签名是 void Invoke(int code, ContainerEnv env)
            TT.InitSDK((code, env) =>
            {
                if (code == 0)
                {
                    Debug.Log("[抖音SDK] 初始化完成");
                }
                else
                {
                    // 1 = SDK 版本不支持，2 = 引擎版本不支持
                    Debug.LogWarning($"[抖音SDK] 初始化失败，错误码 {code}");
                }
                // SDK 初始化会创建 TTSDKLog（OnEnable 里给每条 Unity 日志挂文件写入回调，
                // 该回调内部 lock 的对象在部分环境为空，导致 Monitor ArgumentNullException 刷屏）。
                // 这里初始化完再补跑一次补丁：清掉日志落盘回调 + 补齐为 null 的锁对象。
                TTSDKLogCompatFix.Run();
            });
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[抖音SDK] 初始化异常（不影响广告接口调用）：{e.Message}");
        }
#endif
    }

    /// <summary>TTSDK 延迟兜底：再补跑一次兼容补丁（清日志落盘回调 + 补齐空锁对象）</summary>
    private void DisableSdkFileLogFallback()
    {
        TTSDKLogCompatFix.Run();
    }


    /// <summary>
    /// 播放插屏广告
    /// </summary>
    /// <returns>true 表示本次已真正展示，false 表示被跳过（未加载好/冷却中/无实例）</returns>
    public bool ShowInterstitialAd()
    {
        if (this.platFormIns == null)
        {
            return false;
        }

        return this.platFormIns.ShowInterstitialAd(this.GetRandScreenAdId());
    }

    /// <summary>
    /// 跳转场景
    /// </summary>
    /// <param name="onSuccess">成功回调</param>
    /// <param name="onFail">失败回调</param>
    public void NavigateToScene(Action onSuccess = null, Action<string> onFail = null)
    {
        if (this.platFormIns == null)
        {
            Debug.LogError("广告平台实例未初始化，无法跳转场景");
            onFail?.Invoke("广告平台实例未初始化");
            return;
        }
        
        try
        {
            this.platFormIns.NavigateToScene(onSuccess, onFail);
        }
        catch (Exception e)
        {
            Debug.LogError($"跳转场景时发生异常：{e.Message}");
            onFail?.Invoke($"跳转场景时发生异常：{e.Message}");
        }
    }
    public void ShareGame(Action<bool> callBack = null)
    {
        this.platFormIns?.ShareGame(callBack);
    }

    /// <summary>
    /// 播放激励视频广告
    /// </summary>
    /// <param name="onSuccess">成功回调</param>
    /// <param name="onFail">失败回调</param>
    public void ShowRewardedVideoAd(Action onSuccess, Action<string> onFail)
    {
        if (!this.CheckTimeLimit())
        {
            // 原来这里直接 return，调用方既不会发奖也收不到失败，表现为「点了没反应」
            onFail?.Invoke("点击过于频繁，请稍后再试");
            return;
        }
        
        if (this.platFormIns == null)
        {
            Debug.LogError("广告平台实例未初始化，无法显示广告");
            onFail?.Invoke("广告平台实例未初始化");
            return;
        }

        // 保证一次请求只回调一次：成功、失败、超时三条路先到先算
        Action success = () =>
        {
            if (!this.rewardPending) return;
            this.rewardPending = false;
            this.rewardPendingFail = null;
            onSuccess?.Invoke();
        };

        Action<string> fail = msg =>
        {
            if (!this.rewardPending) return;
            this.rewardPending = false;
            this.rewardPendingFail = null;
            onFail?.Invoke(msg);
        };

        this.rewardPending = true;
        this.rewardRequestTime = Time.unscaledTime;
        this.rewardPendingFail = fail;

        try
        {
            this.platFormIns.ShowRewardedVideoAd(this.GetRandVideoAdId(), success, fail);
        }
        catch (Exception e)
        {
            Debug.LogError($"显示广告时发生异常：{e.Message}");
            fail($"显示广告时发生异常：{e.Message}");
        }
    }

    private void Update()
    {
        if (!this.rewardPending)
            return;

        if (Time.unscaledTime - this.rewardRequestTime < RewardTimeoutSeconds)
            return;

        Action<string> fail = this.rewardPendingFail;
        this.rewardPending = false;
        this.rewardPendingFail = null;

        Debug.LogWarning("[广告] 激励视频超时没有任何回调，按失败收尾并恢复游戏");
        try
        {
            this.platFormIns?.AbortRewardedVideo();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[广告] 恢复游戏状态异常：{e.Message}");
        }

        fail?.Invoke("广告超时，请稍后再试");
    }


    // 随机取一个激励视频广告位ID
    private string GetRandVideoAdId()
    {
        if (this.videoAdIdList.Count == 0)
        {
            Debug.LogWarning("videoAdIdList is empty, using default ad id");
            return "f32bf9970ah83mq1kg"; // 默认广告ID，防止空字符串
        }
        int index = UnityEngine.Random.Range(0, this.videoAdIdList.Count);
        return this.videoAdIdList[index];
    }

    // 随机取一个插屏广告位ID
    private string GetRandScreenAdId()
    {
        if (this.interAdIdList.Count == 0)
        {
            Debug.LogWarning("interAdIdList is empty, using default ad id");
            return "dk217ga5e5b48uo4gh"; // 默认广告ID，防止空字符串
        }
        int index = UnityEngine.Random.Range(0, this.interAdIdList.Count);
        return this.interAdIdList[index];
    }

    // 限制 0.5 秒内广告只能触发一次
    private bool CheckTimeLimit()
    {
        // 必须用 unscaledTime：激励视频播放期间 Time.timeScale = 0，Time.time 会被冻结，
        // 关掉广告后的第一次点击会被误判成「点击过于频繁」而被拦下（表现为点了没反应、也没有奖励）
        float now = Time.unscaledTime;
        if (now - lastAdClickTime < 0.5f)
        {
            Debug.LogWarning("广告点击过于频繁，请稍后再试。");
            return false;
        }
        lastAdClickTime = now;
        return true;
    }
}
