using UnityEngine;

namespace orangesGame
{
	public class RoleBase : MonoBehaviour
	{
		public GameObject objFire;

		protected Transform _tran;

		protected Shooter _shooter;

		protected Transform _shooterTran;

		protected SpriteRenderer[] _renderers;

		public Transform tran => _tran;

		protected void InitAwake()
		{
			_tran = base.transform;
			_shooterTran = _tran.Find("ShooterPos");
			_renderers = GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
		}

		public virtual void Init(PlayerVO vo, Transform parent)
		{
			base.name = vo.name;
			SetParent(parent);
			GameUtils.InitTranLocal(_tran);
		}

		public virtual void SetShooter(ShooterVO vo, bool isChange = false)
		{
			if (_shooter == null || isChange)
			{
				_shooter = Singleton<ShooterManager>.Instance.Spawn(vo, _shooterTran);
			}
			else
			{
				_shooter.Reset(vo);
			}
		}

		public virtual void SetShooting(bool isShooting)
		{
		}

		protected virtual void SetParent(Transform parent, int orderInLayer = -1)
		{
			if (parent != null)
			{
				_tran.SetParent(parent);
			}
			int num;
			if (orderInLayer < 0)
			{
				Vector3 localPosition = parent.localPosition;
				num = (int)localPosition.z;
			}
			else
			{
				num = orderInLayer;
			}
			int num2 = num;
			for (int i = 0; i < _renderers.Length; i++)
			{
				_renderers[i].sortingOrder += num2;
			}
		}
	}
}
