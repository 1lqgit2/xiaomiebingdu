using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace orangesGame
{
	public class VipView : UIView
	{
		public Button BtnClose;

		public Button BtnClaim;

		public Button BtnBuyVip;

		public Text TxtReadContext;

		public Text TxtTimeDeadLine;

		public Transform TranVipPanel;

		public Transform TranNotVipPanel;

		public Transform TranFreeTest;

		public Transform TranPay;

		private Sequence _sequenceIn;

		private Sequence _sequenceOut;

		public Text[] TextContents = new Text[4];

		public Text TranVipPanelTips;

		public Text BtnClaimText;

		public Text HasClaimText;

		public Text BtnPayFreeTestBigText;

		public Text BtnPayFreeTestLittleText;

		public Text BtnPayBigText;

		public Text BtnPayLittleText;

		public Image img_en;

		public Image img_cn;

		public override void OnLoad()
		{
			base.OnLoad();
			Transform layerTran = Singleton<UIViewManager>.Instance.GetLayerTran(100);
			CreateView("GameUI/VIPView/ui_vipview", layerTran);
		}

		public override void OnInit()
		{
			base.OnInit();
			Transform transform = base.Root.transform;
			img_en = transform.Find("bg/tiitleLogo").GetComponent<Image>();
			img_cn = transform.Find("bg/tiitleLogo_cn").GetComponent<Image>();
			BtnClose = transform.Find("btnClose").GetComponent<Button>();
			TxtReadContext = transform.Find("TranNotVipPanel/ScrollView/Viewport/Content/txtContext").GetComponent<Text>();
			TranVipPanel = transform.Find("TranVipPanel");
			TranNotVipPanel = transform.Find("TranNotVipPanel");
			BtnClaim = TranVipPanel.Find("btnClaim").GetComponent<Button>();
			BtnBuyVip = TranNotVipPanel.Find("btnPay").GetComponent<Button>();
			TranFreeTest = BtnBuyVip.transform.Find("freeTest");
			TranPay = BtnBuyVip.transform.Find("pay");
			TxtTimeDeadLine = TranVipPanel.Find("txtTimeDeadLine").GetComponent<Text>();
			for (int i = 0; i < TextContents.Length; i++)
			{
				TextContents[i] = transform.Find("Contents/star" + (i + 1) + "/txtContext").GetComponent<Text>();
				TextContents[i].text = LanguageConfig.STARS_STRINGS[i];
			}
			TranVipPanelTips = TranVipPanel.Find("Tips").GetComponent<Text>();
			TranVipPanelTips.text = LanguageConfig.DAILYBENIFIT;
			BtnClaimText = BtnClaim.transform.Find("Text").GetComponent<Text>();
			BtnClaimText.text = LanguageConfig.CHAIM;
			HasClaimText = TranVipPanel.transform.Find("txtHasClaim").GetComponent<Text>();
			HasClaimText.text = LanguageConfig.HASCHAIM;
			BtnPayFreeTestBigText = BtnBuyVip.transform.Find("freeTest/BigText").GetComponent<Text>();
			BtnPayFreeTestBigText.text = LanguageConfig.TRYFORFREE;
			BtnPayFreeTestLittleText = BtnBuyVip.transform.Find("freeTest/LittleText").GetComponent<Text>();
			BtnPayFreeTestLittleText.text = LanguageConfig.FREE_LITTLE;
			BtnPayBigText = BtnBuyVip.transform.Find("pay/BigText").GetComponent<Text>();
			BtnPayBigText.text = LanguageConfig.PAYBIG;
			BtnPayLittleText = BtnBuyVip.transform.Find("pay/LittleText").GetComponent<Text>();
			BtnPayLittleText.text = LanguageConfig.PAYLITTLE;
			ApplicationFacade.Instance.RegisterMediator(new VipViewMediator("VIP", this));
		}

		public override void OnRemove()
		{
			base.OnRemove();
			ApplicationFacade.Instance.RemoveMediator("VIP");
		}
	}
}
