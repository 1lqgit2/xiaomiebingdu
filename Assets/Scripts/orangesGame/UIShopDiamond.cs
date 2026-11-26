using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace orangesGame
{
	public class UIShopDiamond : MonoBehaviour
	{
		public Button[] btnDiamonds;

		public Button[] btnDiamondsAds;

		public Text[] txtDiamondCounts;

		public Text[] txtDiamondCosts;

		public Text[] txtDiamondDiscounts;

		public Text[] txtAdsDiamondCosts;

		public Text[] txtAdsDiamondRemian;

		private bool _isInit;

		public void Init(DiamondGiftStruct[] data, UnityAction<int> action, UnityAction<int> adsAction)
		{
			if (_isInit)
			{
				return;
			}
			_isInit = true;
			if (data.Length == btnDiamonds.Length)
			{
				for (int i = 0; i < data.Length; i++)
				{
					txtDiamondCounts[i].text = GameUtils.UnitToFloat(data[i].Counts) + GameUtils.UnitToFloat(data[i].Free) + string.Empty;
					txtDiamondCosts[i].text = "$" + data[i].Cost;
					int index = i;
					btnDiamonds[i].onClick.AddListener(delegate
					{
						action(index);
					});
				}
			}
			else
			{
				UnityEngine.Debug.LogError("DiamondGiftStruct Length is match prefab");
			}
			InitAdsDiamond(adsAction);
		}

		private void InitAdsDiamond(UnityAction<int> action)
		{
			for (int i = 0; i < btnDiamondsAds.Length; i++)
			{
				txtAdsDiamondCosts[i].text = Singleton<ShopManager>.Instance.GetAdsDiamondNum(i).ToString();
				ResetAdsDiamond(i);
				int index = i;
				btnDiamondsAds[i].onClick.AddListener(delegate
				{
					action(index);
				});
			}
		}

		public void ResetAdsDiamond(int index)
		{
			txtAdsDiamondRemian[index].text = Singleton<ShopManager>.Instance.GetShowAdsTimes(index).ToString() + " / " + Singleton<ShopManager>.Instance.GetAdsToBuyTimes(index);
		}
	}
}
