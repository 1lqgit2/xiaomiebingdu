using UnityEngine;
using UnityEngine.UI;

namespace orangesGame
{
	public class LevelView : UIView
	{
		public LevelViewComponent levelViewComponent;

		public Button btnClose;

		public ScrollRect srlRect;

		public LevelNewItemComponent[] levelItems;

		public override void OnLoad()
		{
			base.OnLoad();
			Transform layerTran = Singleton<UIViewManager>.Instance.GetLayerTran(90);
			CreateView("GameUI/LevelView/ui_newlevelview", layerTran);
		}

		public override void OnInit()
		{
			base.OnInit();
			Transform transform = base.Root.transform;
			btnClose = transform.Find("btn_close").GetComponent<Button>();
			levelViewComponent = transform.GetComponent<LevelViewComponent>();
			srlRect = transform.Find("slv_list").GetComponent<ScrollRect>();
			levelItems = transform.Find("slv_list/slv_viewport/slv_content").GetComponentsInChildren<LevelNewItemComponent>();
			ApplicationFacade.Instance.RegisterMediator(new LevelViewMediator("LEVEL", this));
		}

		public override void OnRemove()
		{
			base.OnRemove();
			ApplicationFacade.Instance.RemoveMediator("LEVEL");
		}
	}
}
