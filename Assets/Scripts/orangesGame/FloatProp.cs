using DG.Tweening;
using UnityEngine;

namespace orangesGame
{
	public class FloatProp : FloatCoin
	{
		private void Awake()
		{
			AwakeInit();
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
				FloatProp floatProp = this;
				t.OnComplete(((GameItem)floatProp).Clean);
			})
				.SetEase(Ease.OutQuad);
		}

		public override void Clean()
		{
			_tran.DOKill();
			base.Clean();
		}
	}
}
