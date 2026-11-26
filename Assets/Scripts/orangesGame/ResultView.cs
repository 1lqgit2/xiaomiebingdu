using UnityEngine;
using UnityEngine.UI;

namespace orangesGame
{
	public class ResultView : UIView
	{
		public Transform tranContinue;

		public Transform tranShare;

		public Animator animatorContinue;

		public Button btnContinue;

		public Text txtContinue;

		public Button btnDiamondContinue;

		public Text txtDiamondContinue;

		public Button btnAgain;

		public Text txtNoThanks;

		public CanvasGroup cGAgain;

		public Text txtTime;

		public Text txtDiamond;

		public Transform tranPass;

		public Button btnGetReward;

		public Button btnGetAdReward;

		public Text txtReward;

		public Text txtAdReward;

		public Text txtAdRate;

		public Text txtTitle;

		public Transform tranCoin;

		public Transform tranImgCoin;

		public Text txtCoin;

		public Text txtTips;

		public Image txtTipsImg;

		public Text txtResultTitle;

		public Text txtRewardCollect;

		public Image imgSlotsProgress;

		public Button btnSlots;

		public Text textSlots;

		public Text textSlotsRate;

		public Text txtAdNotice;

		public Transform tranDiamond;

		public Transform tranDiamondIcon;

		public Text txtCurDiamond;

		public Transform tranDiamondStorage;

		public Button btnGetDiamond;

		public Button btnJumpLuckyBox;

		public Text txtLuckyBoxProgress;

		public Text TxtFull;

		public Image fullImgDiamond;

		public Text TxtCost;

		public Image ImgDiamond;

		public Image ImgAds;

		public Text TxtFree;

		public GameObject objMark;

		public Transform tranUnLock;

		public UnLockItem[] unLockItems;

		public Text txtUnLockContent;

		public override void OnLoad()
		{
			base.OnLoad();
			Transform layerTran = Singleton<UIViewManager>.Instance.GetLayerTran(80);
			CreateView("GameUI/ResultView/ui_resultview", layerTran);
		}

		public override void OnInit()
		{
			base.OnInit();
			Transform transform = base.Root.transform;
			tranContinue = transform.Find("TranContinue");
			animatorContinue = tranContinue.GetComponent<Animator>();
			txtTime = tranContinue.Find("BtnContinue/TxtTime").GetComponent<Text>();
			btnContinue = tranContinue.Find("BtnContinue").GetComponent<Button>();
			txtContinue = btnContinue.transform.Find("TxtTitle").GetComponent<Text>();
			btnDiamondContinue = tranContinue.Find("BtnDiamondContinue").GetComponent<Button>();
			txtDiamondContinue = btnDiamondContinue.transform.Find("TxtTitle").GetComponent<Text>();
			txtDiamond = tranContinue.Find("BtnDiamondContinue/TxtDiamond").GetComponent<Text>();
			btnAgain = tranContinue.Find("BtnAgain").GetComponent<Button>();
			cGAgain = btnAgain.GetComponent<CanvasGroup>();
			txtNoThanks = btnAgain.GetComponentInChildren<Text>();
			tranPass = transform.Find("TranPass");
			btnGetReward = tranPass.Find("BtnGetReward").GetComponent<Button>();
			btnGetAdReward = tranPass.Find("BtnGetAdReward").GetComponent<Button>();
			txtTips = tranPass.Find("Image/TxtTips").GetComponent<Text>();
			txtTitle = tranPass.Find("TxtTitle").GetComponent<Text>();
			txtTipsImg = tranPass.Find("Image").GetComponent<Image>();
			txtReward = btnGetReward.transform.Find("TxtContent").GetComponent<Text>();
			txtAdReward = btnGetAdReward.transform.Find("TxtContent").GetComponent<Text>();
			txtAdRate = btnGetAdReward.transform.Find("TxtRate").GetComponent<Text>();
			tranCoin = transform.Find("TranCoin");
			txtCoin = tranCoin.Find("TxtCoin").GetComponent<Text>();
			tranImgCoin = tranCoin.Find("ImgIcon");
			txtResultTitle = tranPass.Find("TxtTitle").GetComponent<Text>();
			txtRewardCollect = btnGetReward.transform.Find("TxtTitle").GetComponent<Text>();
			imgSlotsProgress = tranPass.Find("TranSlots/ImgSlotsProgress").GetComponent<Image>();
			btnSlots = tranPass.Find("BtnSlots").GetComponent<Button>();
			textSlots = tranPass.Find("BtnSlots/TxtContent").GetComponent<Text>();
			textSlotsRate = tranPass.Find("BtnSlots/TxtRate").GetComponent<Text>();
			tranDiamondStorage = tranPass.Find("TranDiamondStorage");
			btnGetDiamond = tranDiamondStorage.Find("BtnGet").GetComponent<Button>();
			ImgDiamond = tranDiamondStorage.Find("ImgDiamond").GetComponent<Image>();
			ImgAds = tranDiamondStorage.Find("ImgAds").GetComponent<Image>();
			TxtCost = tranDiamondStorage.Find("TxtCost").GetComponent<Text>();
			TxtFree = tranDiamondStorage.Find("TxtFree").GetComponent<Text>();
			btnJumpLuckyBox = tranDiamondStorage.Find("BtnLuckyBox").GetComponent<Button>();
			txtLuckyBoxProgress = tranDiamondStorage.Find("ImgBg/TxtProgress").GetComponent<Text>();
			TxtFull = tranDiamondStorage.Find("ImgBg/TxtFull").GetComponent<Text>();
			fullImgDiamond = tranDiamondStorage.Find("ImgBg/fullImgDiamond").GetComponent<Image>();
			tranDiamond = transform.Find("TranDiamond");
			tranDiamondIcon = tranDiamond.Find("ImgIcon");
			txtCurDiamond = tranDiamond.Find("TxtCoin").GetComponent<Text>();
			objMark = transform.Find("ImgMark").gameObject;
			tranUnLock = tranPass.Find("root_unlock");
			unLockItems = new UnLockItem[5];
			for (int i = 0; i < unLockItems.Length; i++)
			{
				unLockItems[i] = tranUnLock.Find("item_" + i).GetComponent<UnLockItem>();
			}
			txtUnLockContent = tranUnLock.Find("txt_content").GetComponent<Text>();
			ApplicationFacade.Instance.RegisterMediator(new ResultViewMediator("RESULT", this));
			txtAdNotice = tranPass.Find("TxtAdNotice").GetComponent<Text>();
			txtAdNotice.gameObject.SetActive(value: false);
		}

		public override void OnRegister()
		{
			base.OnRegister();
		}

		public override void OnRemove()
		{
			base.OnRemove();
			ApplicationFacade.Instance.RemoveMediator("RESULT");
		}
	}
}
