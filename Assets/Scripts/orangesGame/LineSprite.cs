using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	public class LineSprite : MonoBehaviour
	{
		public List<Vector3> Positions = new List<Vector3>
		{
			Vector3.zero,
			Vector3.zero
		};

		protected SpriteRenderer _spriteRenderer;

		public SpriteRenderer Renderer => _spriteRenderer;

		private void Awake()
		{
			_spriteRenderer = base.gameObject.GetComponent<SpriteRenderer>();
		}

		public void SetPosition(int index, Vector3 pos)
		{
			Positions[index] = pos;
		}

		private void LateUpdate()
		{
			Vector2 to = Positions[1] - Positions[0];
			float magnitude = to.magnitude;
			base.transform.position = (Positions[1] + Positions[0]) / 2f;
			Vector2 size = _spriteRenderer.sprite.rect.size;
			float x = size.x;
			float num = magnitude / x * 100f;
			Vector3 one = Vector3.one;
			float num2 = num;
			Vector3 localScale = base.transform.parent.localScale;
			one.x = num2 / localScale.x;
			Vector3 localScale2 = base.transform.parent.localScale;
			one.y = 1f / localScale2.y;
			base.transform.localScale = one;
			float z = Vector2.SignedAngle(Vector2.right, to);
			base.transform.rotation = Quaternion.Euler(0f, 0f, z);
		}
	}
}
