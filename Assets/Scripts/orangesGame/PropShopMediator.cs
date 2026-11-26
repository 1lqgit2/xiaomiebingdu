using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;

namespace orangesGame
{
	public class PropShopMediator : Mediator, IMediator
	{
		private List<UIPropShopItem> _list;

		private Timer _countTimer;

		private int _count;

		private float _timeMax;

		private float _curTime;

		private int _selectIndex;

		private int _maxConverCount;

		private int[] _costs;

		private int curCostSelect;

		private int curCostDiamond;

		private const string PROP_POWER = "daoju_1";

		private const string PROP_RATE = "daoju_2";

		private const string PROP_VIRUS = "daoju_3";

		private const string PROP_SHIELD = "daoju_4";

		private const string PROP_WAVE = "daoju_5";

		private const string PROP_BG_UNSELECT = "daoju_xuanzhe_1";

		private const string PROP_BG_SELECT = "daoju_xuanzhe_2";

		private readonly string[] _propNames = new string[7]
		{
			"daoju_1",
			"daoju_2",
			"daoju_3",
			"daoju_4",
			"daoju_5",
			"daoju_xuanzhe_1",
			"daoju_xuanzhe_2"
		};

		private SpriteAtlas _mainAtlas;

		private BattleItemStruct[] _bateItemStructs;

		private Vector2[] AnimPath = new Vector2[5]
		{
			new Vector2(1f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 1f)
		};

		private PropShopView View => (PropShopView)ViewComponent;

		public PropShopMediator(string name, PropShopView viewComponent)
			: base(name, viewComponent)
		{
		}

		public override void OnRegister()
		{
			base.OnRegister();
			View.btnClose.onClick.AddListener(ClickClose);
			View.eventAdd.ActionPointerDown = ClickAddDown;
			View.eventAdd.ActionPointerUp = ClickAddUp;
			View.eventSub.ActionPointerDown = ClickSubDown;
			View.eventSub.ActionPointerUp = ClickSubUp;
			View.btnConver.onClick.AddListener(ClickConver);
			View.btnShop.onClick.AddListener(ClickShop);
			_mainAtlas = Resources.Load<SpriteAtlas>("GameAtlas/MainAtlas");
			View.converTxt.text = LanguageConfig.item_sale_purchase;
			View.useTxt.text = LanguageConfig.item_sale_use;
			View.getTxt.text = LanguageConfig.item_sale_get;
			View.getMoreTxt.text = LanguageConfig.item_sale_getmore;
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				View.btnShop.gameObject.SetActive(value: false);
			}
		}

		public override IList<string> ListNotificationInterests()
		{
			List<string> list = new List<string>();
			list.Add("PROPSHOP_INIT");
			list.Add("PROPSHOP_BUY_SUCCESS");
			list.Add("PROPSHOP_BUY_FAIL");
			return list;
		}

		public override void HandleNotification(INotification notification)
		{
			string name = notification.Name;
			if (name == null)
			{
				return;
			}
			if (!(name == "PROPSHOP_INIT"))
			{
				if (!(name == "PROPSHOP_BUY_FAIL") && name == "PROPSHOP_BUY_SUCCESS")
				{
				}
			}
			else
			{
				SetItemListData();
				SetConverCount(1);
				SetItemBg(_selectIndex, isSelect: true);
				SetMaxConverCount(_costs[_selectIndex]);
				SendNotification("MAIN_PROPSHOP_TIP", false);
			}
		}

		private void SetItemList()
		{
			if (_list == null)
			{
				_list = new List<UIPropShopItem>(5);
			}
			UIPropShopItem[] componentsInChildren = View.tranItemParent.GetComponentsInChildren<UIPropShopItem>();
			if (!GameUtils.IsArryNullOrEmpty(componentsInChildren))
			{
				_list.AddRange(componentsInChildren);
				return;
			}
			for (int i = 0; i < 5; i++)
			{
				UIPropShopItem uIPropShopItem = Singleton<ResLoadManager>.Instance.ResLoad<UIPropShopItem>("GameUI/PropShopView/ui_propshopview_item", isPreLoad: false);
				uIPropShopItem.SetParent(View.tranItemParent);
				int index = i;
				uIPropShopItem.btnBuy.onClick.AddListener(delegate
				{
					ClickSelect(index);
				});
				_list.Add(uIPropShopItem);
			}
		}

		private void SetConverBtnEnable(int index)
		{
			if (GameGlobalData.CurDiamond >= (float)index)
			{
				if (!View.btnConver.enabled)
				{
					View.btnConver.enabled = true;
					View.btnConver.image.material = null;
				}
			}
			else if (View.btnConver.enabled)
			{
				View.btnConver.enabled = false;
				View.btnConver.image.material = GameUtils.GetGrayMaterial();
			}
		}

		private void SetConverCount(int count)
		{
			_count = count;
			View.txtConverCount.text = count.ToString();
			SetUseCostValue(_count);
		}

		private void SetMaxConverCount(int cost)
		{
			_maxConverCount = (int)GameGlobalData.CurDiamond / cost;
			if (_maxConverCount == 0)
			{
				_maxConverCount = 1;
			}
		}

		private void CancelCountTime()
		{
			if (_countTimer != null && !_countTimer.isCancelled)
			{
				_countTimer.Cancel();
			}
		}

		private void ClickSelect(int index)
		{
			if (_selectIndex != index)
			{
				SetItemBg(index, isSelect: true);
				SetItemBg(_selectIndex, isSelect: false);
				_selectIndex = index;
				SetConverCount(1);
				SetMaxConverCount(_costs[_selectIndex]);
				SetCostIcon(index);
			}
		}

		private void ClickClose()
		{
			View.OnHide();
		}

		private void ClickConver()
		{
			if (Singleton<DiamondManager>.Instance.ComsumeDiamond(curCostDiamond))
			{
				Singleton<BattleItemManager>.Instance.SetBattleItemNumber(_bateItemStructs[_selectIndex].ID, _count);
				int count = _count;
				SetConverCount(1);
				UpdataItemOwn();
				Transform layerTran = Singleton<UIViewManager>.Instance.GetLayerTran(110);
				Singleton<AudioManager>.Instance.PlaySound("coin", 0.8f, 4, 0.2f);
				Singleton<CoinManager>.Instance.FloatByCircle<FloatProp>("GameObject/Item/FloatProp", count, View.imaCostIcon.GetComponent<Transform>().position, AnimPath[_selectIndex], 0.25f, delegate
				{
				}, layerTran);
				GameAnalysisData.BuyItem(curCostDiamond, _count, _bateItemStructs[_selectIndex].ID);
			}
		}

		private void ClickShop()
		{
			ClickClose();
			Singleton<UIViewManager>.Instance.ShowView("SHOP_VIEW", "SHOP_INIT");
		}

		private void ClickAddDown(PointerEventData data)
		{
			ClickAdd();
			CancelCountTime();
			_timeMax = 0.8f;
			_countTimer = Timer.Register(Time.deltaTime, null, UpdateAddCount, isLooped: true);
		}

		private void UpdateAddCount(float time)
		{
			_curTime += time;
			if (_curTime >= _timeMax)
			{
				ClickAdd();
				_curTime = 0f;
				if (_timeMax > 0f)
				{
					_timeMax -= 0.2f;
				}
			}
		}

		private void ClickAdd()
		{
			int num = _count + 1;
			if (num <= _maxConverCount)
			{
				_count = num;
				SetConverCount(_count);
			}
			else
			{
				SendNotification("TIP_ANIM", LanguageConfig.NOT_ENOUGH_DIAMONDS);
			}
		}

		private void ClickAddUp(PointerEventData data)
		{
			CancelCountTime();
			_curTime = 0f;
		}

		private void ClickSubDown(PointerEventData data)
		{
			ClickSub();
			CancelCountTime();
			_timeMax = 0.8f;
			_countTimer = Timer.Register(Time.deltaTime, null, UpdateSubCount, isLooped: true);
		}

		private void ClickSub()
		{
			int num = _count - 1;
			if (num >= 1)
			{
				_count = num;
				SetConverCount(_count);
			}
		}

		private void UpdateSubCount(float time)
		{
			_curTime += time;
			if (_curTime >= _timeMax)
			{
				ClickSub();
				_curTime = 0f;
				if (_timeMax > 0f)
				{
					_timeMax -= 0.2f;
				}
			}
		}

		private void ClickSubUp(PointerEventData data)
		{
			CancelCountTime();
			_curTime = 0f;
		}

		private void SetItemListData()
		{
			_bateItemStructs = Singleton<GameJson>.Instance.BattleItemJson;
			_costs = new int[_bateItemStructs.Length];
			AnimPath = new Vector2[_bateItemStructs.Length];
			for (int i = 0; i < _bateItemStructs.Length; i++)
			{
				int index = i;
				BattleItemStruct battleItemStruct = _bateItemStructs[i];
				switch (i)
				{
				case 0:
					View.txtItemName[i].text = LanguageConfig.item_sale_title1;
					break;
				case 1:
					View.txtItemName[i].text = LanguageConfig.item_sale_title2;
					break;
				case 2:
					View.txtItemName[i].text = LanguageConfig.item_sale_title3;
					break;
				case 3:
					View.txtItemName[i].text = LanguageConfig.item_sale_title4;
					break;
				case 4:
					View.txtItemName[i].text = LanguageConfig.item_sale_title5;
					break;
				}
				View.txtItemOwnTxt[i].text = LanguageConfig.item_sale_owned;
				View.txtItemValue[i].text = "x " + battleItemStruct.Price;
				_costs[i] = battleItemStruct.Price;
				View.imaPropItemBg[i].sprite = _mainAtlas.GetSprite("daoju_xuanzhe_1");
				View.txtItemOwn[i].text = Singleton<BattleItemManager>.Instance.GetOwnBattleItem(battleItemStruct.ID).ToString();
				View.btnItem[i].onClick.AddListener(delegate
				{
					ClickSelect(index);
				});
				View.imaItemIcon[index].sprite = _mainAtlas.GetSprite(_propNames[index]);
				AnimPath[i] = View.tranPropItem[i].position;
			}
		}

		private void SetUseCostValue(int index)
		{
			curCostSelect = _costs[_selectIndex];
			int num = index * curCostSelect;
			View.txtCostValue.text = num.ToString();
			curCostDiamond = num;
			SetConverBtnEnable(curCostDiamond);
			SetCostNumber(index);
		}

		private void SetItemBg(int index, bool isSelect)
		{
			string empty = string.Empty;
			empty = ((!isSelect) ? "daoju_xuanzhe_1" : "daoju_xuanzhe_2");
			View.imaPropItemBg[index].sprite = _mainAtlas.GetSprite(empty);
		}

		private void UpdataItemOwn()
		{
			for (int i = 0; i < _bateItemStructs.Length; i++)
			{
				View.txtItemOwn[i].text = Singleton<BattleItemManager>.Instance.GetOwnBattleItem(_bateItemStructs[i].ID).ToString();
			}
		}

		private void SetCostIcon(int index)
		{
			View.imaCostIcon.sprite = _mainAtlas.GetSprite(_propNames[index]);
		}

		private void SetCostNumber(int index)
		{
			View.txtCostNumber.text = "x " + index;
		}
	}
}
