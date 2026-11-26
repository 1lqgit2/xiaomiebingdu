using UnityEngine;
using UnityEngine.UI;

namespace orangesGame
{
	public class LevelBackGroundComponent : MonoBehaviour
	{
		public RawImage imgBg;

		public float speed;

		public RectTransform[] tranUnits;

		public float[] unitsSpeeds;

		private int _length;

		private float[] _bottomBorders;

		private float[] _topBorders;

		private float _dir;

		private void Start()
		{
			_length = tranUnits.Length;
			_bottomBorders = new float[_length];
			_topBorders = new float[_length];
			for (int i = 0; i < _length; i++)
			{
				_bottomBorders[i] = -960f - tranUnits[i].rect.width / 2f;
				_topBorders[i] = 960f + tranUnits[i].rect.width / 2f;
			}
		}

		public void OnUpdate(float dir)
		{
			_dir = dir;
			UvMove();
			UnitsMove();
		}

		private void UnitsMove()
		{
			for (int i = 0; i < _length; i++)
			{
				Vector2 anchoredPosition = tranUnits[i].anchoredPosition;
				if (anchoredPosition.y < _bottomBorders[i])
				{
					RectTransform obj = tranUnits[i];
					Vector2 anchoredPosition2 = tranUnits[i].anchoredPosition;
					obj.anchoredPosition = new Vector2(anchoredPosition2.x, _topBorders[i]);
				}
				tranUnits[i].anchoredPosition -= new Vector2(0f, unitsSpeeds[i] * _dir * Time.deltaTime);
			}
		}

		private void UvMove()
		{
			float num = speed * _dir * Time.deltaTime;
			Rect uvRect = imgBg.uvRect;
			uvRect.y += num;
			imgBg.uvRect = uvRect;
		}
	}
}
