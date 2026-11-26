using DG.Tweening;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	public class BackgroundViewMediator : Mediator, IMediator
	{
		private const float ZOOM_0 = 1.5f;

		private const float ZOOM_1 = 1.1f;

		private const float ZOOM_DURATION = 0.5f;

		private int _curID;

		private Timer _timer;

		private Color[] _bgColors = new Color[6]
		{
			new Color(22f / 255f, 14f / 255f, 44f / 255f),
			new Color(2f / 85f, 8f / 51f, 74f / 255f),
			new Color(39f / 85f, 37f / 255f, 142f / (339f * (float)Math.PI)),
			new Color(22f / 255f, 14f / 255f, 44f / 255f),
			new Color(2f / 85f, 8f / 51f, 74f / 255f),
			new Color(39f / 85f, 37f / 255f, 142f / (339f * (float)Math.PI))
		};

		private BackgroundView View => (BackgroundView)ViewComponent;

		public BackgroundViewMediator(string name, BackgroundView viewComponent)
			: base(name, viewComponent)
		{
		}

		public override void OnRegister()
		{
			base.OnRegister();
		}

		public override void OnRemove()
		{
			base.OnRemove();
		}

		public override IList<string> ListNotificationInterests()
		{
			List<string> list = new List<string>();
			list.Add("BG_INIT");
			list.Add("BG_ZOOM");
			list.Add("BG_MOVE");
			list.Add("BG_CHANGE");
			list.Add("BG_CHANGE_TEST");
			return list;
		}

		public override void HandleNotification(INotification notification)
		{
			string name = notification.Name;
			if (name == null)
			{
				return;
			}
			if (!(name == "BG_INIT"))
			{
				if (!(name == "BG_ZOOM"))
				{
					if (!(name == "BG_CHANGE"))
					{
						if (name == "BG_CHANGE_TEST")
						{
							SetBgTextureByBgID((int)notification.Body);
						}
					}
					else
					{
						SetBgTexture();
						View.rootItem.localScale = Vector3.one * 1.5f;
					}
				}
				else
				{
					SetZoomBg((bool)notification.Body);
					SetBgMove();
					View.item.Play();
				}
			}
			else
			{
				SetBgTexture();
				View.rootItem.localScale = Vector3.one * 1.5f;
			}
		}

		private void SetBgTexture()
		{
			LevelStruct levelStruct = Singleton<GameJson>.Instance.LevelJson[GameGlobalData.CurLevel];
			int num = Mathf.Clamp(levelStruct.BgID, 1, _bgColors.Length);
			if (_curID != num)
			{
				_curID = num;
				if (View.item != null)
				{
					View.item.SetDestroy();
				}
				View.item = UnityEngine.Object.Instantiate(Resources.Load<UIBackgroundItem>("GameUI/BackgroundView/ui_bg_" + num), View.rootItem);
				View.item.Play();
				View.rimgBgLong.color = _bgColors[num - 1];
			}
		}

		private void SetBgTextureByBgID(int bgID)
		{
			_curID = bgID + 1;
			if (View.item != null)
			{
				View.item.SetDestroy();
			}
			View.item = UnityEngine.Object.Instantiate(Resources.Load<UIBackgroundItem>("GameUI/BackgroundView/ui_bg_" + _curID), View.rootItem);
			View.item.Play();
			View.rimgBgLong.color = _bgColors[_curID - 1];
		}

		private void SetZoomBg(bool isZoomIn)
		{
			ShortcutExtensions.DOScale(endValue: (!isZoomIn) ? (Vector3.one * 1.1f) : (Vector3.one * 1.5f), target: View.rootItem, duration: 0.5f).SetAutoKill(autoKillOnCompletion: true);
		}

		private void SetBgMove()
		{
			if (_timer == null)
			{
				_timer = Timer.Register(0.1f, null, BgMove, isLooped: true);
			}
		}

		private void BgMove(float time)
		{
			Vector2 normalized = GameGlobalData.TouchPos.normalized;
			Vector2 vector = View.rootItem.anchoredPosition - normalized;
			float num = Mathf.Clamp(vector.x, -30f, 30f);
			RectTransform rootItem = View.rootItem;
			Vector2 anchoredPosition = View.rootItem.anchoredPosition;
			float x = num;
			Vector2 anchoredPosition2 = View.rootItem.anchoredPosition;
			rootItem.anchoredPosition = Vector2.Lerp(anchoredPosition, new Vector2(x, anchoredPosition2.y), 0.7f);
		}
	}
}
