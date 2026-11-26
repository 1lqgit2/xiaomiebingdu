using UnityEngine;

namespace orangesGame
{
	public class Bullet : MonoBehaviour
	{
		public bool isDirty;

		public BulletType type;

		public Vector3 stayPos;

		protected Transform _tran;

		protected float _atk;

		protected float _speed;

		protected Vector3 _curMoveSpeed;

		protected SpriteRenderer[] _renderers;

		public float atk => _atk;

		public Transform tran => _tran;

		public void AwakeInit()
		{
			_tran = base.transform;
			_renderers = GetComponentsInChildren<SpriteRenderer>();
		}

		public virtual void Init()
		{
			isDirty = false;
			if (_tran != null)
			{
				stayPos = _tran.localPosition;
			}
		}

		private void LateUpdate()
		{
			stayPos = _tran.localPosition;
		}

		public virtual void OnTriggerLogic(float ballValue = 0f)
		{
		}

		public virtual void Clean()
		{
			stayPos = _tran.localPosition;
			_tran.localPosition = GameGlobalData.GameItemHidePos;
			isDirty = true;
		}

		public virtual void SetParent(Transform parent)
		{
			_tran.SetParent(parent);
			Vector3 localPosition = parent.localPosition;
			int num = (int)localPosition.z;
			for (int i = 0; i < _renderers.Length; i++)
			{
				_renderers[i].sortingOrder += num;
			}
		}

		public virtual void PlaySound()
		{
		}
	}
}
