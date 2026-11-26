using UnityEngine;

namespace orangesGame
{
	public class VenomTeamerCaptured : MonoBehaviour
	{
		private VenomCommon _capturedVenom;

		private VenomTeamerHand _hand;

		private float radiusScale;

		private float offsetHandler = 0.1f;

		private void Awake()
		{
			_capturedVenom = GetComponent<VenomCommon>();
		}

		private void Start()
		{
			_capturedVenom.IgnoreWallCollision(value: true);
		}

		public void Init(VenomTeamerHand hand, VenomTeamer teamer)
		{
			CircleCollider2D component = GetComponent<CircleCollider2D>();
			float num = component.radius * 2f;
			Vector3 localScale = teamer.GetParent.localScale;
			radiusScale = num / localScale.y + offsetHandler;
			_hand = hand;
			_hand.BindPos.localPosition = new Vector3(0f, 0f - radiusScale);
		}

		private void Update()
		{
			_capturedVenom.transform.position = _hand.BindPos.transform.position;
		}

		private void OnDisable()
		{
			_hand = null;
			_capturedVenom.IgnoreWallCollision(value: false);
			UnityEngine.Object.Destroy(this);
		}
	}
}
