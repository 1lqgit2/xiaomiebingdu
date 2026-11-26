using UnityEngine;

namespace orangesGame
{
	public class GameItem : MonoBehaviour
	{
		protected Transform _tran;

		protected SpriteRenderer[] _renderers;

		protected bool _isDirty;

		public bool IsDirty => _isDirty;

		protected void AwakeInit()
		{
			_isDirty = false;
			_tran = base.transform;
			_renderers = GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
		}

		public virtual void SetParent(Transform parent)
		{
			if (_tran.parent != parent)
			{
				_tran.SetParent(parent);
				SpriteRenderer obj = _renderers[0];
				int sortingOrder = _renderers[0].sortingOrder;
				Vector3 localPosition = parent.localPosition;
				obj.sortingOrder = sortingOrder + (int)localPosition.z;
				for (int i = 1; i < _renderers.Length; i++)
				{
					_renderers[i].sortingOrder = _renderers[0].sortingOrder;
				}
			}
		}

		public virtual void SetActive(bool isActive)
		{
		}

		public virtual void Pause()
		{
		}

		public virtual void Continue()
		{
		}

		public virtual void Clean()
		{
			_isDirty = true;
			_tran.localPosition = GameGlobalData.GameItemHidePos;
		}

		public virtual void Dispose()
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
