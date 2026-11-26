using UnityEngine;
using UnityEngine.UI;

namespace orangesGame
{
	public class TestView : UIView
	{
		public Text txtVersions;

		public Button btnEnter;

		public Transform tranMain;

		public Transform tranVenomAngle;

		public Transform tranWeapon;

		public Transform tranLevel;

		public Transform tranChangeWeapon;

		public Transform tranChangeBG;

		public Transform tranChangePlayer;

		public Transform tranDropItem;

		public Button btnVenomAngle;

		public Button btnWeapon;

		public Button btnLevel;

		public Button btnChangeWeapon;

		public Button btnClose;

		public Button btnChangeBG;

		public Button btnChangePlayer;

		public Button btnTestGooglePay;

		public Button btnDropItem;

		public Text txtGaId;

		public InputField[] inputAngleParams;

		public InputField[] inputPropertyParams;

		public Button btnEnterAngle;

		public Dropdown dropVenom;

		public InputField[] inputWeaponParams;

		public Button btnEnterWeapon;

		public InputField[] inputLevelParams;

		public Button btnEnterLevel;

		public InputField[] inputGoldParams;

		public InputField[] inputChangeWeaponParams;

		public Button btnEnterChange;

		public InputField inputChangeBG;

		public Button btnEnterChangeBG;

		public InputField inputChangePlayer;

		public InputField inputChangeDiamond;

		public InputField inputChangeBattleItemID;

		public InputField inputChangeBattleItemNums;

		public Button btnEnterChangePlayer;

		public Button btnEnterItem;

		public Dropdown dropItem;

		public InputField[] inputDropItemParams;

		public override void OnLoad()
		{
			base.OnLoad();
			Transform layerTran = Singleton<UIViewManager>.Instance.GetLayerTran(90);
			CreateView("GameUI/TestView/ui_testview", layerTran);
		}

		public override void OnInit()
		{
			base.OnInit();
			Transform transform = base.Root.transform;
			txtVersions = transform.Find("TxtVersions").GetComponent<Text>();
			btnEnter = transform.Find("BtnEnter").GetComponent<Button>();
			tranVenomAngle = transform.Find("TranVenomAngle");
			tranMain = transform.Find("TranMain");
			tranWeapon = transform.Find("TranWeapon");
			tranLevel = transform.Find("TranLevel");
			tranChangeWeapon = transform.Find("TranChangeWeapon");
			tranChangeBG = transform.Find("TranBG");
			tranChangePlayer = transform.Find("TranPlayer");
			tranDropItem = transform.Find("TranItem");
			OnInitTranMain();
			OnInitTranVenomAngle();
			OnInitTranWeapon();
			OnInitTranLevel();
			OnInitTranGold();
			OnInitTranChangeWeapon();
			OnInitTranChangeBG();
			OnInitTranChangePlayer();
			OnInitDropItem();
			ApplicationFacade.Instance.RegisterMediator(new TestViewMediator("TEST", this));
		}

		public override void OnRegister()
		{
			base.OnRegister();
		}

		public override void OnRemove()
		{
			base.OnRemove();
			ApplicationFacade.Instance.RemoveMediator("TEST");
		}

		private void OnInitTranMain()
		{
			btnVenomAngle = tranMain.Find("BtnVenomAngle").GetComponent<Button>();
			btnWeapon = tranMain.Find("BtnWeapon").GetComponent<Button>();
			btnLevel = tranMain.Find("BtnLevel").GetComponent<Button>();
			btnChangeWeapon = tranMain.Find("BtnChangeWeapon").GetComponent<Button>();
			btnClose = tranMain.Find("BtnClose").GetComponent<Button>();
			btnChangeBG = tranMain.Find("BtnChangeBG").GetComponent<Button>();
			btnChangePlayer = tranMain.Find("BtnChangePlayer").GetComponent<Button>();
			btnTestGooglePay = tranMain.Find("BtnTestGooglePay").GetComponent<Button>();
			txtGaId = tranMain.Find("TxtGaId").GetComponent<Text>();
			btnDropItem = tranMain.Find("BtnDropItem").GetComponent<Button>();
		}

		private void OnInitTranVenomAngle()
		{
			btnEnterAngle = tranVenomAngle.Find("BtnEnter").GetComponent<Button>();
			inputAngleParams = new InputField[8];
			for (int i = 0; i < inputAngleParams.Length; i++)
			{
				inputAngleParams[i] = tranVenomAngle.Find("TranInputField/InputField_" + i).GetComponent<InputField>();
			}
			inputPropertyParams = new InputField[1];
			for (int j = 0; j < inputPropertyParams.Length; j++)
			{
				inputPropertyParams[j] = tranVenomAngle.Find("TranInputProperty/InputField_" + j).GetComponent<InputField>();
			}
			dropVenom = tranVenomAngle.Find("DropdownVenom").GetComponent<Dropdown>();
		}

		private void OnInitTranWeapon()
		{
			btnEnterWeapon = tranWeapon.Find("BtnEnter").GetComponent<Button>();
			inputWeaponParams = new InputField[8];
			for (int i = 0; i < inputWeaponParams.Length; i++)
			{
				inputWeaponParams[i] = tranWeapon.Find("TranInputField/InputField_" + i).GetComponent<InputField>();
			}
		}

		private void OnInitTranLevel()
		{
			btnEnterLevel = tranLevel.Find("BtnEnter").GetComponent<Button>();
			inputLevelParams = new InputField[1];
			for (int i = 0; i < inputLevelParams.Length; i++)
			{
				inputLevelParams[i] = tranLevel.Find("TranInputField/InputField_" + i).GetComponent<InputField>();
			}
		}

		private void OnInitTranGold()
		{
			inputGoldParams = new InputField[1];
			for (int i = 0; i < inputGoldParams.Length; i++)
			{
				inputGoldParams[i] = tranLevel.Find("TranGold/InputField_" + i).GetComponent<InputField>();
			}
		}

		private void OnInitTranChangeWeapon()
		{
			btnEnterChange = tranChangeWeapon.Find("BtnEnter").GetComponent<Button>();
			inputChangeWeaponParams = new InputField[1];
			for (int i = 0; i < inputChangeWeaponParams.Length; i++)
			{
				inputChangeWeaponParams[i] = tranChangeWeapon.Find("TranInputField/InputField_" + i).GetComponent<InputField>();
			}
		}

		private void OnInitTranChangeBG()
		{
			btnEnterChangeBG = tranChangeBG.Find("BtnEnter").GetComponent<Button>();
			inputChangeBG = tranChangeBG.Find("TranInputField/InputField_0").GetComponent<InputField>();
		}

		private void OnInitTranChangePlayer()
		{
			btnEnterChangePlayer = tranChangePlayer.Find("BtnEnter").GetComponent<Button>();
			inputChangePlayer = tranChangePlayer.Find("TranInputField/InputField_0").GetComponent<InputField>();
			inputChangeDiamond = tranChangePlayer.Find("TranInputField/InputField_1").GetComponent<InputField>();
			inputChangeBattleItemID = tranChangePlayer.Find("TranInputField/InputField_2").GetComponent<InputField>();
			inputChangeBattleItemNums = tranChangePlayer.Find("TranInputField/InputField_3").GetComponent<InputField>();
		}

		private void OnInitDropItem()
		{
			btnEnterItem = tranDropItem.Find("BtnEnter").GetComponent<Button>();
			dropItem = tranDropItem.Find("DropItem").GetComponent<Dropdown>();
			inputDropItemParams = new InputField[1];
			for (int i = 0; i < inputDropItemParams.Length; i++)
			{
				inputDropItemParams[i] = tranDropItem.Find("TranInputProperty/InputField_" + i).GetComponent<InputField>();
			}
		}
	}
}
