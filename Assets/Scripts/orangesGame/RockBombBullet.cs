using UnityEngine;

namespace orangesGame
{
	public class RockBombBullet : Bullet
	{
		public Collider2D bombRange;

		private const float DELAY = 0.1f;

		private float _scale;

		private Timer _timer;

		private IEffectAction _hitEffect;

		private void Awake()
		{
			AwakeInit();
			_hitEffect = Singleton<EffectManager>.Instance.GetEffAction(5u);
		}

		public void SetProperty(float atk, float scale)
		{
			if (_atk != atk)
			{
				_atk = atk;
				_scale = scale;
				_tran.localScale = Vector2.one * _scale;
			}
		}

		public void OnStart(Vector2 pos)
		{
			_tran.localPosition = pos;
			_hitEffect.Spawn(pos, _tran.localScale);
			_timer = Timer.Register(Time.deltaTime, ((Bullet)this).Clean);
		}

		public override void OnTriggerLogic(float ballValue = 0f)
		{
		}

		public override void Clean()
		{
			if (_timer != null && !_timer.isCancelled)
			{
				_timer.Cancel();
			}
			base.Clean();
		}
	}
}
