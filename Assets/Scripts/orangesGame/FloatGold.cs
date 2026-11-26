using DG.Tweening;
using UnityEngine;

namespace orangesGame
{
	public class FloatGold : FloatCoin
	{
		public Animator anim;

		private int _count;

		private bool _isAddCoin;

		private void Awake()
		{
			AwakeInit();
		}

		public void SetInit(int count)
		{
			_isDirty = false;
			_count = count;
			_isAddCoin = true;
		}

		public override void SetInit()
		{
			_isAddCoin = false;
			base.SetInit();
		}

		public void Moving(Vector2 pos, float angle, float distance)
		{
			Moving(pos, angle, distance, GameGlobalData.CoinRecoverPos, 2f, 0.25f);
		}

		public void Moving(Vector2 pos, float duration, Vector3[] path)
		{
			_tran.localPosition = pos;
			_tran.DOPath(path, duration, PathType.CatmullRom, PathMode.Sidescroller2D).SetAutoKill(autoKillOnCompletion: true).OnComplete(((GameItem)this).Clean);
			anim.enabled = true;
		}

		public override void Moving(Vector2 pos, float angle, float distance, Vector2 target, float preMoveCircleRadius = 2f, float preMoveDuration = 0.7f)
		{
			_tran.localPosition = pos;
			_tran.DOMove(Quaternion.Euler(0f, 0f, angle) * Vector2.up * preMoveCircleRadius + _tran.localPosition, preMoveDuration).SetAutoKill(autoKillOnCompletion: true).OnComplete(delegate
			{
				Vector2 a = _tran.position;
				float magnitude = (a - target).magnitude;
				float duration = magnitude / 10f;
				Tweener t = _tran.DOMove(target, duration).SetAutoKill(autoKillOnCompletion: true);
				FloatGold floatGold = this;
				t.OnComplete(((GameItem)floatGold).Clean);
			})
				.SetEase(Ease.OutQuad);
			anim.enabled = true;
		}

		public override void Clean()
		{
			ApplicationFacade.Instance.SendNotification("BATTLE_VIEW_GET_COIN_ANI");
			if (_isAddCoin)
			{
				Singleton<AudioManager>.Instance.PlaySpeciSound("coin");
				GameGlobalData.AddCurGetCoin(_count);
			}
			_tran.DOKill();
			anim.enabled = false;
			base.Clean();
		}
	}
}
