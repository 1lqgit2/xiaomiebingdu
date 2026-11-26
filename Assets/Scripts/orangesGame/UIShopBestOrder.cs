using UnityEngine;
using UnityEngine.UI;

namespace orangesGame
{
	public class UIShopBestOrder : MonoBehaviour
	{
		public Button BtnPay;

		public Text TwoItemTxtDiamondNum;

		public Text TwoItemTxtCoinNum;

		public Text ThreeItemTxtDiamondNum;

		public Text ThreeItemTxtCoinNum;

		public Text TxtPay;

		public Text TxtDiscontPay;

		public Text TxtDiscont;

		public Transform TwoItemTran;

		public Transform ThreeItemTran;

		public Text TxtRemaintime;

		private string _buykey = string.Empty;

		private bool _isInit;

		public void Init(TimeGiftStruct data)
		{
			_buykey = data.ShopID;
			if (!_isInit)
			{
				_isInit = true;
				BtnPay.onClick.AddListener(ClickBuyTimeGift);
			}
			TwoItemTran.gameObject.SetActive(data.Types.Length <= 2);
			ThreeItemTran.gameObject.SetActive(data.Types.Length > 2);
			if (data.Types.Length > 2)
			{
				ThreeItemTxtDiamondNum.text = data.Counts[0];
				ThreeItemTxtCoinNum.text = data.Counts[1];
			}
			else
			{
				TwoItemTxtDiamondNum.text = data.Counts[0];
				TwoItemTxtCoinNum.text = data.Counts[1];
			}
			TxtDiscont.text = data.Discount * 100.0 + "%OFF";
			TxtDiscontPay.text = "$" + data.Cost;
			TxtPay.text = "$" + data.DisCost;
			if (data.ID != -1)
			{
				long timeLimitGiftTime = GameGlobalData.ShopData.GetTimeLimitGiftTime(data.ID);
				TxtRemaintime.text = GameUtils.GetTimeUnixDate(timeLimitGiftTime);
			}
		}

		private void ClickBuyTimeGift()
		{
			Singleton<SdkUtils>.Instance.GooglePay(_buykey);
		}
	}
}
