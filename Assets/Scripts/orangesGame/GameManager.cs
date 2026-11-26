using UnityEngine;

namespace orangesGame
{
	public class GameManager : MonoBehaviour
	{
		public UrlController urlController;

		public bool isTest;

		public bool withOutSDK;

		public GameTestState testState;

		public void TestAction(string str)
		{
			if (!string.IsNullOrEmpty(str))
			{
				GameGlobalData.GARDE_URL = str;
				Singleton<UIViewManager>.Instance.ShowView("DIALOG_VIEW");
				ApplicationFacade.Instance.SendNotification("DIALOG_DOWNLOAD");
			}
		}

		private void Awake()
		{
			GameGlobalData.curTime = GameGlobalData.GetTimestamp();
			GameGlobalData.GameTest = testState;
			GameGlobalData.IsGameManagerTest = isTest;
			GameGlobalData.WithOutSDK = withOutSDK;
			Application.targetFrameRate = 60;
			Input.multiTouchEnabled = false;
			if (!withOutSDK && !Singleton<SdkUtils>.Instance.IsCorrect())
			{
				//UnityEngine.Debug.LogError("签名校验错误");
				//Application.Quit();
			}
			//Singleton<AnalyzeSdk>.Instance.IsTestUm = (withOutSDK || Singleton<SdkUtils>.Instance.GetUmTest());
			//Singleton<AnalyzeSdk>.Instance.Init();
			//bool umDebug = !withOutSDK && Singleton<SdkUtils>.Instance.GetUmDebug();
			//Singleton<AnalyzeSdk>.Instance.SetUmDebug(umDebug);
			Singleton<GameJson>.Instance.Init();
			Singleton<LanguageManager>.Instance.Init();
			GameGlobalData.InitPlayerPrefs();
			Singleton<ReloadDataManager>.Instance.GetData();
			string text = "type@启动游戏";
			if (GameGlobalData.IsTutorial())
			{
				text = "type@新手启动游戏";
			}
			//Singleton<AnalyzeSdk>.Instance.OnEventByArgs("GameBaseLoad", new string[1]
			//{
			//	text
			//});
			urlController.Init();
			urlController.GetConfig(GameGlobalData.gamePackName, TestAction);
			//GameAnalysisData.SendLoginStart(GameGlobalData.PlayerType);
            /*
			AdManager instance = Singleton<AdManager>.Instance;
			instance.isDebug = (!withOutSDK && Singleton<SdkUtils>.Instance.GetAdDebug());
			instance.isTest = (withOutSDK || Singleton<SdkUtils>.Instance.GetAdTest());
			if (!instance.isTest)
			{
				instance.InitSdk("1000990", "5S2Rx2yQKM5yzkOgHGJmuw==", "releaseAndoird", "5c80cb16203657f48e0003b9");
				instance.InitInterstitial<AdInterAction>(new string[1]
				{
					"35eeefa8-c011-4112-bc9b-4a04fe3c413c"
				});
				instance.InitVideo<AdVideoAction>(new string[1]
				{
					"0a2cad70-9e1c-450f-bcdc-f3798b324b95"
				});
				instance.CreateSDK(2);
				instance.LoadInterstitial("35eeefa8-c011-4112-bc9b-4a04fe3c413c");
				instance.LoadVideo("0a2cad70-9e1c-450f-bcdc-f3798b324b95");
			}
			Singleton<SdkUtils>.Instance.QueryInventoryAsync();
            */
			UIViewManager instance2 = Singleton<UIViewManager>.Instance;
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				instance2.RegisterView("BACKGROUND_VIEW", new BackgroundView());
			}
			else
			{
				instance2.RegisterView("BACKGROUND_VIEW", new OldBackgroundView());
			}
            
			instance2.RegisterView("MAIN_VIEW", new MainView());
			instance2.RegisterView("BATTLE_VIEW", new BattleView());
			instance2.RegisterView("RESULT_VIEW", new ResultView());
			instance2.RegisterView("PROPERTY_VIEW", new PropertyView());
			instance2.RegisterView("DIALOG_VIEW", new DialogView());
			instance2.RegisterView("GRADE_VIEW", new GradeView());
			instance2.RegisterView("TASK_VIEW", new DailyTaskView());
			instance2.RegisterView("TIP_VIEW", new TipView());
			instance2.RegisterView("CONVERSION_VIEW", new ConversionView());
			instance2.RegisterView("SHOP_VIEW", new ShopView());
			instance2.RegisterView("VIP_VIEW", new VipView());
			instance2.RegisterView("SHOP_BEST_ORDER", new ShopBestOrderView());
			instance2.RegisterView("LUCKYBOX_VIEW", new LuckyBoxView());
			instance2.RegisterView("PROPSHOP_VIEW", new PropShopView());
			instance2.RegisterView("LUCK_BOX_DRAW_VIEW", new LuckDrawBoxView());
			instance2.RegisterView("DIALOGUE_VIEW", new DialogueView());
			instance2.RegisterView("LEVEL_VIEW", new LevelView());
			instance2.RegisterView("STORY_VIEW", new StoryView());
			instance2.Init();
			Singleton<AudioManager>.Instance.Init("bgm_1", "coin", "die", "levelGrade", "win", "hit", "fu1", "fu2", "fu3", "fu4", "fu5", "bd1", "bd2", "bd3", "bd4", "bd5", "boss");
			if (GameGlobalData.GameTest != 0)
			{
				Singleton<GameTest>.Instance.Init();
			}
			InitEff();
		}

		private void Start()
		{
			JumpToGame(isDownload: false);
		}

		private void JumpToGame(bool isDownload)
		{
			Singleton<UIViewManager>.Instance.ShowView("BACKGROUND_VIEW", "BG_INIT");
			Singleton<UIViewManager>.Instance.ShowView("TIP_VIEW");
			ApplicationFacade.Instance.SendNotification("GAME_MAIN");
			if (GameGlobalData.GameTest == GameTestState.NoTest)
			{
				Singleton<AudioManager>.Instance.PlayBgm("bgm_1");
			}
			if (!GameGlobalData.IsMusice)
			{
				Singleton<AudioManager>.Instance.SetAllVolume(0f, 0f);
			}
			if (isTest)
			{
				Singleton<GameJson>.Instance.InitTest();
				Singleton<UIViewManager>.Instance.RegisterView("TEST_VIEW", new TestView());
				Singleton<UIViewManager>.Instance.ShowView("TEST_VIEW");
			}
			if (GameGlobalData.IsTutorialNew())
			{
				Singleton<UIViewManager>.Instance.RegisterView("TUTORIAL_VIEW", new TutorialView());
				if (GameGlobalData.TutorialState == 1)
				{
					ApplicationFacade.Instance.SendNotification("TUTORIAL_SET_STEP");
				}
			}
			Singleton<ReloadDataManager>.Instance.SetData();
		}

		private void InitEff()
		{
			Singleton<EffectManager>.Instance.AddEffAction(1u, new BombEffect());
			Singleton<EffectManager>.Instance.AddEffAction(2u, new HitMainEffect());
			Singleton<EffectManager>.Instance.AddEffAction(3u, new HitWeaponFireEffect());
			Singleton<EffectManager>.Instance.AddEffAction(4u, new HitWeaponLaserEffect());
			Singleton<EffectManager>.Instance.AddEffAction(5u, new HitWeaponRocketEffect());
			Singleton<EffectManager>.Instance.AddEffAction(6u, new HitWeaponThunderEffect());
			Singleton<EffectManager>.Instance.AddEffAction(7u, new HitWeaponBoomerangEffect());
		}
	}
}
