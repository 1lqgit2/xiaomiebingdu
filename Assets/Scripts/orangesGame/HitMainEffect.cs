using UnityEngine;

namespace orangesGame
{
	public class HitMainEffect : HitEffect
	{
		public HitMainEffect()
		{
			_order = "eff_player_main1_hit";
			_resPath = "GameEffect/HitEffect/" + _order;
			_countMax = 15;
		}

		public override void Spawn(Vector3 pos)
		{
			EffectBehaviour effectBehaviour = Singleton<EffectManager>.Instance.GetEffect(_order);
			if (effectBehaviour == null)
			{
				if (_count >= _countMax)
				{
					return;
				}
				_count++;
				effectBehaviour = Object.Instantiate(Resources.Load<EffectBehaviour>(_resPath));
				effectBehaviour.SetParent(_parent);
			}
			else
			{
				effectBehaviour.gameObject.SetActive(value: true);
			}
			effectBehaviour.tran.localPosition = pos;
			effectBehaviour.GetSpriteRenderer(0).localRotation = Quaternion.Euler(0f, 0f, Random.Range(0, 360));
		}
	}
}
