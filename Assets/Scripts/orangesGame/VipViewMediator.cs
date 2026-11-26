using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections.Generic;

namespace orangesGame
{
	public class VipViewMediator : Mediator, IMediator
	{
		private VipView View => (VipView)ViewComponent;

		public VipViewMediator(string name, VipView viewComponent)
			: base(name, viewComponent)
		{
		}

		public override void OnRegister()
		{
			base.OnRegister();
			View.BtnClose.onClick.AddListener(ClickClose);
			View.BtnClaim.onClick.AddListener(ClickClaim);
			View.BtnBuyVip.onClick.AddListener(ClickPay);
			if (Singleton<LanguageManager>.Instance.IsCn())
			{
				View.img_cn.gameObject.SetActive(value: true);
				View.img_en.gameObject.SetActive(value: false);
			}
			else
			{
				View.img_en.gameObject.SetActive(value: true);
				View.img_cn.gameObject.SetActive(value: false);
			}
		}

		public override IList<string> ListNotificationInterests()
		{
			List<string> list = new List<string>();
			list.Add("VIP_INIT");
			list.Add("VIP_STATE_UPDATE_SUCCESS");
			return list;
		}

		public override void HandleNotification(INotification notification)
		{
			string name = notification.Name;
			if (name == null)
			{
				return;
			}
			if (!(name == "VIP_INIT"))
			{
				if (name == "VIP_STATE_UPDATE_SUCCESS")
				{
					HandleVipStatusUpdate((string)notification.Body);
				}
			}
			else
			{
				VipViewInit();
			}
		}

		private void HandleVipStatusUpdate(string vipDate)
		{
			VipViewInit();
			ApplicationFacade.Instance.SendNotification("SHOP_INIT");
		}

		private void VipViewInit()
		{
			bool vipStatus = Singleton<VipManager>.Instance.VipStatus;
			if (!vipStatus)
			{
				bool vipCanFree = Singleton<VipManager>.Instance.VipCanFree;
				View.TranFreeTest.gameObject.SetActive(vipCanFree);
				View.TranPay.gameObject.SetActive(!vipCanFree);
				View.TxtReadContext.text = LanguageConfig.ANDROID_MEMBER_TIPS;
			}
			else
			{
				View.TxtTimeDeadLine.text = LanguageConfig.ValidUntil + Singleton<VipManager>.Instance.GetTimeUnixDate(Singleton<VipManager>.Instance.VipDate);
			}
			View.TranNotVipPanel.gameObject.SetActive(!vipStatus);
			View.TranVipPanel.gameObject.SetActive(vipStatus);
		}

		private void ClickPay()
		{
			Singleton<SdkUtils>.Instance.GooglePay("diamond_membership", "subs");
		}

		private void ClickClaim()
		{
			Singleton<VipManager>.Instance.GetFreeDiamond();
			ShowCliamBtnState();
		}

		private void ShowCliamBtnState()
		{
			View.BtnClaim.gameObject.SetActive(!Singleton<VipManager>.Instance.IsGotFreeDiamond);
		}

		private void ClickClose()
		{
			View.OnHide();
		}
	}
}
