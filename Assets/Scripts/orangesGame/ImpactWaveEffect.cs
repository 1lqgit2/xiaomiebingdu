using DG.Tweening;
using UnityEngine;

namespace orangesGame
{
	public class ImpactWaveEffect
	{
		private string _order = "ImpactWave";

		private string _resPath = "GameEffect/revive";

		private Transform _parent;

		private Transform _colliderImpactWave;

		private Transform _colliderHit;

		private EffectBehaviour _this;

		public LayerMask LayerMask;

		public ImpactWaveEffect()
		{
			_parent = Singleton<UIViewManager>.Instance.GetLayerTran(40);
		}

		public void Spawn(Vector3 pos)
		{
			EffectBehaviour effectBehaviour = _this = Singleton<EffectManager>.Instance.GetEffect(_order);
			if (effectBehaviour == null)
			{
				GameObject original = Resources.Load(_resPath) as GameObject;
				GameObject gameObject = Object.Instantiate(original);
				effectBehaviour = gameObject.GetComponent<EffectBehaviour>();
				effectBehaviour.SetParent(_parent);
				_colliderImpactWave = effectBehaviour.transform.Find("ColliderImpactWave");
			}
			else
			{
				effectBehaviour.gameObject.SetActive(value: true);
			}
			effectBehaviour.transform.localPosition = pos + new Vector3(0f, 0.5f, 0f);
			_colliderImpactWave.localScale = new Vector3(1f, 0f, 1f);
			_colliderImpactWave.DOScaleY(1f, 2.7f).onComplete = delegate
			{
				_colliderImpactWave.localScale = Vector3.zero;
			};
		}

		private void LateUpdate()
		{
			if ((bool)_this)
			{
				UnityEngine.Debug.Log("Wave LateUpdate");
				BoxCollider2D component = _this.GetComponent<BoxCollider2D>();
				Vector3 localPosition = _this.transform.localPosition;
				float x = localPosition.x;
				Vector3 localPosition2 = _this.transform.localPosition;
				Vector2 a = new Vector2(x, localPosition2.y);
				Collider2D[] array = Physics2D.OverlapAreaAll(a - component.size, a + component.size, LayerMask);
				if (array.Length > 0)
				{
					UnityEngine.Debug.Log("Wave is collision");
				}
			}
		}
	}
}
