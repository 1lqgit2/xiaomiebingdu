using UnityEngine;

namespace orangesGame
{
	public class HealerModule
	{
		public float HealRating;

		public VenomHealer Master;

		public VenomCommon Target;

		public float HealPeriod = 0.5f;

		public float HealFactor = 1f;

		protected LineSprite _lineRenderer;

		protected float _healTimer;

		public bool GetIsTargetFullHitCount => Target.GetIsFullHitCount;

		public bool GetIsTargetDie => Target.GetIsDie;

		public HealerModule(VenomHealer master, VenomCommon target, LineSprite lineTran, float healRating)
		{
			HealRating = healRating;
			Master = master;
			Target = target;
			_healTimer = HealPeriod;
			_lineRenderer = Object.Instantiate(lineTran, lineTran.transform.parent);
			_lineRenderer.Renderer.enabled = true;
			UpdateLinePos();
			target.GetHealParticle.Play();
		}

		public void Destroy()
		{
			_lineRenderer.gameObject.SetActive(value: false);
			UnityEngine.Object.Destroy(_lineRenderer.gameObject);
			_lineRenderer = null;
			Target.GetHealParticle.Stop();
			Master = null;
			Target = null;
		}

		public void Update()
		{
			UpdateLinePos();
			if (_healTimer <= 0f)
			{
				Heal();
				_healTimer = HealPeriod;
			}
			else
			{
				_healTimer -= Time.deltaTime;
			}
		}

		public float GetTargetDistanceSqr()
		{
			return Vector2.SqrMagnitude(Master.transform.position - Target.transform.position);
		}

		private void UpdateLinePos()
		{
			_lineRenderer.SetPosition(0, (Vector2)Master.transform.position);
			_lineRenderer.SetPosition(1, (Vector2)Target.transform.position);
			_lineRenderer.Renderer.color = Master.GetCurrentColor;
		}

		private void Heal()
		{
			float a = HealRating * HealFactor;
			if (!Mathf.Approximately(a, 0f))
			{
				Target.AddHPRating(HealRating);
			}
		}
	}
}
