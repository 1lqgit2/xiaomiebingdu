using UnityEngine;

namespace orangesGame
{
	public class VenomSlower : VenomCommon
	{
		public LineSprite Line;

		public VenomScanner Scanner;

		protected Player _slowPlayer;

		protected float _scanSmallestRadius = 2f;

		protected int _instanceId;

		protected override void Awake()
		{
			base.Awake();
			_instanceId = _tran.GetInstanceID();
			Scanner.EventOnTargetStay.AddListener(OnTargetStay);
			Scanner.EventOnRemoveTarget.AddListener(OnRemoveTarget);
			Scanner.EventOnProcessTarget.AddListener(OnProcessTarget);
		}

		protected virtual void OnTargetStay(GameObject go)
		{
			Player component = go.GetComponent<Player>();
			if (component != null)
			{
				_slowPlayer = component;
			}
		}

		protected virtual void OnRemoveTarget(GameObject go)
		{
			Player component = go.GetComponent<Player>();
			if (component != null && _slowPlayer == component)
			{
				RecoverPlayerSpeed();
			}
		}

		protected virtual void OnProcessTarget()
		{
			if (_slowPlayer != null && !_slowPlayer.PlayerDead)
			{
				_slowPlayer.SlowVenomDown("VenomSlower_" + _instanceId);
				Line.Renderer.enabled = true;
				Line.SetPosition(0, (Vector2)base.transform.position);
				Line.SetPosition(1, (Vector2)_slowPlayer.tranHead.position);
			}
		}

		protected virtual void RecoverPlayerSpeed()
		{
			if (_slowPlayer != null)
			{
				_slowPlayer.RecoverSpeed("VenomSlower_" + _instanceId);
				Line.Renderer.enabled = false;
				_slowPlayer = null;
			}
		}

		protected override void InitState(BallVO ballVO)
		{
			base.InitState(ballVO);
			Line.Renderer.enabled = false;
			Line.gameObject.SetActive(value: true);
		}

		protected override void OnDie()
		{
			base.OnDie();
			RecoverPlayerSpeed();
		}

		protected override void Update()
		{
			base.Update();
			if (_ballVO.DivideCount == 1)
			{
				Scanner.Radius = 3.5f;
			}
			else if (_ballVO.DivideCount == 2)
			{
				Scanner.Radius = 5f;
			}
			else
			{
				Scanner.Radius = _scanSmallestRadius * (float)_ballVO.DivideCount;
			}
		}

		public override void Pause()
		{
			base.Pause();
			Line.gameObject.SetActive(value: false);
		}

		public override void Continue()
		{
			base.Continue();
			Line.gameObject.SetActive(value: true);
		}
	}
}
