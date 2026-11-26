using DG.Tweening;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	public class ResultViewMediator : Mediator, IMediator
	{
		private enum Type
		{
			None,
			Continue,
			DiamondContinue,
			Result,
			Pass,
			Womman
		}

		private const int _timeContinue = 9;

		private const int _showAgainTime = 6;

		private int _time;

		private bool _isPlayCoin;

		private bool _isClickSlots;

		private Sequence _sequence;

		private Timer _timer;

		private IEffectAction _somkeEffect;

		private ImpactWaveEffect _impactWaveEffect;

		private Tweener _coinTweener;

		private Type _curType;

		private bool _isGetDiamond;

		private int _diamond;

		private ResultView View => (ResultView)ViewComponent;

		public ResultViewMediator(string name, ResultView viewComponent)
			: base(name, viewComponent)
		{
		}

		public override void OnRegister()
		{
			base.OnRegister();
			View.btnContinue.onClick.AddListener(OnClickContinue);
			View.btnDiamondContinue.onClick.AddListener(ClickDiamondContinue);
			View.btnAgain.onClick.AddListener(OnClickAgain);
			View.btnGetReward.onClick.AddListener(OnClickReward);
			View.btnGetAdReward.onClick.AddListener(OnClickAdReward);
			View.btnSlots.onClick.AddListener(OnClickSlots);
			View.btnGetDiamond.onClick.AddListener(OnClickGetDiamond);
			View.btnJumpLuckyBox.onClick.AddListener(OnClickJumpLuckyBox);
			_isPlayCoin = false;
			_isClickSlots = false;
			_isGetDiamond = false;
			_somkeEffect = new SomkeEffect();
			_impactWaveEffect = new ImpactWaveEffect();
			GameGlobalData.CoinRecoverTopLeftPos = View.tranImgCoin.position;
			View.txtNoThanks.text = LanguageConfig.NO_THANKS;
			View.txtContinue.text = LanguageConfig.REVIVE;
			View.txtDiamondContinue.text = LanguageConfig.REVIVE;
			View.txtResultTitle.text = LanguageConfig.RESULT_TITLE;
			View.txtRewardCollect.text = LanguageConfig.COLLECT;
			View.TxtFree.text = LanguageConfig.FREE;
		}

		public override void OnRemove()
		{
			base.OnRemove();
		}

		public override IList<string> ListNotificationInterests()
		{
			List<string> list = new List<string>();
			list.Add("CONTINUE_INIT");
			list.Add("DIAMOND_CONTINUE_INIT");
			list.Add("RESULT_INIT");
			list.Add("PASS_INIT");
			list.Add("DIE_INIT");
			list.Add("CONTINUE_INIT_NOAD");
			list.Add("SET_COIN");
			list.Add("SET_DIAMOND");
			list.Add("BATTLE_VIEW_GET_COIN_ANI");
			list.Add("RESULT_VIEW_HIDE");
			return list;
		}

		public override void HandleNotification(INotification notification)
		{
			switch (notification.Name)
			{
			case "CONTINUE_INIT":
				SetTranActive(Type.Continue);
				HandleContinue();
				SetCurDiamond();
				View.objMark.gameObject.SetActive(value: false);
				View.tranDiamond.gameObject.SetActive(value: false);
				break;
			case "DIAMOND_CONTINUE_INIT":
				SetTranActive(Type.DiamondContinue);
				HandleContinue();
				View.objMark.gameObject.SetActive(value: false);
				View.tranDiamond.gameObject.SetActive(value: false);
				break;
			case "CONTINUE_INIT_NOAD":
				SetTranActive(Type.DiamondContinue, isLoadAd: false);
				HandleContinueNoAdLoad();
				View.objMark.gameObject.SetActive(value: false);
				View.tranDiamond.gameObject.SetActive(value: false);
				break;
			case "RESULT_INIT":
				_isClickSlots = false;
				HideRewardBtn(isActive: true);
				SetNextLevelUnLock(isActive: false);
				SetTranActive(Type.Pass);
				SetRewardText();
				SetCoin();
				SetTips();
				SetSlotsProgress();
				SetSlotsBtnState();
				GetDiamonStorageNum();
				SetResultDiamondStorage();
				SetCurDiamond();
				View.objMark.gameObject.SetActive(value: false);
				View.tranDiamond.gameObject.SetActive(value: true);
				break;
			case "PASS_INIT":
				_isClickSlots = false;
				HideRewardBtn(isActive: true);
				SetNextLevelUnLock(isActive: true);
				SetTranActive(Type.Pass);
				SetRewardText();
				SetCoin();
				SetTips();
				SetSlotsProgress();
				SetSlotsBtnState();
				GetDiamonStorageNum();
				SetResultDiamondStorage();
				SetCurDiamond();
				View.objMark.gameObject.SetActive(value: false);
				View.tranDiamond.gameObject.SetActive(value: true);
				break;
			case "SET_COIN":
				SetCoin();
				break;
			case "SET_DIAMOND":
				SetCurDiamond();
				break;
			case "DIE_INIT":
				OnClickAgain();
				break;
			case "BATTLE_VIEW_GET_COIN_ANI":
				CoinGetAni();
				break;
			case "RESULT_VIEW_HIDE":
			{
				if (GameGlobalData.ActualLevel < 16)
				{
					break;
				}
				if (_isGetDiamond)
				{
					_isGetDiamond = false;
					break;
				}
				View.objMark.gameObject.SetActive(value: true);
				Transform layerTran = Singleton<UIViewManager>.Instance.GetLayerTran(110);
				if (Singleton<DiamondStorageManager>.Instance.GetProcess() < 1f)
				{
					Singleton<CoinManager>.Instance.FloatByCircle<FloatDiamond>("GameObject/Item/FloatDiamond", 20, Vector3.zero, View.btnJumpLuckyBox.transform.position, 0.25f, delegate
					{
						Singleton<DiamondStorageManager>.Instance.AddStorageDiamond(_diamond);
					}, layerTran);
				}
				break;
			}
			}
		}

		private void HandleContinueNoAdLoad()
		{
			View.btnAgain.enabled = true;
			View.cGAgain.alpha = 1f;
			SetCoin();
		}

		private void HandleContinue()
		{
			_time = 9;
			View.cGAgain.DOKill();
			View.btnAgain.enabled = false;
			View.cGAgain.alpha = 0f;
			SetTime();
			CountDownNew();
			SetCoin();
		}

		private void GetDiamonStorageNum()
		{
			_diamond = Singleton<DiamondStorageManager>.Instance.RandomDiamond();
			View.TxtCost.text = "x" + _diamond.ToString();
		}

		private void SetCurDiamond()
		{
			View.txtCurDiamond.text = GameUtils.FloatToUnit(GameGlobalData.CurDiamond);
		}

		private void SetDiamonADSActive(bool isAdLoad)
		{
			View.btnGetDiamond.gameObject.SetActive(isAdLoad);
			View.ImgDiamond.gameObject.SetActive(isAdLoad);
			View.ImgAds.gameObject.SetActive(isAdLoad);
			View.TxtCost.gameObject.SetActive(isAdLoad);
			View.TxtFree.gameObject.SetActive(isAdLoad);
		}

		private void SetNextLevelUnLock(bool isActive)
		{
			if (isActive)
			{
				View.tranUnLock.gameObject.SetActive(value: true);
				for (int i = 0; i < View.unLockItems.Length; i++)
				{
					View.unLockItems[i].gameObject.SetActive(value: false);
				}
				WeaponStruct[] weaponJson = Singleton<GameJson>.Instance.WeaponJson;
				for (int j = 0; j < weaponJson.Length; j++)
				{
					if (GameGlobalData.ActualLevel <= weaponJson[j].UnLockLevel)
					{
						View.unLockItems[j].gameObject.SetActive(value: true);
						View.unLockItems[j].objLock.SetActive(value: true);
						View.unLockItems[j].objUnLock.SetActive(value: false);
						View.txtUnLockContent.color = Color.white;
						View.txtUnLockContent.text = string.Format(LanguageConfig.unlock_content, weaponJson[j].UnLockLevel);
						return;
					}
					if (GameGlobalData.ActualLevel == weaponJson[j].UnLockLevel + 1)
					{
						View.unLockItems[j].gameObject.SetActive(value: true);
						View.unLockItems[j].objLock.SetActive(value: false);
						View.unLockItems[j].objUnLock.SetActive(value: true);
						View.txtUnLockContent.color = Color.yellow;
						View.txtUnLockContent.text = LanguageConfig.unlock_success;
						return;
					}
				}
				View.tranUnLock.gameObject.SetActive(value: false);
			}
			else
			{
				View.tranUnLock.gameObject.SetActive(value: false);
			}
		}

		private void SetResultDiamondStorage()
		{
			if (GameGlobalData.ActualLevel >= 16)
			{
				bool flag = Singleton<AdManager>.Instance.IsLoad("0a2cad70-9e1c-450f-bcdc-f3798b324b95", "OnClickGetResultDiamond");
				View.tranDiamondStorage.gameObject.SetActive(value: true);
				SetDiamonADSActive(flag);
				if (flag)
				{
					float num = Mathf.FloorToInt(Singleton<DiamondStorageManager>.Instance.GetProcess() * 100f);
					if (num >= 100f)
					{
						View.TxtFull.gameObject.SetActive(value: true);
						View.txtLuckyBoxProgress.gameObject.SetActive(value: false);
						View.fullImgDiamond.gameObject.SetActive(value: false);
						View.TxtFull.text = LanguageConfig.FULL;
					}
					else
					{
						View.TxtFull.gameObject.SetActive(value: false);
						View.txtLuckyBoxProgress.gameObject.SetActive(value: true);
						View.fullImgDiamond.gameObject.SetActive(value: true);
						View.txtLuckyBoxProgress.text = Singleton<DiamondStorageManager>.Instance.GetTotalDiamond().ToString();
					}
				}
				else
				{
					float num2 = Mathf.FloorToInt(Singleton<DiamondStorageManager>.Instance.GetProcess() * 100f);
					if (num2 >= 100f)
					{
						View.TxtFull.gameObject.SetActive(value: true);
						View.txtLuckyBoxProgress.gameObject.SetActive(value: false);
						View.fullImgDiamond.gameObject.SetActive(value: false);
						View.TxtFull.text = LanguageConfig.FULL;
					}
					else
					{
						View.TxtFull.gameObject.SetActive(value: false);
						View.txtLuckyBoxProgress.gameObject.SetActive(value: true);
						View.fullImgDiamond.gameObject.SetActive(value: true);
						View.txtLuckyBoxProgress.text = Singleton<DiamondStorageManager>.Instance.GetTotalDiamond().ToString();
					}
				}
			}
			else
			{
				View.tranDiamondStorage.gameObject.SetActive(value: false);
			}
		}

		private void SetTranActive(Type type, bool isLoadAd = true)
		{
			_curType = type;
			switch (type)
			{
			case Type.Continue:
			{
				View.btnContinue.transform.localScale = Vector3.one;
				View.animatorContinue.enabled = true;
				Transform transform4 = View.btnContinue.transform;
				Vector3 localPosition4 = View.btnContinue.transform.localPosition;
				transform4.localPosition = new Vector3(0f, localPosition4.y);
				View.tranContinue.gameObject.SetActive(value: true);
				View.tranPass.gameObject.SetActive(value: false);
				View.btnDiamondContinue.gameObject.SetActive(value: false);
				View.btnContinue.gameObject.SetActive(value: true);
				break;
			}
			case Type.DiamondContinue:
				if (isLoadAd)
				{
					Transform transform = View.btnContinue.transform;
					Vector3 localPosition = View.btnContinue.transform.localPosition;
					transform.localPosition = new Vector3(210f, localPosition.y);
					Transform transform2 = View.btnDiamondContinue.transform;
					Vector3 localPosition2 = View.btnDiamondContinue.transform.localPosition;
					transform2.localPosition = new Vector3(-200f, localPosition2.y);
				}
				else
				{
					Transform transform3 = View.btnDiamondContinue.transform;
					Vector3 localPosition3 = View.btnDiamondContinue.transform.localPosition;
					transform3.localPosition = new Vector3(0f, localPosition3.y);
				}
				View.btnDiamondContinue.gameObject.SetActive(value: true);
				View.txtDiamond.text = Singleton<DiamondManager>.Instance.CalCostDiamondRetry().ToString();
				View.animatorContinue.enabled = true;
				View.btnContinue.transform.localScale = Vector3.one;
				View.btnContinue.gameObject.SetActive(isLoadAd);
				View.btnDiamondContinue.gameObject.SetActive(value: true);
				View.tranContinue.gameObject.SetActive(value: true);
				View.tranPass.gameObject.SetActive(value: false);
				break;
			case Type.Pass:
				View.tranContinue.gameObject.SetActive(value: false);
				View.tranPass.gameObject.SetActive(value: true);
				break;
			case Type.Result:
				View.tranContinue.gameObject.SetActive(value: false);
				View.tranPass.gameObject.SetActive(value: false);
				break;
			case Type.Womman:
				View.tranContinue.gameObject.SetActive(value: false);
				View.tranPass.gameObject.SetActive(value: false);
				break;
			}
		}

		private void SetCoin()
		{
			View.txtCoin.text = GameUtils.FloatToUnit(GameGlobalData.CurCoin);
		}

		private void SetTime()
		{
			View.txtTime.text = _time.ToString();
		}

		private void SetTips()
		{
			int num = Random.Range(0, LanguageConfig.TIPS.Length);
			View.txtTips.text = LanguageConfig.TIPS[num];
			RectTransform component = View.txtTipsImg.gameObject.GetComponent<RectTransform>();
			Rect rect = component.rect;
			Vector2 sizeDelta = View.txtTipsImg.gameObject.GetComponent<RectTransform>().sizeDelta;
			sizeDelta = new Vector2(View.txtTips.preferredWidth + 100f, sizeDelta.y);
		}

		private void SetRewardText()
		{
			View.txtReward.text = GameUtils.FloatToUnit(GameGlobalData.CurGetCoin);
			if (GameGlobalData.CurGetCoin > 0f)
			{
				bool active = Singleton<AdManager>.Instance.IsLoad("0a2cad70-9e1c-450f-bcdc-f3798b324b95", "SetRewardText");
				View.btnGetAdReward.gameObject.SetActive(active);
				bool vipStatus = Singleton<VipManager>.Instance.VipStatus;
				View.txtAdRate.text = ((!vipStatus) ? "x3" : "x4");
				View.txtAdReward.text = GameUtils.FloatToUnit((!vipStatus) ? (GameGlobalData.CurGetCoin * 3f) : (GameGlobalData.CurGetCoin * 4f));
			}
			else
			{
				View.btnGetAdReward.gameObject.SetActive(value: false);
			}
		}

		private void CountDownNew()
		{
			_timer = Timer.Register(1f, delegate
			{
				_time--;
				if (_time < 0)
				{
					_timer.Cancel();
					if (_curType == Type.DiamondContinue)
					{
						View.animatorContinue.enabled = false;
						View.btnContinue.transform.DOScale(Vector3.zero, 0.2f).OnComplete(delegate
						{
							View.btnDiamondContinue.transform.DOLocalMoveX(0f, 0.3f);
						});
					}
					else
					{
						OnClickAgain();
					}
				}
				else
				{
					SetTime();
					if (_time == 6)
					{
						View.btnAgain.enabled = true;
						View.cGAgain.DOFade(1f, 0.8f);
					}
				}
			}, null, isLooped: true);
		}

		private void SetSlotsProgress()
		{
			float fillAmount = (float)GameGlobalData.SlotProgress / 5f;
			View.imgSlotsProgress.fillAmount = fillAmount;
		}

		private void SetSlotsBtnState()
		{
			bool flag = Singleton<AdManager>.Instance.IsLoad("0a2cad70-9e1c-450f-bcdc-f3798b324b95", "SetSlotsBtnState");
			if (GameGlobalData.SlotProgress >= 5 && flag && GameGlobalData.CurGetCoin > 0f)
			{
				View.btnSlots.gameObject.SetActive(value: true);
				View.btnGetAdReward.gameObject.SetActive(value: false);
				bool vipStatus = Singleton<VipManager>.Instance.VipStatus;
				View.textSlotsRate.text = ((!vipStatus) ? "x6" : "x8");
				View.textSlots.text = GameUtils.FloatToUnit((!vipStatus) ? (GameGlobalData.CurGetCoin * 6f) : (GameGlobalData.CurGetCoin * 8f));
			}
			else
			{
				View.btnSlots.gameObject.SetActive(value: false);
			}
		}

		private void HideRewardBtn(bool isActive)
		{
			View.txtTitle.gameObject.SetActive(isActive);
			View.btnGetReward.gameObject.SetActive(isActive);
			View.btnGetAdReward.gameObject.SetActive(isActive);
			View.btnSlots.gameObject.SetActive(isActive);
			SetNextLevelUnLock(isActive: false);
		}

		private void OnClickReward()
		{
			if (!_isPlayCoin)
			{
				View.tranDiamond.gameObject.SetActive(value: true);
				_isPlayCoin = true;
				SendNotification("RESULT_VIEW_HIDE");
				CoinAnim(GameGlobalData.CurGetCoin);
				SendNotification("BATTLE_EXIT_GAME");
				HideRewardBtn(isActive: false);
				float duration = (GameGlobalData.CurGetCoin != 0f) ? 1.5f : 0.5f;
				Timer.Register(duration, delegate
				{
					Singleton<AnalyzeSdk>.Instance.OnEventByArgs("Click", new string[1]
					{
						"type@结算获取金币"
					});
					Singleton<UIViewManager>.Instance.HideView("RESULT_VIEW");
					_isPlayCoin = false;
				});
			}
		}

		private void OnClickAdReward()
		{
			if (!_isPlayCoin)
			{
				View.tranDiamond.gameObject.SetActive(value: true);
				Singleton<AnalyzeSdk>.Instance.OnEventByArgs("Click", new string[1]
				{
					"type@结算看视频获取金币"
				});
				Singleton<AdManager>.Instance.ShowVideo("0a2cad70-9e1c-450f-bcdc-f3798b324b95", "getgold", AdRewardCallback);
			}
		}

		private void OnClickSlots()
		{
			if (!_isClickSlots)
			{
				View.tranDiamond.gameObject.SetActive(value: true);
				Singleton<AnalyzeSdk>.Instance.OnEventByArgs("Click", new string[1]
				{
					"type@点击幸运大奖"
				});
				_isClickSlots = true;
				Singleton<AdManager>.Instance.ShowVideo("0a2cad70-9e1c-450f-bcdc-f3798b324b95", "luckyReward", AdSlotsRollCallback);
			}
		}

		private void OnClickGetDiamond()
		{
			Singleton<AdManager>.Instance.ShowVideo("0a2cad70-9e1c-450f-bcdc-f3798b324b95", "resultdiamond", AdGetDiamondCallback);
		}

		private void AdGetDiamondCallback(bool isReward)
		{
			if (isReward)
			{
				_isGetDiamond = true;
				View.tranDiamondStorage.gameObject.SetActive(value: false);
				Transform layerTran = Singleton<UIViewManager>.Instance.GetLayerTran(110);
				Singleton<CoinManager>.Instance.FloatByCircle<FloatDiamond>("GameObject/Item/FloatDiamond", 20, Vector3.zero, View.tranDiamondIcon.position, 0.25f, delegate
				{
					Singleton<DiamondStorageManager>.Instance.ShowVideoAndClean(_diamond);
				}, layerTran);
				Singleton<AudioManager>.Instance.PlaySound("coin", 0.8f, 4, 0.2f);
			}
		}

		private void OnClickJumpLuckyBox()
		{
			Singleton<UIViewManager>.Instance.ShowView("LUCKYBOX_VIEW", "LUCKYBOX_INIT");
		}

		private void AdSlotsRollCallback(bool isReward)
		{
			if (isReward)
			{
				SendNotification("RESULT_VIEW_HIDE");
				CoinAnim((!Singleton<VipManager>.Instance.VipStatus) ? (GameGlobalData.CurGetCoin * 6f) : (GameGlobalData.CurGetCoin * 8f));
				for (int i = 1; i < 4; i++)
				{
					Timer.Register((float)i * 0.2f, delegate
					{
						CoinAnim();
					});
				}
				HideRewardBtn(isActive: false);
				Timer.Register(1.5f, delegate
				{
					List<string> list = UmConfig.SetCommonParam();
					list.Add("type@结算界面获取金币");
					Singleton<AnalyzeSdk>.Instance.OnEventByArgs("ADSVideoReward", list.ToArray());
					GameAnalysisData.SendShowVedio("getgold");
					GameAnalysisData.GetGold((long)GameGlobalData.CurGetCoin, "resultX6");
					GameGlobalData.AddCurCoin(GameGlobalData.CurGetCoin);
					Singleton<UIViewManager>.Instance.HideView("RESULT_VIEW");
					_isPlayCoin = false;
				});
				GameGlobalData.SetSlotProgress(0);
				SendNotification("BATTLE_EXIT_GAME");
				Singleton<DailyTaskHandle>.Instance.AddProgress(DailyTaskType.CoinReward_3, 1);
			}
			else
			{
				_isPlayCoin = false;
				View.btnGetAdReward.gameObject.SetActive(value: false);
				View.objMark.gameObject.SetActive(value: false);
			}
			_isClickSlots = false;
		}

		private void CoinAnim()
		{
			int num = 20;
			float num2 = 360f / (float)num;
			Transform layerTran = Singleton<UIViewManager>.Instance.GetLayerTran(40);
			for (int i = 0; i < num; i++)
			{
				FloatGold floatGold = Singleton<GameItemManager>.Instance.Spawn<FloatGold>("GameObject/Item/FloatCoin", "GameObject/Item/FloatCoin", layerTran);
				floatGold.SetInit();
				float preMoveCircleRadius = Random.Range(2.5f, 4.3f);
				floatGold.Moving(Vector2.zero, num2 * (float)i, 0.3f, GameGlobalData.CoinRecoverTopLeftPos, preMoveCircleRadius, 0.25f);
			}
			Timer.Register(0.8f, delegate
			{
				for (int j = 0; j < 4; j++)
				{
					Timer.Register((float)j * 0.2f, delegate
					{
						Singleton<AudioManager>.Instance.PlaySound("coin");
					});
				}
			});
		}

		private void AdRewardCallback(bool isReward)
		{
			if (isReward)
			{
				SendNotification("RESULT_VIEW_HIDE");
				CoinAnim((!Singleton<VipManager>.Instance.VipStatus) ? (GameGlobalData.CurGetCoin * 3f) : (GameGlobalData.CurGetCoin * 4f));
				for (int i = 1; i < 3; i++)
				{
					Timer.Register((float)i * 0.2f, delegate
					{
						CoinAnim();
					});
				}
				HideRewardBtn(isActive: false);
				Timer.Register(1.5f, delegate
				{
					List<string> list = UmConfig.SetCommonParam();
					list.Add("type@结算界面获取金币");
					Singleton<AnalyzeSdk>.Instance.OnEventByArgs("ADSVideoReward", list.ToArray());
					GameAnalysisData.SendShowVedio("getgold");
					GameAnalysisData.GetGold((long)GameGlobalData.CurGetCoin, "resultX3");
					GameGlobalData.AddCurCoin(GameGlobalData.CurGetCoin);
					Singleton<UIViewManager>.Instance.HideView("RESULT_VIEW");
					_isPlayCoin = false;
				});
				SendNotification("BATTLE_EXIT_GAME");
				Singleton<DailyTaskHandle>.Instance.AddProgress(DailyTaskType.CoinReward_3, 1);
			}
			else
			{
				_isPlayCoin = false;
				View.btnGetAdReward.gameObject.SetActive(value: false);
				View.objMark.gameObject.SetActive(value: false);
			}
		}

		private void OnClickContinue()
		{
			Singleton<AnalyzeSdk>.Instance.OnEventByArgs("Click", new string[1]
			{
				"type@复活"
			});
			_timer.Cancel();
			Singleton<AdManager>.Instance.ShowVideo("0a2cad70-9e1c-450f-bcdc-f3798b324b95", "retry", AdContinueCallback);
		}

		private void ClickDiamondContinue()
		{
			if (Singleton<DiamondManager>.Instance.IsEnough(Singleton<DiamondManager>.Instance.CalCostDiamondRetry()))
			{
				Singleton<DiamondManager>.Instance.SetUseDiamondRetry();
				Singleton<UIViewManager>.Instance.HideView("RESULT_VIEW");
				ApplicationFacade.Instance.SendNotification("BATTLE_ROLE_REVIVE");
				ApplicationFacade.Instance.SendNotification("GAME_CONTINUE", 0);
				_impactWaveEffect.Spawn(Singleton<PlayerManager>.Instance.GetLeader().localPosition);
				Singleton<DailyTaskHandle>.Instance.AddProgress(DailyTaskType.Revive_3, 1);
			}
			else
			{
				Singleton<UIViewManager>.Instance.ShowView("SHOP_VIEW", "SHOP_INIT");
			}
		}

		private void AdContinueCallback(bool isReward)
		{
			if (isReward)
			{
				List<string> list = UmConfig.SetCommonParam();
				list.Add("type@复活");
				Singleton<AnalyzeSdk>.Instance.OnEventByArgs("ADSVideoReward", list.ToArray());
				GameAnalysisData.SendShowVedio("retry");
				Singleton<UIViewManager>.Instance.HideView("RESULT_VIEW");
				ApplicationFacade.Instance.SendNotification("BATTLE_ROLE_REVIVE");
				ApplicationFacade.Instance.SendNotification("GAME_CONTINUE", 0);
				_impactWaveEffect.Spawn(Singleton<PlayerManager>.Instance.GetLeader().localPosition);
				Singleton<DailyTaskHandle>.Instance.AddProgress(DailyTaskType.Revive_3, 1);
			}
			else
			{
				OnClickAgain();
			}
		}

		private void OnClickAgain()
		{
			// 广告/暂停可能把时间留在 0 或 0.1，下面的抖动动画走 scaled time，不恢复就会卡住
			Time.timeScale = 1f;
			if (_timer != null && !_timer.isCancelled)
			{
				_timer.Cancel();
			}
			SetTranActive(Type.Result);
			Singleton<UIViewManager>.Instance.LayerShake(1, 0.8f, Vector2.up * 10f, 15, 10f, snapping: false, fadeOut: false, delegate
			{
				SendNotification("GAME_RESULT");
			});
			Singleton<AudioManager>.Instance.PlaySound("die");
			Singleton<PlayerManager>.Instance.LeaderDie();
			_somkeEffect.Spawn(Singleton<PlayerManager>.Instance.GetLeader().localPosition);
		}

		private void CoinAnim(float coinCount)
		{
			if (GameGlobalData.CurGetCoin > 0f)
			{
				int num = 20;
				float num2 = 360f / (float)num;
				for (int i = 0; i < num; i++)
				{
					FloatGold floatGold = Singleton<GameItemManager>.Instance.Spawn<FloatGold>("GameObject/Item/FloatCoin", "GameObject/Item/FloatCoin");
					floatGold.SetInit();
					float distance = Random.Range(0.5f, 0.75f);
					float preMoveCircleRadius = Random.Range(2.5f, 4.3f);
					floatGold.Moving(Vector2.zero, num2 * (float)i, distance, GameGlobalData.CoinRecoverTopLeftPos, preMoveCircleRadius, 0.25f);
				}
				Timer.Register(0.8f, delegate
				{
					GameAnalysisData.GetGold((long)coinCount, "result");
					GameGlobalData.AddCurCoin(coinCount);
					for (int j = 0; j < 4; j++)
					{
						Timer.Register((float)j * 0.2f, delegate
						{
							Singleton<AudioManager>.Instance.PlaySound("coin");
						});
					}
				});
			}
			SendNotification("BATTLE_HIDE");
		}

		private void CoinGetAni()
		{
			if (_coinTweener == null || !_coinTweener.IsPlaying())
			{
				_coinTweener = View.tranImgCoin.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.05f).SetLoops(2, LoopType.Yoyo);
			}
		}
	}
}
