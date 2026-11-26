using UnityEngine;

namespace orangesGame
{
	public class EffectBehaviour : MonoBehaviour
	{
		private float _timer;

		private ParticleSystem _particleSystem;

		private ParticleSystemRenderer[] _renderers;

		private SpriteRenderer[] _sprRenderers;

		public float deathtimer = 10f;

		public bool isDestroy;

		public string effectOrder;

		public Transform tran;

		public ParticleSystemWidget particleWidget;

		public Animator animCtrl;

		private void Awake()
		{
			tran = base.transform;
			_particleSystem = GetComponent<ParticleSystem>();
			_renderers = GetComponentsInChildren<ParticleSystemRenderer>();
			_sprRenderers = GetComponentsInChildren<SpriteRenderer>();
			if (!isDestroy)
			{
				Singleton<EffectManager>.Instance.Add(effectOrder, this);
			}
		}

		private void Start()
		{
		}

		private void Update()
		{
			_timer += Time.deltaTime;
			if (_timer >= deathtimer)
			{
				if (isDestroy)
				{
					UnityEngine.Object.Destroy(base.gameObject);
				}
				else
				{
					Clean();
				}
			}
		}

		public void SetParent(Transform parent)
		{
			base.transform.SetParent(parent);
			Vector3 localPosition = parent.localPosition;
			int num = (int)localPosition.z;
			if (_renderers != null)
			{
				for (int i = 0; i < _renderers.Length; i++)
				{
					_renderers[i].sortingOrder = _renderers[i].sortingOrder + num;
				}
			}
			if (_sprRenderers != null)
			{
				for (int j = 0; j < _sprRenderers.Length; j++)
				{
					_sprRenderers[j].sortingOrder = _sprRenderers[j].sortingOrder + num;
				}
			}
		}

		public void Clean()
		{
			Stop();
			_timer = 0f;
			base.gameObject.SetActive(value: false);
		}

		public void Stop()
		{
			if (_particleSystem != null)
			{
				_particleSystem.Stop(withChildren: true);
			}
		}

		public void Pause()
		{
			if (_particleSystem != null)
			{
				_particleSystem.Pause(withChildren: true);
			}
		}

		public void Play()
		{
			if (_particleSystem != null)
			{
				_particleSystem.time = 0f;
				_particleSystem.Play(withChildren: true);
			}
		}

		public void Play(string name)
		{
			if (animCtrl != null)
			{
				animCtrl.Play(name);
			}
		}

		public Transform GetSpriteRenderer(int index)
		{
			return _sprRenderers[index].transform;
		}
	}
}
