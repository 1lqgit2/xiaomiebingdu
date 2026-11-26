using DG.Tweening;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	public class OldBackgroundViewMediator : Mediator, IMediator
	{
		private const float ZOOM_0 = 1.5f;

		private const float ZOOM_1 = 1.1f;

		private const float ZOOM_DURATION = 0.5f;

		private int _curID;

		private Timer _timer;

		private OldBackgroundView View => (OldBackgroundView)ViewComponent;

		public OldBackgroundViewMediator(string name, OldBackgroundView viewComponent)
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
						View.TranBgs[_curID].localScale = Vector3.one * 1.5f;
					}
				}
				else
				{
					SetZoomBg((bool)notification.Body);
					SetBgMove();
				}
			}
			else
			{
				SetBg();
				SetBgTexture();
				View.TranBgs[_curID].localScale = Vector3.one * 1.5f;
			}
		}

		public void SetBg()
		{
			_curID = 0;
		}

		private void SetBgTexture()
		{
			LevelStruct levelStruct = Singleton<GameJson>.Instance.LevelJson[GameGlobalData.CurLevel];
			int num = Mathf.Clamp(levelStruct.BgID, 1, ResPath.BACKGROUNDS.Length) - 1;
			View.RImgBg.texture = (Resources.Load(ResPath.BACKGROUNDS[num]) as Texture);
		}

		private void SetBgTextureByBgID(int bgID)
		{
			View.RImgBg.texture = (Resources.Load(ResPath.BACKGROUNDS[bgID]) as Texture);
		}

		private void SetZoomBg(bool isZoomIn)
		{
			ShortcutExtensions.DOScale(endValue: (!isZoomIn) ? (Vector3.one * 1.1f) : (Vector3.one * 1.5f), target: View.TranBgs[_curID], duration: 0.5f).SetAutoKill(autoKillOnCompletion: true);
		}

		private void SetBgMove(bool isStart = true)
		{
			if (isStart && _timer == null)
			{
				_timer = Timer.Register(0.1f, null, BgMove, isLooped: true);
			}
		}

		private void BgMove(float time)
		{
			Vector2 normalized = GameGlobalData.TouchPos.normalized;
			Vector2 vector = View.TranBgs[_curID].anchoredPosition - normalized;
			float x = Mathf.Clamp(vector.x, -30f, 30f);
			float y = Mathf.Clamp(vector.y, -30f, 30f);
			View.TranBgs[_curID].anchoredPosition = Vector2.Lerp(View.TranBgs[_curID].anchoredPosition, new Vector2(x, y), 0.7f);
		}
	}
}
