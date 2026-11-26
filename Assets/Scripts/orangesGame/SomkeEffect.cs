using System;
using UnityEngine;

namespace orangesGame
{
	public class SomkeEffect : IEffectAction
	{
		private string _order = "eff_player_die_smoke";

		private string _resPath = "GameEffect/Player/Die/eff_player_die_smoke";

		public void Clean()
		{
			throw new NotImplementedException();
		}

		public void Spawn(Vector3 pos)
		{
			EffectBehaviour effectBehaviour = Singleton<EffectManager>.Instance.GetEffect(_order);
			if (effectBehaviour == null)
			{
				GameObject original = Resources.Load(_resPath) as GameObject;
				GameObject gameObject = UnityEngine.Object.Instantiate(original);
				effectBehaviour = gameObject.GetComponent<EffectBehaviour>();
				effectBehaviour.SetParent(Singleton<UIViewManager>.Instance.GetLayerTran(40));
			}
			else
			{
				effectBehaviour.gameObject.SetActive(value: true);
			}
			effectBehaviour.transform.localPosition = pos;
		}

		public void Spawn(Vector3 pos, Vector3 scale)
		{
		}

		public void Spawn(Vector3 pos, Vector3 scale, int index)
		{
		}
	}
}
