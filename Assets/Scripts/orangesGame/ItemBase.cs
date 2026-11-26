using DG.Tweening;
using System;
using UnityEngine;

namespace orangesGame
{
	public abstract class ItemBase : GameItem
	{
		public Color CDLineColor;

		public Sprite ItemIconSprite;

		protected const string TAG_WALLTOP = "WallTop";

		protected const string TAG_WALLLEFT = "WallLeft";

		protected const string TAG_WALLRIGHT = "WallRight";

		protected const string TAG_WALLBUTTOM = "WallButtom";

		protected const string TAG_PLAYER = "Player";

		public EventItemCountDownUpdate EventItemCountDownUpdate = new EventItemCountDownUpdate();

		public EventItemTimeOut EventItemTimeOut = new EventItemTimeOut();

		private int DurationOffset;

		protected Vector3 _originMoveSpeed;

		protected Vector3 _curMoveSpeed;

		protected Vector2 _moveSpeed = Vector2.up * 2.5f;

		protected int _initDirectAngle = 60;

		protected Timer _closeTimer;

		protected Timer _autoKillTimer;

		protected bool _isAffectingPlayer;

		protected bool _isPaused;

		protected SpriteRenderer _icon;

		protected float _blinkPeriod = 0.17f;

		protected Player _target;

		protected CoinManager.Type _type;

		protected abstract void OnInit();

		protected abstract void OnTouchPlayer(Player player);

		protected abstract void OnTimeOut();

		protected abstract void OnPause();

		protected abstract void OnContinue();

		protected abstract void OnClean();

		protected virtual void Awake()
		{
			AwakeInit();
			_icon = _tran.Find("Icon").GetComponent<SpriteRenderer>();
		}

		public virtual void Init(Vector3 pos, CoinManager.Type type, Vector2 normal, bool isPlayer)
		{
			_type = type;
			_isDirty = false;
			_isAffectingPlayer = false;
			_isPaused = false;
			SetActive(isActive: true);
			_tran.localPosition = pos;
			float y = (type > CoinManager.Type.CoinBullet && type != CoinManager.Type.ItemWingman) ? 1 : (-1);
			if (isPlayer)
			{
				ChangeDirection(-_initDirectAngle, _initDirectAngle, -1f);
			}
			else
			{
				ChangeDirection(new Vector2(0f - normal.x, y));
			}
			_autoKillTimer = Timer.Register(10f + (float)DurationOffset, ((GameItem)this).Clean, AutoKillUpdate);
			GameGlobalData.GetDropItemsInGame = GameGlobalData.GetDropItemsInGame + (int)type + ",";
			OnInit();
		}

		protected virtual void AutoKillUpdate(float time)
		{
			time = 10f + (float)DurationOffset - time;
			if (time <= 3f)
			{
				int num = Mathf.FloorToInt(time / _blinkPeriod);
				_icon.gameObject.SetActive(num % 2 == 0);
			}
			else
			{
				_icon.gameObject.SetActive(value: true);
			}
		}

		protected virtual void Update()
		{
			if (!_isPaused)
			{
				Moving();
			}
		}

		protected virtual void OnCollisionEnter2D(Collision2D other)
		{
			if (other.gameObject.CompareTag("WallTop"))
			{
				ChangeDirection(other.contacts[0]);
			}
			if (other.gameObject.CompareTag("WallLeft") || other.gameObject.CompareTag("WallRight"))
			{
				ChangeDirection(other.contacts[0]);
			}
			if (other.gameObject.CompareTag("WallButtom"))
			{
				ChangeDirection(other.contacts[0]);
			}
		}

		protected virtual void OnMoveToTop()
		{
		}

		protected virtual void OnTriggerEnter2D(Collider2D col)
		{
			if (col.gameObject.CompareTag("Player") && !GameGlobalData.IsDie && GameGlobalData.GameState == GameState.Gaming)
			{
				ItemBase itemBase = Singleton<PlayerManager>.Instance.ItemsInEffect.Find((ItemBase x) => x.GetType() == GetType());
				if (itemBase != null)
				{
					itemBase.RefreshEffectTime();
					Clean();
				}
				else
				{
					Singleton<PlayerManager>.Instance.ItemsInEffect.Add(this);
					_isAffectingPlayer = true;
					_target = col.gameObject.GetComponent<Player>();
					OnTouchPlayer(_target);
					ApplicationFacade.Instance.SendNotification("BATTLE_GAIN_ITEM", this);
					StartTimer(6f + (float)DurationOffset, TimeOut);
					SetActive(isActive: false);
					StopAutoKill();
				}
				GameGlobalData.ItemsInCurrentLevel.Add(_type);
				Singleton<DailyTaskHandle>.Instance.AddProgress(DailyTaskType.GetItem_20, 1);
			}
		}

		protected virtual void TimeOut()
		{
			ProcessTimeOut();
			Clean();
		}

		protected virtual void ProcessTimeOut()
		{
			_isAffectingPlayer = false;
			EventItemTimeOut.Invoke();
			OnTimeOut();
			_target = null;
		}

		protected virtual void Moving()
		{
			SetMovingPos(_curMoveSpeed * Time.deltaTime);
		}

		protected virtual void SetMovingPos(Vector3 pos)
		{
			_tran.localPosition += pos;
		}

		public override void Pause()
		{
			base.Pause();
			_isPaused = true;
			_tran.DOPause();
			PauseTimer();
			OnPause();
		}

		public override void Continue()
		{
			base.Continue();
			_isPaused = false;
			_tran.DOPlay();
			ContinueTimer();
			OnContinue();
		}

		public override void Clean()
		{
			base.Clean();
			_tran.DOKill();
			SetActive(isActive: false);
			StopTimer();
			OnClean();
			EventItemTimeOut.RemoveAllListeners();
			EventItemCountDownUpdate.RemoveAllListeners();
			Singleton<PlayerManager>.Instance.ItemsInEffect.Remove(this);
		}

		public override void SetActive(bool isActive)
		{
			base.SetActive(isActive);
			_tran.gameObject.SetActive(isActive);
		}

		protected void ChangeDirection(Vector2 normal)
		{
			Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
			SetMoveSpeed(rotation, _moveSpeed);
		}

		protected void ChangeDirection(float dir = 1f)
		{
			ChangeDirection(120, 250, dir);
		}

		protected void ChangeDirection(int angleMin, int angleMax, float dir = 1f)
		{
			int num = UnityEngine.Random.Range(angleMin, angleMax);
			SetMoveSpeed(Quaternion.Euler(0f, 0f, num), _moveSpeed * dir);
		}

		protected void ChangeDirection(ContactPoint2D point)
		{
			Vector2 zero = Vector2.zero;
			Vector2 vector = _tran.TransformDirection(Vector2.up);
			zero = Vector2.Reflect(_curMoveSpeed, point.normal);
			Quaternion rotation = Quaternion.FromToRotation(Vector3.up, zero);
			SetMoveSpeed(rotation, _moveSpeed);
		}

		public void SetMoveSpeed(Quaternion rotation, Vector2 speed)
		{
			_curMoveSpeed = rotation * speed;
			_originMoveSpeed = _curMoveSpeed;
		}

		private void StartTimer(float time, Action onComplete)
		{
			if (_closeTimer != null)
			{
				_closeTimer.Cancel();
			}
			_closeTimer = Timer.Register(time, onComplete, UpdateCountDown);
		}

		private void UpdateCountDown(float time)
		{
			EventItemCountDownUpdate.Invoke(time, 6f + (float)DurationOffset);
		}

		private void StopTimer()
		{
			if (_closeTimer != null && !_closeTimer.isCompleted)
			{
				if (_isAffectingPlayer)
				{
					ProcessTimeOut();
				}
				_closeTimer.Cancel();
			}
			_closeTimer = null;
			_isDirty = true;
			StopAutoKill();
		}

		private void PauseTimer()
		{
			if (_closeTimer != null && !_closeTimer.isCancelled)
			{
				_closeTimer.Pause();
			}
			if (_autoKillTimer != null && !_autoKillTimer.isCancelled)
			{
				_autoKillTimer.Pause();
			}
		}

		private void ContinueTimer()
		{
			if (_closeTimer != null && _closeTimer.isPaused)
			{
				_closeTimer.Resume();
			}
			if (_autoKillTimer != null && _autoKillTimer.isPaused)
			{
				_autoKillTimer.Resume();
			}
		}

		private void StopAutoKill()
		{
			if (_autoKillTimer != null && !_autoKillTimer.isCancelled)
			{
				_autoKillTimer.Cancel();
			}
			_autoKillTimer = null;
		}

		public void RefreshEffectTime()
		{
			StartTimer(6f + (float)DurationOffset, TimeOut);
		}
	}
}
