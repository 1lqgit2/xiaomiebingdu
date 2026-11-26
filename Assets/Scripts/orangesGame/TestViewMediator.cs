using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine;
using UnityEngine.UI;

namespace orangesGame
{
	public class TestViewMediator : Mediator, IMediator
	{
		private TestView View => (TestView)ViewComponent;

		public TestViewMediator(string name, TestView viewComponent)
			: base(name, viewComponent)
		{
		}

		public override void OnRegister()
		{
			base.OnRegister();
			View.txtVersions.text = Application.version;
			View.btnEnter.onClick.AddListener(OnClickEnter);
			View.btnClose.onClick.AddListener(OnClickClose);
			View.btnVenomAngle.onClick.AddListener(OnClickVenomAngle);
			View.btnWeapon.onClick.AddListener(OnClickWeapon);
			View.btnLevel.onClick.AddListener(OnClickLevel);
			View.btnChangeWeapon.onClick.AddListener(OnClickChangeWeapon);
			View.btnChangeBG.onClick.AddListener(OnClickChangeBG);
			View.btnChangePlayer.onClick.AddListener(OnClickChangePlayer);
			View.btnTestGooglePay.onClick.AddListener(OnClickGoogleClick);
			View.btnDropItem.onClick.AddListener(OnClickItem);
			View.btnEnterAngle.onClick.AddListener(OnClickEnterAngle);
			View.btnEnterWeapon.onClick.AddListener(OnClickEnterWeapon);
			View.btnEnterLevel.onClick.AddListener(OnClickEnterLevel);
			View.btnEnterChange.onClick.AddListener(OnClickEnterChangeWeapon);
			View.btnEnterChangeBG.onClick.AddListener(OnClickEnterChangeBG);
			View.btnEnterChangePlayer.onClick.AddListener(OnClickEnterChangePlayer);
			View.btnEnterItem.onClick.AddListener(OnClickEnterDropItem);
			View.txtGaId.text = GameGlobalData.GetGaid;
			InitVenomAngle();
			InitWeapon();
			InitLevel();
			InitDropItem();
		}

		public override void OnRemove()
		{
			base.OnRemove();
		}

		private void InitVenomAngle()
		{
			int[] array = new int[8]
			{
				GameGlobalData.BarrierBackAngleMin,
				GameGlobalData.BarrierBackAngleMax,
				GameGlobalData.BarrierInitAngleMin,
				GameGlobalData.BarrierInitAngleMax,
				GameGlobalData.BarrierDivideAngleMin,
				GameGlobalData.BarrierDivideAngleMax,
				GameGlobalData.BarrierGreenBallAngleMin,
				GameGlobalData.BarrierGreenBallAngleMax
			};
			for (int i = 0; i < View.inputAngleParams.Length; i++)
			{
				View.inputAngleParams[i].text = array[i].ToString();
			}
			InitDropVenom();
		}

		private void InitDropVenom()
		{
			VenomStruct[] venomJson = Singleton<GameJson>.Instance.VenomJson;
			View.dropVenom.options.Clear();
			for (int i = 0; i < venomJson.Length; i++)
			{
				Dropdown.OptionData optionData = new Dropdown.OptionData();
				optionData.text = i.ToString();
				View.dropVenom.options.Add(optionData);
			}
			View.inputPropertyParams[0].text = venomJson[View.dropVenom.value].MoveSpeed.ToString();
			View.dropVenom.onValueChanged.AddListener(OnDropVenomChanged);
		}

		private void InitWeapon()
		{
			int num = -1;
			int num2 = -1;
			if (GameGlobalData.WeaponID >= 0)
			{
				num = GameGlobalData.WeaponLevels[GameGlobalData.WeaponID * 2];
				num2 = GameGlobalData.WeaponLevels[GameGlobalData.WeaponID * 2 + 1];
			}
			int num3 = (int)Singleton<GameJson>.Instance.CommonJson[0].Value;
			int[] array = new int[8]
			{
				GameGlobalData.FireSpeedLevel,
				GameGlobalData.FirePowerLevel,
				GameGlobalData.WeaponID,
				num,
				num2,
				GameGlobalData.CoinCountLevel,
				GameGlobalData.CoinOffLineLevel,
				num3
			};
			for (int i = 0; i < View.inputWeaponParams.Length; i++)
			{
				View.inputWeaponParams[i].text = (array[i] + 1).ToString();
			}
		}

		private void InitLevel()
		{
			View.inputLevelParams[0].text = (GameGlobalData.CurLevel + 1).ToString();
			View.inputGoldParams[0].text = GameGlobalData.CurCoin.ToString();
		}

		private void InitChangeWeapon()
		{
			View.inputChangeWeaponParams[0].text = (GameGlobalData.WeaponID + 1).ToString();
		}

		private void InitDropItem()
		{
			View.dropItem.options.Clear();
			string[] array = new string[8]
			{
				"病毒减速",
				"射速",
				"火力",
				"金币",
				"飞机减速",
				"病毒加速",
				"病毒放大",
				"僚机"
			};
			for (int i = 0; i < array.Length; i++)
			{
				Dropdown.OptionData optionData = new Dropdown.OptionData();
				optionData.text = array[i];
				View.dropItem.options.Add(optionData);
			}
			View.inputDropItemParams[0].text = array[View.dropItem.value];
			View.dropItem.onValueChanged.AddListener(OnDropItemChanged);
		}

		private void OnClickEnter()
		{
			View.tranMain.gameObject.SetActive(value: true);
			SendNotification("PROPERTY_HIDE_VIEW");
		}

		private void OnClickClose()
		{
			View.tranMain.gameObject.SetActive(value: false);
		}

		private void OnClickVenomAngle()
		{
			View.tranMain.gameObject.SetActive(value: false);
			View.tranVenomAngle.gameObject.SetActive(value: true);
		}

		private void OnClickWeapon()
		{
			View.tranMain.gameObject.SetActive(value: false);
			View.tranWeapon.gameObject.SetActive(value: true);
		}

		private void OnClickLevel()
		{
			View.tranMain.gameObject.SetActive(value: false);
			View.tranLevel.gameObject.SetActive(value: true);
		}

		private void OnClickChangeWeapon()
		{
			View.tranMain.gameObject.SetActive(value: false);
			View.tranChangeWeapon.gameObject.SetActive(value: true);
		}

		private void OnDropVenomChanged(int value)
		{
			VenomStruct[] venomJson = Singleton<GameJson>.Instance.VenomJson;
			View.inputPropertyParams[0].text = venomJson[value].MoveSpeed.ToString();
		}

		private void OnClickChangeBG()
		{
			View.tranMain.gameObject.SetActive(value: false);
			View.tranChangeBG.gameObject.SetActive(value: true);
		}

		private void OnClickItem()
		{
			View.tranMain.gameObject.SetActive(value: false);
			View.tranDropItem.gameObject.SetActive(value: true);
		}

		private void OnDropItemChanged(int value)
		{
			string[] array = new string[8]
			{
				"病毒减速",
				"射速",
				"火力",
				"金币",
				"飞机减速",
				"病毒加速",
				"病毒放大",
				"僚机"
			};
			View.inputDropItemParams[0].text = array[value];
		}

		private void OnClickGoogleClick()
		{
			UnityEngine.Debug.Log("[TestView]OnClickGoogleClick");
			Singleton<SdkUtils>.Instance.GooglePay("diamond_membership", "subs");
		}

		private void OnClickChangePlayer()
		{
			View.tranMain.gameObject.SetActive(value: false);
			View.tranChangePlayer.gameObject.SetActive(value: true);
		}

		private void OnClickEnterAngle()
		{
			GameGlobalData.BarrierBackAngleMin = int.Parse(View.inputAngleParams[0].text);
			GameGlobalData.BarrierBackAngleMax = int.Parse(View.inputAngleParams[1].text);
			GameGlobalData.BarrierInitAngleMin = int.Parse(View.inputAngleParams[2].text);
			GameGlobalData.BarrierInitAngleMax = int.Parse(View.inputAngleParams[3].text);
			GameGlobalData.BarrierDivideAngleMin = int.Parse(View.inputAngleParams[4].text);
			GameGlobalData.BarrierDivideAngleMax = int.Parse(View.inputAngleParams[5].text);
			GameGlobalData.BarrierGreenBallAngleMin = int.Parse(View.inputAngleParams[6].text);
			GameGlobalData.BarrierGreenBallAngleMax = int.Parse(View.inputAngleParams[7].text);
			VenomStruct[] venomJson = Singleton<GameJson>.Instance.VenomJson;
			venomJson[View.dropVenom.value].MoveSpeed = double.Parse(View.inputPropertyParams[0].text);
			View.tranMain.gameObject.SetActive(value: true);
			View.tranVenomAngle.gameObject.SetActive(value: false);
		}

		private void OnClickEnterWeapon()
		{
			GameGlobalData.FireSpeedLevel = int.Parse(View.inputWeaponParams[0].text);
			PlayerPrefs.SetInt("FireSpeedLevel", GameGlobalData.FireSpeedLevel);
			GameGlobalData.FirePowerLevel = int.Parse(View.inputWeaponParams[1].text);
			PlayerPrefs.SetInt("FirePowerLevel", GameGlobalData.FirePowerLevel);
			int num = int.Parse(View.inputWeaponParams[2].text);
			if (num >= 0)
			{
				GameGlobalData.PreWeaponID = num;
				GameGlobalData.SetWeaponID(num);
				int num2 = int.Parse(View.inputWeaponParams[3].text);
				num2 = ((num2 >= 0) ? num2 : 0);
				int num3 = int.Parse(View.inputWeaponParams[4].text);
				num3 = ((num3 >= 0) ? num3 : 0);
				int num4 = num * 2;
				int num5 = num * 2 + 1;
				GameGlobalData.WeaponLevels[num4] = num2;
				PlayerPrefs.SetInt("WeaponLevel_" + num4, GameGlobalData.WeaponLevels[num4]);
				GameGlobalData.WeaponLevels[num5] = num3;
				PlayerPrefs.SetInt("WeaponLevel_" + num5, GameGlobalData.WeaponLevels[num5]);
				Singleton<PlayerManager>.Instance.ChangeLeaderWeapon();
			}
			GameGlobalData.CoinCountLevel = int.Parse(View.inputWeaponParams[5].text);
			PlayerPrefs.SetInt("CoinCountLevel", GameGlobalData.CoinCountLevel);
			GameGlobalData.CoinOffLineLevel = int.Parse(View.inputWeaponParams[6].text);
			PlayerPrefs.SetInt("CoinOffLineLevel", GameGlobalData.CoinOffLineLevel);
			Singleton<GameJson>.Instance.CommonJson[0].Value = int.Parse(View.inputWeaponParams[7].text);
			View.tranMain.gameObject.SetActive(value: true);
			View.tranWeapon.gameObject.SetActive(value: false);
			RefreshGame();
		}

		private void OnClickEnterLevel()
		{
			GameGlobalData.CurLevel = int.Parse(View.inputLevelParams[0].text) - 1;
			PlayerPrefs.SetInt("CurLevel", GameGlobalData.CurLevel);
			if (GameGlobalData.WeaponID < 0)
			{
				GameGlobalData.SetWeaponID(0);
				Singleton<PlayerManager>.Instance.ChangeLeaderWeapon();
			}
			int num = int.Parse(View.inputGoldParams[0].text);
			GameGlobalData.SetCurCoin(num);
			TestWeaponStruct testWeaponStruct = Singleton<GameJson>.Instance.TestWeaponJson[GameGlobalData.CurLevel];
			GameGlobalData.FireSpeedLevel = testWeaponStruct.Speed - 1;
			PlayerPrefs.SetInt("FireSpeedLevel", GameGlobalData.FireSpeedLevel);
			GameGlobalData.FirePowerLevel = testWeaponStruct.Power - 1;
			PlayerPrefs.SetInt("FirePowerLevel", GameGlobalData.FirePowerLevel);
			int weaponID = GameGlobalData.WeaponID;
			if (weaponID >= 0)
			{
				GameGlobalData.SetWeaponID(weaponID);
				int num2 = testWeaponStruct.WeaponLevel - 1;
				num2 = ((num2 >= 0) ? num2 : 0);
				int num3 = testWeaponStruct.Power - 1;
				num3 = ((num3 >= 0) ? num3 : 0);
				int num4 = weaponID * 2;
				int num5 = weaponID * 2 + 1;
				GameGlobalData.WeaponLevels[num4] = num2;
				PlayerPrefs.SetInt("WeaponLevel_" + num4, GameGlobalData.WeaponLevels[num4]);
				GameGlobalData.WeaponLevels[num5] = num3;
				PlayerPrefs.SetInt("WeaponLevel_" + num5, GameGlobalData.WeaponLevels[num5]);
				Singleton<PlayerManager>.Instance.ChangeLeaderWeapon();
			}
			GameGlobalData.PreWeaponID = GameGlobalData.WeaponID;
			GameGlobalData.CoinCountLevel = testWeaponStruct.Coin - 1;
			PlayerPrefs.SetInt("CoinCountLevel", GameGlobalData.CoinCountLevel);
			InitWeapon();
			View.tranMain.gameObject.SetActive(value: true);
			View.tranLevel.gameObject.SetActive(value: false);
			RefreshGame();
		}

		private void OnClickEnterChangeWeapon()
		{
			View.tranMain.gameObject.SetActive(value: true);
			View.tranChangeWeapon.gameObject.SetActive(value: false);
			if (!string.IsNullOrEmpty(View.inputChangeWeaponParams[0].text))
			{
				int num = int.Parse(View.inputChangeWeaponParams[0].text) - 1;
				if (num >= 0)
				{
					int tempWeaponID = GameGlobalData.TempWeaponID;
					int maxWeaponState = GameGlobalData.MaxWeaponState;
					GameGlobalData.TempWeaponID = num;
					GameGlobalData.SetWeaponID(num, isSave: false);
					GameGlobalData.MaxWeaponState = 1;
					Singleton<PlayerManager>.Instance.PauseLeaderWeapon();
					Singleton<PlayerManager>.Instance.ChangeLeaderWeapon();
					GameGlobalData.TempWeaponID = num;
					GameGlobalData.MaxWeaponState = maxWeaponState;
				}
			}
		}

		private void OnClickEnterChangeBG()
		{
			View.tranMain.gameObject.SetActive(value: true);
			View.tranChangeBG.gameObject.SetActive(value: false);
			if (!string.IsNullOrEmpty(View.inputChangeBG.text))
			{
				int num = int.Parse(View.inputChangeBG.text) - 1;
				if (num >= 0)
				{
					ApplicationFacade.Instance.SendNotification("BG_CHANGE_TEST", num);
				}
			}
		}

		private void OnClickEnterChangePlayer()
		{
			View.tranMain.gameObject.SetActive(value: true);
			View.tranChangePlayer.gameObject.SetActive(value: false);
			if (!string.IsNullOrEmpty(View.inputChangePlayer.text))
			{
				float num = float.Parse(View.inputChangePlayer.text);
				if (num < 0f)
				{
					return;
				}
				Singleton<PlayerManager>.Instance.GetLeaderObj().TestSpeedScale = num;
			}
			if (!string.IsNullOrEmpty(View.inputChangeDiamond.text))
			{
				int num2 = int.Parse(View.inputChangeDiamond.text);
				if (num2 < 0)
				{
					return;
				}
				GameGlobalData.CurDiamond = num2;
			}
			if (!string.IsNullOrEmpty(View.inputChangeBattleItemID.text) && !string.IsNullOrEmpty(View.inputChangeBattleItemNums.text))
			{
				int num3 = int.Parse(View.inputChangeBattleItemID.text);
				int num4 = int.Parse(View.inputChangeBattleItemNums.text);
				if (num3 > 0 && num4 > 0)
				{
					Singleton<BattleItemManager>.Instance.SetBattleItemNumber(num3, num4);
				}
			}
		}

		private void OnClickEnterDropItem()
		{
			string text = View.inputDropItemParams[0].text;
			string[] array = new string[8]
			{
				"病毒减速",
				"射速",
				"火力",
				"金币",
				"飞机减速",
				"病毒加速",
				"病毒放大",
				"僚机"
			};
			int num = -1;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].Equals(text))
				{
					num = i;
					break;
				}
			}
			if (num > -1)
			{
				LevelStruct levelStruct = Singleton<GameJson>.Instance.LevelJson[GameGlobalData.CurLevel];
				levelStruct.ItemsDropNum[0] = 10;
				for (int j = 0; j < levelStruct.ItemsDropWeight.Length; j++)
				{
					levelStruct.ItemsDropWeight[j] = 0;
				}
				levelStruct.ItemsDropWeight[num] = 100;
			}
			View.tranMain.gameObject.SetActive(value: true);
			View.tranDropItem.gameObject.SetActive(value: false);
		}

		private void RefreshGame()
		{
			GameGlobalData.GameState = GameState.Result;
			SendNotification("GAME_MAIN");
		}
	}
}
