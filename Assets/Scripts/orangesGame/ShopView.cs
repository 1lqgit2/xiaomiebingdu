using UnityEngine;
using UnityEngine.UI;

namespace orangesGame
{
	public class ShopView : UIView
	{
		public Text shopTitle;

		public Text bestOrder;

		public Text bestOrder1_title;

		public Text bestOrder2_title;

		public Text bestOrder3_title;

		public Text bestOrder4_title;

		public Text bestOrder5_title;

		public Text bestOrder6_title;

		public Text bestOrder7_title;

		public Text bestOrder8_title;

		public Text recommend;

		public Text diamondMembership;

		public Text detail;

		public Text vip4pri;

		public Text moveAds;

		public Text diamondTitle;

		public Button btnClose;

		public Text txtCoin;

		public Text txtDiamond;

		public Transform tranCoin;

		public Transform tranDiamond;

		public UIShopDiamond shopDiamond;

		public UIShopRecommend shopRecommend;

		public UIShopBestOrder[] shopBestOrders = new UIShopBestOrder[5];

		public UIShopBestOrderItem[] shopBestOrderItems = new UIShopBestOrderItem[3];

		public override void OnLoad()
		{
			base.OnLoad();
			Transform layerTran = Singleton<UIViewManager>.Instance.GetLayerTran(90);
			CreateView("GameUI/ShopView/ui_shopview", layerTran);
		}

		public override void OnInit()
		{
			base.OnInit();
			Transform transform = base.Root.transform;
			btnClose = transform.Find("btn_close").GetComponent<Button>();
			txtCoin = transform.Find("root_currency/root_coin/txt_count").GetComponent<Text>();
			txtDiamond = transform.Find("root_currency/root_diamond/txt_count").GetComponent<Text>();
			tranCoin = transform.Find("root_currency/root_coin/img_icon");
			tranDiamond = transform.Find("root_currency/root_diamond/img_icon");
			shopRecommend = transform.Find("slv_list/slv_viewport/slv_content/ui_shopview_recommend").GetComponent<UIShopRecommend>();
			shopDiamond = transform.Find("slv_list/slv_viewport/slv_content/ui_shopview_diamond").GetComponent<UIShopDiamond>();
			shopTitle = transform.Find("txt_title").GetComponent<Text>();
			bestOrder = transform.Find("slv_list/slv_viewport/slv_content/ui_shopview_bestorder1/txt_title").GetComponent<Text>();
			bestOrder1_title = transform.Find("slv_list/slv_viewport/slv_content/ui_shopview_bestorder1/ui_shopview_item_0/txt_title").GetComponent<Text>();
			bestOrder2_title = transform.Find("slv_list/slv_viewport/slv_content/ui_shopview_bestorder2/ui_shopview_item_0/txt_title").GetComponent<Text>();
			bestOrder3_title = transform.Find("slv_list/slv_viewport/slv_content/ui_shopview_bestorder3/ui_shopview_item_0/txt_title").GetComponent<Text>();
			bestOrder4_title = transform.Find("slv_list/slv_viewport/slv_content/ui_shopview_bestorder4/ui_shopview_item_0/txt_title").GetComponent<Text>();
			bestOrder5_title = transform.Find("slv_list/slv_viewport/slv_content/ui_shopview_bestorder5/ui_shopview_item_0/txt_title").GetComponent<Text>();
			bestOrder6_title = transform.Find("slv_list/slv_viewport/slv_content/ui_shopview_bestorder6/ui_shopview_item_0/txt_title").GetComponent<Text>();
			bestOrder7_title = transform.Find("slv_list/slv_viewport/slv_content/ui_shopview_bestorder7/ui_shopview_item_0/txt_title").GetComponent<Text>();
			bestOrder8_title = transform.Find("slv_list/slv_viewport/slv_content/ui_shopview_bestorder8/ui_shopview_item_0/txt_title").GetComponent<Text>();
			recommend = transform.Find("slv_list/slv_viewport/slv_content/ui_shopview_recommend/txt_title").GetComponent<Text>();
			diamondMembership = transform.Find("slv_list/slv_viewport/slv_content/ui_shopview_recommend/ui_shopview_item_0/txt_title").GetComponent<Text>();
			vip4pri = transform.Find("slv_list/slv_viewport/slv_content/ui_shopview_recommend/ui_shopview_item_0/txt_content").GetComponent<Text>();
			detail = transform.Find("slv_list/slv_viewport/slv_content/ui_shopview_recommend/ui_shopview_item_0/btn_detail/txt_cotent").GetComponent<Text>();
			moveAds = transform.Find("slv_list/slv_viewport/slv_content/ui_shopview_recommend/ui_shopview_item_1/txt_title").GetComponent<Text>();
			diamondTitle = transform.Find("slv_list/slv_viewport/slv_content/ui_shopview_diamond/txt_title").GetComponent<Text>();
			for (int i = 0; i < shopBestOrders.Length; i++)
			{
				shopBestOrders[i] = transform.Find("slv_list/slv_viewport/slv_content/ui_shopview_bestorder" + (i + 1)).GetComponent<UIShopBestOrder>();
			}
			for (int j = 0; j < shopBestOrderItems.Length; j++)
			{
				shopBestOrderItems[j] = transform.Find("slv_list/slv_viewport/slv_content/ui_shopview_bestorder" + (j + 6)).GetComponent<UIShopBestOrderItem>();
			}
			SetText();
			ApplicationFacade.Instance.RegisterMediator(new ShopViewMediator("SHOP", this));
		}

		private void SetText()
		{
			shopTitle.text = LanguageConfig.Shop_Title;
			bestOrder.text = LanguageConfig.Shop_bestOrder;
			bestOrder1_title.text = LanguageConfig.Shop_bestOrder1_title;
			bestOrder2_title.text = LanguageConfig.Shop_bestOrder2_title;
			bestOrder3_title.text = LanguageConfig.Shop_bestOrder3_title;
			bestOrder4_title.text = LanguageConfig.Shop_bestOrder4_title;
			bestOrder5_title.text = LanguageConfig.Shop_bestOrder5_title;
			bestOrder6_title.text = LanguageConfig.Shop_bestOrder6_title;
			bestOrder7_title.text = LanguageConfig.Shop_bestOrder7_title;
			bestOrder8_title.text = LanguageConfig.Shop_bestOrder8_title;
			recommend.text = LanguageConfig.Shop_recommend;
			diamondMembership.text = LanguageConfig.Shop_diamondMembership;
			vip4pri.text = LanguageConfig.Shop_vip4pri;
			detail.text = LanguageConfig.Shop_detail;
			moveAds.text = LanguageConfig.Shop_moveAds;
			diamondTitle.text = LanguageConfig.Shop_diamondTitle;
		}

		public override void OnRemove()
		{
			base.OnRemove();
			ApplicationFacade.Instance.RemoveMediator("SHOP");
		}
	}
}
