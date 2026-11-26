using UnityEngine;
using UnityEngine.UI;

namespace orangesGame
{
	public class TipView : UIView
	{
		public Transform tranAnimTip;

		public Text txtAnimTip;

		public override void OnLoad()
		{
			base.OnLoad();
			Transform layerTran = Singleton<UIViewManager>.Instance.GetLayerTran(130);
			CreateView("GameUI/TipView/ui_tipview", layerTran);
		}

		public override void OnInit()
		{
			base.OnInit();
			Transform transform = base.Root.transform;
			tranAnimTip = transform.Find("root_animtip");
			txtAnimTip = tranAnimTip.Find("img_bg_0/txt_content").GetComponent<Text>();
			ApplicationFacade.Instance.RegisterMediator(new TipViewMediator("TIP", this));
		}

		public override void OnRegister()
		{
			base.OnRegister();
		}

		public override void OnRemove()
		{
			base.OnRemove();
			ApplicationFacade.Instance.RemoveMediator("TIP");
		}
	}
}
