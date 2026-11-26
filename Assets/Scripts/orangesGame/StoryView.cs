using UnityEngine;
using UnityEngine.UI;

namespace orangesGame
{
	public class StoryView : UIView
	{
		public UIBackgroundItem item;

		public Button btnClick;

		public Text txtClick;

		public Text txtContent;

		public override void OnLoad()
		{
			base.OnLoad();
			Transform layerTran = Singleton<UIViewManager>.Instance.GetLayerTran(100);
			CreateView("GameUI/StoryView/ui_storyview", layerTran);
		}

		public override void OnInit()
		{
			base.OnInit();
			Transform transform = base.Root.transform;
			item = transform.Find("root_uiitem/ui_bg_1").GetComponent<UIBackgroundItem>();
			txtContent = transform.Find("txt_content").GetComponent<Text>();
			btnClick = transform.Find("btn_click").GetComponent<Button>();
			txtClick = btnClick.transform.Find("txt_content").GetComponent<Text>();
			ApplicationFacade.Instance.RegisterMediator(new StoryViewMediator("STORY", this));
		}

		public override void OnRemove()
		{
			base.OnRemove();
			ApplicationFacade.Instance.RemoveMediator("STORY");
		}
	}
}
