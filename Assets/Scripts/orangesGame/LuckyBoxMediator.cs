using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	public class LuckyBoxMediator : Mediator, IMediator
	{
		private LuckyBoxView View => (LuckyBoxView)ViewComponent;

		public LuckyBoxMediator(string name, LuckyBoxView viewComponent)
			: base(name, viewComponent)
		{
		}

		public override void OnRegister()
		{
			base.OnRegister();
			View.btnClose.onClick.AddListener(OnClickClose);
			View.btnBuy.onClick.AddListener(OnClickBuy);
			SetBuyCost();
			SetContent();
			if (Singleton<LanguageManager>.Instance.IsCn())
			{
				View.img_cn.gameObject.SetActive(value: true);
				View.img_en.gameObject.SetActive(value: false);
			}
			else
			{
				View.img_cn.gameObject.SetActive(value: false);
				View.img_en.gameObject.SetActive(value: true);
			}
		}

		public override void OnRemove()
		{
			base.OnRemove();
		}

		public override IList<string> ListNotificationInterests()
		{
			List<string> list = new List<string>();
			list.Add("LUCKYBOX_INIT");
			list.Add("LUCKYBOX_BUY_SUCCESS");
			list.Add("LUCKYBOX_BUY_FAIL");
			return list;
		}

		public override void HandleNotification(INotification notification)
		{
			string name = notification.Name;
			if (name == null)
			{
				return;
			}
			if (!(name == "LUCKYBOX_INIT"))
			{
				if (!(name == "LUCKYBOX_BUY_FAIL"))
				{
					if (name == "LUCKYBOX_BUY_SUCCESS")
					{
						SetBuyDianomd();
					}
				}
				else
				{
					GameAnalysisData.BuyFailFromShop("hold_up_vessel");
				}
			}
			else
			{
				SetProgress();
				SetBuyBtnEnable();
			}
		}

		private void SetBuyDianomd()
		{
			GameAnalysisData.BuyFromShop("hold_up_vessel");
			Singleton<UIViewManager>.Instance.HideView("LUCKYBOX_VIEW");
			Transform layerTran = Singleton<UIViewManager>.Instance.GetLayerTran(110);
			Singleton<CoinManager>.Instance.FloatByCircle<FloatDiamond>("GameObject/Item/FloatDiamond", 20, Vector3.zero, GameGlobalData.DiamondRecoverTopLeftPos, 0.25f, delegate
			{
				Singleton<DiamondStorageManager>.Instance.BuyAndClean();
				SendNotification("MAIN_DIAMOND_STORE_PROGRESS");
			}, layerTran);
			Singleton<AudioManager>.Instance.PlaySound("coin", 0.8f, 4, 0.2f);
		}

		private void SetProgress()
		{
			float process = Singleton<DiamondStorageManager>.Instance.GetProcess();
			View.imgProgress.fillAmount = process;
		}

		private void SetBuyBtnEnable()
		{
			bool flag = Singleton<DiamondStorageManager>.Instance.IsCanBuy();
			if (flag)
			{
				if (!View.btnBuy.enabled)
				{
					View.btnBuy.enabled = flag;
					View.btnBuy.image.material = null;
				}
			}
			else if (View.btnBuy.enabled)
			{
				View.btnBuy.enabled = flag;
				View.btnBuy.image.material = GameUtils.GetGrayMaterial();
			}
		}

		private void SetBuyCost()
		{
			View.txtCost.text = "$" + Singleton<DiamondStorageManager>.Instance.GetBuyCost();
		}

		private void SetContent()
		{
			View.txtContent.text = LanguageConfig.LUCKYBOX_CONTENT;
		}

		private void OnClickBuy()
		{
			Singleton<SdkUtils>.Instance.GooglePay("hold_up_vessel");
		}

		private void OnClickClose()
		{
			Singleton<UIViewManager>.Instance.HideView("LUCKYBOX_VIEW");
		}
	}
}
