using UnityEngine;

namespace orangesGame
{
	public struct BallVO
	{
		public Vector3 InitPos;

		public int BallID;

		public int Type;

		public int DivideCount;

		public int DivideValue;

		public float HitCount;

		public int Dir;

		public float RatateSpeed;

		public CoinVO[][] CoinDatas;

		public float MoveSpeed;

		public float Scale;

		public int ShowValue;

		public int DivideID;

		public float HitEffect;

		public float BombScale;

		public float Shake;

		public int HasItem;

		public int ChildFlag;

		public bool isLaserHit;
	}
}
