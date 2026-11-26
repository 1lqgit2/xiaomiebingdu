using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	public class VenomTeamer : VenomCommon
	{
		public Transform HandsContent;

		public List<VenomTeamerHand> _hands = new List<VenomTeamerHand>();

		protected override void Awake()
		{
			base.Awake();
			foreach (VenomTeamerHand hand in _hands)
			{
				hand.EventVenomTeamerHandCaptureTarget.AddListener(OnTargetCapture);
			}
		}

		protected virtual void OnTargetCapture(VenomCommon venom, VenomTeamerHand hand)
		{
			VenomTeamerCaptured component = GetComponent<VenomTeamerCaptured>();
			if ((!(component != null) || !(venom is VenomTeamer)) && !_isDie)
			{
				hand.SetCapturedVenom(venom);
			}
		}

		protected override void OnDie()
		{
			base.OnDie();
			foreach (VenomTeamerHand hand in _hands)
			{
				hand.Release();
			}
		}

		protected override void HurtZoom()
		{
			if (!_isExpandAnim && !_venomAnimation.isPlaying)
			{
				_venomAnimation.Play("VenomHitTeam");
			}
		}

		protected override void SetRotate()
		{
			float duration = 6 * _ballVO.DivideCount;
			DOTween.Kill(HandsContent);
			HandsContent.transform.DORotate(HandsContent.transform.rotation.eulerAngles + new Vector3(0f, 0f, 360f), duration, RotateMode.FastBeyond360).SetLoops(-1);
		}
	}
}
