using UnityEngine;

namespace orangesGame
{
	public class ChipEffect : IEffectAction
	{
		private string _ordersChip = "eff_venom_bomb_chip";

		private string _resPathChip = "GameEffect/Venom/Bomb/eff_venom_bomb_chip";

		private string _ordersLight = "eff_venom_bomb_light";

		private string _resPathLight = "GameEffect/Venom/Bomb/eff_venom_bomb_light";

		private Color[] _colors = new Color[4]
		{
			new Color(196f / 255f, 0.235294119f, 1f),
			new Color(12f / 85f, 1f, 0.117647059f),
			new Color(0.117647059f, 67f / 85f, 1f),
			new Color(43f / 51f, 1f, 0.117647059f)
		};

		private Transform _parent;

		private int _count;

		private const int COUNT_MAX = 2;

		public ChipEffect()
		{
			_parent = Singleton<UIViewManager>.Instance.GetLayerTran(40);
		}

		public void Spawn(Vector3 pos)
		{
		}

		public void Spawn(Vector3 pos, Vector3 scale)
		{
		}

		public void Spawn(Vector3 pos, Vector3 scale, int index)
		{
			SpawnLight(pos, scale);
			EffectBehaviour effectBehaviour = Singleton<EffectManager>.Instance.GetEffect(_ordersChip);
			if (effectBehaviour == null)
			{
				if (_count >= 2)
				{
					return;
				}
				_count++;
				effectBehaviour = Object.Instantiate(Resources.Load<EffectBehaviour>(_resPathChip));
				effectBehaviour.SetParent(_parent);
			}
			else
			{
				effectBehaviour.gameObject.SetActive(value: true);
			}
			effectBehaviour.particleWidget.SetColor(_colors[index]);
			effectBehaviour.transform.localPosition = pos;
			effectBehaviour.transform.localScale = scale / 4f;
		}

		private void SpawnLight(Vector3 pos, Vector3 scale)
		{
			EffectBehaviour effectBehaviour = Singleton<EffectManager>.Instance.GetEffect(_ordersLight);
			if (effectBehaviour == null)
			{
				if (_count >= 2)
				{
					return;
				}
				effectBehaviour = Object.Instantiate(Resources.Load<EffectBehaviour>(_resPathLight));
				effectBehaviour.SetParent(_parent);
			}
			else
			{
				effectBehaviour.gameObject.SetActive(value: true);
			}
			effectBehaviour.transform.localPosition = pos;
			effectBehaviour.transform.localScale = scale;
		}

		public void Clean()
		{
		}
	}
}
