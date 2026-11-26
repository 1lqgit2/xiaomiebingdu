using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	public class VenomHealer : VenomCommon
	{
		public VenomScanner Scanner;

		public LineSprite LineTran;

		public ParticleSystemWidget HealParticle;

		protected int _maxHealCount = 3;

		protected List<HealerModule> _healerModules = new List<HealerModule>();

		protected override void Awake()
		{
			base.Awake();
			Scanner.EventOnTargetStay.AddListener(OnTargetStay);
			Scanner.EventOnRemoveTarget.AddListener(OnRemoveTarget);
			Scanner.EventOnProcessTarget.AddListener(StartHealing);
		}

		protected override void InitState(BallVO ballVO)
		{
			base.InitState(ballVO);
			ClearHealerModules();
		}

		protected virtual void OnTargetStay(GameObject go)
		{
			VenomCommon target = go.gameObject.GetComponent<VenomCommon>();
			if (target != null && _healerModules.Count < _maxHealCount && !(target is VenomHealer) && !target.GetIsFullHitCount && !target.GetIsDie && _healerModules.Find((HealerModule x) => x.Target == target) == null)
			{
				HealerModule item = new HealerModule(this, target, LineTran, 0.02f);
				_healerModules.Add(item);
			}
		}

		protected virtual void OnRemoveTarget(GameObject go)
		{
			VenomCommon target = go.gameObject.GetComponent<VenomCommon>();
			if (target != null)
			{
				RemoveTargetHealer(_healerModules.Find((HealerModule x) => x.Target == target));
			}
		}

		protected virtual void RemoveTargetHealer(HealerModule healer)
		{
			if (healer != null)
			{
				healer.Destroy();
				_healerModules.Remove(healer);
			}
		}

		protected override void OnDie()
		{
			base.OnDie();
			ClearHealerModules();
		}

		protected void ClearHealerModules()
		{
			foreach (HealerModule healerModule in _healerModules)
			{
				healerModule.Destroy();
			}
			_healerModules.Clear();
		}

		protected virtual void StartHealing()
		{
			for (int num = _healerModules.Count - 1; num >= 0; num--)
			{
				if (_healerModules[num].GetIsTargetFullHitCount || _healerModules[num].GetIsTargetDie)
				{
					RemoveTargetHealer(_healerModules[num]);
				}
			}
			foreach (HealerModule healerModule in _healerModules)
			{
				healerModule.Update();
			}
		}

		public override void Pause()
		{
			base.Pause();
			foreach (HealerModule healerModule in _healerModules)
			{
				healerModule.HealFactor = 0f;
			}
		}

		public override void Continue()
		{
			base.Continue();
			foreach (HealerModule healerModule in _healerModules)
			{
				healerModule.HealFactor = 1f;
			}
		}
	}
}
