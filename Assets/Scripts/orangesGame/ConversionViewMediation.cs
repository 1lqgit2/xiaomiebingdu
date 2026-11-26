using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace orangesGame
{
	public class ConversionViewMediation : Mediator, IMediator
	{
		private int _diamondCount;

		private float _diamondTime;

		private float _curDiamondTime;

		private Timer _diamondCountTimer;

		private ConversionView View => (ConversionView)ViewComponent;

		public ConversionViewMediation(string name, ConversionView viewComponent)
			: base(name, viewComponent)
		{
		}

		public override void OnRegister()
		{
			base.OnRegister();
			View.eventAdd.ActionPointerDown = OnClickAddDown;
			View.eventAdd.ActionPointerUp = OnClickAddUp;
			View.eventSub.ActionPointerDown = OnClickSubDown;
			View.eventSub.ActionPointerUp = OnClickSubUp;
			View.btnConver.onClick.AddListener(OnClickConver);
			View.btnFree.onClick.AddListener(OnClickFree);
			View.btnClose.onClick.AddListener(OnClickClose);
			View.txtTitle.text = LanguageConfig.CONVERSION_TITLE;
			View.txtConvert.text = LanguageConfig.CONVERT;
			View.txtGetMore.text = LanguageConfig.item_sale_getmore;
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				View.btnFree.gameObject.SetActive(value: false);
			}
		}

		public override void OnRemove()
		{
			base.OnRemove();
		}

		public override IList<string> ListNotificationInterests()
		{
			List<string> list = new List<string>();
			list.Add("CONVERSION_INIT");
			return list;
		}

		public override void HandleNotification(INotification notification)
		{
			string name = notification.Name;
			if (name != null && name == "CONVERSION_INIT")
			{
				_diamondCount = 0;
				int diamondCount = 1;
				bool btnConverEnable = true;
				if (GameGlobalData.CurDiamond == 0f)
				{
					diamondCount = 0;
					btnConverEnable = false;
				}
				SetDiamondCount(diamondCount);
				SetBtnConverEnable(btnConverEnable);
			}
		}

		private void SetDiamondCount(int value)
		{
			if (value == 0)
			{
				_diamondCount = 0;
			}
			else
			{
				_diamondCount = Mathf.Max(_diamondCount, 1, value);
			}
			View.txtDiamondCount.text = _diamondCount.ToString();
			View.txtConverDiamond.text = string.Format(LanguageConfig.CONVERSION_USE_DIAMOND, _diamondCount);
			float value2 = Singleton<DiamondManager>.Instance.CalDiamondExchangeGold(_diamondCount);
			View.txtConverCoin.text = string.Format(LanguageConfig.CONVERSION_GET_COIN, GameUtils.FloatToUnit(value2));
		}

		private void SetBtnConverEnable(bool isEnable)
		{
			if (isEnable)
			{
				if (!View.btnConver.enabled)
				{
					View.btnConver.image.material = null;
					View.btnConver.enabled = true;
				}
			}
			else if (View.btnConver.enabled)
			{
				View.btnConver.enabled = false;
				View.btnConver.image.material = GameUtils.GetGrayMaterial();
			}
		}

		private void OnClickAddDown(PointerEventData data)
		{
			OnClickAdd();
			if (_diamondCountTimer != null && !_diamondCountTimer.isCancelled)
			{
				_diamondCountTimer.Cancel();
			}
			_diamondTime = 0.8f;
			_diamondCountTimer = Timer.Register(Time.deltaTime, null, OnUpdateAddDiamond, isLooped: true);
		}

		private void OnClickAddUp(PointerEventData data)
		{
			if (_diamondCountTimer != null && !_diamondCountTimer.isCancelled)
			{
				_diamondCountTimer.Cancel();
			}
			_diamondTime = 0.8f;
			_curDiamondTime = 0f;
		}

		private void OnClickSubDown(PointerEventData data)
		{
			OnClickSub();
			if (_diamondCountTimer != null && !_diamondCountTimer.isCancelled)
			{
				_diamondCountTimer.Cancel();
			}
			_diamondTime = 0.8f;
			_diamondCountTimer = Timer.Register(Time.deltaTime, null, OnUpdateSubDiamond, isLooped: true);
		}

		private void OnClickSubUp(PointerEventData data)
		{
			if (_diamondCountTimer != null && !_diamondCountTimer.isCancelled)
			{
				_diamondCountTimer.Cancel();
			}
			_diamondTime = 0.8f;
			_curDiamondTime = 0f;
		}

		private void OnUpdateAddDiamond(float time)
		{
			_curDiamondTime += time;
			if (_curDiamondTime >= _diamondTime)
			{
				OnClickAdd();
				_curDiamondTime = 0f;
				if (_diamondTime <= 0f)
				{
					_diamondTime = 0f;
				}
				else
				{
					_diamondTime -= 0.2f;
				}
			}
		}

		private void OnUpdateSubDiamond(float time)
		{
			_curDiamondTime += time;
			if (_curDiamondTime >= _diamondTime)
			{
				OnClickSub();
				_curDiamondTime = 0f;
				if (_diamondTime <= 0f)
				{
					_diamondTime = 0f;
				}
				else
				{
					_diamondTime -= 0.2f;
				}
			}
		}

		private void OnClickAdd()
		{
			int num = _diamondCount + 1;
			if (Singleton<DiamondManager>.Instance.IsEnough(num))
			{
				_diamondCount = num;
				SetDiamondCount(_diamondCount);
			}
		}

		private void OnClickSub()
		{
			int num = _diamondCount - 1;
			if (num >= 0)
			{
				_diamondCount = num;
				SetDiamondCount(_diamondCount);
			}
		}

		private void OnClickConver()
		{
			Transform layerTran = Singleton<UIViewManager>.Instance.GetLayerTran(110);
			Singleton<CoinManager>.Instance.FloatByCircle<FloatGold>("GameObject/Item/FloatCoin", 20, Vector3.zero, GameGlobalData.CoinRecoverTopLeftPos, 0.25f, delegate
			{
				Singleton<DiamondManager>.Instance.DiamondExchangeGold(_diamondCount);
				SendNotification("PROPERTY_INIT");
				OnClickClose();
			}, layerTran);
			Singleton<AudioManager>.Instance.PlaySound("coin", 0.8f, 4, 0.2f);
		}

		private void OnClickFree()
		{
			Singleton<UIViewManager>.Instance.ShowView("SHOP_VIEW", "SHOP_INIT");
			Singleton<UIViewManager>.Instance.HideView("CONVERSION_VIEW");
		}

		private void OnClickClose()
		{
			Singleton<UIViewManager>.Instance.HideView("CONVERSION_VIEW");
		}
	}
}
