using UnityEngine;

namespace orangesGame
{
	public class UIItemIcon : MonoBehaviour
	{
		public Vector2 GridSize;

		public float ApproachSpeed = 4f;

		protected RectTransform _rectTran;

		private void Awake()
		{
			_rectTran = base.gameObject.GetComponent<RectTransform>();
		}

		private void Update()
		{
			for (int i = 0; i < _rectTran.childCount; i++)
			{
				RectTransform rectTransform = _rectTran.GetChild(i) as RectTransform;
				Vector2 anchoredPosition = rectTransform.anchoredPosition;
				rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, GetAnchoredPos(i), Time.deltaTime * ApproachSpeed);
			}
		}

		private Vector2 GetAnchoredPos(int index)
		{
			return index * GridSize + new Vector2(0f, 550f);
		}

		public void AppendChild(Transform tran)
		{
			tran.SetParent(_rectTran);
			tran.SetAsLastSibling();
			(tran as RectTransform).anchoredPosition = GetAnchoredPos(_rectTran.childCount - 1);
		}
	}
}
