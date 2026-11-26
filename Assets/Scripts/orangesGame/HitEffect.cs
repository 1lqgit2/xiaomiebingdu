using UnityEngine;

namespace orangesGame
{
	public class HitEffect : IEffectAction
	{
		protected Transform _parent;

		protected string _order;

		protected string _resPath;

		protected int _count;

		protected int _countMax = 10;

		public HitEffect()
		{
			_parent = Singleton<UIViewManager>.Instance.GetLayerTran(40);
		}

		public virtual void Spawn(Vector3 pos)
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
		}

		public void Spawn(Vector3 pos, Vector3 scale)
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
			effectBehaviour.tran.localScale = scale;
		}

		public void Spawn(Vector3 pos, Vector3 scale, int index)
		{
		}

		public void Clean()
		{
		}
	}
}
