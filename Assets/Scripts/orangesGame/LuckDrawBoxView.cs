using UnityEngine;
using UnityEngine.UI;

namespace orangesGame
{
	public class LuckDrawBoxView : UIView
	{
		public Button btnConfirm;

		public Transform TreasureBox;

		public Transform BoxItemTransform;

		public Transform AnimationTransform;

		public Text[] ItemNumsTexts = new Text[8];

		public Text TitleText;

		public Text btn_conofirm_text;

		public override void OnLoad()
		{
			base.OnLoad();
			Transform layerTran = Singleton<UIViewManager>.Instance.GetLayerTran(90);
			CreateView("GameUI/LuckyDrawBox/ui_luckydrawboxview", layerTran);
		}

		public override void OnInit()
		{
			base.OnInit();
			Transform transform = base.Root.transform;
			btnConfirm = transform.Find("boxItemPanel/btn_conofirm").GetComponent<Button>();
			TreasureBox = transform.Find("TreasureBox");
			BoxItemTransform = transform.Find("boxItemPanel");
			TitleText = BoxItemTransform.Find("txtTitle").GetComponent<Text>();
			AnimationTransform = BoxItemTransform.Find("animationParent");
			btn_conofirm_text = btnConfirm.transform.Find("Text").GetComponent<Text>();
			for (int i = 0; i < 8; i++)
			{
				ItemNumsTexts[i] = BoxItemTransform.Find("animationParent/itemList/item" + (i + 1) + "/nums").GetComponent<Text>();
			}
			TitleText.text = LanguageConfig.LUCKYDRAW;
			btn_conofirm_text.text = LanguageConfig.LUCKYDRAW_CONFIRM;
			ApplicationFacade.Instance.RegisterMediator(new LuckDrawBoxMediator("LUCKYDRAWBOX", this));
		}

		public override void OnRemove()
		{
			base.OnRemove();
			ApplicationFacade.Instance.RemoveMediator("LUCKYBOX");
		}
	}
}
