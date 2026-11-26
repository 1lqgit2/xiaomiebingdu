using UnityEngine;

namespace orangesGame
{
	public class FloatCoin : GameItem
	{
		public virtual void SetInit()
		{
			_isDirty = false;
		}

		public virtual void Moving(Vector2 pos, float angle, float distance, Vector2 target, float preMoveCircleRadius = 2f, float preMoveDuration = 0.7f)
		{
		}
	}
}
