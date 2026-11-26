using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace orangesGame
{
	public class UIShopRecommend : MonoBehaviour
	{
		public Button btnMember;

		public Button btnMemberDetail;

		public Text txtMemberTime;

		public Button btnNoAds;

		public Image imgNoAdsBtnBg;

		public Text txtNoAdsCost;

		private bool _isInit;

		private string _noAdsCost;

		public void Init(MenberGiftStruct[] datas, UnityAction actionMenber, UnityAction actionNoAds)
		{
			if (!_isInit)
			{
				_isInit = true;
				_noAdsCost = "$" + datas[0].Cost;
				txtNoAdsCost.text = _noAdsCost;
				btnNoAds.onClick.AddListener(actionNoAds);
				btnMemberDetail.onClick.AddListener(actionMenber);
				btnMember.onClick.AddListener(actionMenber);
			}
			txtMemberTime.text = ((!Singleton<VipManager>.Instance.VipStatus) ? string.Empty : ("Valid until " + Singleton<VipManager>.Instance.GetTimeUnixDate(Singleton<VipManager>.Instance.VipDate)));
			SetNoAdsEnable(!GameGlobalData.ShopData.isNoAds);
		}

		public void SetNoAdsEnable(bool isEnable)
		{
			if (isEnable)
			{
				if (!btnNoAds.enabled)
				{
					btnNoAds.enabled = true;
					btnNoAds.image.material = null;
					imgNoAdsBtnBg.material = null;
					txtNoAdsCost.text = _noAdsCost;
				}
			}
			else if (btnNoAds.enabled)
			{
				btnNoAds.enabled = false;
				btnNoAds.image.material = GameUtils.GetGrayMaterial();
				imgNoAdsBtnBg.material = btnNoAds.image.material;
				txtNoAdsCost.text = LanguageConfig.SOLD_OUT;
			}
		}
	}
}
