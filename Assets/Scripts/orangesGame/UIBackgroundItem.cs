using UnityEngine;

namespace orangesGame
{
	public class UIBackgroundItem : MonoBehaviour
	{
		public RawImageWidget widget;

		public RectTransform[] trans;

		public float[] speeds;

		public bool isPause;

		private float[] _bottomBorders;

		private float[] _topBorders;

		private Vector2[] _origin;

		private int _length;

		private void Start()
		{
			isPause = true;
			_length = trans.Length;
			_bottomBorders = new float[_length];
			_origin = new Vector2[_length];
			_topBorders = new float[_length];
			for (int i = 0; i < trans.Length; i++)
			{
				_origin[i] = trans[i].anchoredPosition;
				_bottomBorders[i] = 0f - Singleton<UIViewManager>.Instance.uiHeightHalf - trans[i].rect.width / 2f;
				_topBorders[i] = Singleton<UIViewManager>.Instance.uiHeightHalf + trans[i].rect.width / 2f;
			}
			if (widget != null)
			{
				widget.Pause();
			}
		}

		private void Update()
		{
			if (isPause)
			{
				return;
			}
			for (int i = 0; i < _length; i++)
			{
				Vector2 anchoredPosition = trans[i].anchoredPosition;
				if (anchoredPosition.y < _bottomBorders[i])
				{
					RectTransform obj = trans[i];
					Vector2 anchoredPosition2 = trans[i].anchoredPosition;
					obj.anchoredPosition = new Vector2(anchoredPosition2.x, _topBorders[i]);
				}
				trans[i].anchoredPosition -= new Vector2(0f, speeds[i] * Time.deltaTime);
			}
		}

		public void SetDestroy()
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}

		public void Play()
		{
			isPause = false;
			if (widget != null)
			{
				widget.Play();
			}
		}

		public void Pause()
		{
			isPause = true;
			if (widget != null)
			{
				widget.Pause();
			}
		}
	}
}
