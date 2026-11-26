using DG.Tweening;
using System;
using UnityEngine;

namespace orangesGame
{
	public class VenomCopy : VenomCommon
	{
		public VenomScanner Scanner;

		public Transform Needle1;

		public Transform Needle2;

		private float _normalDistance = 0.15f;

		private float _copyStateDistance = 0.45f;

		private float _scanSmallestRadius = 1f;

		private float _copyStayTime = 6f;

		private float _copyPowerTime = 2f;

		private float _copyStartTime = 2f;

		private bool _canTeam;

		private BallVO _copyTargetBallVo;

		private Vector2 _copySpeed;

		public Timer CopyStartTimer;

		public Timer CopyStatTimer;

		public Timer CantCopyTimer;

		private bool isMoveInit;

		public bool IsCanTeamrHand => _canTeam;

		protected override void Awake()
		{
			base.Awake();
		}

		protected override void InitState(BallVO ballVo)
		{
			base.InitState(ballVo);
			if (Math.Abs(Scanner.Radius - (_circleCollider2D.radius + _scanSmallestRadius)) > 0.01f)
			{
				Scanner.Radius = _circleCollider2D.radius + _scanSmallestRadius;
			}
			Scanner.EventOnTargetStay.AddListener(TargetStay);
			_isCanCopy = false;
			_canTeam = true;
			MoveYTweenDis(_normalDistance);
			CopyStartTimer = Timer.Register(_copyStartTime, delegate
			{
				_isCanCopy = true;
				CopyStartTimer = null;
			});
		}

		private void MoveYTweenDis(float distance)
		{
			Needle1.localPosition = Vector3.zero;
			DOTween.Kill(Needle1, complete: true);
			Transform needle = Needle1;
			Vector3 localPosition = Needle1.localPosition;
			needle.DOLocalMoveY(localPosition.y + distance, 0.3f).SetLoops(-1, LoopType.Yoyo);
			Needle2.localPosition = Vector3.zero;
			DOTween.Kill(Needle2, complete: true);
			Transform needle2 = Needle2;
			Vector3 localPosition2 = Needle2.localPosition;
			needle2.DOLocalMoveY(localPosition2.y - distance, 0.3f).SetLoops(-1, LoopType.Yoyo);
		}

		protected virtual void TargetStay(GameObject go)
		{
			if (GameGlobalData.IsDie || GameGlobalData.GameState != GameState.Gaming)
			{
				return;
			}
			VenomCommon component = go.gameObject.GetComponent<VenomCommon>();
			if (!(component != null) || component is VenomCopy || !_isCanCopy)
			{
				return;
			}
			int divideCount = _ballVO.DivideCount;
			BallVO getBallVO = component.GetBallVO;
			if (divideCount > getBallVO.DivideCount && component.GetCanCopy && !CheckBounds() && !component.GetIsDie && !component.GetIsPause)
			{
				Scanner.EventOnTargetStay.RemoveListener(TargetStay);
				_copyTargetBallVo = component.GetBallVO;
				_copySpeed = component.GetCurrMoveSpeed;
				component.HandleCopyProess(_tran.position, HandlerCopy, this);
				if ((bool)GetComponent<VenomTeamerCaptured>())
				{
					UnityEngine.Object.Destroy(GetComponent<VenomTeamerCaptured>());
				}
				IgnoreWallCollision(value: true);
				IgnoreBulletCollision(value: true);
				_isCanCopy = false;
				_canTeam = false;
				SetMoveSpeed(Quaternion.identity, Vector2.zero);
				isMoveInit = false;
			}
		}

		public override void Pause()
		{
			base.Pause();
			if (CopyStartTimer != null)
			{
				CopyStartTimer.Cancel();
				CopyStartTimer = null;
			}
			if (CopyStatTimer != null)
			{
				CopyStatTimer.Cancel();
				CopyStatTimer = null;
			}
			if (CantCopyTimer != null)
			{
				CantCopyTimer.Cancel();
				CantCopyTimer = null;
			}
		}

		protected override void Update()
		{
			base.Update();
			if (isMoveInit && _curMoveSpeed == Vector3.zero)
			{
				if (_ballVO.Dir >= 0)
				{
					ChangeDirection(GameGlobalData.BarrierDivideAngleMin, GameGlobalData.BarrierDivideAngleMax);
				}
				else
				{
					ChangeDirection(-GameGlobalData.BarrierDivideAngleMax, -GameGlobalData.BarrierDivideAngleMin);
				}
			}
		}

		public override void Continue()
		{
			base.Continue();
			if (!_isCanCopy)
			{
				CopyCanel();
			}
		}

		public void StartApperaAnim()
		{
			Vector3 endValue = Vector3.one * _ballVO.Scale;
			_parentTran.localScale = Vector3.zero;
			_parentTran.DOScale(endValue, 0.4f).SetAutoKill(autoKillOnCompletion: true).SetEase(Ease.InOutBack)
				.OnComplete(delegate
				{
				});
		}

		public void CopyCanel()
		{
			CopyStatTimer = null;
			_canTeam = true;
			IgnoreBulletCollision(value: false);
			IgnoreWallCollision(value: false);
			MoveYTweenDis(_normalDistance);
			if (_ballVO.Dir >= 0)
			{
				ChangeDirection(GameGlobalData.BarrierDivideAngleMin, GameGlobalData.BarrierDivideAngleMax);
			}
			else
			{
				ChangeDirection(-GameGlobalData.BarrierDivideAngleMax, -GameGlobalData.BarrierDivideAngleMin);
			}
			CantCopyTimer = null;
			_isCanCopy = true;
			isMoveInit = true;
			Scanner.EventOnTargetStay.AddListener(TargetStay);
		}

		private void HandlerCopy()
		{
			MoveYTweenDis(_copyStateDistance);
			CopyStatTimer = Timer.Register(_copyPowerTime, CopyStayCompelte);
			CantCopyTimer = Timer.Register(_copyStayTime, CantCopyCompelte);
		}

		private bool CheckBounds()
		{
			Vector3 localPosition = _tran.localPosition;
			if (localPosition.y > Singleton<UIViewManager>.Instance.screenHeightHalf - 2f)
			{
				return true;
			}
			Vector3 localPosition2 = _tran.localPosition;
			if (localPosition2.y < 0f - (Singleton<UIViewManager>.Instance.screenHeightHalf - 2f))
			{
				return true;
			}
			Vector3 localPosition3 = _tran.localPosition;
			if (localPosition3.x > Singleton<UIViewManager>.Instance.screenWideHalf - 2f)
			{
				return true;
			}
			Vector3 localPosition4 = _tran.localPosition;
			if (localPosition4.x < 0f - (Singleton<UIViewManager>.Instance.screenWideHalf - 2f))
			{
				return true;
			}
			return false;
		}

		private void CopyStayCompelte()
		{
			CopyStatTimer = null;
			_canTeam = true;
			IgnoreBulletCollision(value: false);
			IgnoreWallCollision(value: false);
			MoveYTweenDis(_normalDistance);
			if (_ballVO.Dir >= 0)
			{
				ChangeDirection(GameGlobalData.BarrierDivideAngleMin, GameGlobalData.BarrierDivideAngleMax);
			}
			else
			{
				ChangeDirection(-GameGlobalData.BarrierDivideAngleMax, -GameGlobalData.BarrierDivideAngleMin);
			}
			isMoveInit = true;
			SpawnChild();
		}

		private void CantCopyCompelte()
		{
			CantCopyTimer = null;
			_isCanCopy = true;
			Scanner.EventOnTargetStay.AddListener(TargetStay);
		}

		protected virtual void SpawnChild()
		{
			BallManger instance = Singleton<BallManger>.Instance;
			Vector3 localPosition = _tran.localPosition;
			BallVO getBallVO = base.GetBallVO;
			VenomCopy venomCopy = instance.CreateOne(localPosition, getBallVO.BallID, _copyTargetBallVo.HitCount, 0) as VenomCopy;
			GameGlobalData.ShowValue -= _copyTargetBallVo.ShowValue;
			GameGlobalData.SetCleanVenomNeedHitCount(_copyTargetBallVo.DivideCount);
			Vector2 v = -_copySpeed.normalized;
			Quaternion quaternion = Quaternion.FromToRotation(Vector3.up, v);
			if (venomCopy != null)
			{
				VenomCopy venomCopy2 = venomCopy;
				Quaternion rotation = quaternion;
				Vector2 up = Vector2.up;
				BallVO getBallVO2 = venomCopy.GetBallVO;
				venomCopy2.SetMoveSpeed(rotation, up * getBallVO2.MoveSpeed);
				venomCopy.StartApperaAnim();
			}
		}

		protected override void SetRotate()
		{
		}
	}
}
