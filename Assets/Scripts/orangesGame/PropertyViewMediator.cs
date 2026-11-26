using DG.Tweening;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace orangesGame
{
	public class PropertyViewMediator : Mediator, IMediator
	{
		private const string UPGRADE_ACTIVE = "anniu_b_1";

		private const string UPGRADE_INACTIVE = "anniu_b_3";

		private const string UPGRADE_GREEN = "anniu_b_4";

		private const string PROP_SELECT = "anniu_a_2";

		private const string PROP_UNSELECT = "anniu_a_1";

		private const string WEAPON_ITEM_SELECT = "anniu_c_3";

		private const string WEAPON_ITEM_UN_SELECT = "anniu_c_3";

		private const string WEAPON_ITEM_LOCK = "anniu_c_2";

		private const string ADICON = "ADStubiao";

		private const string COINICON = "zhuanniu3-0";

		private const string UPGRADE_TIP = "biaoshi_1";

		private const string FREE_UPGRADE_TIP = "biaoshi_2";

		private const string ANIM_TAB_UP = "anim_propertyview_tab_up";

		private const string ANIM_TAB_DOWN = "anim_propertyview_tab_down";

		private const string ANIM_SUBVIEW_IN = "anim_propertyview_subview_in";

		private const string ANIM_SUBVIEW_APPEAR = "anim_propertyview_subview_appear";

		private int _index = -1;

		private int _weaponID;

		private int[] _levelMaxs;

		private int _propertyIndex;

		private List<UIWeaponItem> _itemList;

		private SpriteAtlas _weaponAtlas;

		private SpriteAtlas _mainAtlas;

		private Color col = new Color(139f / 255f, 41f / 255f, 41f / 255f, 1f);

		private PropertyView View => (PropertyView)ViewComponent;

		public PropertyViewMediator(string name, PropertyView viewComponent)
			: base(name, viewComponent)
		{
		}

		public override void OnRegister()
		{
			base.OnRegister();
			for (int i = 0; i < View.btnPropertys.Length; i++)
			{
				int temp2 = i;
				View.btnPropertys[i].onClick.AddListener(delegate
				{
					OnClickTab(temp2);
				});
			}
			for (int j = 0; j < View.btnMainUpGrades.Length; j++)
			{
				int temp = j;
				View.btnMainUpGrades[j].onClick.AddListener(delegate
				{
					OnClickMainUpGrade(temp);
				});
				View.btnWeaponUpGrades[j].onClick.AddListener(delegate
				{
					OnClickWeaponUpGrade(temp);
				});
				View.btnCoinUpGrades[j].onClick.AddListener(delegate
				{
					OnClickCoinUpGrade(temp);
				});
			}
			_weaponAtlas = Resources.Load<SpriteAtlas>("GameAtlas/WeaponAtlas");
			_mainAtlas = Resources.Load<SpriteAtlas>("GameAtlas/MainAtlas");
			WeaponStruct[] weaponJson = Singleton<GameJson>.Instance.WeaponJson;
			_itemList = new List<UIWeaponItem>();
			_weaponID = GameGlobalData.WeaponID;
			for (int k = 0; k < GameGlobalData.OpenWeaponCount; k++)
			{
				UIWeaponItem uIWeaponItem = Singleton<ResLoadManager>.Instance.ResLoad<UIWeaponItem>("GameUI/PropertyView/ui_propertyview_item_weapon", isPreLoad: false);
				uIWeaponItem.SetParent(View.tranListParent);
				uIWeaponItem.SetOnClick(OnClickWeaponSelect);
				uIWeaponItem.SetIcon(_weaponAtlas.GetSprite(weaponJson[k].ImageName));
				uIWeaponItem.ID = int.Parse(weaponJson[k].ID);
				uIWeaponItem.TxtContent.text = string.Empty;
				_itemList.Add(uIWeaponItem);
			}
			for (int l = GameGlobalData.OpenWeaponCount; l < weaponJson.Length; l++)
			{
				UIWeaponItem uIWeaponItem2 = Singleton<ResLoadManager>.Instance.ResLoad<UIWeaponItem>("GameUI/PropertyView/ui_propertyview_item_weapon", isPreLoad: false);
				uIWeaponItem2.SetParent(View.tranListParent);
				uIWeaponItem2.SetOnClick(OnClickWeaponSelect);
				uIWeaponItem2.SetIcon(_weaponAtlas.GetSprite(weaponJson[l].ImageName));
				uIWeaponItem2.ID = int.Parse(weaponJson[l].ID);
				_itemList.Add(uIWeaponItem2);
			}
		}

		public override void OnRemove()
		{
			base.OnRemove();
		}

		public override IList<string> ListNotificationInterests()
		{
			List<string> list = new List<string>();
			list.Add("PROPERTY_INIT");
			list.Add("PROPERTY_REFRESH_VIEW");
			list.Add("PROPERTY_NEW_WEAPON_TIP");
			list.Add("PROPERTY_HIDE_VIEW");
			list.Add("PROPERTY_GOLD_REFRESH_GUIDE");
			list.Add("SET_COIN");
			return list;
		}

		public override void HandleNotification(INotification notification)
		{
			string name = notification.Name;
			if (name == null)
			{
				return;
			}
			if (!(name == "PROPERTY_INIT"))
			{
				if (!(name == "SET_COIN") && !(name == "PROPERTY_REFRESH_VIEW"))
				{
					if (!(name == "PROPERTY_NEW_WEAPON_TIP"))
					{
						if (!(name == "PROPERTY_HIDE_VIEW"))
						{
							if (name == "PROPERTY_GOLD_REFRESH_GUIDE")
							{
								SetGuideTipSetActive(-1, temp: false);
							}
						}
						else
						{
							OnClickSelect();
						}
					}
					else
					{
						View.objNewWeaponTip.SetActive(value: true);
					}
					return;
				}
				CheckMainUpGradeTip();
				CheckWeaponUpGradeTip();
				CheckCoinUpGradeTip();
				SetMainTabView();
				if (GameGlobalData.WeaponID >= 0)
				{
					SetWeaponTabView();
				}
				SetCoinTabView();
				return;
			}
			_index = -1;
			_weaponID = GameGlobalData.WeaponID;
			CheckMainUpGradeTip();
			CheckCoinUpGradeTip();
			if (GameGlobalData.CurLevel < 5)
			{
				View.btnPropertys[1].gameObject.SetActive(value: false);
			}
			else
			{
				View.btnPropertys[1].gameObject.SetActive(value: true);
				CheckWeaponUpGradeTip();
				SetWeaponIcon();
				SetWeaponTabView();
				SetWeaponLockState();
			}
			SetMainTabView();
			SetCoinTabView();
			SetTabBtnState(_index);
			View.btnSelcet.gameObject.SetActive(value: false);
			SetMaxWeaponState();
			SetGuideTipSetActive(-1, temp: false);
		}

		private void SetGuideTipSetActive(int index, bool temp)
		{
			View.upTranSpeed.gameObject.SetActive(value: false);
			View.upTranFire.gameObject.SetActive(value: false);
			for (int i = 0; i < 3; i++)
			{
				View.tranGuideTips[i].gameObject.SetActive(value: false);
			}
			if (!Singleton<TutorialManager>.Instance.HasGuideTips())
			{
				for (int j = 0; j < 3; j++)
				{
					View.tranGuideTips[j].gameObject.SetActive(value: false);
				}
				return;
			}
			for (int k = 0; k < 3; k++)
			{
				if (k == index)
				{
					View.tranGuideTips[k].gameObject.SetActive(temp);
				}
				else
				{
					View.tranGuideTips[k].gameObject.SetActive(!temp);
				}
			}
		}

		private void SendGuideUp(int index)
		{
			View.upTranSpeed.gameObject.SetActive(value: false);
			View.upTranFire.gameObject.SetActive(value: false);
			if (index == 0)
			{
				switch (Singleton<TutorialManager>.Instance.IsGuideToUpgrade())
				{
				case 1:
					View.upTranFire.gameObject.SetActive(value: true);
					Singleton<TutorialManager>.Instance.SetGuideFirePower(temp: true);
					break;
				case 2:
					View.upTranSpeed.gameObject.SetActive(value: true);
					Singleton<TutorialManager>.Instance.SetGuideFireSpeed(temp: true);
					break;
				}
			}
		}

		private void ClickGuideUp(int index)
		{
			View.upTranSpeed.gameObject.SetActive(value: false);
			View.upTranFire.gameObject.SetActive(value: false);
			if (index != 0)
			{
			}
		}

		private void OnClickTab(int index)
		{
			if (View.objNewWeaponTip.activeSelf)
			{
				View.objNewWeaponTip.SetActive(value: false);
				SetMaxWeaponState();
			}
			if (_index == index)
			{
				View.tranMain.gameObject.SetActive(value: false);
				View.tranWeapon.gameObject.SetActive(value: false);
				View.tranCoin.gameObject.SetActive(value: false);
				_index = -1;
				MovePlayer(isUp: false);
				Singleton<AnalyzeSdk>.Instance.OnEventByArgs("Click", new string[1]
				{
					"type@主武器Tab"
				});
				SetGuideTipSetActive(-1, temp: false);
				MovePlayerDown();
			}
			else
			{
				string empty = string.Empty;
				switch (index)
				{
				case 0:
					View.tranMain.gameObject.SetActive(value: true);
					View.tranWeapon.gameObject.SetActive(value: false);
					View.tranCoin.gameObject.SetActive(value: false);
					empty = "type@主武器Tab";
					SetGuideTipSetActive(-1, temp: true);
					break;
				case 1:
					View.tranMain.gameObject.SetActive(value: false);
					View.tranWeapon.gameObject.SetActive(value: true);
					View.tranCoin.gameObject.SetActive(value: false);
					empty = "type@副武器Tab";
					SetGuideTipSetActive(index, temp: true);
					break;
				default:
					View.tranMain.gameObject.SetActive(value: false);
					View.tranWeapon.gameObject.SetActive(value: false);
					View.tranCoin.gameObject.SetActive(value: true);
					empty = "type@金币离线Tab";
					SetGuideTipSetActive(index, temp: true);
					break;
				}
				Singleton<AnalyzeSdk>.Instance.OnEventByArgs("Click", new string[1]
				{
					empty
				});
				MovePlayer(isUp: true);
				if (_index == -1)
				{
					MovePlayerUp();
					propertyMoveIn(index);
					SendNotification("MAIN_EXTEND_INACTIVE");
				}
				_index = index;
			}
			SetTabBtnState(_index);
			if (GameGlobalData.OpenWeaponCount > 0)
			{
				SetWeaponTabView();
			}
			SetCoinTabView();
			if (_index == -1)
			{
				MoveDownPropertyView();
				return;
			}
			MoveUpPropertyView();
			SendGuideUp(index);
		}

		private void OnClickMainUpGrade(int index)
		{
			ClickGuideUp(index);
			_propertyIndex = index;
			if (GameGlobalData.FreeUpGradeIndex == index)
			{
				string scene = (index != 0) ? "levelup_main_2" : "levelup_main_1";
				Singleton<AdManager>.Instance.ShowVideo("0a2cad70-9e1c-450f-bcdc-f3798b324b95", scene, AdMainUpdateGradeCallback);
			}
			else if (View.txtMainUpGradeCosts[index].color == Color.white)
			{
				SendNotification("MAIN_PORPERTY_UPGRADE", index);
			}
			else
			{
				Singleton<UIViewManager>.Instance.ShowView("CONVERSION_VIEW", "CONVERSION_INIT");
			}
		}

		private void AdMainUpdateGradeCallback(bool isReward)
		{
			if (isReward)
			{
				SendNotification("MAIN_PORPERTY_UPGRADE_FREE", _propertyIndex);
			}
			else
			{
				SendNotification("PORPERTY_UPGRADE_FREE_FAIL");
			}
		}

		private void AdMaxWeaponCallback(bool isReward)
		{
			if (isReward)
			{
				GameGlobalData.SetMaxWeaponState(4);
				SelectWeapon(GameGlobalData.TempWeaponID);
				View.txtContent.text = string.Empty;
				List<string> list = UmConfig.SetCommonParam();
				list.Add($"id@{GameGlobalData.TempWeaponID}");
				Singleton<AnalyzeSdk>.Instance.OnEventByArgs("ADSVedioMaxWeapon", list.ToArray());
				List<string> list2 = UmConfig.SetCommonParam();
				list2.Add("type@满级试用");
				Singleton<AnalyzeSdk>.Instance.OnEventByArgs("ADSVideoReward", list2.ToArray());
				GameAnalysisData.SendMaxWeapon(ads: true, GameGlobalData.TempWeaponID);
			}
			else
			{
				GameGlobalData.SetMaxWeaponState(0);
				GameGlobalData.SetTempWeaponID(-1);
				SelectWeapon(GameGlobalData.WeaponID);
				SetWeaponLockState();
				View.rectTranFreeTip.gameObject.SetActive(value: false);
				View.objMaxWeapon.SetActive(value: false);
			}
		}

		private void OnClickCoinUpGrade(int index)
		{
			_propertyIndex = index;
			int num = GameGlobalData.FreeUpGradeIndex - 4;
			if (num == index)
			{
				string scene = (num != 0) ? "goldup2" : "goldup1";
				Singleton<AdManager>.Instance.ShowVideo("0a2cad70-9e1c-450f-bcdc-f3798b324b95", scene, AdCoinUpdateGradeCallback);
			}
			else if (View.txtCoinUpGradeCosts[index].color == Color.white)
			{
				SendNotification("COIN_PORPERTY_UPGRADE", index);
			}
			else
			{
				Singleton<UIViewManager>.Instance.ShowView("CONVERSION_VIEW", "CONVERSION_INIT");
			}
		}

		private void AdCoinUpdateGradeCallback(bool isReward)
		{
			if (isReward)
			{
				SendNotification("COIN_PORPERTY_UPGRADE_FREE", _propertyIndex);
			}
			else
			{
				SendNotification("PORPERTY_UPGRADE_FREE_FAIL");
			}
		}

		private void OnClickWeaponSelect(int id)
		{
			if (GameGlobalData.TempWeaponID == id && GameGlobalData.MaxWeaponState == 3)
			{
				Singleton<AdManager>.Instance.ShowVideo("0a2cad70-9e1c-450f-bcdc-f3798b324b95", "maxweapon", AdMaxWeaponCallback);
				return;
			}
			if (GameGlobalData.TempWeaponID != id)
			{
				GameGlobalData.PreWeaponID = id;
			}
			SelectWeapon(id);
		}

		private void SelectWeapon(int id)
		{
			if (_weaponID != id)
			{
				_itemList[id].btnClick.image.sprite = _mainAtlas.GetSprite("anniu_c_3");
				SetSelectWeaponFarmPos(id);
				if (_weaponID >= 0)
				{
					_itemList[_weaponID].btnClick.image.sprite = _mainAtlas.GetSprite("anniu_c_3");
				}
				_weaponID = id;
				GameGlobalData.SetWeaponID(_weaponID, _weaponID < GameGlobalData.OpenWeaponCount);
				Singleton<PlayerManager>.Instance.ChangeLeaderWeapon();
				SetWeaponIcon();
				if (GameGlobalData.TempWeaponID >= 0)
				{
					bool flag = GameGlobalData.TempWeaponID == id;
					View.rectTranFreeTip.gameObject.SetActive(!flag);
				}
			}
			SetWeaponTabView();
			CheckWeaponUpGradeTip();
		}

		private void SetSelectWeaponFarmPos(int index)
		{
			if (!View.tranSelectWeapon.gameObject.activeSelf)
			{
				View.tranSelectWeapon.gameObject.SetActive(value: true);
			}
			View.tranSelectWeapon.SetParent(_itemList[index].transform);
			View.tranSelectWeapon.localScale = Vector3.one;
			View.tranSelectWeapon.anchoredPosition = new Vector2(0f, 8.5f);
		}

		private void MovePlayer(bool isUp)
		{
			Transform leader = Singleton<PlayerManager>.Instance.GetLeader();
			if (isUp)
			{
				SendNotification("MAIN_TIP_SET_ACTIVE", false);
			}
			else
			{
				SendNotification("MAIN_TIP_SET_ACTIVE", true);
			}
		}

		private void OnClickWeaponUpGrade(int index)
		{
			_propertyIndex = index;
			int num = GameGlobalData.FreeUpGradeIndex - 2;
			if (num == index)
			{
				string scene = (num != 0) ? "weaponlevelup2" : "weaponlevelup1";
				Singleton<AdManager>.Instance.ShowVideo("0a2cad70-9e1c-450f-bcdc-f3798b324b95", scene, AdWeaponUpdateGradeCallback);
			}
			else if (View.txtWeaponUpGradeCosts[index].color == Color.white)
			{
				SendNotification("WEAPON_PORPERTY_UPGRADE", index);
			}
			else
			{
				Singleton<UIViewManager>.Instance.ShowView("CONVERSION_VIEW", "CONVERSION_INIT");
			}
		}

		private void AdWeaponUpdateGradeCallback(bool isReward)
		{
			if (isReward)
			{
				SendNotification("WEAPON_PORPERTY_UPGRADE_FREE", _propertyIndex);
			}
			else
			{
				SendNotification("PORPERTY_UPGRADE_FREE_FAIL");
			}
		}

		private void OnClickSelect()
		{
			if (_index != -1)
			{
				_index = -1;
				if (View.tranMain.gameObject.activeSelf)
				{
					View.tranMain.gameObject.SetActive(value: false);
				}
				if (View.tranWeapon.gameObject.activeSelf)
				{
					View.tranWeapon.gameObject.SetActive(value: false);
				}
				if (View.tranCoin.gameObject.activeSelf)
				{
					View.tranCoin.gameObject.SetActive(value: false);
				}
				MovePlayer(isUp: false);
				SetTabBtnState(_index);
				MoveDownPropertyView();
				MovePlayerDown();
				SetGuideTipSetActive(-1, temp: false);
			}
		}

		public void SetTabBtnState(int index)
		{
			switch (index)
			{
			case 0:
				View.btnPropertys[0].image.sprite = _mainAtlas.GetSprite("anniu_a_2");
				View.btnPropertys[1].image.sprite = _mainAtlas.GetSprite("anniu_a_1");
				View.btnPropertys[2].image.sprite = _mainAtlas.GetSprite("anniu_a_1");
				break;
			case 1:
				View.btnPropertys[0].image.sprite = _mainAtlas.GetSprite("anniu_a_1");
				View.btnPropertys[1].image.sprite = _mainAtlas.GetSprite("anniu_a_2");
				View.btnPropertys[2].image.sprite = _mainAtlas.GetSprite("anniu_a_1");
				break;
			case 2:
				View.btnPropertys[2].image.sprite = _mainAtlas.GetSprite("anniu_a_2");
				View.btnPropertys[0].image.sprite = _mainAtlas.GetSprite("anniu_a_1");
				View.btnPropertys[1].image.sprite = _mainAtlas.GetSprite("anniu_a_1");
				break;
			default:
				View.btnPropertys[2].image.sprite = _mainAtlas.GetSprite("anniu_a_1");
				View.btnPropertys[0].image.sprite = _mainAtlas.GetSprite("anniu_a_1");
				View.btnPropertys[1].image.sprite = _mainAtlas.GetSprite("anniu_a_1");
				break;
			}
		}

		private void SetMaxWeaponState()
		{
			MaxWeaponState maxWeaponState = (MaxWeaponState)GameGlobalData.MaxWeaponState;
			bool flag = Singleton<AdManager>.Instance.IsLoad("0a2cad70-9e1c-450f-bcdc-f3798b324b95", "SetMaxWeaponState");
			if (maxWeaponState == MaxWeaponState.UnUsed || View.objNewWeaponTip.activeSelf || (maxWeaponState == MaxWeaponState.HasBeenFree && !flag) || (maxWeaponState == MaxWeaponState.AdUse && !flag) || (maxWeaponState == MaxWeaponState.HasBeenAdUse && !flag))
			{
				View.rectTranFreeTip.gameObject.SetActive(value: false);
				View.objMaxWeapon.SetActive(value: false);
				return;
			}
			View.rectTranFreeTip.gameObject.SetActive(value: true);
			View.objMaxWeapon.SetActive(value: true);
			View.objMaxWeapon.transform.SetParent(_itemList[GameGlobalData.TempWeaponID].transform);
			(View.objMaxWeapon.transform as RectTransform).anchoredPosition = new Vector2(0f, -105f);
			if (maxWeaponState == MaxWeaponState.Free)
			{
				View.txtContent.text = LanguageConfig.FREE;
			}
			else
			{
				View.txtContent.text = LanguageConfig.ADS;
			}
			Singleton<WeaponMaxManager>.Instance.SetMaxOpenToday(1);
		}

		private void CheckMainUpGradeTip()
		{
			bool flag = GameGlobalData.FreeUpGradeIndex == 0 || GameGlobalData.FreeUpGradeIndex == 1;
			if (flag)
			{
				View.tranUpGradeTips[0].GetComponent<Image>().sprite = _mainAtlas.GetSprite("biaoshi_2");
			}
			else
			{
				FirePowerStruct firePowerData = GameGlobalData.GetFirePowerData();
				float fireCost = GameGlobalData.GetFireCost(firePowerData);
				if (GameGlobalData.FireSpeedLevel >= Singleton<GameJson>.Instance.FireSpeedJson.Length - 1)
				{
					flag = (GameGlobalData.CurCoin >= fireCost);
				}
				else
				{
					FireSpeedStruct fireSpeedData = GameGlobalData.GetFireSpeedData();
					float fireSpeedCost = GameGlobalData.GetFireSpeedCost(fireSpeedData);
					flag = (GameGlobalData.CurCoin >= fireSpeedCost || GameGlobalData.CurCoin >= fireCost);
				}
				if (flag)
				{
					GameGlobalData.isCanUpadteMain = 1;
					View.tranUpGradeTips[0].GetComponent<Image>().sprite = _mainAtlas.GetSprite("biaoshi_1");
				}
				else
				{
					GameGlobalData.isCanUpadteMain = 0;
				}
			}
			View.tranUpGradeTips[0].gameObject.SetActive(flag);
		}

		private void CheckWeaponUpGradeTip()
		{
			bool flag = GameGlobalData.FreeUpGradeIndex == 2 || GameGlobalData.FreeUpGradeIndex == 3;
			if (flag)
			{
				View.tranUpGradeTips[1].GetComponent<Image>().sprite = _mainAtlas.GetSprite("biaoshi_2");
			}
			else
			{
				flag = (IsWeaponUpGradeTip() && GameGlobalData.TempWeaponID < 0 && !View.objNewWeaponTip.activeSelf);
				if (flag)
				{
					GameGlobalData.isCanUpadteWeapon = 1;
					View.tranUpGradeTips[1].GetComponent<Image>().sprite = _mainAtlas.GetSprite("biaoshi_1");
				}
				else
				{
					GameGlobalData.isCanUpadteWeapon = 0;
				}
			}
			View.tranUpGradeTips[1].gameObject.SetActive(flag);
		}

		private void CheckCoinUpGradeTip()
		{
			bool flag = GameGlobalData.FreeUpGradeIndex == 4 || GameGlobalData.FreeUpGradeIndex == 5;
			if (flag)
			{
				View.tranUpGradeTips[2].GetComponent<Image>().sprite = _mainAtlas.GetSprite("biaoshi_2");
			}
			else
			{
				CoinLevelStruct coinLevelData = GameGlobalData.GetCoinLevelData(GameGlobalData.CoinCountLevel);
				CoinLevelStruct coinLevelData2 = GameGlobalData.GetCoinLevelData(GameGlobalData.CoinOffLineLevel);
				flag = (GameGlobalData.CurCoin >= GameGlobalData.GetCoinValueCost(coinLevelData) || GameGlobalData.CurCoin >= GameGlobalData.GetCoinValueCost(coinLevelData2));
				if (flag)
				{
					GameGlobalData.isCanUpadteCoin = 1;
					View.tranUpGradeTips[2].GetComponent<Image>().sprite = _mainAtlas.GetSprite("biaoshi_1");
				}
				else
				{
					GameGlobalData.isCanUpadteCoin = 0;
				}
			}
			View.tranUpGradeTips[2].gameObject.SetActive(flag);
		}

		private bool IsWeaponUpGradeTip()
		{
			if (GameGlobalData.WeaponID == -1)
			{
				return false;
			}
			WeaponLevelStruct[] weaponLevelJson = Singleton<GameJson>.Instance.WeaponLevelJson;
			int weapID = GameGlobalData.WeaponID * 2;
			int weaponLevel = GameGlobalData.GetWeaponLevel(weapID, 0);
			int weaponLevel2 = GameGlobalData.GetWeaponLevel(weapID, 1);
			WeaponStruct weaponData = GameGlobalData.GetWeaponData();
			float[] array = new float[5]
			{
				GameGlobalData.GetWeaponScaleCostTxt(0, weaponLevelJson[weaponLevel]),
				GameGlobalData.GetWeaponScaleCostTxt(1, weaponLevelJson[weaponLevel]),
				GameGlobalData.GetWeaponScaleCostTxt(2, weaponLevelJson[weaponLevel]),
				GameGlobalData.GetWeaponScaleCostTxt(3, weaponLevelJson[weaponLevel]),
				GameGlobalData.GetWeaponScaleCostTxt(4, weaponLevelJson[weaponLevel])
			};
			float[] array2 = new float[5]
			{
				GameGlobalData.GetWeaponFireCostTxt(0, weaponLevelJson[weaponLevel2]),
				GameGlobalData.GetWeaponFireCostTxt(1, weaponLevelJson[weaponLevel2]),
				GameGlobalData.GetWeaponFireCostTxt(2, weaponLevelJson[weaponLevel2]),
				GameGlobalData.GetWeaponFireCostTxt(3, weaponLevelJson[weaponLevel2]),
				GameGlobalData.GetWeaponFireCostTxt(4, weaponLevelJson[weaponLevel2])
			};
			if (GameGlobalData.CurCoin >= array[GameGlobalData.WeaponID] && weaponLevel < weaponData.LevelMax[0] - 1)
			{
				return true;
			}
			if (GameGlobalData.CurCoin >= array2[GameGlobalData.WeaponID] && GameGlobalData.FirePowerLevel > weaponLevel2 && weaponLevel2 < weaponData.LevelMax[1] - 1)
			{
				return true;
			}
			return false;
		}

		private void SetWeaponIcon()
		{
			WeaponStruct weaponData = GameGlobalData.GetWeaponData();
			for (int i = 0; i < 2; i++)
			{
				View.imgWeapons[i].sprite = _weaponAtlas.GetSprite(weaponData.ImageName);
				View.imgWeapons[i].SetNativeSize();
			}
		}

		private void SetMainTabView()
		{
			FireSpeedStruct fireSpeedData = GameGlobalData.GetFireSpeedData();
			FirePowerStruct firePowerData = GameGlobalData.GetFirePowerData();
			float fireSpeedCost = GameGlobalData.GetFireSpeedCost(fireSpeedData);
			float fireCost = GameGlobalData.GetFireCost(firePowerData);
			int[] array = new int[2]
			{
				GameGlobalData.FireSpeedLevel,
				GameGlobalData.FirePowerLevel
			};
			string[] array2 = new string[2]
			{
				GameGlobalData.GetFireSpeedTxt(fireSpeedData),
				GameGlobalData.GetFireTxt(firePowerData)
			};
			string[] array3 = new string[2]
			{
				GameUtils.FloatToUnit(fireSpeedCost),
				GameUtils.FloatToUnit(fireCost)
			};
			string[] array4 = new string[2]
			{
				LanguageConfig.MAIN_PROPERTY_1,
				LanguageConfig.MAIN_PROPERTY_2
			};
			bool[] array5 = new bool[2]
			{
				GameGlobalData.CurCoin >= fireSpeedCost,
				GameGlobalData.CurCoin >= fireCost
			};
			bool[] array6 = new bool[2]
			{
				GameGlobalData.FireSpeedLevel >= Singleton<GameJson>.Instance.FireSpeedJson.Length - 1,
				GameGlobalData.FirePowerLevel >= Singleton<GameJson>.Instance.FirePowerJson.Length - 1
			};
			for (int i = 0; i < 2; i++)
			{
				SetMainUpGradeInfo(i, array4[i], array[i], array2[i], array3[i], array5[i], array6[i]);
			}
		}

		private void SetMainUpGradeInfo(int index, string title, int level, string porperty, string cost, bool enough, bool max)
		{
			View.txtMainUpGradeTitles[index].text = string.Format(title, level + 1);
			View.txtMainLevels[index].text = porperty;
			View.txtMainUpGradeCosts[index].text = cost;
			SetMainUpGradeBtnActive(index, enough, max);
		}

		private void SetMainUpGradeBtnActive(int index, bool enough, bool max)
		{
			if (max)
			{
				View.btnMainUpGrades[index].enabled = !max;
				View.btnMainUpGrades[index].image.sprite = _mainAtlas.GetSprite("anniu_b_3");
				View.txtMainUpGradeCosts[index].text = LanguageConfig.MAX;
				return;
			}
			int freeUpGradeIndex = GameGlobalData.FreeUpGradeIndex;
			if (freeUpGradeIndex == index)
			{
				View.btnMainUpGrades[index].enabled = true;
				View.btnMainUpGrades[index].image.sprite = _mainAtlas.GetSprite("anniu_b_4");
				View.txtMainUpGradeCosts[index].text = LanguageConfig.FREE;
				View.txtMainUpGradeCosts[index].color = Color.white;
				View.imgMainUpGradeIcons[index].sprite = _mainAtlas.GetSprite("ADStubiao");
				View.imgMainUpGradeIcons[index].rectTransform.sizeDelta = new Vector2(77f, 56.5f);
			}
			else
			{
				if (enough)
				{
					View.txtMainUpGradeCosts[index].color = Color.white;
				}
				else
				{
					View.txtMainUpGradeCosts[index].color = col;
				}
				View.btnMainUpGrades[index].image.sprite = _mainAtlas.GetSprite("anniu_b_1");
				View.imgMainUpGradeIcons[index].sprite = _mainAtlas.GetSprite("zhuanniu3-0");
				View.imgMainUpGradeIcons[index].rectTransform.sizeDelta = new Vector2(60f, 60f);
			}
		}

		private void SetWeaponTabView()
		{
			MaxWeaponState maxWeaponState = (MaxWeaponState)GameGlobalData.MaxWeaponState;
			if (GameGlobalData.TempWeaponID == GameGlobalData.WeaponID && (maxWeaponState == MaxWeaponState.Free || maxWeaponState == MaxWeaponState.HasBeenAdUse))
			{
				SetMaxWeaponTabView();
			}
			else
			{
				SetNotMaxWeaponTabView();
			}
		}

		private void SetWeaponLockState()
		{
			WeaponStruct[] weaponJson = Singleton<GameJson>.Instance.WeaponJson;
			for (int i = 0; i < GameGlobalData.OpenWeaponCount; i++)
			{
				if (GameGlobalData.WeaponID == i)
				{
					_itemList[i].SetUnLock(_mainAtlas.GetSprite("anniu_c_3"));
					SetSelectWeaponFarmPos(i);
				}
				else
				{
					_itemList[i].SetUnLock(_mainAtlas.GetSprite("anniu_c_3"));
				}
			}
			for (int j = GameGlobalData.OpenWeaponCount; j < weaponJson.Length; j++)
			{
				_itemList[j].SetLock(_mainAtlas.GetSprite("anniu_c_2"), weaponJson[j].UnLockLevel);
			}
			if (GameGlobalData.TempWeaponID <= 0 || GameGlobalData.TempWeaponID == GameGlobalData.WeaponID)
			{
				return;
			}
			switch (GameGlobalData.MaxWeaponState)
			{
			case 1:
				_itemList[GameGlobalData.TempWeaponID].SetUnLock(_mainAtlas.GetSprite("anniu_c_3"));
				break;
			case 3:
				if (Singleton<AdManager>.Instance.IsLoad("0a2cad70-9e1c-450f-bcdc-f3798b324b95", "SetWeaponLockState"))
				{
					_itemList[GameGlobalData.TempWeaponID].SetUnLock(_mainAtlas.GetSprite("anniu_c_3"));
				}
				break;
			}
		}

		private void SetMaxWeaponTabView()
		{
			WeaponStruct weaponData = GameGlobalData.GetWeaponData();
			_levelMaxs = new int[2];
			for (int i = 0; i < _levelMaxs.Length; i++)
			{
				_levelMaxs[i] = weaponData.LevelMax[i] - 1;
			}
			string[] propertys = new string[2];
			float[] costs = new float[2];
			SetWeaponProperty(GameGlobalData.WeaponID, _levelMaxs, ref propertys, ref costs);
			for (int j = 0; j < 2; j++)
			{
				SetWeaponUpGradeInfo(j, LanguageConfig.WEAPON_PROPTERY_NAMES[int.Parse(weaponData.ID)][j], _levelMaxs[j], propertys[j], 0f);
			}
		}

		private void SetNotMaxWeaponTabView()
		{
			WeaponStruct weaponData = GameGlobalData.GetWeaponData();
			_levelMaxs = weaponData.LevelMax;
			string[] propertys = new string[2];
			float[] costs = new float[2];
			int[] array = new int[2];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = GameGlobalData.GetWeaponLevel(GameGlobalData.WeaponID * 2, i);
			}
			SetWeaponProperty(GameGlobalData.WeaponID, array, ref propertys, ref costs);
			for (int j = 0; j < 2; j++)
			{
				SetWeaponUpGradeInfo(j, LanguageConfig.WEAPON_PROPTERY_NAMES[int.Parse(weaponData.ID)][j], array[j], propertys[j], costs[j]);
			}
		}

		private void SetWeaponProperty(int weaponID, int[] levels, ref string[] propertys, ref float[] costs)
		{
			WeaponLevelStruct[] weaponLevelJson = Singleton<GameJson>.Instance.WeaponLevelJson;
			WeaponLevelStruct weaponStruct = weaponLevelJson[levels[0]];
			WeaponLevelStruct weaponStruct2 = weaponLevelJson[levels[1]];
			switch (weaponID)
			{
			case 0:
				propertys[0] = GameGlobalData.GetWeaponScaleTxt(weaponID, weaponStruct);
				propertys[1] = GameGlobalData.GetWeaponFireTxt(weaponID, weaponStruct2);
				costs[0] = GameGlobalData.GetWeaponScaleCostTxt(weaponID, weaponStruct);
				costs[1] = GameGlobalData.GetWeaponFireCostTxt(weaponID, weaponStruct2);
				break;
			case 1:
				propertys[0] = GameGlobalData.GetWeaponScaleTxt(weaponID, weaponStruct);
				propertys[1] = GameGlobalData.GetWeaponFireTxt(weaponID, weaponStruct2);
				costs[0] = GameGlobalData.GetWeaponScaleCostTxt(weaponID, weaponStruct);
				costs[1] = GameGlobalData.GetWeaponFireCostTxt(weaponID, weaponStruct2);
				break;
			case 2:
				propertys[0] = GameGlobalData.GetWeaponScaleTxt(weaponID, weaponStruct);
				propertys[1] = GameGlobalData.GetWeaponFireTxt(weaponID, weaponStruct2);
				costs[0] = GameGlobalData.GetWeaponScaleCostTxt(weaponID, weaponStruct);
				costs[1] = GameGlobalData.GetWeaponFireCostTxt(weaponID, weaponStruct2);
				break;
			case 3:
				propertys[0] = GameGlobalData.GetWeaponScaleTxt(weaponID, weaponStruct);
				propertys[1] = GameGlobalData.GetWeaponFireTxt(weaponID, weaponStruct2);
				costs[0] = GameGlobalData.GetWeaponScaleCostTxt(weaponID, weaponStruct);
				costs[1] = GameGlobalData.GetWeaponFireCostTxt(weaponID, weaponStruct2);
				break;
			case 4:
				propertys[0] = GameGlobalData.GetWeaponScaleTxt(weaponID, weaponStruct);
				propertys[1] = GameGlobalData.GetWeaponFireTxt(weaponID, weaponStruct2);
				costs[0] = GameGlobalData.GetWeaponScaleCostTxt(weaponID, weaponStruct);
				costs[1] = GameGlobalData.GetWeaponFireCostTxt(weaponID, weaponStruct2);
				break;
			}
		}

		private void SetWeaponUpGradeInfo(int index, string title, int level, string porperty, float cost)
		{
			View.txtWeaponUpGradeTitles[index].text = $"{title} (Lv{level + 1})";
			View.txtWeaponLevels[index].text = porperty;
			View.txtWeaponUpGradeCosts[index].text = ((cost != 0f) ? GameUtils.FloatToUnit(cost) : string.Empty);
			SetWeaponUpGradeBtnActive(index, cost, level);
		}

		private void SetWeaponUpGradeBtnActive(int index, float cost, int level)
		{
			if (level >= _levelMaxs[index])
			{
				View.btnWeaponUpGrades[index].image.sprite = _mainAtlas.GetSprite("anniu_b_3");
				View.txtWeaponUpGradeCosts[index].text = LanguageConfig.MAX;
				View.btnWeaponUpGrades[index].enabled = false;
				return;
			}
			int num = GameGlobalData.FreeUpGradeIndex - 2;
			if (num == index)
			{
				View.btnWeaponUpGrades[index].enabled = true;
				View.btnWeaponUpGrades[index].image.sprite = _mainAtlas.GetSprite("anniu_b_4");
				View.txtWeaponUpGradeCosts[index].text = LanguageConfig.FREE;
				View.txtWeaponUpGradeCosts[index].color = Color.white;
				View.imgWeaponUpGradeIcons[index].sprite = _mainAtlas.GetSprite("ADStubiao");
				View.imgWeaponUpGradeIcons[index].rectTransform.sizeDelta = new Vector2(77f, 56.5f);
				return;
			}
			bool flag = GameGlobalData.CurCoin >= cost;
			View.imgWeaponUpGradeIcons[index].sprite = _mainAtlas.GetSprite("zhuanniu3-0");
			View.imgWeaponUpGradeIcons[index].rectTransform.sizeDelta = new Vector2(60f, 60f);
			if (flag)
			{
				if (index % 2 != 0 && level >= GameGlobalData.FirePowerLevel)
				{
					View.btnWeaponUpGrades[index].image.sprite = _mainAtlas.GetSprite("anniu_b_3");
					View.txtWeaponUpGradeCosts[index].text = LanguageConfig.UPGRED_LIMITED;
					View.btnWeaponUpGrades[index].enabled = false;
				}
				else
				{
					View.btnWeaponUpGrades[index].image.sprite = _mainAtlas.GetSprite("anniu_b_1");
					View.btnWeaponUpGrades[index].enabled = true;
				}
				View.txtWeaponUpGradeCosts[index].color = Color.white;
			}
			else
			{
				View.btnWeaponUpGrades[index].image.sprite = _mainAtlas.GetSprite("anniu_b_1");
				View.btnWeaponUpGrades[index].enabled = true;
				View.txtWeaponUpGradeCosts[index].color = col;
			}
		}

		private void SetCoinTabView()
		{
			int num = Singleton<GameJson>.Instance.CoinLevelJson.Length - 1;
			CoinLevelStruct coinLevelData = GameGlobalData.GetCoinLevelData(GameGlobalData.CoinCountLevel);
			CoinLevelStruct coinLevelData2 = GameGlobalData.GetCoinLevelData(GameGlobalData.CoinOffLineLevel);
			string[] array = new string[2]
			{
				GameUtils.FloatToUnit(GameGlobalData.GetCoinValueTxt(coinLevelData)),
				coinLevelData2.OffLine.ToString()
			};
			int[] array2 = new int[2]
			{
				GameGlobalData.CoinCountLevel,
				GameGlobalData.CoinOffLineLevel
			};
			float[] array3 = new float[2]
			{
				GameGlobalData.GetCoinValueCost(coinLevelData),
				GameGlobalData.GetOfflineValueCost(coinLevelData2)
			};
			string[] array4 = new string[2]
			{
				LanguageConfig.COIN_PROPERTY_1,
				LanguageConfig.COIN_PROPERTY_2
			};
			bool[] array5 = new bool[2]
			{
				GameGlobalData.CoinCountLevel >= num,
				GameGlobalData.CoinOffLineLevel >= num
			};
			for (int i = 0; i < 2; i++)
			{
				SetCoinUpGradeInfo(i, array4[i], array2[i], array[i], array3[i], array5[i]);
			}
		}

		private void SetCoinUpGradeInfo(int index, string title, int level, string porperty, float cost, bool max)
		{
			View.txtCoinUpGradeTitles[index].text = string.Format(title, level + 1);
			View.txtCoinLevels[index].text = porperty;
			View.txtCoinUpGradeCosts[index].text = GameUtils.FloatToUnit(cost);
			SetCoinUpGradeBtnActive(index, cost, max);
		}

		private void SetCoinUpGradeBtnActive(int index, float cost, bool max)
		{
			if (max)
			{
				View.btnCoinUpGrades[index].enabled = !max;
				View.btnCoinUpGrades[index].image.sprite = _mainAtlas.GetSprite("anniu_b_3");
				View.txtCoinUpGradeCosts[index].text = LanguageConfig.MAX;
				return;
			}
			int num = GameGlobalData.FreeUpGradeIndex - 4;
			if (num == index)
			{
				View.btnCoinUpGrades[index].enabled = true;
				View.btnCoinUpGrades[index].image.sprite = _mainAtlas.GetSprite("anniu_b_4");
				View.txtCoinUpGradeCosts[index].text = LanguageConfig.FREE;
				View.txtCoinUpGradeCosts[index].color = Color.white;
				View.imgCoinUpGradeIcons[index].sprite = _mainAtlas.GetSprite("ADStubiao");
				View.imgCoinUpGradeIcons[index].rectTransform.sizeDelta = new Vector2(77f, 56.5f);
				return;
			}
			bool flag = GameGlobalData.CurCoin >= cost;
			View.btnCoinUpGrades[index].enabled = true;
			View.btnCoinUpGrades[index].image.sprite = _mainAtlas.GetSprite("anniu_b_1");
			if (flag)
			{
				View.txtCoinUpGradeCosts[index].color = Color.white;
			}
			else
			{
				View.txtCoinUpGradeCosts[index].color = col;
			}
			View.imgCoinUpGradeIcons[index].sprite = _mainAtlas.GetSprite("zhuanniu3-0");
			View.imgCoinUpGradeIcons[index].rectTransform.sizeDelta = new Vector2(60f, 60f);
		}

		private void MoveUpPropertyView()
		{
			View.ani.Play("anim_propertyview_tab_up");
			Singleton<UIViewManager>.Instance.SetPropertyViewShow(_isPropertyViewShow: true);
		}

		private void MoveDownPropertyView()
		{
			View.ani.Play("anim_propertyview_tab_down");
			Singleton<UIViewManager>.Instance.SetPropertyViewShow(_isPropertyViewShow: false);
		}

		private void MovePlayerUp()
		{
			Transform leader = Singleton<PlayerManager>.Instance.GetLeader();
			Transform target = leader;
			Vector3 localPosition = leader.localPosition;
			target.DOMoveY(localPosition.y + 1.7f, 0.3f);
		}

		private void MovePlayerDown()
		{
			Transform leader = Singleton<PlayerManager>.Instance.GetLeader();
			Transform target = leader;
			Vector3 localPosition = leader.localPosition;
			target.DOMoveY(localPosition.y - 1.7f, 0.3f);
		}

		private void propertyMoveIn(int index)
		{
			if (_index == -1)
			{
				switch (index)
				{
				case 0:
				{
					Animator component3 = View.tranMain.GetComponent<Animator>();
					component3.Play("anim_propertyview_subview_in");
					break;
				}
				case 1:
				{
					Animator component2 = View.tranWeapon.GetComponent<Animator>();
					component2.Play("anim_propertyview_subview_in");
					break;
				}
				case 2:
				{
					Animator component = View.tranCoin.GetComponent<Animator>();
					component.Play("anim_propertyview_subview_in");
					break;
				}
				}
			}
		}
	}
}
