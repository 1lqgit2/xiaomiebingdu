using PureMVC.Interfaces;
using PureMVC.Patterns;

namespace orangesGame
{
	public class TutorialCommand : SimpleCommand, ICommand
	{
		public override void Execute(INotification notification)
		{
			string name = notification.Name;
			if (name == null || !(name == "TUTORIAL_SET_STEP"))
			{
				return;
			}
			TutorialType tutorialState = (TutorialType)GameGlobalData.TutorialState;
			switch (tutorialState)
			{
			case TutorialType.HowToPlay:
				Singleton<UIViewManager>.Instance.ShowView("TUTORIAL_VIEW", "TUTORIAL_GAMING_TIP");
				return;
			case TutorialType.OpenUpGradeView:
				if (GameGlobalData.ActualLevel == 2)
				{
					FirePowerStruct firePowerData = GameGlobalData.GetFirePowerData();
					float fireCost = GameGlobalData.GetFireCost(firePowerData);
					if (GameGlobalData.CurCoin < fireCost)
					{
						GameGlobalData.SetCurCoin(fireCost);
						SendNotification("PROPERTY_REFRESH_VIEW");
					}
					PropertyViewMediator propertyViewMediator = base.Facade.RetrieveMediator("PROPERTY") as PropertyViewMediator;
					Singleton<UIViewManager>.Instance.ShowView("TUTORIAL_VIEW");
					PropertyView propertyView = (PropertyView)propertyViewMediator.ViewComponent;
					SendNotification("TUTORIAL_OPEN_UPGAREDVIEW", new object[2]
					{
						propertyView.btnPropertys[0],
						propertyView.mainPropertyPos
					});
					return;
				}
				break;
			}
			if (tutorialState == TutorialType.UpGradePower)
			{
				PropertyViewMediator propertyViewMediator2 = base.Facade.RetrieveMediator("PROPERTY") as PropertyViewMediator;
				PropertyView propertyView2 = (PropertyView)propertyViewMediator2.ViewComponent;
				SendNotification("TUTORIAL_UPGARED_POWER", propertyView2.btnMainUpGrades[1]);
			}
		}
	}
}
