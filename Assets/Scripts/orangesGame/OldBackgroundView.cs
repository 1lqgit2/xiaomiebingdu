using UnityEngine;
using UnityEngine.UI;

namespace orangesGame
{
	public class OldBackgroundView : UIView
	{
		public RectTransform[] TranBgs;

		public RawImage RImgBg;

		public override void OnLoad()
		{
			base.OnLoad();
			Transform layerTran = Singleton<UIViewManager>.Instance.GetLayerTran(1);
			CreateView("GameUI/BackgroundView/ui_oldbackgroundview", layerTran);
		}

		public override void OnInit()
		{
			base.OnInit();
			Transform transform = base.Root.transform;
			TranBgs = new RectTransform[1];
			for (int i = 0; i < TranBgs.Length; i++)
			{
				TranBgs[i] = (transform.Find("TranBg" + i) as RectTransform);
			}
			RImgBg = TranBgs[0].GetComponent<RawImage>();
			ApplicationFacade.Instance.RegisterMediator(new OldBackgroundViewMediator("BACKGROUND", this));
		}

		public override void OnRegister()
		{
			base.OnRegister();
		}

		public override void OnRemove()
		{
			base.OnRemove();
			ApplicationFacade.Instance.RegisterMediator(new OldBackgroundViewMediator("BACKGROUND", this));
		}
	}
}
