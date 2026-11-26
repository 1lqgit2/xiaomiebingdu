using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace orangesGame
{
	public class MainView : UIView
	{
		public CustomEventHandler eventStart;

		public Text txtCoin;

		public Transform traIcon;

		public Button btnCoinAdd;

		public RectTransform tranCoin;

		public RectTransform tranDiamond;

		public Transform traDiamondIcon;

		public Text txtDiamond;

		public Button btnDiamondAdd;

		public GameObject objShootText;

		public Image shooterImg;

		public RectTransform tranOffline;

		public Button btnOffline;

		public Image imgYellowBg;

		public Image imgProgress;

		public Text txtOfflineCost;

		public Text txtOfflineTime;

		public Text txtCollect;

		public Button btnTask;

		public Transform tranTaskTip;

		public RectTransform tranTaskUnlockTip;

		public RectTransform tranDiamondOffline;

		public RectTransform tranDiamondOfflineTip;

		public GameObject objDiamondOfflineBubble;

		public Image imgDiamond;

		public Button btnDiamondOffline;

		public Image imgDiamondOffline;

		public Text txtDiamondOfflineTime;

		public Text txtDiamondOfflineFree;

		public Text txtFree;

		public Button btnShop;

		public Transform TranLimitTimeGiftTips;

		public Text TxtLimitTimeGiftTips;

		public Text TxtGiftTips;

		public Button btnFull;

		public Text txtFull;

		public Text txtNum;

		public Image fullImgDiamond;

		public Transform TranFullTips;

		public Button btnProp;

		public Transform tranPropTip;

		public RectTransform tranPropUnlockTip;

		public Button btnLuckDrawBox;

		public Image imgLuckDrawBoxUnLock;

		public Image imgLuckDrawBoxLock;

		public Text txtLuckDrawBoxTime;

		public GameObject objLuckUnlockRedTip;

		public RectTransform tranLuckUnlockTip;

		public Button btnLevel;

		public Text txtLevelName;

		public Button btnExtend;

		public Button btnExtendClose;

		public RectTransform tranExtend;

		public GameObject objExtendTip;

		private Animator ani;

		private Sequence _sequenceIn;

		private Sequence _sequenceOut;

		private const string ANIM_IN = "anim_mainview_in";

		private const string ANIM_OUT = "anim_mainview_out";

		public override void OnLoad()
		{
			base.OnLoad();
			// 主界面 UI 必须画在游戏内精灵（玩家飞机 sortingOrder=40）之上，
			// 否则玩家拖动飞机时飞机会压住界面和按钮。60 是主界面专用层：
			// 高于 40（游戏精灵）、低于 80（战斗/属性等界面）。
			Transform layerTran = Singleton<UIViewManager>.Instance.GetLayerTran(60);
			CreateView("GameUI/MainView/ui_mainview", layerTran);
		}

		public override void OnInit()
		{
			base.OnInit();
			Transform transform = base.Root.transform;
			tranCoin = (transform.Find("TranCoin") as RectTransform);
			txtCoin = tranCoin.Find("TxtCoin").GetComponent<Text>();
			traIcon = tranCoin.Find("ImgIcon");
			btnCoinAdd = tranCoin.Find("BtnAdd").GetComponent<Button>();
			objShootText = transform.Find("ShootText").gameObject;
			shooterImg = transform.Find("ShootText").GetComponent<Image>();
			eventStart = transform.Find("BtnStart").GetComponent<CustomEventHandler>();
			tranDiamond = (transform.Find("TranDiamond") as RectTransform);
			txtDiamond = tranDiamond.Find("TxtDiamond").GetComponent<Text>();
			btnDiamondAdd = tranDiamond.Find("BtnAdd").GetComponent<Button>();
			traDiamondIcon = tranDiamond.Find("ImgDiamond");
			tranOffline = (transform.Find("TranOffline") as RectTransform);
			btnOffline = tranOffline.Find("ImgOffline/BtnGet").GetComponent<Button>();
			imgYellowBg = tranOffline.Find("ImgOffline/ImgBg").GetComponent<Image>();
			imgProgress = tranOffline.Find("ImgOffline/ImgProgress").GetComponent<Image>();
			txtOfflineCost = tranOffline.Find("ImgOffline/TxtCost").GetComponent<Text>();
			txtOfflineTime = tranOffline.Find("ImgOffline/TxtTime").GetComponent<Text>();
			txtCollect = tranOffline.Find("ImgOffline/TxtTip").GetComponent<Text>();
			btnTask = transform.Find("root_extent/btn_extend/img_extend/BtnTask").GetComponent<Button>();
			tranTaskTip = btnTask.transform.Find("ImgTip");
			tranTaskUnlockTip = (btnTask.transform.Find("ImgFinger") as RectTransform);
			tranDiamondOffline = (transform.Find("root_extent/btn_extend/img_extend/TranDiamondOffline") as RectTransform);
			btnDiamondOffline = tranDiamondOffline.Find("BtnGet").GetComponent<Button>();
			imgDiamondOffline = btnDiamondOffline.transform.Find("img_bg").GetComponent<Image>();
			tranDiamondOfflineTip = (btnDiamondOffline.transform.Find("ImgFinger") as RectTransform);
			objDiamondOfflineBubble = tranDiamondOffline.Find("img_bg_0").gameObject;
			txtDiamondOfflineTime = tranDiamondOffline.Find("TxtTime").GetComponent<Text>();
			txtDiamondOfflineFree = tranDiamondOffline.Find("img_bg_0/TxtFree").GetComponent<Text>();
			imgDiamond = tranDiamondOffline.Find("img_bg_0/TxtCost/ImgDiamond").GetComponent<Image>();
			txtFree = tranDiamondOffline.Find("img_bg_0/TxtFree").GetComponent<Text>();
			btnShop = transform.Find("BtnShop").GetComponent<Button>();
			TranLimitTimeGiftTips = transform.Find("BtnShop/timeLimitGift");
			TxtLimitTimeGiftTips = TranLimitTimeGiftTips.Find("txtTime").GetComponent<Text>();
			TxtGiftTips = TranLimitTimeGiftTips.Find("Text").GetComponent<Text>();
			TranFullTips = transform.Find("root_extent/btn_extend/img_extend/BtnFull/FullTip");
			btnFull = transform.Find("root_extent/btn_extend/img_extend/BtnFull").GetComponent<Button>();
			txtFull = transform.Find("root_extent/btn_extend/img_extend/BtnFull/FullTip/txtFull").GetComponent<Text>();
			txtNum = transform.Find("root_extent/btn_extend/img_extend/BtnFull/FullTip/txtNum").GetComponent<Text>();
			fullImgDiamond = transform.Find("root_extent/btn_extend/img_extend/BtnFull/FullTip/ImgDiamond").GetComponent<Image>();
			btnProp = transform.Find("root_extent/btn_extend/img_extend/BtnProp").GetComponent<Button>();
			tranPropTip = btnProp.transform.Find("ImgTip");
			tranPropUnlockTip = (btnProp.transform.Find("ImgFinger") as RectTransform);
			btnLuckDrawBox = transform.Find("root_extent/btn_extend/img_extend/BtnLuckyBox").GetComponent<Button>();
			imgLuckDrawBoxUnLock = btnLuckDrawBox.transform.Find("img_bg_0").GetComponent<Image>();
			imgLuckDrawBoxLock = btnLuckDrawBox.transform.Find("img_bg_1").GetComponent<Image>();
			txtLuckDrawBoxTime = btnLuckDrawBox.transform.Find("txt_time").GetComponent<Text>();
			tranLuckUnlockTip = (btnLuckDrawBox.transform.Find("ImgFinger") as RectTransform);
			objLuckUnlockRedTip = btnLuckDrawBox.transform.Find("ImgTip").gameObject;
			btnLevel = transform.Find("BtnLevel").GetComponent<Button>();
			txtLevelName = btnLevel.GetComponentInChildren<Text>();
			btnExtend = transform.Find("root_extent/btn_extend").GetComponent<Button>();
			btnExtendClose = btnExtend.transform.Find("btn_extend").GetComponent<Button>();
			objExtendTip = btnExtend.transform.Find("img_redtip").gameObject;
			tranExtend = (btnExtend.transform.Find("img_extend/img_extend") as RectTransform);
			ApplicationFacade.Instance.RegisterMediator(new MainViewMediator("MAIN", this));
			ani = transform.GetComponent<Animator>();
			GameGlobalData.CoinRecoverTopLeftPos = traIcon.position;
			GameGlobalData.DiamondRecoverTopLeftPos = traDiamondIcon.position;
		}

		public override void OnRegister()
		{
			base.OnRegister();
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
			ani.Play("anim_mainview_in", 0, 0f);
		}

		public override void OnHide(Action callback = null)
		{
			eventStart.gameObject.SetActive(value: false);
			if (_sequenceOut == null)
			{
				_sequenceOut = DOTween.Sequence();
				_sequenceOut.onComplete = delegate
				{
					if (callback != null)
					{
						callback();
					}
				};
				_sequenceOut.SetAutoKill(autoKillOnCompletion: false);
				_sequenceOut.Play();
			}
			else
			{
				_sequenceOut.Restart();
			}
			ani.Play("anim_mainview_out", 0, 0f);
			Timer.Register(0.3f, MoveOutCallback);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			ApplicationFacade.Instance.RemoveMediator("MAIN");
		}

		public void MoveOutCallback()
		{
			base.OnHide();
		}
	}
}
