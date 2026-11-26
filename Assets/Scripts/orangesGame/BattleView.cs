using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace orangesGame
{
	public class BattleView : UIView
	{
		public Text txtProgress;

		public Transform imgProgress;

		public Text txtCurCoin;

		public Text[] txtLevels;

		public RectTransform[] bossTou;

		public Button btnNoSound;

		public Button btnSound;

		public Button btnShake;

		public Button btnSetting;

		public Button btnShare;

		public Button btnSettingClose;

		public Button btnShareFb;

		public Button btnShareMessage;

		public Text shareTxt;

		public Transform tranSettingTips;

		public Transform tranSettingDetail;

		public Transform tranShare;

		public Transform tranShareDiamondBG;

		public RectTransform tranSettingbg;

		public RectTransform rectTranSharePanel;

		public Transform tranCoin;

		public RectTransform tranSound;

		public RectTransform tranLevel;

		public Image imgMark_0;

		public Image imgMark_1;

		public AnimatorWidget animWidgetLevel;

		public AnimatorWidget animWidgetProgress;

		public UIItemIcon uIItemIconContent;

		private Sequence _sequenceIn;

		private Sequence _sequenceOut;

		public Transform bossTips;

		public Image bossImg;

		public Image bossTipsBg;

		public Text FinalTxt;

		public Text txtBossTip;

		public Transform TranBattleItem;

		public Button[] BattleItemButtons = new Button[5];

		public CustomEventHandler[] BattleItemButtonsCustomEventHandler = new CustomEventHandler[5];

		public Text[] BattleItemButtonsTexts = new Text[5];

		public CustomEventHandler BattleItemBgClose;

		public Transform TranBattleItemButtonUse;

		public Text BattleItemButtonUseNumText;

		public Text BattleItemButtonUseTitleText;

		public Transform TranUseItemHasnt;

		public Text BattleUseItemHasntTitleText;

		public Transform tranWall;

		public Transform tranTask;

		public Text txtTaskProgress;

		public override void OnLoad()
		{
			base.OnLoad();
			Transform layerTran = Singleton<UIViewManager>.Instance.GetLayerTran(80);
			CreateView("GameUI/BattleView/ui_battleview", layerTran);
		}

		public override void OnInit()
		{
			base.OnInit();
			Transform transform = base.Root.transform;
			tranWall = transform.Find("TranWall");
			tranSound = (transform.Find("TraSettingDetailCull/TraSettingDetail/TranSound") as RectTransform);
			btnSound = tranSound.Find("BtnSound").GetComponent<Button>();
			btnNoSound = tranSound.Find("BtnNoSound").GetComponent<Button>();
			btnSetting = transform.Find("TranSetting/BtnSetting").GetComponent<Button>();
			btnSettingClose = transform.Find("BtnCloseSetting").GetComponent<Button>();
			tranSettingTips = transform.Find("TranSetting/SettingTips");
			tranSettingDetail = transform.Find("TraSettingDetailCull/TraSettingDetail");
			tranShare = tranSettingDetail.Find("TranShare");
			tranShareDiamondBG = tranShare.Find("ShareDiamondBG");
			tranSettingbg = (tranSettingDetail.Find("TranSettingbg") as RectTransform);
			tranLevel = (transform.Find("TranLevel") as RectTransform);
			btnShare = tranShare.Find("BtnShare").GetComponent<Button>();
			rectTranSharePanel = (transform.Find("TranSharePanel") as RectTransform);
			btnShareFb = rectTranSharePanel.Find("btnFaceBookShare").GetComponent<Button>();
			shareTxt = rectTranSharePanel.Find("Text").GetComponent<Text>();
			btnShareMessage = rectTranSharePanel.Find("btnMessageShare").GetComponent<Button>();
			animWidgetProgress = transform.Find("TranProgress").GetComponent<AnimatorWidget>();
			txtProgress = transform.Find("TranProgress/TxtProgress").GetComponent<Text>();
			imgProgress = transform.Find("TranProgress/TranProgress/ImgProgress");
			txtCurCoin = transform.Find("TranProgress/TxtCurCoin").GetComponent<Text>();
			tranCoin = transform.Find("TranProgress/TxtCurCoin/ImgCoin");
			imgMark_0 = transform.Find("ImgMark_0").GetComponent<Image>();
			imgMark_1 = transform.Find("ImgMark_1").GetComponent<Image>();
			bossTips = (transform.Find("BossTips") as RectTransform);
			bossImg = bossTips.Find("img").GetComponent<Image>();
			bossTipsBg = bossTips.Find("bosstips").GetComponent<Image>();
			FinalTxt = bossTips.Find("FinalTxt").GetComponent<Text>();
			txtBossTip = bossTipsBg.transform.Find("Text").GetComponent<Text>();
			TranBattleItem = transform.Find("TranBattleItem");
			BattleItemBgClose = TranBattleItem.Find("bgClose").GetComponent<CustomEventHandler>();
			for (int i = 0; i < BattleItemButtons.Length; i++)
			{
				BattleItemButtons[i] = TranBattleItem.Find("btnItemParent/btnItem" + (i + 1)).GetComponent<Button>();
				BattleItemButtonsTexts[i] = TranBattleItem.Find("btnItemParent/btnItem" + (i + 1) + "/Text").GetComponent<Text>();
				BattleItemButtonsCustomEventHandler[i] = BattleItemButtons[i].transform.GetComponent<CustomEventHandler>();
			}
			TranBattleItemButtonUse = TranBattleItem.Find("btnItemParent/useitemTips");
			BattleItemButtonUseNumText = TranBattleItem.Find("btnItemParent/useitemTips/useitemNums").GetComponent<Text>();
			BattleItemButtonUseTitleText = TranBattleItem.Find("btnItemParent/useitemTips/tiptitle").GetComponent<Text>();
			TranUseItemHasnt = TranBattleItem.Find("useitemHasnt");
			BattleUseItemHasntTitleText = TranUseItemHasnt.Find("tiptitle").GetComponent<Text>();
			btnShake = transform.Find("TraSettingDetailCull/TraSettingDetail/TranShake/BtnShake").GetComponent<Button>();
			btnSettingClose.gameObject.SetActive(value: false);
			tranSettingDetail.localPosition = new Vector3(800f, -18f);
			rectTranSharePanel.anchoredPosition = new Vector2(0f, -960f);
			TranBattleItem.localScale = Vector3.zero;
			txtLevels = new Text[4];
			bossTou = new RectTransform[4];
			for (int j = 0; j < txtLevels.Length; j++)
			{
				txtLevels[j] = tranLevel.Find($"ImgLevel{j}/TxtLevel").GetComponent<Text>();
				bossTou[j] = tranLevel.Find($"ImgLevel{j}/Tou").GetComponent<RectTransform>();
			}
			animWidgetLevel = tranLevel.GetComponent<AnimatorWidget>();
			uIItemIconContent = transform.Find("TranItemIcon").GetComponent<UIItemIcon>();
			tranTask = (transform.Find("TranTask") as RectTransform);
			txtTaskProgress = tranTask.Find("TxtTaskProgress").GetComponent<Text>();
			ApplicationFacade.Instance.RegisterMediator(new BattleViewMediator("BATTLE", this));
		}

		public override void OnRegister()
		{
			base.OnRegister();
		}

		public override void OnRemove()
		{
			base.OnRemove();
			ApplicationFacade.Instance.RemoveMediator("BATTLE");
		}

		public override void OnShow(Action callback = null)
		{
			base.OnShow();
			if (_sequenceIn == null)
			{
				_sequenceIn = DOTween.Sequence();
				_sequenceIn.onComplete = delegate
				{
					if (callback != null)
					{
						callback();
					}
				};
				_sequenceIn.SetAutoKill(autoKillOnCompletion: false);
				_sequenceIn.Play();
			}
			else
			{
				_sequenceIn.Restart();
			}
		}

		public override void OnHide(Action callback = null)
		{
			Sequence sequence = DOTween.Sequence();
			sequence.onComplete = delegate
			{
				if (callback != null)
				{
					callback();
				}
				base.OnHide();
			};
			sequence.Play();
		}
	}
}
