using System;

public abstract class IAdPlatform
{
    /* 调起侧边栏 */
    public abstract void NavigateToScene(Action onSuccess, Action<string> onFail);

    /* 激励视频广告 */
    public abstract void ShowRewardedVideoAd(string adId, Action onSuccess, Action<string> onFail);

    /* 激励视频异常兜底：超时没有任何回调时，强制恢复游戏状态并销毁广告实例 */
    public virtual void AbortRewardedVideo()
    {
    }

    /* 插屏广告：返回 true 表示本次已经真正展示出去了（用于冷却判断） */
    public abstract bool ShowInterstitialAd(string adId);

    /** 游戏分享 */
    public abstract void ShareGame(Action<bool> callBack);
}