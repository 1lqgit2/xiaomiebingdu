using DG.Tweening;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	public class TipViewMediator : Mediator, IMediator
	{
		private List<string> _animTip;

		private TipView View => (TipView)ViewComponent;

		public TipViewMediator(string name, TipView viewComponent)
			: base(name, viewComponent)
		{
		}

		public override void OnRegister()
		{
			base.OnRegister();
			_animTip = new List<string>();
		}

		public override void OnRemove()
		{
			base.OnRemove();
		}

		public override IList<string> ListNotificationInterests()
		{
			List<string> list = new List<string>();
			list.Add("TIP_ANIM");
			return list;
		}

		public override void HandleNotification(INotification notification)
		{
			string name = notification.Name;
			if (name != null && name == "TIP_ANIM")
			{
				string item = (string)notification.Body;
				_animTip.Add(item);
				SetAnimTip();
			}
		}

		private void SetAnimTip()
		{
			if (_animTip.Count > 0)
			{
				if (!View.tranAnimTip.gameObject.activeSelf)
				{
					View.tranAnimTip.gameObject.SetActive(value: true);
				}
				string text = _animTip[0];
				View.txtAnimTip.text = text;
				CanvasGroup group = View.tranAnimTip.GetComponent<CanvasGroup>();
				group.DOFade(1f, 0.5f).OnComplete(delegate
				{
					group.DOFade(0f, 0.5f).SetDelay(1f).OnComplete(delegate
					{
						_animTip.RemoveAt(0);
						SetAnimTip();
					});
				});
			}
			else
			{
				View.tranAnimTip.gameObject.SetActive(value: false);
			}
		}
	}
}
