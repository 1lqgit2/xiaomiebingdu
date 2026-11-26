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
	public class BattleViewMediator : Mediator, IMediator
	{
		private float _curTimeScale;

		private Timer _timer;

		private const string LEVEL_OUT = "anim_battleview_level_out";

		private const string LEVEL_IN = "anim_battleview_level_in";

		private const string LEVEL_RECOVER = "anim_battleview_level_recover";

		private const string LEVEL_NEXT = "anim_battleview_level_nextlevel";

		private const string LEVEL_PROGRESS_IN = "anim_battleview_progress_in";

		private const string LEVEL_PROGRESS_OUT = "anim_battleview_progress_out";

		private const string LEVEL_PROGRESS_MOVE = "anim_battleview_progress_move";

		private const string SHAKE = "zhendong-01";

		private const string UNSHAKE = "zhendong-02";

		private Tweener _coinTweener;

		private const string ANIM_BG_OUT = "anim_battleview_bg_out";

		private const string ANIM_BG_IN = "anim_battleview_bg_in";

		private const string ANIM_EXIT_GAME = "anim_battleview_level_exit_to_main";

		private const string ANIM_NEXT_LEVEL_EXIT_GAME = "anim_battleview_level_nextleveltomain";

		private const string ANIM_ENTRY_MAIN = "anim_battleview_level_entry_to_main";

		private bool _crossLevel;

		private bool _isMark;

		private int _useBattleItemTimes;

		// 设置按钮弹插屏：连点保护 5 秒，成功展示后 60 秒内不再弹（抖音侧另硬性限制 30 秒间隔）
		private const float SETTING_INTER_AD_REQUEST_GAP = 5f;

		private const float SETTING_INTER_AD_INTERVAL = 60f;

		private float _lastSettingInterAdRequest = -999f;

		private float _lastSettingInterAdShown = -999f;

		private BattleView View => (BattleView)ViewComponent;

		public BattleViewMediator(string name, BattleView viewComponent)
			: base(name, viewComponent)
		{
		}

		public override void OnRegister()
		{
			base.OnRegister();
			View.btnSound.onClick.AddListener(OnClickSound);
			View.btnNoSound.onClick.AddListener(OnClickSound);
			View.btnShake.onClick.AddListener(OnClickShake);
			View.btnSetting.onClick.AddListener(ClickSetting);
			View.btnSettingClose.onClick.AddListener(ClickSettingClose);
			View.btnShare.onClick.AddListener(ClickSettingShare);
			View.btnShareFb.onClick.AddListener(ClickShareFb);
			View.btnShareMessage.onClick.AddListener(ClickShareMessage);
			View.BattleItemBgClose.ActionPointerDown = ClickCloseBgClose;
			View.BattleItemBgClose.ActionDrag = ClickCloseBgClose;
			for (int i = 0; i < 5; i++)
			{
				int index = i;
				View.BattleItemButtons[i].onClick.AddListener(delegate
				{
					ClickBattleItem(index);
				});
				View.BattleItemButtonsCustomEventHandler[i].ActionDrag = ClickCloseBgClose;
			}
			GameGlobalData.CoinRecoverPos = View.tranCoin.position;
			View.FinalTxt.text = LanguageConfig.FINAL_WAVE;
			View.txtBossTip.text = LanguageConfig.BOSS_TIP;
			View.shareTxt.text = LanguageConfig.Share;
		}

		public override void OnRemove()
		{
			base.OnRemove();
		}

		public override IList<string> ListNotificationInterests()
		{
			List<string> list = new List<string>();
			list.Add("BATTLE_INIT");
			list.Add("BATTLE_START_GAME");
			list.Add("BATTLE_SET_PROGRESS");
			list.Add("BATTLE_END_GAME");
			list.Add("BATTLE_EXIT_GAME");
			list.Add("BATTLE_PASS_GAME");
			list.Add("BATTLE_MARK");
			list.Add("BATTLE_HIDE");
			list.Add("SHARE_FAIL");
			list.Add("SHARE_SUCCESS");
			list.Add("SET_TEMPCOIN");
			list.Add("BATTLE_GAIN_ITEM");
			list.Add("BATTLE_BOSSTIPS");
			list.Add("BATTLE_VIEW_GET_COIN_ANI");
			list.Add("BATTLE_ROLE_BEHIT");
			list.Add("BATTLE_ROLE_REVIVE");
			list.Add("BATTLE_LEVEL_RESULT_MOVEDOWN");
			list.Add("BATTLE_ITEM_STATE_TIMEOUT");
			list.Add("BATTLE_ITEM_STATE_START");
			list.Add("BATTLE_WALL_ACTIVE");
			return list;
		}

		public override void HandleNotification(INotification notification)
		{
			switch (notification.Name)
			{
			case "BATTLE_INIT":
				View.tranWall.gameObject.SetActive(value: false);
				View.animWidgetLevel.Play("anim_battleview_level_recover");
				SetLevel(GameGlobalData.CurLevel);
				SetProgress(0f);
				SetSound();
				SetCurCoint();
				SetShake();
				SetShareSetting();
				SetBattleItemPanel();
				ResetBattleItemUseTimes();
				GiveProps();
				Timer.Register(0.1f, delegate
				{
					View.animWidgetLevel.Play("anim_battleview_level_entry_to_main");
				});
				break;
			case "BATTLE_START_GAME":
				View.animWidgetLevel.Play("anim_battleview_level_out", delegate
				{
					View.animWidgetProgress.Play("anim_battleview_progress_in");
				});
				ResetAllBattleItemIcon();
				break;
			case "BATTLE_SET_PROGRESS":
				SetProgress(GameGlobalData.GetCurHitCountPro());
				break;
			case "BATTLE_END_GAME":
				View.animWidgetLevel.Play("anim_battleview_level_in");
				View.animWidgetProgress.Play("anim_battleview_progress_move", delegate
				{
					SendNotification("RESULT_INIT");
				});
				View.imgMark_1.DOFade(0f, 0.1f).SetAutoKill(autoKillOnCompletion: true);
				View.imgMark_0.DOFade(0f, 0.1f).SetAutoKill(autoKillOnCompletion: true);
				View.TranBattleItem.localScale = Vector3.zero;
				break;
			case "BATTLE_EXIT_GAME":
				// 回主界面前强制恢复时间缩放：广告播放、战斗暂停都可能把 timeScale 留在 0，
				// 而下面的计时器如果用 scaled time 会永远不触发，表现为“点了领取/再来一局没反应”。
				Time.timeScale = 1f;
				Timer.Register(1.5f, delegate
				{
					View.animWidgetProgress.transform.localPosition -= Vector3.one * 3000f;
					SendNotification("GAME_MAIN");
				}, null, false, true);
				SetBackGroundAnim(isActive: false);
				exitGameAnim();
				Singleton<GameItemManager>.Instance.CleanAllItems();
				Singleton<BallManger>.Instance.Clean();
				break;
			case "BATTLE_PASS_GAME":
				View.imgMark_1.DOFade(0.7f, 0.5f).SetAutoKill(autoKillOnCompletion: true);
				SetPlayerOff();
				View.animWidgetProgress.Play("anim_battleview_progress_out");
				View.animWidgetLevel.Play("anim_battleview_level_in", delegate
				{
					Timer.Register(1f, delegate
					{
						View.animWidgetLevel.Play("anim_battleview_level_nextlevel", delegate
						{
							Singleton<UIViewManager>.Instance.ShowView("RESULT_VIEW", "PASS_INIT");
							SendNotification("DIALOGUE_CHECK");
						});
					});
				});
				_crossLevel = true;
				break;
			case "SET_TEMPCOIN":
				SetCurCoint();
				break;
			case "BATTLE_MARK":
			{
				_isMark = (bool)notification.Body;
				float endValue = _isMark ? 1 : 0;
				if (_isMark && View.TranBattleItem.localScale == Vector3.zero && GameGlobalData.ActualLevel > 19)
				{
					View.TranBattleItem.localScale = Vector3.one;
				}
				View.imgMark_0.DOFade(endValue, 0.5f).SetAutoKill(autoKillOnCompletion: true).SetUpdate(UpdateType.Normal, isIndependentUpdate: true);
				break;
			}
			case "BATTLE_GAIN_ITEM":
			{
				ItemBase item = (ItemBase)notification.Body;
				UIItemIconRenderer uIItemIconRenderer2 = UnityEngine.Object.Instantiate(Resources.Load<UIItemIconRenderer>("GameUI/BattleView/ui_battleview_itemicon"), View.uIItemIconContent.transform);
				View.uIItemIconContent.AppendChild(uIItemIconRenderer2.transform);
				uIItemIconRenderer2.SetItem(item);
				break;
			}
			case "BATTLE_ITEM_STATE_START":
			{
				BattleItemBase battleItem = (BattleItemBase)notification.Body;
				UIItemIconRenderer uIItemIconRenderer = UnityEngine.Object.Instantiate(Resources.Load<UIItemIconRenderer>("GameUI/BattleView/ui_battleview_itemicon"), View.uIItemIconContent.transform);
				View.uIItemIconContent.AppendChild(uIItemIconRenderer.transform);
				uIItemIconRenderer.SetBattleItem(battleItem);
				break;
			}
			case "BATTLE_HIDE":
				View.imgMark_1.DOFade(0f, 0.1f).SetAutoKill(autoKillOnCompletion: true);
				View.imgMark_0.DOFade(0f, 0.1f).SetAutoKill(autoKillOnCompletion: true);
				View.TranBattleItem.localScale = Vector3.zero;
				View.animWidgetProgress.Play("anim_battleview_progress_out", delegate
				{
					View.animWidgetProgress.transform.localPosition += Vector3.one * 3000f;
				});
				break;
			case "BATTLE_BOSSTIPS":
				SetBossActive(isActive: true, (bool)notification.Body);
				break;
			case "BATTLE_VIEW_GET_COIN_ANI":
				CoinGetAni();
				break;
			case "BATTLE_ROLE_BEHIT":
				SetBackGroundAnim(isActive: true);
				break;
			case "BATTLE_ROLE_REVIVE":
				SetBackGroundAnim(isActive: false);
				break;
			case "SHARE_FAIL":
				ShareReviveSuccessCallBack(isSuccess: false);
				break;
			case "SHARE_SUCCESS":
				ShareReviveSuccessCallBack(isSuccess: true);
				break;
			case "BATTLE_LEVEL_RESULT_MOVEDOWN":
			{
				RectTransform rectTransform = View.tranLevel.parent as RectTransform;
				RectTransform target = rectTransform;
				Vector2 anchoredPosition = rectTransform.anchoredPosition;
				target.DOAnchorPosY(anchoredPosition.y - 100f, 0.3f);
				break;
			}
			case "BATTLE_ITEM_STATE_TIMEOUT":
				SetBattleItemIconState((int)notification.Body - 1);
				break;
			case "BATTLE_WALL_ACTIVE":
				View.tranWall.gameObject.SetActive(value: true);
				break;
			}
		}

		private void SetBackGroundAnim(bool isActive)
		{
			Animator component = View.Root.transform.GetComponent<Animator>();
			if (isActive)
			{
				component.Play("anim_battleview_bg_in");
			}
			else
			{
				component.Play("anim_battleview_bg_out");
			}
		}

		private void SetBossActive(bool isActive, bool isboss)
		{
			int loops = 2;
			if (isboss)
			{
				loops = 5;
				View.bossImg.gameObject.SetActive(value: true);
				View.bossTipsBg.gameObject.SetActive(value: true);
			}
			else
			{
				View.FinalTxt.gameObject.SetActive(value: true);
			}
			if (isboss)
			{
				Singleton<AudioManager>.Instance.PlaySpeciSound("boss");
				View.bossImg.DOFade(0f, 1f).SetDelay(0.5f).SetEase(Ease.Linear)
					.SetLoops(loops, LoopType.Yoyo)
					.OnComplete(delegate
					{
					});
				View.bossTipsBg.DOFade(0f, 1f).SetDelay(0.5f).SetEase(Ease.Linear)
					.SetLoops(loops, LoopType.Yoyo)
					.OnComplete(delegate
					{
						CloseBossTips();
					});
			}
			else
			{
				View.FinalTxt.DOFade(0f, 0.01f).SetDelay(1.5f).SetLoops(1, LoopType.Yoyo)
					.OnComplete(delegate
					{
						CloseBossTips();
					});
			}
		}

		private void CloseBossTips()
		{
			View.bossImg.DOKill();
			View.bossImg.color = Color.white;
			View.bossImg.gameObject.SetActive(value: false);
			View.bossTipsBg.DOKill();
			View.bossTipsBg.color = Color.white;
			View.bossTipsBg.gameObject.SetActive(value: false);
			View.FinalTxt.DOKill();
			View.FinalTxt.color = Color.white;
			View.FinalTxt.gameObject.SetActive(value: false);
			Singleton<BallManger>.Instance.Invading();
		}

		private void SetCurCoint()
		{
			View.txtCurCoin.text = GameUtils.FloatToUnit(GameGlobalData.CurGetCoin);
		}

		private void SetProgress(float progress)
		{
			View.imgProgress.localScale = new Vector3(1f - progress, 1f, 1f);
			View.txtProgress.text = string.Format(LanguageConfig.BATTLE_PROGRESS, Mathf.Ceil(Mathf.Lerp(100f, 0f, progress)));
		}

		private void SetLevel(int level)
		{
			LevelStruct[] levelJson = Singleton<GameJson>.Instance.LevelJson;
			for (int i = 0; i < View.txtLevels.Length; i++)
			{
				View.txtLevels[i].text = (level + i).ToString();
				if (level + i != 0)
				{
					LevelStruct levelStruct = levelJson[level + i - 1];
					bool flag = levelStruct.Type == 2;
					View.bossTou[i].gameObject.SetActive(flag);
					View.txtLevels[i].gameObject.SetActive(!flag);
				}
			}
			View.txtLevels[0].transform.parent.gameObject.SetActive(level != 0);
		}

		private void SetPlayerOff()
		{
			Singleton<PlayerManager>.Instance.GetLeader().DOMoveY(Singleton<UIViewManager>.Instance.screenHeightHalf + 8f, 0.45f).SetDelay(1f)
				.SetAutoKill(autoKillOnCompletion: true)
				.SetEase(Ease.InSine);
		}

		private void SetSound()
		{
			View.btnSound.gameObject.SetActive(GameGlobalData.IsMusice);
			View.btnNoSound.gameObject.SetActive(!GameGlobalData.IsMusice);
		}

		private void SetShareSetting()
		{
			bool flag = GameGlobalData.ActualLevel > 15;
			View.tranSettingTips.gameObject.SetActive(flag && !Singleton<ShareManager>.Instance.HasClickShareTips());
			View.tranShareDiamondBG.gameObject.SetActive(flag && !Singleton<ShareManager>.Instance.HasShareToday());
		}

		private void SetShake()
		{
			SpriteAtlas spriteAtlas = Resources.Load<SpriteAtlas>("GameAtlas/MainAtlas");
			View.btnShake.image.sprite = spriteAtlas.GetSprite((GameGlobalData.ShakeFlag != 0) ? "zhendong-02" : "zhendong-01");
			View.btnShake.image.SetNativeSize();
		}

		private void GiveProps()
		{
			if (GameGlobalData.ActualLevel > 19 && !GameGlobalData.BattleItemData.IsGiveProps)
			{
				GameGlobalData.BattleItemData.IsGiveProps = true;
				Singleton<BattleItemManager>.Instance.GivePropsFree();
			}
		}

		private void SetBattleItemPanel()
		{
			View.BattleItemButtonUseTitleText.text = LanguageConfig.BATTLE_ITEM_LIMIT_ITEM_TITLE;
			View.BattleUseItemHasntTitleText.text = LanguageConfig.BATTLE_ITEM_NO_ITEM_TITLE;
		}

		private void ResetBattleItemUseTimes()
		{
			_useBattleItemTimes = 0;
		}

		private void ResetAllBattleItemIcon()
		{
			bool flag = false;
			for (int i = 0; i < 5; i++)
			{
				if (SetBattleItemIconState(i))
				{
					flag = true;
				}
			}
			View.TranUseItemHasnt.gameObject.SetActive(!flag && _useBattleItemTimes < GameGlobalData.BattleItemData.BattleItemLimitTimes);
			View.TranBattleItemButtonUse.gameObject.SetActive(flag);
		}

		private bool SetBattleItemIconState(int index)
		{
			BattleItemStruct battleItemStructByIndex = Singleton<BattleItemManager>.Instance.GetBattleItemStructByIndex(index);
			int iD = battleItemStructByIndex.ID;
			bool flag = Singleton<BattleItemManager>.Instance.CheckItemStateReady(iD);
			bool flag2 = Singleton<BattleItemManager>.Instance.CheckItemNums(iD);
			bool flag3 = _useBattleItemTimes < GameGlobalData.BattleItemData.BattleItemLimitTimes;
			bool interactable = flag && flag2 && flag3;
			int ownBattleItem = Singleton<BattleItemManager>.Instance.GetOwnBattleItem(iD);
			View.BattleItemButtonsTexts[index].text = "x" + ownBattleItem;
			View.BattleItemButtons[index].gameObject.SetActive(flag2 && flag3);
			View.BattleItemButtons[index].interactable = interactable;
			View.BattleItemButtonUseNumText.text = ((!flag3) ? "<color=#ff2121>2/2</color>" : (_useBattleItemTimes + "/2"));
			return flag2;
		}

		private void ClickSetting()
		{
			if (!Singleton<ShareManager>.Instance.HasClickShareTips())
			{
				Singleton<ShareManager>.Instance.SetClickShareTips(click: true);
				View.tranSettingTips.gameObject.SetActive(!Singleton<ShareManager>.Instance.HasClickShareTips());
			}
			View.tranSettingDetail.DOKill();
			Vector3 localPosition = View.tranSettingDetail.localPosition;
			float num = (!(Math.Abs(localPosition.x) < 0.01f)) ? 0f : 800f;
			Tweener t = View.tranSettingDetail.DOLocalMoveX(num, 0.4f).SetAutoKill(autoKillOnCompletion: true).SetEase(Ease.OutQuart);
			t.SetUpdate(isIndependentUpdate: true);
			if (Math.Abs(num) < 0.01f)
			{
				View.Root.transform.SetAsLastSibling();
				View.btnSettingClose.gameObject.SetActive(value: true);
				bool flag = GameGlobalData.ActualLevel > 15;
				View.tranShare.gameObject.SetActive(flag);
				RectTransform tranSettingbg = View.tranSettingbg;
				float x = (!flag) ? 220f : 89f;
				Vector2 offsetMin = View.tranSettingbg.offsetMin;
				tranSettingbg.offsetMin = new Vector2(x, offsetMin.y);
				ShowSettingInterstitial();
			}
			else
			{
				View.Root.transform.SetAsFirstSibling();
			}
			ClickShareBgClose();
		}

		/// <summary>
		/// 打开设置面板时尝试弹一次插屏。
		/// 会跳过：VIP / 已购去广告、新手引导期、5 秒内连点、以及 60 秒内的重复展示。
		/// </summary>
		private void ShowSettingInterstitial()
		{
			if (Singleton<VipManager>.Instance.VipStatus)
			{
				return;
			}
			if (GameGlobalData.ShopData != null && GameGlobalData.ShopData.isNoAds)
			{
				return;
			}
			if (GameGlobalData.IsTutorial())
			{
				return;
			}
			float now = Time.unscaledTime;
			if (now - _lastSettingInterAdRequest < SETTING_INTER_AD_REQUEST_GAP)
			{
				return;
			}
			if (now - _lastSettingInterAdShown < SETTING_INTER_AD_INTERVAL)
			{
				return;
			}
			_lastSettingInterAdRequest = now;
			Singleton<AdManager>.Instance.ShowInterstitial("35eeefa8-c011-4112-bc9b-4a04fe3c413c", "setting", OnSettingInterstitialResult);
		}

		private void OnSettingInterstitialResult(bool shown)
		{
			if (!shown)
			{
				return;
			}
			_lastSettingInterAdShown = Time.unscaledTime;
			GameAnalysisData.ShowInterstitial("setting");
		}

		private void ClickSettingClose()
		{
			View.Root.transform.SetAsFirstSibling();
			Tweener t = View.tranSettingDetail.DOLocalMoveX(800f, 0.4f).SetAutoKill(autoKillOnCompletion: true).SetEase(Ease.OutQuart);
			t.SetUpdate(isIndependentUpdate: true);
			View.btnSettingClose.gameObject.SetActive(value: false);
			ClickShareBgClose();
		}

		private void ClickSettingShare()
		{
			View.rectTranSharePanel.DOKill();
			View.rectTranSharePanel.DOAnchorPos(new Vector2(0f, 960f), 0.4f).SetEase(Ease.OutQuart).SetAutoKill(autoKillOnCompletion: true);
		}

		private void ClickShareBgClose()
		{
			View.rectTranSharePanel.DOKill();
			View.rectTranSharePanel.DOAnchorPos(new Vector2(0f, -960f), 0.4f).SetEase(Ease.OutQuart).SetAutoKill(autoKillOnCompletion: true);
		}

		private void ClickShareFb()
		{
			GameAnalysisData.Share("FaceBook");
			Singleton<SdkUtils>.Instance.ShareFB(LanguageConfig.FaceBookShareTitle, LanguageConfig.shareLink, string.Empty);
		}

		private void ClickShareMessage()
		{
			GameAnalysisData.Share("Message");
			Singleton<SdkUtils>.Instance.ShareMessenger(LanguageConfig.MessageShareTitle, LanguageConfig.shareLink, string.Empty);
		}

		private void ClickBattleItem(int index)
		{
			if (_useBattleItemTimes < GameGlobalData.BattleItemData.BattleItemLimitTimes)
			{
				BattleItemStruct battleItemStructByIndex = Singleton<BattleItemManager>.Instance.GetBattleItemStructByIndex(index);
				int iD = battleItemStructByIndex.ID;
				if (Singleton<BattleItemManager>.Instance.ConsumeItem(iD))
				{
					GameGlobalData.IsGameAuto = true;
					ApplicationFacade.Instance.SendNotification("BATTLE_MARK", false);
					Time.timeScale = 1f;
					View.TranBattleItem.localScale = Vector3.zero;
					_useBattleItemTimes++;
					ResetAllBattleItemIcon();
				}
			}
		}

		private void ClickCloseBgClose(PointerEventData data)
		{
			if (View.TranBattleItem.localScale == Vector3.one)
			{
				View.TranBattleItem.localScale = Vector3.zero;
			}
		}

		private void ShareReviveSuccessCallBack(bool isSuccess)
		{
			GameAnalysisData.ShareCallBack(isSuccess);
			ClickShareBgClose();
			if (isSuccess)
			{
				Singleton<ShareManager>.Instance.GotShareReward();
				if (Singleton<ShareManager>.Instance.HasShareToday())
				{
					View.tranShareDiamondBG.gameObject.SetActive(!Singleton<ShareManager>.Instance.HasShareToday());
				}
			}
			else
			{
				ApplicationFacade.Instance.SendNotification("TIP_ANIM", LanguageConfig.ShareTips);
			}
		}

		private void OnClickShake()
		{
			GameGlobalData.SetShake();
			SetShake();
			GameAnalysisData.SendShakeState(GameGlobalData.ShakeFlag);
		}

		private void OnClickSound()
		{
			GameGlobalData.IsMusice = !GameGlobalData.IsMusice;
			GameGlobalData.SetIsMusice(GameGlobalData.IsMusice);
			SetSound();
			if (GameGlobalData.IsMusice)
			{
				Singleton<AudioManager>.Instance.SetAllVolume(GameGlobalData.BGM_VOLUME, GameGlobalData.SOUND_VOLUME);
			}
			else
			{
				Singleton<AudioManager>.Instance.SetAllVolume(0f, 0f);
			}
			GameAnalysisData.SendSoundState((!GameGlobalData.IsMusice) ? 1 : 0);
		}

		private void CoinGetAni()
		{
			if (_coinTweener == null || !_coinTweener.IsPlaying())
			{
				_coinTweener = View.tranCoin.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.05f).SetLoops(2, LoopType.Yoyo);
			}
		}

		private void exitGameAnim()
		{
			if (_crossLevel)
			{
				View.animWidgetLevel.Play("anim_battleview_level_nextleveltomain");
			}
			else
			{
				View.animWidgetLevel.Play("anim_battleview_level_exit_to_main");
			}
			_crossLevel = false;
		}
	}
}
