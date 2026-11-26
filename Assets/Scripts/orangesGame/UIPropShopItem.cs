using UnityEngine;
using UnityEngine.UI;

namespace orangesGame
{
	public class UIPropShopItem : MonoBehaviour
	{
		public Button btnBuy;

		public Text txtCost;

		public Text txtCount;

		public Text txtName;

		public void SetParent(Transform parent)
		{
			Transform transform = base.transform;
			transform.SetParent(parent);
			transform.localScale = Vector3.one;
		}
	}
}
