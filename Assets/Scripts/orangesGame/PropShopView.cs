using UnityEngine;
using UnityEngine.UI;

namespace orangesGame
{
	public class PropShopView : UIView
	{
		public Transform tranShop;

		public Button btnClose;

		public Text txtTitle;

		public Transform tranItemParent;

		public Transform tranConversion;

		public CustomEventHandler eventAdd;

		public CustomEventHandler eventSub;

		public Button btnConver;

		public Button btnShop;

		public Text txtConverCount;

		public Text txtCostValue;

		public Text txtCostNumber;

		public Image imaCostIcon;

		public Transform tranPropList;

		public Transform[] tranPropItem;

		public Image[] imaPropItemBg;

		public Button[] btnItem;

		public Text[] txtItemName;

		public Text[] txtItemValue;

		public Image[] imaItemIcon;

		public Text[] txtItemOwn;

		public Text[] txtItemOwnTxt;

		public Text converTxt;

		public Text useTxt;

		public Text getTxt;

		public Text getMoreTxt;

		public override void OnLoad()
		{
			base.OnLoad();
			Transform layerTran = Singleton<UIViewManager>.Instance.GetLayerTran(90);
			CreateView("GameUI/PropShopView/ui_propshopview", layerTran);
		}

		public override void OnInit()
		{
			base.OnInit();
			Transform transform = base.Root.transform;
			tranShop = transform.Find("root_shop");
			btnClose = tranShop.Find("btn_close").GetComponent<Button>();
			txtTitle = tranShop.Find("txt_title").GetComponent<Text>();
			tranItemParent = tranShop.Find("slv_list/slv_viewport/slv_content");
			tranConversion = transform.Find("root_conversion");
			Button component = tranConversion.Find("btn_sub").GetComponent<Button>();
			Button component2 = tranConversion.Find("btn_add").GetComponent<Button>();
			eventAdd = component2.GetComponent<CustomEventHandler>();
			eventSub = component.GetComponent<CustomEventHandler>();
			btnConver = tranConversion.Find("btn_conver").GetComponent<Button>();
			btnShop = tranConversion.Find("btn_shop").GetComponent<Button>();
			txtConverCount = tranConversion.Find("txt_convercount").GetComponent<Text>();
			txtCostValue = tranConversion.Find("img_bg_1/txt_value").GetComponent<Text>();
			txtCostNumber = tranConversion.Find("img_bg_1/txt_number").GetComponent<Text>();
			imaCostIcon = tranConversion.Find("img_bg_1/ima_propicon").GetComponent<Image>();
			tranPropList = transform.Find("root_shop/root_list");
			BattleItemStruct[] battleItemJson = Singleton<GameJson>.Instance.BattleItemJson;
			tranPropItem = new Transform[battleItemJson.Length];
			btnItem = new Button[battleItemJson.Length];
			imaPropItemBg = new Image[battleItemJson.Length];
			txtItemName = new Text[battleItemJson.Length];
			txtItemValue = new Text[battleItemJson.Length];
			txtItemOwn = new Text[battleItemJson.Length];
			imaItemIcon = new Image[battleItemJson.Length];
			txtItemOwnTxt = new Text[battleItemJson.Length];
			for (int i = 0; i < battleItemJson.Length; i++)
			{
				string n = "prop_item_" + i.ToString();
				tranPropItem[i] = tranPropList.Find(n);
				btnItem[i] = tranPropItem[i].GetComponent<Button>();
				imaPropItemBg[i] = tranPropItem[i].GetComponent<Image>();
				txtItemName[i] = tranPropItem[i].Find("txt_name").GetComponent<Text>();
				txtItemValue[i] = tranPropItem[i].Find("img_icon_1/txt_value").GetComponent<Text>();
				txtItemOwn[i] = tranPropItem[i].Find("txt_cotent").GetComponent<Text>();
				imaItemIcon[i] = tranPropItem[i].Find("img_icon_0").GetComponent<Image>();
				txtItemOwnTxt[i] = tranPropItem[i].Find("txt_owned").GetComponent<Text>();
			}
			converTxt = tranConversion.Find("btn_conver/TxtCost").GetComponent<Text>();
			useTxt = tranConversion.Find("img_bg_1/txt_use").GetComponent<Text>();
			getTxt = tranConversion.Find("img_bg_1/txt_get").GetComponent<Text>();
			getMoreTxt = tranConversion.Find("btn_shop/TxtCost").GetComponent<Text>();
			ApplicationFacade.Instance.RegisterMediator(new PropShopMediator("PROPSHOP", this));
		}

		public override void OnRemove()
		{
			base.OnRemove();
			ApplicationFacade.Instance.RemoveMediator("PROPSHOP");
		}
	}
}
