using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	public class ShopViewMediator : Mediator, IMediator
	{
		private string _buyID;

		private Vector2 propshopPos;

		private List<TimeGiftStruct> _timeGiftJson;

		private ItemGiftStruct[] _dataGiftJson;

		private readonly Dictionary<int, UIShopBestOrderItem> _shopBestOrderItemdata = new Dictionary<int, UIShopBestOrderItem>();

		private ShopView View => (ShopView)ViewComponent;

		public ShopViewMediator(string name, ShopView viewComponent)
			: base(name, viewComponent)
		{
		}

		public override void OnRegister()
		{
			base.OnRegister();
			View.btnClose.onClick.AddListener(OnClickClose);
		}

		public override void OnRemove()
		{
			base.OnRemove();
		}

		public override IList<string> ListNotificationInterests()
		{
			List<string> list = new List<string>();
			list.Add("SHOP_INIT");
			list.Add("SHOP_DIAMOND_BUY");
			list.Add("SHOP_BUY_FAIL");
			list.Add("SHOP_NOADS_BUY");
			list.Add("SET_DIAMOND");
			list.Add("SHOP_TIMEGIFT_INIT");
			list.Add("SET_COIN");
			list.Add("SHOP_TIMEGIFT_BUY");
			list.Add("SHOP_ITEMGIFT_BUY");
			return list;
		}

		public override void HandleNotification(INotification notification)
		{
			switch (notification.Name)
			{
			case "SHOP_INIT":
				SetCoin(GameGlobalData.CurCoin);
				SetDiamond(GameGlobalData.CurDiamond);
				SetDiamondGiftInfo();
				SetRecommendGiftInfo();
				SetTimeGiftInfo();
				SetItemGiftInfo();
				SetPropShopPos();
				break;
			case "SHOP_TIMEGIFT_BUY":
				HandleTimeGiftState(notification.Body.ToString());
				break;
			case "SHOP_ITEMGIFT_BUY":
				HandleItemGiftState(notification.Body.ToString());
				break;
			case "SHOP_DIAMOND_BUY":
				SetBuyDiamondGift((int)notification.Body);
				break;
			case "SHOP_BUY_FAIL":
				GameAnalysisData.BuyFailFromShop((string)notification.Body);
				break;
			case "SHOP_NOADS_BUY":
				SetNoAdsGift();
				break;
			case "SET_DIAMOND":
				SetDiamond(GameGlobalData.CurDiamond);
				break;
			case "SET_COIN":
				SetCoin(GameGlobalData.CurCoin);
				break;
			}
		}

		private void SetCoin(float count)
		{
			View.txtCoin.text = GameUtils.FloatToUnit(count);
		}

		private void SetDiamond(float count)
		{
			View.txtDiamond.text = GameUtils.FloatToUnit(count);
		}

		private void HandleItemGiftState(string buyKey)
		{
			Transform layerTran = Singleton<UIViewManager>.Instance.GetLayerTran(110);
			ItemGiftStruct[] dataGiftJson = _dataGiftJson;
			int num = 0;
			ItemGiftStruct itemGiftStruct;
			while (true)
			{
				if (num < dataGiftJson.Length)
				{
					itemGiftStruct = dataGiftJson[num];
					if (itemGiftStruct.ShopID == buyKey)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			Singleton<CoinManager>.Instance.FloatByCircle<FloatProp>("GameObject/Item/FloatProp", 5, _shopBestOrderItemdata[itemGiftStruct.ID].BtnPay.transform.position, propshopPos, 0.25f, delegate
			{
			}, layerTran);
			for (int i = 0; i < itemGiftStruct.Types.Length; i++)
			{
				int itemId = itemGiftStruct.Types[i];
				Singleton<BattleItemManager>.Instance.SetBattleItemNumber(itemId, int.Parse(itemGiftStruct.Counts[i]));
			}
		}

		private void HandleTimeGiftState(string buyKey)
		{
			GameAnalysisData.BuyFromShop(buyKey);
			if (_timeGiftJson.Count <= 0)
			{
				return;
			}
			bool flag = false;
			TimeGiftStruct timeGiftStruct = default(TimeGiftStruct);
			for (int i = 0; i < _timeGiftJson.Count; i++)
			{
				TimeGiftStruct timeGiftStruct2 = _timeGiftJson[i];
				if (timeGiftStruct2.ShopID == buyKey)
				{
					flag = true;
					timeGiftStruct = _timeGiftJson[i];
					break;
				}
			}
			if (!flag)
			{
				return;
			}
			int num = -1;
			float num2 = 0f;
			float counts2;
			float counts;
			for (int j = 0; j < timeGiftStruct.Types.Length; j++)
			{
				num = timeGiftStruct.Types[j];
				num2 = GameUtils.UnitToFloat(timeGiftStruct.Counts[j]);
				switch (num)
				{
				case 1:
				{
					Transform layerTran2 = Singleton<UIViewManager>.Instance.GetLayerTran(110);
					counts2 = num2;
					Singleton<CoinManager>.Instance.FloatByCircle<FloatDiamond>("GameObject/Item/FloatDiamond", 20, Vector3.zero, View.tranDiamond.position, 0.25f, delegate
					{
						GameGlobalData.SetCurDiamond(GameGlobalData.CurDiamond + counts2);
					}, layerTran2);
					Singleton<AudioManager>.Instance.PlaySound("coin", 0.8f, 4, 0.2f);
					break;
				}
				case 2:
				{
					Transform layerTran = Singleton<UIViewManager>.Instance.GetLayerTran(110);
					counts = num2;
					Singleton<CoinManager>.Instance.FloatByCircle<FloatGold>("GameObject/Item/FloatCoin", 20, Vector2.zero, View.tranCoin.position, 0.25f, delegate
					{
						GameGlobalData.AddCurCoin(counts);
					}, layerTran);
					break;
				}
				case 3:
					View.shopRecommend.SetNoAdsEnable(isEnable: false);
					GameGlobalData.ShopData.SaveNoAdsState(isNoAdsState: true);
					break;
				}
			}
			GameGlobalData.ShopData.SaveTimeLimitGiftState(timeGiftStruct.ID, 1);
			SetTimeGiftInfo();
		}

		private void SetItemGiftInfo()
		{
			_dataGiftJson = Singleton<GameJson>.Instance.ItemGiftJson;
			_shopBestOrderItemdata.Clear();
			for (int i = 0; i < _dataGiftJson.Length; i++)
			{
				ItemGiftStruct itemGiftStruct = _dataGiftJson[i];
				View.shopBestOrderItems[i].Init(itemGiftStruct);
				View.shopBestOrderItems[i].gameObject.SetActive(GameGlobalData.ActualLevel > 20);
				_shopBestOrderItemdata.Add(itemGiftStruct.ID, View.shopBestOrderItems[i]);
			}
		}

		private void SetTimeGiftInfo()
		{
			_timeGiftJson = GameGlobalData.ShopData.CheckGiftJson();
			UIShopBestOrder[] shopBestOrders = View.shopBestOrders;
			foreach (UIShopBestOrder uIShopBestOrder in shopBestOrders)
			{
				uIShopBestOrder.transform.gameObject.SetActive(value: false);
			}
			if (_timeGiftJson.Count > 0)
			{
				for (int j = 0; j < _timeGiftJson.Count; j++)
				{
					if (j < View.shopBestOrders.Length)
					{
						View.shopBestOrders[j].gameObject.SetActive(value: true);
						View.shopBestOrders[j].Init(_timeGiftJson[j]);
					}
				}
			}
			ApplicationFacade.Instance.SendNotification("MAIN_TIME_LIMIT_TIPS", false);
		}

		private void SetRecommendGiftInfo()
		{
			MenberGiftStruct[] menberGiftJson = Singleton<GameJson>.Instance.MenberGiftJson;
			View.shopRecommend.Init(menberGiftJson, OnClickBuyMenber, OnClickBuyNoAds);
		}

		private void SetDiamondGiftInfo()
		{
			DiamondGiftStruct[] diamondGiftJson = Singleton<GameJson>.Instance.DiamondGiftJson;
			View.shopDiamond.Init(diamondGiftJson, OnClickBuyDiamond, OnClickAdsDiamond);
		}

		private void SetBuyDiamondGift(int index)
		{
			string[] array = new string[6]
			{
				"diamond_1",
				"diamond_2",
				"diamond_3",
				"diamond_4",
				"diamond_5",
				"diamond_6"
			};
			GameAnalysisData.BuyFromShop(array[index]);
			DiamondGiftStruct json = Singleton<GameJson>.Instance.DiamondGiftJson[index];
			Transform layerTran = Singleton<UIViewManager>.Instance.GetLayerTran(110);
			Singleton<CoinManager>.Instance.FloatByCircle<FloatDiamond>("GameObject/Item/FloatDiamond", 20, Vector3.zero, View.tranDiamond.position, 0.25f, delegate
			{
				GameGlobalData.SetCurDiamond(GameGlobalData.CurDiamond + GameUtils.UnitToFloat(json.Counts) + GameUtils.UnitToFloat(json.Free));
			}, layerTran);
			Singleton<AudioManager>.Instance.PlaySound("coin", 0.8f, 4, 0.2f);
		}

		private void SetNoAdsGift()
		{
			GameAnalysisData.BuyFromShop("no_ads");
			View.shopRecommend.SetNoAdsEnable(isEnable: false);
			GameGlobalData.ShopData.SaveNoAdsState(isNoAdsState: true);
		}

		private void OnClickBuyMenber()
		{
			Singleton<UIViewManager>.Instance.ShowView("VIP_VIEW", "VIP_INIT");
		}

		private void OnClickBuyNoAds()
		{
			if (GameGlobalData.ShopData.isNoAds)
			{
				SendNotification("TIP_ANIM", LanguageConfig.NO_ADS_IS_BUY);
				return;
			}
			_buyID = "no_ads";
			Singleton<SdkUtils>.Instance.GooglePay("no_ads");
		}

		private void OnClickBuyDiamond(int index)
		{
			string[] array = new string[6]
			{
				"diamond_1",
				"diamond_2",
				"diamond_3",
				"diamond_4",
				"diamond_5",
				"diamond_6"
			};
			_buyID = array[index];
			Singleton<SdkUtils>.Instance.GooglePay(array[index]);
		}

		private void OnClickAdsDiamond(int index)
		{
			if (Singleton<AdManager>.Instance.IsLoad("0a2cad70-9e1c-450f-bcdc-f3798b324b95", "OnClickAdsDiamond"))
			{
				Singleton<ShopManager>.Instance.SetTempIndex(index);
				Singleton<AdManager>.Instance.ShowVideo("0a2cad70-9e1c-450f-bcdc-f3798b324b95", "shopbuydiamond", ShowVedioByDiamond);
			}
		}

		private void ShowVedioByDiamond(bool isReward)
		{
			if (isReward)
			{
				GameAnalysisData.SendShowVedio("shopbuydiamond");
				int tempIndex = Singleton<ShopManager>.Instance.GetTempIndex();
				int addCount = Singleton<ShopManager>.Instance.ShowVedioAndGetReward();
				if (addCount > 0)
				{
					Transform layerTran = Singleton<UIViewManager>.Instance.GetLayerTran(110);
					View.shopDiamond.ResetAdsDiamond(tempIndex);
					Singleton<CoinManager>.Instance.FloatByCircle<FloatDiamond>("GameObject/Item/FloatDiamond", 20, Vector3.zero, View.tranDiamond.position, 0.25f, delegate
					{
						GameGlobalData.SetCurDiamond(GameGlobalData.CurDiamond + (float)addCount);
					}, layerTran);
					Singleton<AudioManager>.Instance.PlaySound("coin", 0.8f, 4, 0.2f);
				}
				else if (addCount == 0)
				{
					View.shopDiamond.ResetAdsDiamond(tempIndex);
				}
			}
		}

		private void OnClickClose()
		{
			Singleton<UIViewManager>.Instance.HideView("SHOP_VIEW");
		}

		private void SetPropShopPos()
		{
			propshopPos = GameGlobalData.MainViewPropBtnPos;
		}
	}
}
