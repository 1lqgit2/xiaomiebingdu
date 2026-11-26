using DG.Tweening;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;

namespace orangesGame
{
	public class MainViewMediator : Mediator, IMediator
	{
		private Timer _offlintTimer;

		private Timer _diamondOfflineTimer;

		private Timer _timeGiftTimer;

		private Timer _fullTimer;

		private Timer _luckyDrawTimer;

		private long _diamondOfflineTime;

		private float _curTime;

		private bool _isClickOffline;

		private Tweener _coinTweener;

		private SpriteAtlas _mainAtlas;

		private bool _isExtend;

		private const string Shooting_en = "drag-to-shoot";

		private const string Shooting_cn = "drag-to-shoot_cn";

		private const string Shooting_korean = "drag-to-shoot_korean";

		private const string Shooting_portuguese = "drag-to-shoot_portuguese";

		private const string Shooting_spanish = "drag-to-shoot_spanish";

		private const string Shooting_traditional = "drag-to-shoot_traditional";

		private const string Shooting_ru = "drag-to-shoot_ru";

		private const string Shooting_de = "drag-to-shoot_de";

		private bool _isClickBtnProp;

		private MainView View => (MainView)ViewComponent;

		public MainViewMediator(string name, MainView viewComponent)
			: base(name, viewComponent)
		{
		}

		public override void OnRegister()
		{
			base.OnRegister();
			View.eventStart.ActionPointerDown = OnClickStart;
			View.btnOffline.onClick.AddListener(OnClickOffline);
			View.btnTask.onClick.AddListener(OnClickTask);
			View.btnProp.onClick.AddListener(OnClickProp);
			View.btnLuckDrawBox.onClick.AddListener(ClickLuckDrawBox);
			View.btnCoinAdd.onClick.AddListener(OnClickCoinAdd);
			View.btnDiamondOffline.onClick.AddListener(OnClickDiamondOffline);
			View.btnShop.onClick.AddListener(OnClickShop);
			View.btnFull.onClick.AddListener(OnClickFull);
			View.btnLevel.onClick.AddListener(OnClickLevel);
			View.btnExtend.onClick.AddListener(OnClickExtend);
			View.btnExtendClose.onClick.AddListener(OnClickExtend);
			View.btnExtendClose.gameObject.SetActive(value: false);
			_isClickOffline = false;
			View.txtCollect.text = LanguageConfig.COLLECT;
			View.txtFree.text = LanguageConfig.FREE;
			View.TxtGiftTips.text = LanguageConfig.Main_Tip_Shop;
			_mainAtlas = Resources.Load<SpriteAtlas>("GameAtlas/MainAtlas");
			SetShooterImg();
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				View.btnShop.gameObject.SetActive(value: false);
			}
			else
			{
				View.btnDiamondAdd.onClick.AddListener(OnClickDiamondAdd);
			}
		}

		public override void OnRemove()
		{
			base.OnRemove();
		}

		public override IList<string> ListNotificationInterests()
		{
			List<string> list = new List<string>();
			list.Add("MAIN_INIT");
			list.Add("MAIN_HIDE");
			list.Add("SET_COIN");
			list.Add("SET_DIAMOND");
			list.Add("MAIN_TIP_SET_ACTIVE");
			list.Add("MAIN_GET_OFFLINE");
			list.Add("MAIN_SET_CLICK_OFFLINE_STATE");
			list.Add("BATTLE_VIEW_GET_COIN_ANI");
			list.Add("MAIN_TASK_TIP");
			list.Add("MAIN_TIME_LIMIT_TIPS");
			list.Add("MAIN_DIAMOND_STORE_PROGRESS");
			list.Add("MAIN_PROPSHOP_TIP");
			list.Add("MAIN_START_GAME");
			list.Add("MAIN_EXTEND_INACTIVE");
			return list;
		}

		public override void HandleNotification(INotification notification)
		{
			switch (notification.Name)
			{
			case "MAIN_INIT":
				if (GameGlobalData.ActualLevel > 7)
				{
					View.btnTask.gameObject.SetActive(value: true);
					Singleton<DailyTaskHandle>.Instance.Init();
				}
				_isExtend = false;
				_curTime = 0f;
				SetCoin();
				SetDiamond();
				SetOffline();
				SetTaskTip();
				SetDiamondOffline();
				SetDiamondFull();
				SetBtnPropShopState();
				SetLuckDrawBoxState();
				SetLevelName();
				CheckExtendTip();
				Timer.Register(1f, delegate
				{
					View.eventStart.gameObject.SetActive(value: true);
					SetMainViewPropBtnPos();
				});
				if (!_isExtend)
				{
					SetDiamondFullInActive();
				}
				CheckPropShopState();
				View.btnExtend.gameObject.SetActive(GameGlobalData.ActualLevel >= 8);
				SetBtnExtendBgHeight();
				break;
			case "MAIN_HIDE":
				CloseOfflineTimer();
				SetDiamondOfflineTimerCancel();
				SetGiftTimerCanel();
				SetFullTimeCanel();
				SetLuckyDrawTimerCanel();
				break;
			case "SET_COIN":
				SetCoin();
				break;
			case "SET_DIAMOND":
				SetDiamond();
				break;
			case "MAIN_TIP_SET_ACTIVE":
				SetTipActive((bool)notification.Body);
				break;
			case "MAIN_GET_OFFLINE":
				GetOffLineReward((bool)notification.Body);
				break;
			case "MAIN_SET_CLICK_OFFLINE_STATE":
				OnSetOfflineState((bool)notification.Body);
				break;
			case "BATTLE_VIEW_GET_COIN_ANI":
				CoinGetAni();
				break;
			case "MAIN_TASK_TIP":
				SetTaskTip();
				CheckExtendTip();
				break;
			case "MAIN_TIME_LIMIT_TIPS":
				SetLimitGift((bool)notification.Body);
				break;
			case "MAIN_DIAMOND_STORE_PROGRESS":
				SetDiamondStoreProgress();
				break;
			case "MAIN_PROPSHOP_TIP":
				SetPropShopTip((bool)notification.Body);
				CheckExtendTip();
				break;
			case "MAIN_START_GAME":
				View.eventStart.ActionPointerDown(null);
				break;
			case "MAIN_EXTEND_INACTIVE":
				if (_isExtend)
				{
					OnClickExtend();
				}
				break;
			}
		}

		private void SetBtnExtendBgHeight()
		{
			int num = 0;
			if (View.btnTask.gameObject.activeSelf)
			{
				num++;
			}
			if (View.btnFull.gameObject.activeSelf)
			{
				num++;
			}
			if (View.btnProp.gameObject.activeSelf)
			{
				num++;
			}
			if (View.tranDiamondOffline.gameObject.activeSelf)
			{
				num++;
			}
			if (View.btnLuckDrawBox.gameObject.activeSelf)
			{
				num++;
			}
			RectTransform tranExtend = View.tranExtend;
			Vector2 sizeDelta = View.tranExtend.sizeDelta;
			tranExtend.sizeDelta = new Vector2(sizeDelta.x, 148 * num);
		}

		private void CheckExtendTip()
		{
			bool flag = View.tranTaskTip.gameObject.activeSelf || View.tranPropTip.gameObject.activeSelf || (View.btnLuckDrawBox.gameObject.activeSelf && Singleton<LuckyDrawManager>.Instance.CanLuckyDraw());
			View.objExtendTip.SetActive(flag && !_isExtend);
		}

		private void SetLevelName()
		{
			int[] array = new int[13]
			{
				5,
				10,
				20,
				40,
				60,
				80,
				100,
				120,
				140,
				160,
				180,
				200,
				999
			};
			string[] array2 = new string[13]
			{
				"ARI",
				"TAU",
				"GEN",
				"CMC",
				"LEO",
				"VIR",
				"LIB",
				"SCO",
				"SGR",
				"CAP",
				"AGR",
				"PSC",
				"EXT"
			};
			int num = 0;
			while (true)
			{
				if (num < array.Length)
				{
					if (GameGlobalData.ActualLevel <= array[num])
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			View.txtLevelName.text = array2[num];
		}

		private void SetShooterImg()
		{
			if (Singleton<LanguageManager>.Instance.IsCn())
			{
				View.shooterImg.sprite = _mainAtlas.GetSprite("drag-to-shoot_cn");
			}
			else if (Singleton<LanguageManager>.Instance.IsKorean())
			{
				View.shooterImg.sprite = _mainAtlas.GetSprite("drag-to-shoot_korean");
			}
			else if (Singleton<LanguageManager>.Instance.IsTraditional())
			{
				View.shooterImg.sprite = _mainAtlas.GetSprite("drag-to-shoot_traditional");
			}
			else if (Singleton<LanguageManager>.Instance.IsSpanish())
			{
				View.shooterImg.sprite = _mainAtlas.GetSprite("drag-to-shoot_spanish");
			}
			else if (Singleton<LanguageManager>.Instance.IsPortuguese())
			{
				View.shooterImg.sprite = _mainAtlas.GetSprite("drag-to-shoot_portuguese");
			}
			else if (Singleton<LanguageManager>.Instance.IsRu())
			{
				View.shooterImg.sprite = _mainAtlas.GetSprite("drag-to-shoot_ru");
			}
			else if (Singleton<LanguageManager>.Instance.IsDe())
			{
				View.shooterImg.sprite = _mainAtlas.GetSprite("drag-to-shoot_de");
			}
			else
			{
				View.shooterImg.sprite = _mainAtlas.GetSprite("drag-to-shoot");
			}
		}

		private void OnSetOfflineState(bool isClickOffline)
		{
			_isClickOffline = isClickOffline;
		}

		private void OnClickOffline()
		{
			if (GameGlobalData.OffLineCoin != 0 && !_isClickOffline)
			{
				_isClickOffline = true;
				int num = (int)GameGlobalData.GetOutOfflineTime();
				if (num > GameGlobalData.ShowOfflineAdsTime)
				{
					Singleton<UIViewManager>.Instance.ShowView("DIALOG_VIEW", "DIALOG_OFFLINE");
				}
				else
				{
					GetOffLineReward(isADS: false);
				}
			}
		}

		private void OnClickTask()
		{
			if (GameGlobalData.ActualLevel > 7)
			{
				Singleton<DailyTaskHandle>.Instance.Init();
				Singleton<UIViewManager>.Instance.ShowView("TASK_VIEW", "TASK_INIT");
			}
		}

		private void ClickLuckDrawBox()
		{
			if (View.tranLuckUnlockTip.gameObject.activeSelf)
			{
				View.tranLuckUnlockTip.gameObject.SetActive(value: false);
			}
			Singleton<UIViewManager>.Instance.ShowView("LUCK_BOX_DRAW_VIEW", "LUCK_DRAW_BOX_INIT");
			SetLuckDrawBoxTime();
		}

		private void OnClickProp()
		{
			if (View.tranPropUnlockTip.gameObject.activeSelf)
			{
				View.tranPropUnlockTip.gameObject.SetActive(value: false);
			}
			Singleton<UIViewManager>.Instance.ShowView("PROPSHOP_VIEW", "PROPSHOP_INIT");
		}

		private void OnClickCoinAdd()
		{
			Singleton<UIViewManager>.Instance.ShowView("CONVERSION_VIEW", "CONVERSION_INIT");
		}

		private void OnClickDiamondAdd()
		{
			Singleton<UIViewManager>.Instance.ShowView("SHOP_VIEW", "SHOP_INIT");
		}

		private void OnClickDiamondOffline()
		{
			if (View.tranDiamondOfflineTip.gameObject.activeSelf)
			{
				View.tranDiamondOfflineTip.DOKill();
				View.tranDiamondOfflineTip.gameObject.SetActive(value: false);
			}
			bool flag = Singleton<AdManager>.Instance.IsLoad("0a2cad70-9e1c-450f-bcdc-f3798b324b95", "ClickDiamondOffline");
			if (!_isClickOffline && flag && Singleton<DiamondManager>.Instance.IsCanGotShowVideoReward())
			{
				_isClickOffline = true;
				Singleton<AdManager>.Instance.ShowVideo("0a2cad70-9e1c-450f-bcdc-f3798b324b95", "offlinediamond", ClickDiamondOfflineCallback);
			}
		}

		private void OnClickFull()
		{
			Singleton<UIViewManager>.Instance.ShowView("LUCKYBOX_VIEW", "LUCKYBOX_INIT");
		}

		private void OnClickShop()
		{
			Singleton<UIViewManager>.Instance.ShowView("SHOP_VIEW", "SHOP_INIT");
		}

		private void ClickDiamondOfflineCallback(bool isReward)
		{
			if (isReward)
			{
				GameAnalysisData.SendShowVedio("offlinediamond");
				Transform layerTran = Singleton<UIViewManager>.Instance.GetLayerTran(110);
				Singleton<CoinManager>.Instance.FloatByCircle<FloatDiamond>("GameObject/Item/FloatDiamond", 20, Vector3.zero, View.traDiamondIcon.position, 0.25f, delegate
				{
					Singleton<DiamondManager>.Instance.DoGotShowVideoReward();
					SetDiamondOfflineTime();
					_isClickOffline = false;
				}, layerTran);
				Singleton<AudioManager>.Instance.PlaySound("coin", 0.8f, 4, 0.2f);
			}
			else
			{
				_isClickOffline = false;
			}
		}

		private void CloseOfflineTimer()
		{
			if (_offlintTimer != null)
			{
				_offlintTimer.Cancel();
			}
		}

		private void OnClickExtend()
		{
			RectTransform target = View.btnExtend.transform as RectTransform;
			if (_isExtend)
			{
				target.DOAnchorPosX(43f, 0.3f);
				SetDiamondFullInActive();
				View.btnExtendClose.gameObject.SetActive(value: false);
				View.btnExtend.image.enabled = true;
				SetPropShopState();
				_isExtend = !_isExtend;
				CheckExtendTip();
				if (View.tranLuckUnlockTip.gameObject.activeSelf)
				{
					View.tranLuckUnlockTip.gameObject.SetActive(value: false);
				}
				return;
			}
			target.DOAnchorPosX(264f, 0.3f);
			View.objExtendTip.SetActive(value: false);
			View.btnExtendClose.gameObject.SetActive(value: true);
			View.btnExtend.image.enabled = false;
			SetDiamondFull();
			SetPropShopState();
			SendNotification("PROPERTY_HIDE_VIEW");
			_isExtend = !_isExtend;
			if (GameGlobalData.ActualLevel == 26 && Singleton<LuckyDrawManager>.Instance.CanLuckyDraw())
			{
				View.tranLuckUnlockTip.gameObject.SetActive(value: true);
			}
		}

		private void OnClickLevel()
		{
			Singleton<UIViewManager>.Instance.ShowView("LEVEL_VIEW");
			SendNotification("LEVEL_VIEW_SHOW");
		}

		private void GetOffLineReward(bool isADS)
		{
			List<string> list = UmConfig.SetCommonParam();
			list.Add(string.Format("video@{0}", (!isADS) ? "0" : "1"));
			Singleton<AnalyzeSdk>.Instance.OnEventByArgs("GetOfflineGold", list.ToArray());
			if (isADS)
			{
				List<string> list2 = UmConfig.SetCommonParam();
				list2.Add("type@离线金币");
				Singleton<AnalyzeSdk>.Instance.OnEventByArgs("ADSVideoReward", list2.ToArray());
			}
			if (isADS)
			{
				GameAnalysisData.SendShowVedio("offlinegold");
			}
			GameAnalysisData.SendGetOffline(isADS, GameGlobalData.OffLineCoin);
			View.txtOfflineCost.text = "0";
			SetOffLineBgProgress(86400f);
			_curTime = 0f;
			// 金币飞行动画要盖在主界面 UI 之上，主界面已提到 60 层，所以这里用 110（精灵层 110）。
			Transform layerTran = Singleton<UIViewManager>.Instance.GetLayerTran(110);
			Singleton<CoinManager>.Instance.FloatByCircle<FloatGold>("GameObject/Item/FloatCoin", 20, View.btnOffline.transform.position, View.traIcon.position, 0.25f, OnCompleteRewardOffline, layerTran);
			Singleton<AudioManager>.Instance.PlaySound("coin", 0.5f, 4, 0.2f);
		}

		private void SetOffLineBgProgress(float progress)
		{
			View.imgYellowBg.fillAmount = ((float)GameGlobalData.OffLineTime - progress) / (float)GameGlobalData.OffLineTime;
		}

		private void OnCompleteRewardOffline()
		{
			GameAnalysisData.GetGold(GameGlobalData.OffLineCoin, "offline");
			GameGlobalData.AddCurCoin(GameGlobalData.OffLineCoin);
			GameGlobalData.OffLineCoin = 0;
			GameGlobalData.UpdateOfflineTimeRecord(clean: true);
			SendNotification("PROPERTY_REFRESH_VIEW");
			_isClickOffline = false;
			SetOffLineBgProgress(GameGlobalData.OffLineTime);
			View.txtOfflineCost.text = "0";
			View.txtOfflineTime.text = GameUtils.ParseSecondToMMSS(GameGlobalData.OffLineTime);
		}

		private void SetTipActive(bool isActive)
		{
			View.objShootText.SetActive(isActive);
		}

		private void OnClickStart(PointerEventData data)
		{
			if (Singleton<UIViewManager>.Instance.GetPropertyViewShow())
			{
				ApplicationFacade.Instance.SendNotification("PROPERTY_HIDE_VIEW", false);
				return;
			}
			if (_isExtend)
			{
				OnClickExtend();
			}
			Vector2 v;
			if (data == null)
			{
				v = Vector2.zero;
			}
			else
			{
				RaycastResult pointerCurrentRaycast = data.pointerCurrentRaycast;
				v = pointerCurrentRaycast.screenPosition;
			}
			GameGlobalData.ClickStartPos = v;
			ApplicationFacade.Instance.SendNotification("GAME_START");
			ApplicationFacade.Instance.SendNotification("BG_ZOOM", false);
			ApplicationFacade.Instance.SendNotification("BATTLE_START_GAME");
		}

		private void SetCoin()
		{
			View.txtCoin.text = GameUtils.FloatToUnit(GameGlobalData.CurCoin);
		}

		private void SetDiamondFull()
		{
			if (GameGlobalData.ActualLevel >= 16 && Application.platform != RuntimePlatform.IPhonePlayer)
			{
				SetFullTimeCanel();
				View.TranFullTips.gameObject.SetActive(value: true);
				_fullTimer = Timer.Register(5f, delegate
				{
					View.TranFullTips.gameObject.SetActive(!View.TranFullTips.gameObject.activeInHierarchy);
				}, null, isLooped: true);
				//View.btnFull.gameObject.SetActive(value: true);
				SetDiamondStoreProgress();
			}
			else
			{
				View.btnFull.gameObject.SetActive(value: false);
			}
		}

		private void SetDiamondFullInActive()
		{
			SetFullTimeCanel();
			View.TranFullTips.gameObject.SetActive(value: false);
		}

		private void SetDiamondStoreProgress()
		{
			float num = Mathf.FloorToInt(Singleton<DiamondStorageManager>.Instance.GetProcess() * 100f);
			if (num >= 100f)
			{
				View.txtFull.text = LanguageConfig.FULL;
				View.txtFull.gameObject.SetActive(value: true);
				View.txtNum.gameObject.SetActive(value: false);
				View.fullImgDiamond.gameObject.SetActive(value: false);
			}
			else
			{
				View.txtFull.gameObject.SetActive(value: false);
				View.txtNum.gameObject.SetActive(value: true);
				View.fullImgDiamond.gameObject.SetActive(value: true);
				View.txtNum.text = Singleton<DiamondStorageManager>.Instance.GetTotalDiamond().ToString();
			}
		}

		private void SetDiamond()
		{
			View.txtDiamond.text = GameUtils.FloatToUnit(GameGlobalData.CurDiamond);
		}

		private void SetOffline()
		{
			int num = (int)GameGlobalData.GetOutOfflineTime();
			CoinLevelStruct coinLevelStruct = Singleton<GameJson>.Instance.CoinLevelJson[GameGlobalData.CoinOffLineLevel];
			float value = (float)coinLevelStruct.OffLine / 10f;
			GameGlobalData.OffLineCoin = GameGlobalData.MaxGetOutOfflineGold(0, value);
			View.txtOfflineCost.text = GameUtils.FloatToUnit(GameGlobalData.OffLineCoin);
			View.txtOfflineTime.text = GameUtils.ParseSecondToMMSS(GameGlobalData.OffLineTime - num);
			_offlintTimer = Timer.Register(10f, null, UpdateLogic, isLooped: true);
			SetOffLineBgProgress(GameGlobalData.OffLineTime - num);
			GameGlobalData.UpdateOfflineTimeRecord(clean: false);
		}

		private void UpdateLogic(float time)
		{
			View.imgProgress.fillAmount = 0.1f * time;
			if (time >= 10f)
			{
				CoinLevelStruct coinLevelStruct = Singleton<GameJson>.Instance.CoinLevelJson[GameGlobalData.CoinOffLineLevel];
				float value = (float)coinLevelStruct.OffLine / 10f;
				GameGlobalData.OffLineCoin = GameGlobalData.MaxGetOutOfflineGold(0, value);
				_curTime += 10f;
				if (Math.Abs(_curTime - 60f) < 0.01f)
				{
					_curTime = 0f;
					int num = (int)GameGlobalData.GetOutOfflineTime();
					View.txtOfflineTime.text = GameUtils.ParseSecondToMMSS(GameGlobalData.OffLineTime - num);
					SetOffLineBgProgress(GameGlobalData.OffLineTime - num);
				}
				View.txtOfflineCost.text = GameUtils.CoinParse(GameGlobalData.OffLineCoin);
			}
		}

		private void CoinGetAni()
		{
			if (_coinTweener == null || !_coinTweener.IsPlaying())
			{
				_coinTweener = View.traIcon.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.05f).SetLoops(2, LoopType.Yoyo);
			}
		}

		private void SetLimitGift(bool isShowView)
		{
			List<TimeGiftStruct> list = GameGlobalData.ShopData.CheckGiftJson();
			View.TranLimitTimeGiftTips.gameObject.SetActive(list.Count > 0);
			SetGiftTimerCanel();
			if (list.Count <= 0)
			{
				return;
			}
			_timeGiftTimer = Timer.Register(5f, delegate
			{
				View.TranLimitTimeGiftTips.gameObject.SetActive(!View.TranLimitTimeGiftTips.gameObject.activeInHierarchy);
			}, null, isLooped: true);
			ShopPrefsData shopData = GameGlobalData.ShopData;
			TimeGiftStruct timeGiftStruct = list[0];
			long timeLimitGiftTime = shopData.GetTimeLimitGiftTime(timeGiftStruct.ID);
			View.TxtLimitTimeGiftTips.text = GameUtils.GetTimeUnixDate(timeLimitGiftTime);
			if (!isShowView)
			{
				return;
			}
			TimeGiftStruct timeGiftStruct2 = default(TimeGiftStruct);
			bool flag = false;
			for (int i = 0; i < list.Count; i++)
			{
				ShopPrefsData shopData2 = GameGlobalData.ShopData;
				TimeGiftStruct timeGiftStruct3 = list[i];
				if (shopData2.GetTimeLimitGiftViewState(timeGiftStruct3.ID) == 0)
				{
					timeGiftStruct2 = list[i];
					flag = true;
					break;
				}
			}
			if (flag)
			{
				GameGlobalData.ShopData.SaveTimeLimitGiftViewState(timeGiftStruct2.ID, 1);
				Singleton<UIViewManager>.Instance.ShowView("SHOP_BEST_ORDER", "SHOP_TIMEGIFT_VIEW_INIT", timeGiftStruct2);
			}
		}

		private void SetTaskTip()
		{
			if (GameGlobalData.ActualLevel > 7)
			{
				bool active = Singleton<DailyTaskHandle>.Instance.IsReward();
				View.tranTaskTip.gameObject.SetActive(active);
			}
			else
			{
				View.tranTaskTip.gameObject.SetActive(value: false);
			}
		}

		private void SetTaskLockState()
		{
			if (GameGlobalData.ActualLevel > 7 && GameGlobalData.TaskLock == 0)
			{
				GameGlobalData.SetTaskLock(1);
				View.tranTaskUnlockTip.gameObject.SetActive(value: true);
				RectTransform tranTaskUnlockTip = View.tranTaskUnlockTip;
				Vector2 anchoredPosition = View.tranTaskUnlockTip.anchoredPosition;
				tranTaskUnlockTip.DOAnchorPosY(anchoredPosition.y + 15f, 0.5f).SetLoops(-1, LoopType.Yoyo);
			}
			else
			{
				View.tranTaskUnlockTip.gameObject.SetActive(value: false);
			}
		}

		private void CheckPropShopState()
		{
			if ((GameGlobalData.ActualLevel == 8 && !_isExtend) || (GameGlobalData.ActualLevel == 16 && !_isExtend) || (GameGlobalData.ActualLevel == 20 && !View.tranPropUnlockTip.gameObject.activeSelf && !_isExtend) || (GameGlobalData.ActualLevel == 26 && !_isExtend && Singleton<LuckyDrawManager>.Instance.CanLuckyDraw()))
			{
				OnClickExtend();
			}
		}

		private void SetBtnPropShopState()
		{
			bool active = GameGlobalData.ActualLevel >= 20;
			View.btnProp.gameObject.SetActive(active);
		}

		private void SetPropShopState()
		{
			bool flag = GameGlobalData.ActualLevel >= 20;
			View.btnProp.gameObject.SetActive(flag);
			if (flag)
			{
				if (GameGlobalData.ActualLevel == 20 && !View.tranPropUnlockTip.gameObject.activeSelf && !_isExtend && !_isClickBtnProp)
				{
					_isClickBtnProp = true;
					View.tranPropUnlockTip.gameObject.SetActive(value: true);
					RectTransform tranPropUnlockTip = View.tranPropUnlockTip;
					Vector2 anchoredPosition = View.tranPropUnlockTip.anchoredPosition;
					tranPropUnlockTip.DOAnchorPosX(anchoredPosition.x - 15f, 0.5f).SetLoops(-1, LoopType.Yoyo);
				}
				else
				{
					View.tranPropUnlockTip.gameObject.SetActive(value: false);
				}
			}
		}

		private void SetLuckDrawBoxState()
		{
			bool flag = GameGlobalData.ActualLevel > 25;
			View.btnLuckDrawBox.gameObject.SetActive(flag);
			if (flag)
			{
				SetLuckDrawBoxTime();
				bool flag2 = Singleton<LuckyDrawManager>.Instance.CanLuckyDraw();
				if (GameGlobalData.ActualLevel == 26 && !View.tranLuckUnlockTip.gameObject.activeSelf && flag2)
				{
					View.tranLuckUnlockTip.gameObject.SetActive(value: true);
					RectTransform tranLuckUnlockTip = View.tranLuckUnlockTip;
					Vector2 anchoredPosition = View.tranLuckUnlockTip.anchoredPosition;
					tranLuckUnlockTip.DOAnchorPosX(anchoredPosition.x + 15f, 0.5f).SetLoops(-1, LoopType.Yoyo);
				}
				else
				{
					View.tranLuckUnlockTip.gameObject.SetActive(value: false);
				}
			}
		}

		private void SetLuckDrawBoxTime()
		{
			bool flag = Singleton<LuckyDrawManager>.Instance.CanLuckyDraw();
			SetLuckyDrawTimerCanel();
			if (flag)
			{
				View.btnLuckDrawBox.enabled = true;
				View.imgLuckDrawBoxUnLock.enabled = true;
				View.imgLuckDrawBoxLock.enabled = false;
				View.txtLuckDrawBoxTime.text = string.Empty;
				View.objLuckUnlockRedTip.SetActive(value: true);
			}
			else
			{
				View.btnLuckDrawBox.enabled = false;
				View.imgLuckDrawBoxUnLock.enabled = false;
				View.imgLuckDrawBoxLock.enabled = true;
				View.objLuckUnlockRedTip.SetActive(value: false);
				View.txtLuckDrawBoxTime.text = GameUtils.ParseSecondToHHMMSS((int)GameUtils.GetNextDayTime());
				_luckyDrawTimer = Timer.Register(1f, delegate
				{
					View.txtLuckDrawBoxTime.text = GameUtils.ParseSecondToHHMMSS((int)GameUtils.GetNextDayTime());
				}, null, isLooped: true);
			}
		}

		private void SetPropShopTip(bool isTip = true)
		{
			View.tranPropTip.gameObject.SetActive(isTip);
		}

		private void SetDiamondOffline()
		{
			if (GameGlobalData.ActualLevel <= 7)
			{
				return;
			}
			if (!View.tranDiamondOffline.gameObject.activeSelf)
			{
				View.tranDiamondOffline.gameObject.SetActive(value: true);
			}
			if (GameGlobalData.ActualLevel == 8)
			{
				if (!View.tranDiamondOfflineTip.gameObject.activeSelf && Singleton<DiamondManager>.Instance.IsCanGotShowVideoReward())
				{
					View.tranDiamondOfflineTip.gameObject.SetActive(value: true);
					RectTransform tranDiamondOfflineTip = View.tranDiamondOfflineTip;
					Vector2 anchoredPosition = View.tranDiamondOfflineTip.anchoredPosition;
					tranDiamondOfflineTip.DOAnchorPosY(anchoredPosition.y + 15f, 0.5f).SetLoops(-1, LoopType.Yoyo);
				}
			}
			else
			{
				View.tranDiamondOfflineTip.gameObject.SetActive(value: false);
			}
			SetDiamondOfflineTime();
		}

		private void SetDiamondOfflineTime()
		{
			SetDiamondOfflineTimerCancel();
			if (Singleton<DiamondManager>.Instance.IsCanGotShowVideoReward())
			{
				View.txtDiamondOfflineTime.gameObject.SetActive(value: false);
				View.txtDiamondOfflineFree.gameObject.SetActive(value: true);
				View.imgDiamondOffline.gameObject.SetActive(value: false);
				SetBtnDiamondOfflineEnable(isAble: true);
				return;
			}
			View.txtDiamondOfflineTime.gameObject.SetActive(value: true);
			View.txtDiamondOfflineFree.gameObject.SetActive(value: false);
			View.imgDiamondOffline.gameObject.SetActive(value: true);
			_diamondOfflineTime = Singleton<DiamondManager>.Instance.RemainGetShowVideoRewardTime() / 1000;
			View.txtDiamondOfflineTime.text = GameUtils.ParseSecondToHHMMSS((int)_diamondOfflineTime);
			_diamondOfflineTimer = Timer.Register(1f, OnDiamondOfflineTimerComplete, null, isLooped: true);
			SetBtnDiamondOfflineEnable(isAble: false);
			CheckExtendTip();
		}

		private void SetDiamondOfflineTimerCancel()
		{
			if (_diamondOfflineTimer != null && !_diamondOfflineTimer.isCancelled)
			{
				_diamondOfflineTimer.Cancel();
				_diamondOfflineTimer = null;
			}
		}

		private void SetGiftTimerCanel()
		{
			if (_timeGiftTimer != null && !_timeGiftTimer.isCancelled)
			{
				_timeGiftTimer.Cancel();
			}
		}

		private void SetFullTimeCanel()
		{
			if (_fullTimer != null && !_fullTimer.isCancelled)
			{
				_fullTimer.Cancel();
				_fullTimer = null;
			}
		}

		private void SetLuckyDrawTimerCanel()
		{
			if (_luckyDrawTimer != null)
			{
				_luckyDrawTimer.Cancel();
				_luckyDrawTimer = null;
			}
		}

		private void OnDiamondOfflineTimerComplete()
		{
			_diamondOfflineTime--;
			if (_diamondOfflineTime <= 0)
			{
				SetDiamondOfflineTime();
				return;
			}
			View.txtDiamondOfflineTime.text = GameUtils.ParseSecondToHHMMSS((int)_diamondOfflineTime);
			View.imgDiamondOffline.fillAmount = 1 - _diamondOfflineTime / Singleton<DiamondManager>.Instance.GetCoolTime();
		}

		private void SetBtnCoinAddEnable()
		{
			if (GameGlobalData.CurDiamond <= 0f)
			{
				if (View.btnCoinAdd.enabled)
				{
					View.btnCoinAdd.enabled = false;
					View.btnCoinAdd.image.material = GameUtils.GetGrayMaterial();
				}
			}
			else if (!View.btnCoinAdd.enabled)
			{
				View.btnCoinAdd.enabled = true;
				View.btnCoinAdd.image.material = null;
			}
		}

		private void SetBtnDiamondOfflineEnable(bool isAble)
		{
			if (isAble)
			{
				if (!View.btnDiamondOffline.enabled)
				{
					View.btnDiamondOffline.enabled = true;
				}
			}
			else if (View.btnDiamondOffline.enabled)
			{
				View.btnDiamondOffline.enabled = false;
			}
		}

		private void SetMainViewPropBtnPos()
		{
			Vector3 position = View.btnProp.transform.position;
			float x = position.x;
			Vector2 pivot = View.btnProp.GetComponent<RectTransform>().pivot;
			float x2 = x - pivot.x;
			Vector3 position2 = View.btnProp.transform.position;
			GameGlobalData.MainViewPropBtnPos = new Vector2(x2, position2.y);
		}
	}
}
