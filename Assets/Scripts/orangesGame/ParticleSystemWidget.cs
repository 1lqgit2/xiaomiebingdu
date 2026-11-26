using UnityEngine;

namespace orangesGame
{
	public class ParticleSystemWidget : MonoBehaviour
	{
		public ParticleSystem[] particleSystems;

		public ParticleSystemRenderer[] particleSystemRenderers;

		protected Color _cacheColor = Color.clear;

		private void Awake()
		{
			particleSystems = GetComponentsInChildren<ParticleSystem>(includeInactive: true);
			particleSystemRenderers = GetComponentsInChildren<ParticleSystemRenderer>(includeInactive: true);
		}

		public void SetColor(Color color)
		{
			if (color == _cacheColor)
			{
				return;
			}
			_cacheColor = color;
			for (int i = 0; i < particleSystems.Length; i++)
			{
                ParticleSystem.MainModule m = particleSystems[i].main;
                m.startColor = color;
                ParticleSystem.ColorOverLifetimeModule c = particleSystems[i].colorOverLifetime;
                c.color = color;
                ParticleSystem.Particle[] array = new ParticleSystem.Particle[particleSystems[i].particleCount];
                int particles = particleSystems[i].GetParticles(array);
                for (int j = 0; j < particles; j++)
				{
					array[j].startColor = color;
				}
				particleSystems[i].SetParticles(array, particles);
			}
		}

		public void SetOrderInLayer(int order)
		{
			for (int i = 0; i < particleSystems.Length; i++)
			{
				particleSystemRenderers[i].sortingOrder += order;
			}
		}

		public void Play()
		{
			for (int i = 0; i < particleSystems.Length; i++)
			{
				if (!particleSystems[i].isPlaying)
				{
					particleSystems[i].Play();
				}
			}
		}

		public void Stop()
		{
			for (int i = 0; i < particleSystems.Length; i++)
			{
				if (!particleSystems[i].isStopped)
				{
					particleSystems[i].Stop();
				}
			}
		}
	}
}
