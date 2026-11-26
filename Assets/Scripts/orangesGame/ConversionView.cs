using UnityEngine;
using UnityEngine.UI;

namespace orangesGame
{
	public class ConversionView : UIView
	{
		public Button btnClose;

		public RectTransform tranConversion;

		public Button btnAdd;

		public Button btnSub;

		public CustomEventHandler eventAdd;

		public CustomEventHandler eventSub;

		public Button btnConver;

		public Button btnFree;

		public Text txtDiamondCount;

		public Text txtConverDiamond;

		public Text txtConverCoin;

		public Text txtTitle;

		public Text txtConvert;

		public Text txtGetMore;

		public override void OnLoad()
		{
			base.OnLoad();
			Transform layerTran = Singleton<UIViewManager>.Instance.GetLayerTran(90);
			CreateView("GameUI/ConversionView/ui_conversionView", layerTran);
		}

		public override void OnInit()
		{
			base.OnInit();
			Transform transform = base.Root.transform;
			btnClose = transform.Find("btn_close").GetComponent<Button>();
			tranConversion = (transform.Find("root_conversion") as RectTransform);
			btnAdd = tranConversion.Find("btn_add").GetComponent<Button>();
			btnSub = tranConversion.Find("btn_sub").GetComponent<Button>();
			eventAdd = btnAdd.GetComponent<CustomEventHandler>();
			eventSub = btnSub.GetComponent<CustomEventHandler>();
			btnConver = tranConversion.Find("btn_conver").GetComponent<Button>();
			btnFree = tranConversion.Find("btn_free").GetComponent<Button>();
			txtDiamondCount = tranConversion.Find("txt_diamondcount").GetComponent<Text>();
			txtConverDiamond = tranConversion.Find("txt_converdiamond").GetComponent<Text>();
			txtConverCoin = txtConverDiamond.transform.Find("txt_convercoin").GetComponent<Text>();
			txtTitle = tranConversion.Find("txt_title").GetComponent<Text>();
			txtConvert = btnConver.transform.Find("TxtCost").GetComponent<Text>();
			txtGetMore = tranConversion.transform.Find("btn_free/TxtCost").GetComponent<Text>();
			ApplicationFacade.Instance.RegisterMediator(new ConversionViewMediation("CONVERSION", this));
		}

		public override void OnRegister()
		{
			base.OnRegister();
		}

		public override void OnRemove()
		{
			base.OnRemove();
			ApplicationFacade.Instance.RemoveMediator("CONVERSION");
		}
	}
}
