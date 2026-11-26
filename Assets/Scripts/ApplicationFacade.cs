using orangesGame;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;

public class ApplicationFacade : Facade
{
	public new static IFacade Instance
	{
		get
		{
			if (Facade.m_instance == null)
			{
				lock (Facade.m_staticSyncRoot)
				{
					if (Facade.m_instance == null)
					{
						Facade.m_instance = new ApplicationFacade();
					}
				}
			}
			return Facade.m_instance;
		}
	}

	protected ApplicationFacade()
	{
	}

	static ApplicationFacade()
	{
	}

	protected override void InitializeFacade()
	{
		base.InitializeFacade();
	}

	protected override void InitializeModel()
	{
		base.InitializeModel();
		RegisterProxy(new GameProxy("GAMING"));
		RegisterProxy(new MainProxy("MAIN"));
		RegisterProxy(new PropertyProxy("PROPERTY"));
	}

	protected override void InitializeController()
	{
		base.InitializeController();
		Type typeFromHandle = typeof(GameCommand);
		RegisterCommand("GAME_MAIN", typeFromHandle);
		RegisterCommand("GAME_START", typeFromHandle);
		RegisterCommand("GAME_PASS", typeFromHandle);
		RegisterCommand("GAME_FAIL", typeFromHandle);
		RegisterCommand("GAME_CONTINUE", typeFromHandle);
		RegisterCommand("GAME_RESULT", typeFromHandle);
		RegisterCommand("GAME_AD_UPGRADE", typeFromHandle);
		typeFromHandle = typeof(PropertyCommand);
		RegisterCommand("MAIN_PORPERTY_UPGRADE", typeFromHandle);
		RegisterCommand("WEAPON_PORPERTY_UPGRADE", typeFromHandle);
		RegisterCommand("COIN_PORPERTY_UPGRADE", typeFromHandle);
		RegisterCommand("MAIN_PORPERTY_UPGRADE_FREE", typeFromHandle);
		RegisterCommand("WEAPON_PORPERTY_UPGRADE_FREE", typeFromHandle);
		RegisterCommand("COIN_PORPERTY_UPGRADE_FREE", typeFromHandle);
		RegisterCommand("PORPERTY_SHOW", typeFromHandle);
		RegisterCommand("PORPERTY_UPGRADE_FREE_FAIL", typeFromHandle);
		typeFromHandle = typeof(TutorialCommand);
		RegisterCommand("TUTORIAL_SET_STEP", typeFromHandle);
		typeFromHandle = typeof(DialogueCommand);
		RegisterCommand("DIALOGUE_CHECK_START", typeFromHandle);
		RegisterCommand("DIALOGUE_CHECK", typeFromHandle);
		typeFromHandle = typeof(DailyTaskCommand);
		RegisterCommand("TASK_GETREWARD", typeFromHandle);
		RegisterCommand("TASK_UPGRADE", typeFromHandle);
		RegisterCommand("TASK_GETREWARD_FINAL", typeFromHandle);
		RegisterCommand("TASK_COIN_MULT", typeFromHandle);
	}
}
