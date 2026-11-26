using UnityEngine;
using UnityEngine.UI;

namespace orangesGame
{
	public class LuckyBoxView : UIView
	{
		public Transform tranBox;

		public Button btnClose;

		public Button btnBuy;

		public Text txtContent;

		public Text txtCost;

		public Image imgProgress;

		public Image img_en;

		public Image img_cn;

		public override void OnLoad()
		{
			base.OnLoad();
			Transform layerTran = Singleton<UIViewManager>.Instance.GetLayerTran(90);
			CreateView("GameUI/LuckyBox/ui_luckyboxview", layerTran);
		}

		public override void OnInit()
		{
			base.OnInit();
			Transform transform = base.Root.transform;
			btnClose = transform.Find("btn_close").GetComponent<Button>();
			tranBox = transform.Find("root_box");
			btnBuy = tranBox.Find("btn_buy").GetComponent<Button>();
			txtCost = btnBuy.GetComponentInChildren<Text>();
			txtContent = tranBox.Find("txt_content").GetComponent<Text>();
			imgProgress = tranBox.Find("img_progress_mask").GetComponent<Image>();
			img_en = tranBox.Find("img_bg_0/img_bg_1").GetComponent<Image>();
			img_cn = tranBox.Find("img_bg_0/img_bg_2").GetComponent<Image>();
			ApplicationFacade.Instance.RegisterMediator(new LuckyBoxMediator("LUCKYBOX", this));
		}

		public override void OnRemove()
		{
			base.OnRemove();
			ApplicationFacade.Instance.RemoveMediator("LUCKYBOX");
		}
	}
}
