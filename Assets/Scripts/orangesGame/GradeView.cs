using UnityEngine;
using UnityEngine.UI;

namespace orangesGame
{
	public class GradeView : UIView
	{
		public Button[] btnStars;

		public Image[] imgStars;

		public Button btnExit;

		public Button btnConfirm;

		public Text txtContent;

		public RectTransform tranStar;

		public RectTransform tranContent;

		public override void OnLoad()
		{
			base.OnLoad();
			Transform layerTran = Singleton<UIViewManager>.Instance.GetLayerTran(90);
			CreateView("GameUI/GradeView/ui_gradeview", layerTran);
		}

		public override void OnInit()
		{
			base.OnInit();
			Transform transform = base.Root.transform;
			tranStar = transform.Find("tranStar").GetComponent<RectTransform>();
			tranContent = transform.Find("tranContent").GetComponent<RectTransform>();
			btnExit = transform.Find("btnCancel").GetComponent<Button>();
			btnConfirm = transform.Find("btnSubmit").GetComponent<Button>();
			btnStars = new Button[5];
			imgStars = new Image[5];
			for (int i = 1; i <= 5; i++)
			{
				btnStars[i - 1] = tranStar.Find("stars/starBtn/btn" + i).GetComponent<Button>();
				imgStars[i - 1] = tranStar.Find("stars/starBtn/btn" + i + "/star").GetComponent<Image>();
			}
			txtContent = tranContent.Find("inputContent/inputText").GetComponent<Text>();
			ApplicationFacade.Instance.RegisterMediator(new GradeViewMediator("GRADE", this));
		}

		public override void OnRegister()
		{
			base.OnRegister();
		}

		public override void OnRemove()
		{
			base.OnRemove();
			ApplicationFacade.Instance.RemoveMediator("GRADE");
		}
	}
}
