using UnityEngine;
using UnityEngine.UI;

namespace orangesGame
{
	public class DailyTaskView : UIView
	{
		public Transform tranTask;

		public Button btnClose;

		public Text txtTime;

		public Text txtTitle;

		public Text txtTimeTitle;

		public override void OnLoad()
		{
			base.OnLoad();
			Transform layerTran = Singleton<UIViewManager>.Instance.GetLayerTran(80);
			CreateView("GameUI/DailyTask/ui_dailytaskview", layerTran);
		}

		public override void OnInit()
		{
			base.OnInit();
			Transform transform = base.Root.transform;
			tranTask = transform.Find("TranTask");
			btnClose = tranTask.Find("BtnClose").GetComponent<Button>();
			txtTime = tranTask.Find("TxtTime").GetComponent<Text>();
			txtTitle = tranTask.Find("TxtTitle").GetComponent<Text>();
			txtTimeTitle = tranTask.Find("TxtTimeTitle").GetComponent<Text>();
			ApplicationFacade.Instance.RegisterMediator(new DailyTaskViewMediator("DAILYTASK", this));
		}

		public override void OnRemove()
		{
			base.OnRemove();
			ApplicationFacade.Instance.RemoveMediator("DAILYTASK");
		}
	}
}
