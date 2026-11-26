using UnityEngine;
using UnityEngine.UI;

namespace orangesGame
{
	public class ShopBestOrderView : UIView
	{
		public Button BtnPay;

		public Button BtnClose;

		public Text TwoItemTxtDiamondNum;

		public Text TwoItemTxtCoinNum;

		public Text ThreeItemTxtDiamondNum;

		public Text ThreeItemTxtCoinNum;

		public Text TxtPay;

		public Text txt_title;

		public Text TxtDiscontPay;

		public Text TxtDiscont;

		public Transform TwoItemTran;

		public Transform ThreeItemTran;

		public Text TxtRemaintime;

		public override void OnLoad()
		{
			base.OnLoad();
			Transform layerTran = Singleton<UIViewManager>.Instance.GetLayerTran(120);
			CreateView("GameUI/ShopView/ui_shopview_bestorder", layerTran);
		}

		public override void OnInit()
		{
			base.OnInit();
			Transform transform = base.Root.transform;
			BtnClose = transform.Find("btnClose").GetComponent<Button>();
			Transform transform2 = transform.Find("ui_shopview_item_0");
			BtnPay = transform2.GetComponent<Button>();
			txt_title = transform2.Find("txt_title").GetComponent<Text>();
			TwoItemTxtDiamondNum = transform2.Find("two_item/txt_diamond_num").GetComponent<Text>();
			TwoItemTxtCoinNum = transform2.Find("two_item/txt_coin_num").GetComponent<Text>();
			ThreeItemTxtDiamondNum = transform2.Find("three_item/txt_diamond_num").GetComponent<Text>();
			ThreeItemTxtCoinNum = transform2.Find("three_item/txt_coin_num").GetComponent<Text>();
			TxtPay = transform2.Find("btn_pay/txt_cotent").GetComponent<Text>();
			TxtDiscontPay = transform2.Find("txt_cotent_count").GetComponent<Text>();
			TxtDiscont = transform2.Find("DisCount/txtDisCount").GetComponent<Text>();
			TxtRemaintime = transform2.Find("txt_time_title/txtTime").GetComponent<Text>();
			TwoItemTran = transform2.Find("two_item");
			ThreeItemTran = transform2.Find("three_item");
			ApplicationFacade.Instance.RegisterMediator(new ShopBestOrderMediator("BESTORDER", this));
		}

		public override void OnRemove()
		{
			base.OnRemove();
			ApplicationFacade.Instance.RemoveMediator("BESTORDER");
		}
	}
}
