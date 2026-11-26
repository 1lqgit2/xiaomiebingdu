# Unity2018_消灭病毒（闯破层关寻迹探秘）

## 构建 / 打包约定
- 目标平台：抖音小游戏（WebGL + TTSDK/ByteGame），Unity 2022.3.62f3c1。
- **OPS 混淆必须关闭**：`Assets/OPS/Obfuscator/Settings/Obfuscator_Settings.json` 里
  `Global_Enable_Obfuscation = False`、`Enable_AntiTampering_Protection = False`。
  - AntiTampering 开启 → 启动即 `FileNotFoundException "/Assembly-CSharp.dll"` + 类型初始化器连环失败。
  - 改名开启 → PureMVC（`View.cs` 用字符串 `"handleNotification"` + `GetMethod(变量)`）反射断链，
    wasm 直接 trap：`null function or function signature mismatch` → 游戏 quit status 1。
- 改完 OPS 配置后必须删 `Library/Bee/artifacts/WebGL/ManagedStripped/Assembly-CSharp.dll(.pdb)` 与
  `Temp/StagingArea/Data/Managed/Assembly-CSharp.dll(.pdb)`，否则增量构建复用旧（被改过的）dll。
  体积判据：混淆版约 3.87MB，正常约 1.6MB。
- 崩溃还原真名的入口：`tt-minigame/tt-minigame/webgl.symbols.json`（`{wasm 函数索引: 符号名}`）。

## 代码约定
- 广告统一走 `Assets/AdSDK`（TikTokPlatform），业务层 `Assets/Scripts/orangesGame/AdManager.cs` 只做转发。
- `TTSDKLogCompatFix`（`Assets/AdSDK/Scripts/`）修 ttsdk 的 `lock(null)` 刷屏；该补丁类**不要写静态字段初始化器**
  （避免被混淆器注入 cctor）。成功生效会打印 `[TTSDK] 静态锁补丁已生效…`。
- `UrlController` 里的 `sdk.ihippogame.com` 是旧 iHippo 渠道接口，WebGL 打包版已跳过（无域名白名单）。

## UI 层级
- UI 层是硬刻度：`UIViewManager.GetLayerTran(key)` 直接 `canvas.sortingOrder = key`。
  1=主界面背景、40=精灵/飞机层、60=主界面、80=战斗/属性/每日任务、90=通用弹窗、100=剧情、110=金币飞行/精灵层、120=商店、130=顶层。
- **抖音原生浮层盖不住**：屏幕右侧的「UP」奖励卡 + 中间复访进度条不在 Unity 侧（全 102 个 prefab + 场景都扫不到），
  是平台把原生视图画在 WebGL canvas 之上，Unity 改 sortingOrder 无效，只能让自家 UI 避开该区域。
- 弹窗要压过游戏内所有界面时，别挪层，给弹窗根节点挂嵌套 `Canvas(overrideSorting=true, sortingOrder=200)`
  **并同时加 `GraphicRaycaster`**（否则按钮点不到）。参考 `Assets/AdSDK/Scripts/cebian.cs`。
