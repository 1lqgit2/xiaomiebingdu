using UnityEngine;

namespace orangesGame
{
	public class VenomTeamerHand : MonoBehaviour
	{
		public EventVenomTeamerHandCaptureTarget EventVenomTeamerHandCaptureTarget = new EventVenomTeamerHandCaptureTarget();

		public Transform BindPos;

		private bool isIngoreScale;

		private VenomCommon _venom;

		[SerializeField]
		private VenomTeamer teamerVenom;

		private void OnTriggerEnter2D(Collider2D collision)
		{
			VenomCommon component = collision.gameObject.GetComponent<VenomCommon>();
			VenomTeamerCaptured component2 = collision.gameObject.GetComponent<VenomTeamerCaptured>();
			if (!(component != null) || !(_venom == null) || !(component2 == null) || component is VenomObstacle || !component.GetCanCopy)
			{
				return;
			}
			BallVO getBallVO = teamerVenom.GetBallVO;
			int divideCount = getBallVO.DivideCount;
			BallVO getBallVO2 = component.GetBallVO;
			if (divideCount >= getBallVO2.DivideCount)
			{
				VenomCopy venomCopy = component as VenomCopy;
				if (!(venomCopy != null) || venomCopy.IsCanTeamrHand)
				{
					EventVenomTeamerHandCaptureTarget.Invoke(component, this);
				}
			}
		}

		public void SetCapturedVenom(VenomCommon venom)
		{
			isIngoreScale = true;
			_venom = venom;
			VenomTeamerCaptured venomTeamerCaptured = venom.gameObject.AddComponent<VenomTeamerCaptured>();
			venomTeamerCaptured.Init(this, teamerVenom);
		}

		public void Release()
		{
			if (_venom != null)
			{
				UnityEngine.Object.Destroy(_venom.GetComponent<VenomTeamerCaptured>());
				_venom = null;
				isIngoreScale = false;
			}
		}
	}
}
