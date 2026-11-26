using UnityEngine;

namespace orangesGame
{
	public class BarrierBase : MonoBehaviour
	{
		protected const string TAG_WALLTOP = "WallTop";

		protected const string TAG_WALLLEFT = "WallLeft";

		protected const string TAG_WALLRIGHT = "WallRight";

		protected const string TAG_WALLBUTTOM = "WallButtom";

		protected const string TAG_PLAYER = "Player";

		protected const string TAG_IMPACTWAVE = "ImpactWave";

		protected const string TAG_IMPACTWAVEHIT = "ImpactWaveHit";

		protected const string TAG_VENOM = "Venom";

		protected Transform _tran;

		protected bool _isCanHit;

		protected bool _isDie;

		protected Rigidbody2D _rig2D;

		protected BallVO _ballVO;

		protected float _hitCount;

		protected IEffectAction _effBombAction;

		public BallVO GetBallVO => _ballVO;

		protected void AwakeInit()
		{
			_tran = base.transform;
			_rig2D = GetComponent<Rigidbody2D>();
			_effBombAction = Singleton<EffectManager>.Instance.GetEffAction(1u);
		}

		public virtual void SetParent(Transform parent)
		{
			_tran.SetParent(parent);
		}

		public virtual void Spawn(BallVO ballVO)
		{
		}

		public virtual void Divide(BallVO vo)
		{
		}

		protected virtual void InitState(BallVO ballVO)
		{
			_ballVO = ballVO;
		}

		protected virtual void Showing()
		{
		}

		protected virtual void Falling()
		{
		}

		protected virtual void Moving()
		{
		}

		protected virtual void Hitting(Vector2 pos)
		{
		}

		protected virtual void Hitting(Vector2 pos, Bullet bullet)
		{
		}

		protected virtual void Hitting(int atk)
		{
		}

		protected virtual void Dying()
		{
			_isDie = true;
			_tran.gameObject.SetActive(value: false);
		}

		protected virtual void Destroying()
		{
		}

		public virtual void Stopping(bool active)
		{
		}

		public virtual void Clean()
		{
		}

		public virtual void Pause()
		{
		}

		public virtual void Continue()
		{
		}
	}
}
