using UnityEngine;

namespace orangesGame
{
	public class BombEffect : IEffectAction
	{
		private string _order = "eff_venom_bomb";

		private string _resPath = "GameEffect/Venom/Bomb/eff_venom_bomb";

		private string[] _animNames = new string[4]
		{
			"anim_venom_bomb_1",
			"anim_venom_bomb_2",
			"anim_venom_bomb_3",
			"anim_venom_bomb_4"
		};

		private Transform _parent;

		private int _length;

		private ChipEffect _chipEffect;

		private int _count;

		private const int COUNT_MAX = 10;

		public BombEffect()
		{
			_parent = Singleton<UIViewManager>.Instance.GetLayerTran(40);
			_length = _animNames.Length;
			_chipEffect = new ChipEffect();
		}

		public void Spawn(Vector3 pos)
		{
		}

		public void Spawn(Vector3 pos, Vector3 scale)
		{
			EffectBehaviour effectBehaviour = Singleton<EffectManager>.Instance.GetEffect(_order);
			if (effectBehaviour == null)
			{
				if (_count >= 10)
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
			effectBehaviour.tran.localScale = scale;
			int num = Random.Range(0, _length);
			effectBehaviour.Play(_animNames[num]);
			_chipEffect.Spawn(pos, scale, num);
		}

		public void Spawn(Vector3 pos, Vector3 scale, int index)
		{
		}

		public void Clean()
		{
		}
	}
}
