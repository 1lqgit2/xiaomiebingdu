using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

namespace orangesGame
{
	public class VenomCommon : BarrierBase
	{
		public ParticleSystemWidget particleSystemWidget;

		public SpriteRenderer[] sprRenderers;

		public SpriteRenderer sprQuan;

		public SpriteRenderer sprQuan_1;

		public SpriteRenderer sprQuan_2;

		public Transform RotateQiu;

		public ParticleSystemWidget HealParticlePrefab;

		public float CircleRadius = 0.58f;

		public SpriteRenderer hitLight;

		protected const float SLOW_SPEED = 0.3f;

		protected const float NORMAL_SPEED = 1f;

		protected Vector3 _originMoveSpeed;

		protected Vector3 _curMoveSpeed;

		protected Vector3 _impactMoveSpeed;

		protected Vector2 _moveSpeed;

		protected Vector2 _impactRotate;

		protected float _slowDownSpeed;

		protected float _slowTime;

		protected float _impactSpeed;

		protected Tween _tweem;

		protected Vector3 _hitEffectZoom;

		protected bool _isSlow;

		protected bool _isImpact;

		protected bool _canImpact;

		protected Color _colorCache = Color.white;

		protected ParticleSystemWidget _healParticle;

		protected int _createItemLevelLimit = 2;

		protected float _createItemLevelFactor;

		protected bool _isTop;

		protected float _coinBulletCreateCoinPeriod = 0.1f;

		protected float _coinBulletCreateCoinTimer;

		protected bool _isPaused;

		protected bool _isExpandAnim;

		protected bool _isIgnoreWall;

		protected bool _isIgnoreBullet;

		protected bool _isCanCopy;

		protected EffectBehaviour _effObj;

		protected Animation _venomAnimation;

		protected Vector3 _tempVenomScale;

		protected Transform _parentTran;

		protected CircleCollider2D _circleCollider2D;

		protected TextMesh _textMeshSp;

		protected TextMeshPro _textMesh;

		protected float _onTriggerStayTime;

		protected Tween _hitTween;

		private VenomObstacle _venomObstacle;

		private static int[] arr = new int[5]
		{
			15,
			15,
			15,
			15,
			400
		};

		private static int[] arr1 = new int[2];

		private bool _isCopyProess;

		private VenomCopy _copyerTarget;

		public Color GetCurrentColor => GetBallColor(_hitCount);

		public Vector3 GetCurrMoveSpeed => _curMoveSpeed;

		public bool GetIsFullHitCount => _hitCount >= _ballVO.HitCount;

		public bool GetIsDie => _isDie;

		public bool GetIsPause => _isPaused;

		public ParticleSystemWidget GetHealParticle
		{
			get
			{
				if (_healParticle == null)
				{
					_healParticle = UnityEngine.Object.Instantiate(HealParticlePrefab, _parentTran);
				}
				return _healParticle;
			}
		}

		public Vector2 GetInitMoveSpeed => _moveSpeed;

		public bool GetCanCopy => _isCanCopy;

		public Transform GetParent => _parentTran;

		protected virtual void Awake()
		{
			AwakeInit();
			_parentTran = base.transform.Find("Parent");
			_textMeshSp = _parentTran.Find("HitText").GetComponent<TextMesh>();
			_textMesh = GetComponentInChildren<TextMeshPro>();
			_venomAnimation = GetComponent<Animation>();
			_circleCollider2D = GetComponent<CircleCollider2D>();
			ItemExpandVenom.EventOnGetExpandVenomItem.AddListener(OnExpand);
		}

		protected virtual void OnExpand()
		{
			SetScale(isAnim: true);
		}

		protected virtual void SetScale(bool isAnim = false)
		{
			_isExpandAnim = isAnim;
			_tempVenomScale = Vector3.one * _ballVO.Scale * GameGlobalData.ExpandVenomScaleFactor;
			_circleCollider2D.radius = CircleRadius * _tempVenomScale.y;
			if (!isAnim)
			{
				_parentTran.localScale = _tempVenomScale;
			}
			else if (Math.Abs(GameGlobalData.ExpandVenomScaleFactor - 1f) < 0.1f)
			{
				Vector3 endValue = Vector3.one * _ballVO.Scale * 1.8f;
				_parentTran.DOScale(endValue, 0.2f).OnComplete(delegate
				{
					_parentTran.DOScale(_tempVenomScale, 0.3f).OnComplete(delegate
					{
						_isExpandAnim = false;
					});
				});
			}
			else
			{
				Vector3 endValue2 = Vector3.one * _ballVO.Scale * 1.8f;
				_parentTran.DOScale(endValue2, 0.3f).OnComplete(delegate
				{
					_parentTran.DOScale(_tempVenomScale, 0.2f).OnComplete(delegate
					{
						_isExpandAnim = false;
					});
				});
			}
		}

		protected virtual void Update()
		{
			if (!_isDie && _isCanHit)
			{
				Moving();
			}
			UpdateHealParticle();
			Vector3 localPosition = _tran.localPosition;
			if (!(localPosition.x < -7f))
			{
				Vector3 localPosition2 = _tran.localPosition;
				if (!(localPosition2.x > 7f))
				{
					Vector3 localPosition3 = _tran.localPosition;
					if (!(localPosition3.y < -12f))
					{
						Vector3 localPosition4 = _tran.localPosition;
						if (!(localPosition4.y > 13f))
						{
							goto IL_00bb;
						}
					}
				}
			}
			_tran.localPosition = GameGlobalData.BarrierInitPos;
			ChangeDirection(GameGlobalData.BarrierBackAngleMin, GameGlobalData.BarrierBackAngleMax);
			goto IL_00bb;
			IL_00bb:
			if (_coinBulletCreateCoinTimer > 0f && !_isPaused)
			{
				_coinBulletCreateCoinTimer -= Time.deltaTime;
			}
		}

		protected virtual void UpdateHealParticle()
		{
			if (_healParticle != null)
			{
				_healParticle.SetColor(GetCurrentColor);
			}
		}

		protected virtual void OnCollisionEnter2D(Collision2D other)
		{
			if (_isIgnoreWall)
			{
				return;
			}
			_venomObstacle = other.gameObject.GetComponent<VenomObstacle>();
			if (_venomObstacle != null)
			{
				OnCollideObstacleVenom(other.contacts[0].normal);
			}
			if (other.gameObject.CompareTag("WallTop") && _isTop)
			{
				ChangeDirection(other.contacts[0]);
				_isTop = false;
			}
			if (other.gameObject.CompareTag("WallLeft") || other.gameObject.CompareTag("WallRight"))
			{
				ChangeDirection(other.contacts[0]);
			}
			if (other.gameObject.CompareTag("WallButtom"))
			{
				_isTop = false;
				Vector3 localPosition = _tran.localPosition;
				float x = Mathf.Clamp(localPosition.x, 0f - UIViewManager.Screen_WF + 2f, UIViewManager.Screen_WF - 2f);
				_ballVO.InitPos = new Vector2(x, 10.2f);
				_tran.localPosition = _ballVO.InitPos;
				Vector3 localPosition2 = _tran.localPosition;
				if (localPosition2.x <= 0f)
				{
					ChangeDirection(-GameGlobalData.BarrierBackAngleMax, -GameGlobalData.BarrierBackAngleMin);
				}
				else
				{
					ChangeDirection(GameGlobalData.BarrierBackAngleMin, GameGlobalData.BarrierBackAngleMax);
				}
			}
		}

		protected virtual void OnCollideObstacleVenom(Vector2 normal)
		{
			ChangeDirection(normal);
		}

		protected virtual void OnTriggerEnter2D(Collider2D col)
		{
			if (col.gameObject.CompareTag("Player"))
			{
				if (!GameGlobalData.IsDie && !GameGlobalData.IsHurt && GameGlobalData.GameState == GameState.Gaming)
				{
					GameGlobalData.IsDie = true;
					Singleton<SdkUtils>.Instance.Shake(GameGlobalData.ShakeFlag);
					ApplicationFacade.Instance.SendNotification("GAME_FAIL");
				}
			}
			else if (col.gameObject.CompareTag("ImpactWave"))
			{
				Transform componentInParent = col.GetComponentInParent<Transform>();
				Vector3 position = componentInParent.position;
				Vector3 localPosition = base.transform.localPosition;
				ImpactWave(new Vector2(localPosition.x, position.y));
				Vector2 normal = Vector3.one - _curMoveSpeed.normalized;
				ChangeDirection(normal);
			}
			else
			{
				if (_isIgnoreBullet)
				{
					return;
				}
				Bullet component = col.GetComponent<Bullet>();
				if (component != null && !component.isDirty && component.type != BulletType.Boomerang)
				{
					if (Time.timeScale == 1f)
					{
						component.PlaySound();
					}
					Hitting(component.stayPos, component);
				}
			}
		}

		protected void OnTriggerStay2D(Collider2D col)
		{
			if (!col.gameObject.CompareTag("Untagged"))
			{
				return;
			}
			_onTriggerStayTime += Time.deltaTime;
			if (!(_onTriggerStayTime >= 0.1f))
			{
				return;
			}
			_onTriggerStayTime = 0f;
			if (_isIgnoreBullet)
			{
				return;
			}
			Bullet component = col.GetComponent<Bullet>();
			if (component != null && !component.isDirty && component.type == BulletType.Boomerang)
			{
				if (Time.timeScale == 1f)
				{
					component.PlaySound();
				}
				Hitting(component.stayPos, component);
			}
		}

		public override void SetParent(Transform parent)
		{
			base.SetParent(parent);
			Vector3 localPosition = parent.localPosition;
			int num = (int)localPosition.z;
			for (int i = 0; i < sprRenderers.Length; i++)
			{
				sprRenderers[i].sortingOrder += num;
			}
			sprQuan.sortingOrder += num;
			if (sprQuan_1 != null)
			{
				sprQuan_1.sortingOrder += num;
				sprQuan_2.sortingOrder += num;
			}
			if (_textMeshSp != null)
			{
				_textMeshSp.SetSortingOrder(num);
			}
			if (_textMesh != null)
			{
				_textMesh.sortingOrder += num;
			}
			if (hitLight != null)
			{
				hitLight.sortingOrder += num;
			}
			particleSystemWidget.SetOrderInLayer(num);
		}

		public override void Spawn(BallVO ballVO)
		{
			InitState(ballVO);
			GameGlobalData.ShowValue += _ballVO.ShowValue;
			SetRotate();
			Showing();
		}

		protected virtual void SetRotate()
		{
			float duration = 2 * _ballVO.DivideCount;
			for (int i = 0; i < sprRenderers.Length; i++)
			{
				DOTween.Kill(sprRenderers[i]);
				sprRenderers[i].transform.DORotate(sprRenderers[i].transform.rotation.eulerAngles + new Vector3(0f, 0f, 360 * ((i % 2 == 0) ? 1 : (-1))), duration, RotateMode.FastBeyond360).SetLoops(-1);
			}
			DOTween.Kill(RotateQiu);
			RotateQiu.transform.DORotate(RotateQiu.transform.rotation.eulerAngles + new Vector3(0f, 0f, 360f), duration, RotateMode.FastBeyond360).SetLoops(-1);
		}

		protected override void InitState(BallVO ballVo)
		{
			base.InitState(ballVo);
			_isTop = false;
			_isDie = false;
			_isIgnoreWall = false;
			_isIgnoreBullet = false;
			_isCanCopy = true;
			_canImpact = true;
			_isCanHit = false;
			_isPaused = false;
			_coinBulletCreateCoinTimer = 0f;
			_tran.localPosition = _ballVO.InitPos;
			_tran.localScale = Vector3.one;
			_tran.localRotation = Quaternion.identity;
			SetScale();
			_moveSpeed = new Vector2(0f, _ballVO.MoveSpeed);
			_slowDownSpeed = 1f;
			_isSlow = false;
			_isImpact = false;
			_slowTime = 0f;
			_hitCount = Mathf.Floor(_ballVO.HitCount);
			SetText();
			SetSprite();
			_onTriggerStayTime = 0.1f;
			_createItemLevelFactor = (float)Singleton<GameJson>.Instance.LevelJson[GameGlobalData.CurLevel].ItemsDropChance[_ballVO.DivideCount - 1] / 100f;
			GetHealParticle.Stop();
			ReleaseTeamrCaptured();
			if (hitLight != null && hitLight.enabled)
			{
				hitLight.color = new Color(1f, 1f, 1f, 0f);
			}
		}

		protected override void Showing()
		{
			base.Showing();
			Falling();
		}

		protected override void Falling()
		{
			base.Falling();
			_isCanHit = true;
			ChangeDirection(GameGlobalData.BarrierInitAngleMin, GameGlobalData.BarrierInitAngleMax);
		}

		protected override void Moving()
		{
			if (_isSlow)
			{
				_slowTime += Time.deltaTime;
				if (_slowTime >= 2f)
				{
					_isSlow = false;
					_slowTime = 0f;
					_slowDownSpeed = 1f;
				}
				else if (_slowDownSpeed != 0f)
				{
					_slowDownSpeed = Mathf.Lerp(0.3f, 1f, _slowTime * 3f);
				}
				SetSlowSpeed();
			}
			if (_isImpact)
			{
				_impactSpeed -= Time.deltaTime * 40f;
				_impactMoveSpeed = _impactSpeed * _impactRotate;
				if (_impactSpeed <= 0f)
				{
					_isImpact = false;
					_impactSpeed = 0f;
				}
				_tran.localPosition += _impactMoveSpeed * Time.deltaTime;
			}
			else
			{
				SetMovingPos(_curMoveSpeed * Time.deltaTime * GameGlobalData.WeakenVenomSpeedFactor * GameGlobalData.EnhanceVenomSpeedFactor * GameGlobalData.BattleItemData.BattleItemWeakenVenomSpeedFactor);
			}
		}

		protected virtual void SetMovingPos(Vector3 pos)
		{
			_tran.localPosition += pos;
		}

		protected override void Dying()
		{
			base.Dying();
		}

		protected override void Destroying()
		{
			base.Dying();
		}

		public override void Divide(BallVO vo)
		{
			InitState(vo);
			_isTop = true;
			if (_ballVO.Dir >= 0)
			{
				ChangeDirection(GameGlobalData.BarrierDivideAngleMin, GameGlobalData.BarrierDivideAngleMax);
			}
			else
			{
				ChangeDirection(-GameGlobalData.BarrierDivideAngleMax, -GameGlobalData.BarrierDivideAngleMin);
			}
			SetRotate();
			_isCanHit = true;
		}

		public void ImpactWave(Vector3 pos)
		{
			if (!_isImpact && _canImpact)
			{
				_impactSpeed = 20f;
				_isImpact = true;
				_impactRotate = ((Vector2)(base.transform.localPosition - pos)).normalized;
				_impactMoveSpeed = Vector3.zero;
			}
		}

		public void Hitting(Bullet bullet)
		{
			Hitting(Vector2.zero, bullet);
		}

		protected override void Hitting(Vector2 pos, Bullet bullet)
		{
			if (_isDie || GameGlobalData.IsDie)
			{
				return;
			}
			if (_ballVO.isLaserHit)
			{
				_ballVO.isLaserHit = false;
				return;
			}
			_hitCount -= bullet.atk * GameGlobalData.EnhanceBulletDamageFactor * (float)GameGlobalData.BattleItemData.BattleEnhanceBulletDamageFactor;
			_hitCount = Mathf.Floor(_hitCount);
			HitEff(bullet, pos);
			if (!_isDie && _hitCount <= 0f)
			{
				ProcessDie();
			}
			else if (_hitCount > 0f)
			{
				SetText();
				SetSprite();
				HurtZoom();
				HitCoin();
			}
			if (_isDie)
			{
				_effBombAction.Spawn(_tran.localPosition, _ballVO.BombScale * GameGlobalData.ExpandVenomScaleFactor * Vector2.one);
			}
		}

		protected override void Hitting(int atk)
		{
			if (!_isDie && !GameGlobalData.IsDie)
			{
				_hitCount -= (float)atk * GameGlobalData.EnhanceBulletDamageFactor * (float)GameGlobalData.BattleItemData.BattleEnhanceBulletDamageFactor;
				_hitCount = Mathf.Floor(_hitCount);
				if (!_isDie && _hitCount <= 0f)
				{
					ProcessDie();
				}
				else if (_hitCount > 0f)
				{
					SetText();
					SetSprite();
					HurtZoom();
					HitCoin();
				}
				Singleton<EffectManager>.Instance.GetEffAction(2u).Spawn(_tran.localPosition);
				if (_isDie)
				{
					_effBombAction.Spawn(_tran.localPosition);
				}
			}
		}

		protected virtual void ProcessDie()
		{
			Singleton<AudioManager>.Instance.PlaySpeciSound(GameGlobalData.VenomDieSounds[_ballVO.DivideCount - 1]);
			_isDie = true;
			if (_effObj != null)
			{
				_effObj.Clean();
			}
			int divideCount = _ballVO.DivideCount;
			int hasItem = _ballVO.HasItem;
			if (hasItem == 1)
			{
				_ballVO.HasItem = 0;
			}
			if (divideCount != 1)
			{
				StartDivide();
				Singleton<DailyTaskHandle>.Instance.AddProgress(DailyTaskType.Destroy_1000, 1);
			}
			else
			{
				GameGlobalData.HitCount++;
				Singleton<DailyTaskHandle>.Instance.AddProgress(DailyTaskType.Destroy_1000, 1);
				GameGlobalData.ShowValue--;
				if (_ballVO.ChildFlag == 0)
				{
					int coinIndex = Singleton<CoinDataHandle>.Instance.GetCoinIndex();
					float coinLevelValue = Singleton<CoinDataHandle>.Instance.GetCoinLevelValue();
					Singleton<CoinManager>.Instance.SpawnFloatCoin((float)coinIndex * coinLevelValue, coinIndex, _tran.localPosition);
					GameGlobalData.CoinInCurrentLevel.Add(coinIndex);
				}
				ApplicationFacade.Instance.SendNotification("BATTLE_SET_PROGRESS");
			}
			if (GameGlobalData.ShakeFlag == 0)
			{
				ShowVibrator(divideCount);
			}
			if (_tweem != null)
			{
				_tweem.Kill();
				_tweem = null;
			}
			_tran.gameObject.SetActive(value: false);
			OnDie();
			if (GameGlobalData.HitCount == GameGlobalData.NeedHitCount)
			{
				ApplicationFacade.Instance.SendNotification("GAME_PASS");
			}
			else
			{
				CreateRandomItem(hasItem == 1);
			}
			Singleton<UIViewManager>.Instance.LayerShake(1, 0.15f, Vector2.one * _ballVO.Shake, 10, 45f, fadeOut: false);
		}

		protected virtual void HitCoin()
		{
			if (!GameGlobalData.IsPlayerCoinBullet || !(_coinBulletCreateCoinTimer <= 0f))
			{
				return;
			}
			_coinBulletCreateCoinTimer = _coinBulletCreateCoinPeriod;
			int curLevel = GameGlobalData.CurLevel;
			curLevel = ((curLevel <= 100) ? curLevel : 100);
			curLevel = ((curLevel >= 4) ? curLevel : 4);
			if (curLevel != 0)
			{
				float num = curLevel / _ballVO.ShowValue;
				if (num < 1f)
				{
					num = 1f;
				}
				Singleton<CoinManager>.Instance.SpawnCoinBulletCoin(Mathf.CeilToInt(num), _tran.localPosition);
			}
		}

		protected virtual void StartDivide()
		{
			_isCanHit = false;
			Singleton<BallManger>.Instance.Divide(_tran.localPosition, _ballVO);
		}

		protected virtual void CreateRandomItem(bool isDrop)
		{
			float num = _createItemLevelFactor;
			if (isDrop)
			{
				num = 1f;
			}
			if (UnityEngine.Random.Range(0f, 1f) < num)
			{
				GameGlobalData.DropItemsInLevel++;
				int num2 = Singleton<ItemDropManager>.Instance.DropItemCount();
				if (GameGlobalData.DropItemsInLevel > num2)
				{
					return;
				}
				int num3 = 0;
				int[] array = Singleton<ItemDropManager>.Instance.DropItemWeight();
				int[] array2 = array;
				foreach (int num4 in array2)
				{
					num3 += num4;
				}
				if (num3 == 0)
				{
					return;
				}
				float num5 = UnityEngine.Random.Range(0f, num3);
				for (int j = 0; j < array.Length; j++)
				{
					if (num5 <= (float)array[j])
					{
						CoinManager.Type type = (CoinManager.Type)(j + 1);
						Singleton<CoinManager>.Instance.Sqawn(base.transform.localPosition, type, _curMoveSpeed.normalized);
						break;
					}
					num5 -= (float)array[j];
				}
			}
			if (_ballVO.DivideCount >= ItemTest.Instance.MinVenomLevel && UnityEngine.Random.Range(0f, 1f) < ItemTest.Instance.Chance && ItemTest.Instance.TestItem >= CoinManager.Type.WeakenVenom && ItemTest.Instance.TestItem < CoinManager.Type.COUNT)
			{
				Singleton<CoinManager>.Instance.Sqawn(base.transform.localPosition, ItemTest.Instance.TestItem, _curMoveSpeed.normalized);
			}
		}

		public override void Pause()
		{
			base.Pause();
			_tran.DOPause();
			_isCanHit = false;
			_isPaused = true;
		}

		public override void Continue()
		{
			base.Continue();
			_tran.DOPlay();
			_isCanHit = true;
			_isPaused = false;
		}

		protected virtual void OnDie()
		{
		}

		protected virtual void ReleaseTeamrCaptured()
		{
			VenomTeamerCaptured component = _tran.GetComponent<VenomTeamerCaptured>();
			if (component != null)
			{
				UnityEngine.Object.Destroy(component);
			}
		}

		public void HitEff(Bullet bullet, Vector2 hitPos)
		{
			switch (bullet.type)
			{
			case BulletType.SmallBullet:
			case BulletType.EnhancedSmallBullet:
				Slowing(0.3f);
				hitPos = new Vector2(hitPos.x, hitPos.y + 0.8f);
				bullet.OnTriggerLogic();
				Singleton<EffectManager>.Instance.GetEffAction(2u).Spawn(hitPos);
				break;
			case BulletType.FireBall:
				Slowing(0.3f);
				bullet.OnTriggerLogic();
				Singleton<EffectManager>.Instance.GetEffAction(3u).Spawn(hitPos);
				break;
			case BulletType.GreenBall:
			{
				bullet.OnTriggerLogic(_hitCount + bullet.atk);
				Vector3 localPosition2 = _tran.localPosition;
				if (localPosition2.y <= 9.6f)
				{
					_isTop = true;
					ChangeDirection(GameGlobalData.BarrierGreenBallAngleMin, GameGlobalData.BarrierGreenBallAngleMax);
				}
				break;
			}
			case BulletType.DefenseBullet:
				if (_hitTween == null)
				{
					_hitTween = hitLight.DOFade(1f, 0.1f).SetLoops(2, LoopType.Yoyo);
				}
				else if (!_hitTween.IsPlaying())
				{
					_hitTween = hitLight.DOFade(1f, 0.1f).SetLoops(2, LoopType.Yoyo);
				}
				Singleton<EffectManager>.Instance.GetEffAction(4u).Spawn(hitPos);
				break;
			case BulletType.Boomerang:
				Singleton<EffectManager>.Instance.GetEffAction(7u).Spawn(hitPos);
				break;
			case BulletType.Lase:
				_ballVO.isLaserHit = true;
				hitPos = _tran.localPosition;
				Singleton<EffectManager>.Instance.GetEffAction(4u).Spawn(hitPos);
				break;
			case BulletType.Rocket:
			case BulletType.RocketBomb:
				bullet.OnTriggerLogic(_hitCount);
				break;
			case BulletType.Thunder:
				bullet.OnTriggerLogic(_hitCount);
				break;
			case BulletType.ThunderBomb:
				if (!_isDie)
				{
					IEffectAction effAction = Singleton<EffectManager>.Instance.GetEffAction(6u);
					Vector3 localPosition = _tran.localPosition;
					Vector2 one = Vector2.one;
					Vector3 localScale = _tran.localScale;
					effAction.Spawn(localPosition, one * localScale.x * 0.5f);
					Slowing(0f);
				}
				bullet.OnTriggerLogic(_hitCount);
				break;
			}
		}

		private void ShakeCallback()
		{
			GameGlobalData.IsLayerShake = false;
		}

		public override void Clean()
		{
			base.Clean();
			_tran.DOKill();
			if (_tweem != null)
			{
				_tweem.Kill();
				_tweem = null;
			}
			if (hitLight != null)
			{
				hitLight.DOKill();
				_hitTween = null;
			}
			for (int i = 0; i < sprRenderers.Length; i++)
			{
				sprRenderers[i].transform.DOKill();
			}
			_tran.gameObject.SetActive(value: false);
			if (!_isDie)
			{
				OnDie();
			}
		}

		protected void ChangeDirection(int angleMin, int angleMax)
		{
			int num = UnityEngine.Random.Range(angleMin, angleMax);
			SetMoveSpeed(Quaternion.Euler(0f, 0f, num), _moveSpeed);
		}

		protected void ChangeDirection(int angleMin, int angleMax, Vector3 normal)
		{
			int num = UnityEngine.Random.Range(angleMin, angleMax);
			SetMoveSpeed(Quaternion.Euler(0f, 0f, num), _moveSpeed * normal);
		}

		protected void ChangeDirection(ContactPoint2D point)
		{
			Vector2 v = Vector2.Reflect(_curMoveSpeed, point.normal);
			Quaternion rotation = Quaternion.FromToRotation(Vector3.up, v);
			SetMoveSpeed(rotation, _moveSpeed);
		}

		public void CollideObstacleVenom(Vector2 normal)
		{
			Vector2 v = -_curMoveSpeed.normalized;
			Quaternion rotation = Quaternion.FromToRotation(Vector3.up, v);
			SetMoveSpeed(rotation, _moveSpeed);
		}

		public void ChangeDirection(Vector2 normal)
		{
			int num = UnityEngine.Random.Range(-1, 1);
			float z = (num <= 0) ? (-45) : 45;
			Quaternion lhs = Quaternion.FromToRotation(Vector3.up, normal);
			Quaternion rhs = Quaternion.Euler(0f, 0f, z);
			SetMoveSpeed(lhs * rhs, _moveSpeed);
		}

		public virtual void SetMoveSpeed(Quaternion rotation, Vector2 speed)
		{
			_curMoveSpeed = rotation * speed * _slowDownSpeed;
			if (!_isSlow)
			{
				_originMoveSpeed = _curMoveSpeed;
			}
			else
			{
				_originMoveSpeed = rotation * speed;
			}
		}

		protected virtual void SetSlowSpeed()
		{
			_curMoveSpeed = _originMoveSpeed * _slowDownSpeed;
		}

		protected virtual void SetImpactSpeed()
		{
			_impactMoveSpeed = _impactSpeed * _impactRotate;
		}

		protected virtual void HurtZoom()
		{
			if (!_isExpandAnim && !_venomAnimation.isPlaying)
			{
				_venomAnimation.Play("VenomHit");
			}
		}

		protected void SetTextZero()
		{
			if (_textMeshSp != null)
			{
				_textMeshSp.Parse("0");
			}
			if (_textMesh != null)
			{
				_textMesh.text = "0";
			}
		}

		protected void SetText()
		{
			if (_textMeshSp != null)
			{
				_textMeshSp.Parse(GameUtils.FloatToUnit(_hitCount));
			}
			if (_textMesh != null)
			{
				_textMesh.text = GameUtils.FloatToUnit(_hitCount);
			}
		}

		protected void Slowing(float speed)
		{
			if (_slowDownSpeed != 0f)
			{
				_isSlow = true;
				_slowDownSpeed = speed;
				_slowTime = 0f;
			}
		}

		protected void SetSprite()
		{
			Color ballColor = GetBallColor(_hitCount);
			if (!(ballColor == _colorCache))
			{
				_colorCache = ballColor;
				ChangeColor(ballColor);
			}
		}

		protected virtual void ChangeColor(Color color)
		{
			for (int i = 0; i < sprRenderers.Length; i++)
			{
				SpriteRenderer[] componentsInChildren = sprRenderers[i].GetComponentsInChildren<SpriteRenderer>();
				foreach (SpriteRenderer spriteRenderer in componentsInChildren)
				{
					spriteRenderer.color = color;
				}
			}
			particleSystemWidget.SetColor(color);
			sprQuan.color = color;
			if (sprQuan_1 != null)
			{
				sprQuan_1.color = color;
				sprQuan_2.color = color;
			}
		}

		public Color GetBallColor(float value)
		{
			float num = Mathf.Clamp01(value / GameGlobalData.BallRedValue);
			if (num >= 1f)
			{
				return GameGlobalData.BallColors[8];
			}
			for (int i = 0; i < 7; i++)
			{
				float num2 = 0.89f - 0.112f * (float)i;
				if (num >= num2)
				{
					num = (num - num2) * 4f;
					return Color.Lerp(GameGlobalData.BallColors[7 - i], GameGlobalData.BallColors[8 - i], num);
				}
			}
			num *= 4f;
			return Color.Lerp(GameGlobalData.BallColors[0], GameGlobalData.BallColors[1], num);
		}

		public void SetHP(float value)
		{
			if (!_isDie)
			{
				_hitCount = Mathf.Floor(Mathf.Clamp(value, 0f, _ballVO.HitCount));
				SetText();
				SetSprite();
			}
		}

		public void AddHP(float delta)
		{
			SetHP(_hitCount + delta);
		}

		public void AddHPRating(float rating)
		{
			AddHP(Mathf.Ceil(_ballVO.HitCount * rating));
		}

		public void IgnoreWallCollision(bool value)
		{
			_isIgnoreWall = value;
		}

		public void IgnoreBulletCollision(bool value)
		{
			_isIgnoreBullet = value;
		}

		private void ShowVibrator(int divideCount)
		{
			Singleton<SdkUtils>.Instance.ShowVibratorNoDelay(arr[divideCount - 1]);
			if (divideCount <= 4)
			{
				arr1[0] = 1;
			}
			else if (divideCount == 5)
			{
				arr1[1] = 1;
			}
		}

		public void HandleCopyProess(Vector3 posVector3, Action callBack, VenomCopy copyer)
		{
			if (_isCopyProess)
			{
				if (_copyerTarget != copyer)
				{
					copyer.CopyCanel();
				}
				return;
			}
			_copyerTarget = copyer;
			_isCanCopy = false;
			_isCopyProess = true;
			IgnoreWallCollision(value: true);
			IgnoreBulletCollision(value: true);
			_tran.DOKill();
			if (_tweem != null)
			{
				_tweem.Kill();
				_tweem = null;
			}
			for (int i = 0; i < sprRenderers.Length; i++)
			{
				sprRenderers[i].transform.DOKill();
			}
			if (!_isDie)
			{
				OnDie();
			}
			SetMoveSpeed(Quaternion.identity, Vector2.zero);
			_tran.DORotateQuaternion(Quaternion.Euler(0f, 0f, 180f), 1f);
			_tran.DOMove(posVector3, 1f).onComplete = delegate
			{
				callBack();
				_tran.localRotation = Quaternion.identity;
				_tran.gameObject.SetActive(value: false);
				_isCopyProess = false;
				_copyerTarget = null;
			};
		}
	}
}
