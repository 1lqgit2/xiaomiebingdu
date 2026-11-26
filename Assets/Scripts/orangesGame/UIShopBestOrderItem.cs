using UnityEngine;
using UnityEngine.UI;

namespace orangesGame
{
	public class UIShopBestOrderItem : MonoBehaviour
	{
		public Button BtnPay;

		public Text[] CostTexts;

		public Text PriceText;

		public Text TitleText;

		public Text ItemTitleText;

		private string _buykey = string.Empty;

		private bool _isInit;

		public void Init(ItemGiftStruct itemGiftStruct)
		{
			if (!_isInit)
			{
				_isInit = true;
				_buykey = itemGiftStruct.ShopID;
				BtnPay.onClick.AddListener(ClickBuyTimeGift);
				for (int i = 0; i < CostTexts.Length; i++)
				{
					CostTexts[i].text = itemGiftStruct.Counts[i];
				}
				PriceText.text = "$" + itemGiftStruct.Cost[0];
				if (TitleText != null)
				{
					TitleText.text = LanguageConfig.Shop_bestOrder_Item;
				}
			}
		}

		private void ClickBuyTimeGift()
		{
			Singleton<SdkUtils>.Instance.GooglePay(_buykey);
		}
	}
}
