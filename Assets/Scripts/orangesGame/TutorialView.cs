using UnityEngine;
using UnityEngine.UI;

namespace orangesGame
{
	public class TutorialView : UIView
	{
		public RectTransform rectTranBg;

		public RectTransform rectTranFinger;

		public RectTransform rectTranFingerRow;

		public RectTransform rectTranTip;

		public Transform tranGamingTip;

		public RectTransform tranPlane;

		public Text txtTip;

		public Button btnComfirn;

		public override void OnLoad()
		{
			base.OnLoad();
			Transform layerTran = Singleton<UIViewManager>.Instance.GetLayerTran(90);
			CreateView("GameUI/TutorialView/ui_tutorialview", layerTran);
		}

		public override void OnInit()
		{
			base.OnInit();
			Transform transform = base.Root.transform;
			tranGamingTip = transform.Find("TranGamingTip");
			rectTranBg = (transform.Find("ImgBg") as RectTransform);
			rectTranFinger = (transform.Find("TranFinger") as RectTransform);
			rectTranFingerRow = (transform.Find("TranFinger2") as RectTransform);
			rectTranTip = (transform.Find("ImgTip") as RectTransform);
			txtTip = rectTranTip.Find("TxtTip").GetComponent<Text>();
			tranPlane = (tranGamingTip.Find("ImgMark/ImgPlane") as RectTransform);
			btnComfirn = tranGamingTip.Find("BtnComfirn").GetComponent<Button>();
			ApplicationFacade.Instance.RegisterMediator(new TutorialViewMediator("TUTORIAL", this));
		}

		public override void OnRegister()
		{
			base.OnRegister();
		}

		public override void OnRemove()
		{
			base.OnRemove();
			ApplicationFacade.Instance.RemoveMediator("TUTORIAL");
		}
	}
}
