using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	public class ShopBestOrderMediator : Mediator, IMediator
	{
		private string _buyKey = string.Empty;

		private TimeGiftStruct _tempGiftStruct;

		private ShopBestOrderView View => (ShopBestOrderView)ViewComponent;

		public ShopBestOrderMediator(string name, ShopBestOrderView viewComponent)
			: base(name, viewComponent)
		{
		}

		public override void OnRegister()
		{
			base.OnRegister();
			View.BtnClose.onClick.AddListener(ClickClose);
		}

		public override IList<string> ListNotificationInterests()
		{
			List<string> list = new List<string>();
			list.Add("SHOP_TIMEGIFT_VIEW_INIT");
			list.Add("SHOP_TIMEGIFT_BUY");
			list.Add("SHOP_BUY_FAIL");
			return list;
		}

		public override void HandleNotification(INotification notification)
		{
			string name = notification.Name;
			if (name == null)
			{
				return;
			}
			if (!(name == "SHOP_TIMEGIFT_VIEW_INIT"))
			{
				if (!(name == "SHOP_TIMEGIFT_BUY"))
				{
					if (name == "SHOP_BUY_FAIL" && notification.Body.ToString() == _buyKey)
					{
						ClickClose();
					}
				}
				else
				{
					HandleTimeGiftState(notification.Body.ToString());
				}
			}
			else
			{
				Init((TimeGiftStruct)notification.Body);
			}
		}

		public void Init(TimeGiftStruct data)
		{
			_buyKey = string.Empty;
			_tempGiftStruct = data;
			View.txt_title.text = LanguageConfig.Shop_bestOrder1_title;
			View.BtnPay.onClick.AddListener(delegate
			{
				ClickBuyTimeGift(data.ShopID);
			});
			View.TwoItemTran.gameObject.SetActive(data.Types.Length <= 2);
			View.ThreeItemTran.gameObject.SetActive(data.Types.Length > 2);
			if (data.Types.Length > 2)
			{
				View.ThreeItemTxtDiamondNum.text = data.Counts[0];
				View.ThreeItemTxtCoinNum.text = data.Counts[1];
			}
			else
			{
				View.TwoItemTxtDiamondNum.text = data.Counts[0];
				View.TwoItemTxtCoinNum.text = data.Counts[1];
			}
			View.TxtDiscont.text = data.Discount * 100.0 + "%OFF";
			View.TxtDiscontPay.text = "$" + data.Cost;
			View.TxtPay.text = "$" + data.DisCost;
			if (data.ID != -1)
			{
				long timeLimitGiftTime = GameGlobalData.ShopData.GetTimeLimitGiftTime(data.ID);
				View.TxtRemaintime.text = GameUtils.GetTimeUnixDate(timeLimitGiftTime);
			}
		}

		private void ClickBuyTimeGift(string buyKey)
		{
			_buyKey = buyKey;
			Singleton<SdkUtils>.Instance.GooglePay(buyKey);
		}

		private void HandleTimeGiftState(string buyKey)
		{
			if (string.IsNullOrEmpty(_buyKey) || !(_buyKey == buyKey))
			{
				return;
			}
			int num = -1;
			float num2 = 0f;
			float counts2;
			float counts;
			for (int i = 0; i < _tempGiftStruct.Types.Length; i++)
			{
				num = _tempGiftStruct.Types[i];
				num2 = GameUtils.UnitToFloat(_tempGiftStruct.Counts[i]);
				switch (num)
				{
				case 1:
				{
					Transform layerTran2 = Singleton<UIViewManager>.Instance.GetLayerTran(110);
					counts2 = num2;
					Singleton<CoinManager>.Instance.FloatByCircle<FloatDiamond>("GameObject/Item/FloatDiamond", 20, Vector3.zero, GameGlobalData.DiamondRecoverTopLeftPos, 0.25f, delegate
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
					Singleton<CoinManager>.Instance.FloatByCircle<FloatGold>("GameObject/Item/FloatCoin", 20, Vector2.zero, GameGlobalData.CoinRecoverTopLeftPos, 0.25f, delegate
					{
						GameGlobalData.AddCurCoin(counts);
					}, layerTran);
					Singleton<AudioManager>.Instance.PlaySound("coin", 0.8f, 4, 0.2f);
					break;
				}
				case 3:
					GameGlobalData.ShopData.SaveNoAdsState(isNoAdsState: true);
					break;
				}
			}
			GameGlobalData.ShopData.SaveTimeLimitGiftState(_tempGiftStruct.ID, 1);
			ClickClose();
		}

		private void ClickClose()
		{
			View.OnHide();
		}
	}
}
