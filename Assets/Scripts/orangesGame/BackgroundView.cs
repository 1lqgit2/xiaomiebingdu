using UnityEngine;
using UnityEngine.UI;

namespace orangesGame
{
	public class BackgroundView : UIView
	{
		public RectTransform rootItem;

		public Image rimgBgLong;

		public UIBackgroundItem item;

		public override void OnLoad()
		{
			base.OnLoad();
			Transform layerTran = Singleton<UIViewManager>.Instance.GetLayerTran(1);
			CreateView("GameUI/BackgroundView/ui_backgroundview", layerTran);
		}

		public override void OnInit()
		{
			base.OnInit();
			Transform transform = base.Root.transform;
			rimgBgLong = transform.Find("img_bg_0").GetComponent<Image>();
			rootItem = (transform.Find("root_uiitem") as RectTransform);
			ApplicationFacade.Instance.RegisterMediator(new BackgroundViewMediator("BACKGROUND", this));
		}

		public override void OnRemove()
		{
			base.OnRemove();
			ApplicationFacade.Instance.RegisterMediator(new BackgroundViewMediator("BACKGROUND", this));
		}
	}
}
