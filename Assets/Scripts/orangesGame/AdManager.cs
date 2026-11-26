using System;
using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	/// <summary>
	/// 广告统一入口。
	/// 平台已统一为抖音小游戏（TTSDK）：Unity Ads 与 Hippo/MoPub 已全部移除，
	/// 所有广告请求转发到 Assets/AdSDK/Scripts/AdManager.cs（TikTokPlatform）。
	/// 对外 API 与原实现保持一致，业务调用点不需要改动。
	/// </summary>
	public class AdManager : Singleton<AdManager>
	{
		public const int TYPE_HIPPO_ANDROID = 0;

		public const int TYPE_HIPPO_IOS = 1;

		public const int TYPE_HIPPO_ANDROID_SIMPLETON = 2;

		public bool isDebug;

		public bool isTest;

		private AdArgs _adArgs;

		private Dictionary<string, AdAction> _AdAction;

		public System.Action onAdResult;

		// 激励视频请求防抖：抖音侧 0.5 秒内的重复请求会被直接丢弃且不回调
		private float _lastRewardRequestTime = -10f;

		/// <summary>
		/// 抖音广告管理器（全局命名空间的 AdManager）。
		/// 场景里没挂时自动创建一个常驻实例，避免老代码直接调广告时拿不到平台实例。
		/// </summary>
		private static global::AdManager TikTok
		{
			get
			{
				// AdSDK 的 AdManager.Instance 会在场景没有挂载时自动创建常驻实例
				return global::AdManager.Instance;
			}
		}

		private AdManager()
		{
			_AdAction = new Dictionary<string, AdAction>();
		}

		public bool GetRewardedUnityAdsReady()
		{
			// 抖音激励视频随时可以创建实例，这里只确认平台管理器可用
			return TikTok != null;
		}

		public void ShowRewardedAds()
		{
			ShowRewardedAds(delegate(bool success)
			{
				if (onAdResult != null)
				{
					onAdResult.Invoke();
				}
			});
		}

		public void ShowRewardedAds(System.Action<bool> callback)
		{
			if (Time.unscaledTime - _lastRewardRequestTime < 0.5f)
			{
				Debug.LogWarning("[广告] 激励视频请求过于频繁，已忽略");
				InvokeCallback(callback, false);
				return;
			}
			_lastRewardRequestTime = Time.unscaledTime;

			global::AdManager mgr = TikTok;
			if (mgr == null)
			{
				Debug.LogError("[广告] 抖音 AdManager 不可用");
				InvokeCallback(callback, false);
				return;
			}

			Debug.Log("[广告] 播放抖音激励视频");
			mgr.ShowRewardedVideoAd(
				delegate
				{
					InvokeCallback(callback, true);
				},
				delegate(string msg)
				{
					Debug.LogWarning("[广告] 抖音激励视频失败：" + msg);
					InvokeCallback(callback, false);
				});
		}

		private static void InvokeCallback(System.Action<bool> callback, bool value)
		{
			if (callback != null)
			{
				callback(value);
			}
		}

		public void InitSdk(string appKey, string appSecret, string appChannel, string umKey)
		{
			// 保留旧接口：抖音小游戏的 AppID 与广告位在 AdSDK/AdManager 面板上配置
			if (_adArgs == null)
			{
				_adArgs = new AdArgs();
			}
			_adArgs.Appkey = appKey;
			_adArgs.Appsecret = appSecret;
			_adArgs.Appchannel = appChannel;
			_adArgs.UMKey = umKey;
		}

		public void InitBanner<T>(string[] ids) where T : AdAction, new()
		{
			if (_adArgs != null)
			{
				_adArgs.BannerIDs = ids;
				_adArgs.BannerEvent = CreateAdAction<T>(ids).Adevent;
			}
			else
			{
				UnityEngine.Debug.LogError("AdArgs参数为空,请先执行InitSdkID");
			}
		}

		public void InitInterstitial<T>(string[] ids) where T : AdAction, new()
		{
			if (_adArgs != null)
			{
				_adArgs.InterstitialIDs = ids;
				_adArgs.InterstitialEvent = CreateAdAction<T>(ids).Adevent;
			}
			else
			{
				UnityEngine.Debug.LogError("AdArgs参数为空,请先执行InitSdkID");
			}
		}

		public void InitVideo<T>(string[] ids) where T : AdAction, new()
		{
			if (_adArgs != null)
			{
				_adArgs.VideoIDs = ids;
				_adArgs.VideoEvent = CreateAdAction<T>(ids).Adevent;
			}
			else
			{
				UnityEngine.Debug.LogError("AdArgs参数为空,请先执行InitSdkID");
			}
		}

		public void CreateSDK(int type)
		{
			// 抖音小游戏 SDK 由 AdSDK/AdManager 统一初始化，这里不再创建第三方 SDK
			UnityEngine.Debug.Log("[广告] 广告平台已切换为抖音小游戏，忽略 CreateSDK(" + type + ")");
		}

		public void LoadBanner(string id)
		{
			// 抖音小游戏没有 Banner 广告位
		}

		public void ShowBanner(string id, string scene)
		{
			UnityEngine.Debug.Log("[广告] 抖音小游戏不支持 Banner，已忽略 scene=" + scene);
		}

		public void LoadInterstitial(string id)
		{
			// 抖音插屏由 TikTokPlatform 内部创建与预创建，无需手动加载
		}

		public void ShowInterstitial(string id, string scene, Action<bool> onComplete = null)
		{
			global::AdManager mgr = TikTok;
			if (mgr == null)
			{
				InvokeCallback(onComplete, false);
				return;
			}

			// 平台侧会因「未加载好 / 30 秒间隔 / 启动 15 秒保护」跳过展示，
			// 这里把真实结果回传，调用方才不会白白消耗自己的冷却
			bool shown = mgr.ShowInterstitialAd();
			Debug.Log("[广告] 抖音插屏广告 scene=" + scene + " 是否展示=" + shown);
			InvokeCallback(onComplete, shown);
		}

		public void LoadVideo(string id)
		{
			// 抖音激励视频在播放时创建实例，并会在关闭后自动预创建下一个
		}

		public void ShowVideo(string id, string scene, Action<bool> onComplete = null)
		{
			Debug.Log("[广告] 激励视频 scene=" + scene);
			ShowRewardedAds(onComplete);
		}

		private AdAction CreateAdAction<T>(string[] ids) where T : AdAction, new()
		{
			T val = new T();
			for (int i = 0; i < ids.Length; i++)
			{
				if (!_AdAction.ContainsKey(ids[i]))
				{
					_AdAction.Add(ids[i], val);
				}
				else
				{
					UnityEngine.Debug.LogError("CreateAdAction is contain:" + ids[i]);
				}
			}
			return val;
		}

		public bool IsLoad(string id, string path)
		{
			// 抖音广告不提供同步的加载状态查询，统一认为可用，展示失败会走回调
			return true;
		}

		public void OnComplete(string id, Action<bool> callback)
		{
			// 保留旧接口，抖音广告的结果通过 Show 系列方法的回调返回
			if (_AdAction.ContainsKey(id))
			{
				_AdAction[id].onComplete = null;
				_AdAction[id].onComplete = callback;
			}
		}
	}
}
