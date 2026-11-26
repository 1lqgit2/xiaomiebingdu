using PureMVC.Interfaces;
using PureMVC.Patterns;

namespace orangesGame
{
	public class PropertyCommand : SimpleCommand, ICommand
	{
		public override void Execute(INotification notification)
		{
			switch (notification.Name)
			{
			case "PORPERTY_SHOW":
			{
				PropertyProxy propertyProxy = base.Facade.RetrieveProxy("PROPERTY") as PropertyProxy;
				if (GameGlobalData.CurLevel == 5 && GameGlobalData.WeaponID == -1)
				{
					GameGlobalData.SetWeaponID(0);
					GameGlobalData.PreWeaponID = GameGlobalData.WeaponID;
					GameGlobalData.SetOpenWeaponCount(1);
					Singleton<PlayerManager>.Instance.ChangeLeaderWeapon();
					SendNotification("PROPERTY_NEW_WEAPON_TIP");
					propertyProxy.isInit = false;
				}
				else
				{
					propertyProxy.SetOpenWeaopon();
				}
				if (GameGlobalData.WeaponID >= 0)
				{
					if (propertyProxy.isInit)
					{
						if (propertyProxy.IsBossLevelUse())
						{
							propertyProxy.SetMaxWeaponState();
						}
						if (GameGlobalData.TempWeaponID < 0)
						{
							propertyProxy.SetFreeUpGrade();
						}
						else
						{
							GameGlobalData.FreeUpGradeIndex = -1;
						}
					}
					else
					{
						propertyProxy.isInit = true;
					}
				}
				Singleton<UIViewManager>.Instance.ShowView("PROPERTY_VIEW");
				SendNotification("PROPERTY_INIT");
				break;
			}
			case "MAIN_PORPERTY_UPGRADE":
			{
				MainProxy mainProxy7 = base.Facade.RetrieveProxy("MAIN") as MainProxy;
				mainProxy7.MainUpGradeByCoin(notification.Body);
				SendNotification("PROPERTY_REFRESH_VIEW");
				break;
			}
			case "WEAPON_PORPERTY_UPGRADE":
			{
				MainProxy mainProxy6 = base.Facade.RetrieveProxy("MAIN") as MainProxy;
				mainProxy6.WeaponUpGradeByCoin(notification.Body);
				SendNotification("PROPERTY_REFRESH_VIEW");
				break;
			}
			case "MAIN_PORPERTY_UPGRADE_FREE":
			{
				MainProxy mainProxy5 = base.Facade.RetrieveProxy("MAIN") as MainProxy;
				mainProxy5.MainUpGradeByFree(notification.Body);
				SendNotification("PROPERTY_REFRESH_VIEW");
				break;
			}
			case "WEAPON_PORPERTY_UPGRADE_FREE":
			{
				MainProxy mainProxy4 = base.Facade.RetrieveProxy("MAIN") as MainProxy;
				mainProxy4.WeaponUpGradeByFree(notification.Body);
				SendNotification("PROPERTY_REFRESH_VIEW");
				break;
			}
			case "PORPERTY_UPGRADE_FREE_FAIL":
			{
				MainProxy mainProxy3 = base.Facade.RetrieveProxy("MAIN") as MainProxy;
				mainProxy3.SetFreeUpGrade();
				SendNotification("PROPERTY_REFRESH_VIEW");
				break;
			}
			case "COIN_PORPERTY_UPGRADE":
			{
				MainProxy mainProxy2 = base.Facade.RetrieveProxy("MAIN") as MainProxy;
				MainViewMediator mainViewMediator = base.Facade.RetrieveMediator("MAIN") as MainViewMediator;
				MainView mainView = (MainView)mainViewMediator.ViewComponent;
				mainProxy2.upgradeCoinEffPos = mainView.btnOffline.transform.position;
				mainProxy2.CoinUpGradeByCoin(notification.Body);
				SendNotification("PROPERTY_REFRESH_VIEW");
				break;
			}
			case "COIN_PORPERTY_UPGRADE_FREE":
			{
				MainProxy mainProxy = base.Facade.RetrieveProxy("MAIN") as MainProxy;
				mainProxy.CoinUpGradeByFree(notification.Body);
				SendNotification("PROPERTY_REFRESH_VIEW");
				break;
			}
			}
		}
	}
}
