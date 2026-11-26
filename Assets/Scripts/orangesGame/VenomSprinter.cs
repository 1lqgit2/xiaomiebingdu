using DG.Tweening;
using UnityEngine;

namespace orangesGame
{
	public class VenomSprinter : VenomCommon
	{
		public SpriteRenderer Cross1;

		public SpriteRenderer Cross2;

		protected float _cd = 1.5f;

		protected float _sprintDuration = 1.5f;

		protected Sequence _sprintProcess;

		protected float _speedFactor = 1f;

		protected float _sprintFactor = 1.4f;

		protected override void InitState(BallVO ballVO)
		{
			base.InitState(ballVO);
			StopSprint();
			StartSprint();
		}

		protected override void OnDie()
		{
			base.OnDie();
			StopSprint();
		}

		protected override void SetMovingPos(Vector3 pos)
		{
			base.SetMovingPos(pos * _speedFactor);
		}

		public override void Pause()
		{
			base.Pause();
			if (_sprintProcess != null && _sprintProcess.IsActive())
			{
				_sprintProcess.Pause();
			}
		}

		public override void Continue()
		{
			base.Continue();
			if (_sprintProcess != null && _sprintProcess.IsActive())
			{
				_sprintProcess.Play();
			}
		}

		public void StartSprint()
		{
			StopSprint();
			_sprintProcess = DOTween.Sequence();
			_sprintProcess.AppendInterval(_cd);
			_sprintProcess.AppendCallback(delegate
			{
				_speedFactor = _sprintFactor;
				SetCrossRotate();
			});
			_sprintProcess.AppendInterval(_sprintDuration);
			_sprintProcess.AppendCallback(delegate
			{
				_speedFactor = 1f;
				SetCrossRotate();
			});
			_sprintProcess.SetLoops(-1).SetEase(Ease.Linear);
		}

		public void StopSprint()
		{
			if (_sprintProcess != null)
			{
				_sprintProcess.Kill();
			}
			_sprintProcess = null;
			_speedFactor = 1f;
		}

		protected override void ChangeColor(Color color)
		{
			base.ChangeColor(color);
			Cross2.color = color;
		}

		protected override void SetRotate()
		{
			SetCrossRotate();
		}

		protected void SetCrossRotate()
		{
			float duration = 1f / ((_speedFactor - 1f) * 3.5f + 1f);
			DOTween.Kill(Cross2);
			Cross2.transform.DORotate(new Vector3(0f, 0f, -360f), duration, RotateMode.WorldAxisAdd).SetLoops(-1, LoopType.Incremental);
		}
	}
}
