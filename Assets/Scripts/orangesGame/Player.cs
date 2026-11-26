using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	public class Player : RoleBase
	{
		public Transform[] shooterOtherPosArry;

		public GameObject[] objDefaultWeapon;

		public Transform tranHead;

		public Transform tranShield;

		public Animator anim;

		public Transform coinBulletCoin;

		public LayerMask layerMask;

		private const string ZOOMOUT = "anim_player_zoomout";

		private const string DIE = "anim_player_die";

		private const string RECOVER = "anim_player_recover";

		private const float BORDER_Y = 8.5f;

		private const float BORDER_YY = 10f;

		private const float NORMAL_APPROACH_SPEED = 60f;

		private static float BORDER_X;

		private Vector3 _touchPrePos;

		private Vector3 _touchCurPos;

		private WaitForSeconds _waitForSeconds;

		private Camera _camera;

		private Collider2D _collider2D;

		private Vector3 _pressPrePos;

		private bool _isTouch;

		private PlayerVO _playerVO;

		private Weapon[] _weapons;

		private bool _isShootWeapon;

		private float _approachSpeed = 60f;

		private Vector3 _approachPos;

		private Dictionary<string, float> _approachSpeedFactor = new Dictionary<string, float>();

		private Dictionary<string, float> _approachVenomSpeedFactor = new Dictionary<string, float>();

		private bool _hasDefault;

		private Vector3 _lineEnd;

		private RaycastHit2D[] _hits;

		private GameObject _objShield;

		private SpriteRenderer _rendererShield;

		private bool _isDead;

		private float testSpeedScale = 1f;

		public bool isTouch => _isTouch;

		public float TestSpeedScale
		{
			set
			{
				testSpeedScale = value;
			}
		}

		public PlayerVO GetPlayerVO => _playerVO;

		public bool PlayerDead => _isDead;

		private void Awake()
		{
			InitAwake();
			_camera = Singleton<UIViewManager>.Instance.mainCamera;
			_collider2D = GetComponent<Collider2D>();
			_isTouch = false;
			_isShootWeapon = false;
			_weapons = new Weapon[2];
			_lineEnd = new Vector2(0f, 100f);
			_hits = new RaycastHit2D[25];
			_objShield = (Object.Instantiate(Resources.Load("GameObject/Player/player_shield")) as GameObject);
			_objShield.transform.SetParent(tranShield);
			_objShield.transform.localScale = Vector3.one;
			_rendererShield = _objShield.GetComponentInChildren<SpriteRenderer>();
			_objShield.SetActive(value: false);
		}

		private void Update()
		{
			if (GameGlobalData.IsGamePause || GameGlobalData.GameState == GameState.Main)
			{
				return;
			}
			if (Input.GetMouseButtonDown(0))
			{
				ApplicationFacade.Instance.SendNotification("BATTLE_MARK", false);
				_approachPos = base.tran.localPosition;
				Time.timeScale = 1f;
				_touchPrePos = GetTouchPrePos(UnityEngine.Input.mousePosition);
				_pressPrePos = _touchPrePos;
				if (!_isTouch)
				{
					_isTouch = true;
					SetShooting(_isTouch);
					SetWeaponsSetShooting(isShoot: true);
				}
				GameGlobalData.IsGameAuto = false;
			}
			else if (Input.GetMouseButtonUp(0))
			{
				if (GameGlobalData.IsGameAuto)
				{
					return;
				}
				ApplicationFacade.Instance.SendNotification("BATTLE_MARK", true);
				Time.timeScale = 0.1f;
				_isTouch = false;
				SetShooting(_isTouch);
			}
			else if (Input.GetMouseButton(0))
			{
				if (_approachPos == Vector3.zero)
				{
					_approachPos = base.tran.localPosition;
					_touchPrePos = GetTouchPrePos(UnityEngine.Input.mousePosition);
					_pressPrePos = _touchPrePos;
				}
				_approachPos = GetTouchCurPos(UnityEngine.Input.mousePosition);
				ApproachPos(_approachPos, _approachSpeed);
				_pressPrePos = _touchPrePos;
				GameGlobalData.TouchPos = _pressPrePos;
			}
			float num = 60f;
			foreach (KeyValuePair<string, float> item in _approachSpeedFactor)
			{
				num *= item.Value;
			}
			if (_approachVenomSpeedFactor.Count > 0)
			{
				num = Mathf.Max(1f - ((float)_approachVenomSpeedFactor.Count * 0.01f + 0.92f), 0.05f) * num;
			}
			_approachSpeed = Mathf.Lerp(_approachSpeed, num, Time.deltaTime * 7f);
			if (_isTouch && !GameGlobalData.IsDie && Physics2D.LinecastNonAlloc(_tran.localPosition, _tran.localPosition + _lineEnd, _hits, layerMask) == 0)
			{
				Singleton<AudioManager>.Instance.StopSpeciSound("hit");
			}
		}

		protected virtual void ApproachPos(Vector3 pos, float speed)
		{
			testSpeedScale = 1f;
			_tran.localPosition = Vector3.Lerp(_tran.localPosition, pos, testSpeedScale * speed * Time.deltaTime);
		}

		public override void Init(PlayerVO vo, Transform parent)
		{
			base.Init(vo, parent);
			BORDER_X = UIViewManager.Screen_WF;
			InitWeapons(vo);
			_playerVO = vo;
			_tran.localPosition = GameGlobalData.Player_InitPos;
			SpriteRenderer rendererShield = _rendererShield;
			int sortingOrder = rendererShield.sortingOrder;
			Vector3 localPosition = parent.localPosition;
			rendererShield.sortingOrder = sortingOrder + (int)localPosition.z;
			ShowCoinBulletCoin(isshow: false);
		}

		public override void SetShooter(ShooterVO vo, bool isChange = false)
		{
			base.SetShooter(vo, isChange);
			_playerVO.shooter = vo;
			_waitForSeconds = new WaitForSeconds(_shooter.distance);
		}

		public void SetWeapon(ShooterVO[] vo, bool isChange = false)
		{
			if (GameGlobalData.WeaponID >= 0)
			{
				_playerVO.weapons = vo;
				for (int i = 0; i < 2; i++)
				{
					_weapons[i].SetShooter(vo[i]);
				}
			}
		}

		public void ResetShoot()
		{
			_touchPrePos = GetTouchPrePos(GameGlobalData.ClickStartPos);
			SetShooting(isPlay: true);
		}

		public void SetTouchState(bool isState)
		{
			_isTouch = isState;
			SetShootWeaponState(isState);
		}

		public void SetShootWeaponState(bool state)
		{
			_isShootWeapon = state;
		}

		public void SetWeaponsSetShooting(bool isShoot)
		{
			if (_hasDefault)
			{
				return;
			}
			if (isShoot)
			{
				if (!_isShootWeapon)
				{
					_isShootWeapon = true;
					for (int i = 0; i < 2; i++)
					{
						_weapons[i].SetShooting(isShooting: true);
					}
				}
			}
			else
			{
				for (int j = 0; j < 2; j++)
				{
					_weapons[j].SetShooting(isShooting: false);
				}
			}
		}

		public void InitWeapons(PlayerVO vo)
		{
			_hasDefault = (GameGlobalData.WeaponID < 0);
			for (int i = 0; i < objDefaultWeapon.Length; i++)
			{
				objDefaultWeapon[i].SetActive(_hasDefault);
			}
			if (!_hasDefault)
			{
				string resPath = $"GameObject/Player/player_weapon_{GameGlobalData.WeaponID + 1}";
				for (int j = 0; j < 2; j++)
				{
					_weapons[j] = Singleton<ResLoadManager>.Instance.ResLoad<Weapon>(resPath, isPreLoad: false);
					_weapons[j].Init(j, vo, shooterOtherPosArry[j]);
					_weapons[j].SetShooter(vo.weapons[j]);
				}
			}
		}

		public void ChangeWeapon(ShooterVO[] vos)
		{
			_hasDefault = (GameGlobalData.WeaponID < 0);
			if (_hasDefault)
			{
				return;
			}
			_playerVO.weapons = vos;
			for (int i = 0; i < _weapons.Length; i++)
			{
				if (_weapons[i] != null)
				{
					UnityEngine.Object.Destroy(_weapons[i].gameObject);
				}
			}
			InitWeapons(_playerVO);
		}

		private Vector3 GetTouchPrePos(Vector3 pos, bool isWorld = true)
		{
			Vector2 vector = (!isWorld) ? pos : _camera.ScreenToWorldPoint(pos);
			float x = Mathf.Clamp(vector.x, 0f - BORDER_X, BORDER_X);
			float y = Mathf.Clamp(vector.y, -10f, 8.5f);
			return new Vector3(x, y);
		}

		private Vector3 GetTouchCurPos(Vector3 pos, bool isWorld = true)
		{
			_touchPrePos = ((!isWorld) ? pos : _camera.ScreenToWorldPoint(pos));
			Vector2 vector = _approachPos + (_touchPrePos - _pressPrePos);
			float x = Mathf.Clamp(vector.x, 0f - BORDER_X, BORDER_X);
			float y = Mathf.Clamp(vector.y, -10f, 8.5f);
			return new Vector3(x, y);
		}

		public void Hurt()
		{
			Singleton<AudioManager>.Instance.StopSpeciSound("hit");
			ApplicationFacade.Instance.SendNotification("BATTLE_ROLE_BEHIT");
			_isShootWeapon = false;
			_isTouch = false;
			_renderers[4].DOColor(Color.red, 0.5f).SetAutoKill(autoKillOnCompletion: true).SetLoops(6, LoopType.Yoyo);
		}

		public void Protect()
		{
			SetShooting(isPlay: true);
			_isShootWeapon = false;
			GameGlobalData.IsHurt = true;
			_renderers[4].color = Color.white;
			_renderers[4].DOKill();
			_objShield.SetActive(value: true);
			_rendererShield.color = Color.white;
			_rendererShield.DOFade(0f, 0.2f).SetAutoKill(autoKillOnCompletion: true).SetLoops(8, LoopType.Yoyo)
				.SetDelay(3f)
				.OnComplete(delegate
				{
					_objShield.SetActive(value: false);
					GameGlobalData.IsHurt = false;
				});
		}

		public void ShieldItemActive(bool isActive)
		{
			if (isActive)
			{
				_isShootWeapon = false;
				_renderers[4].color = Color.white;
				_renderers[4].DOKill();
				_rendererShield.color = Color.white;
			}
			GameGlobalData.IsHurt = isActive;
			_objShield.SetActive(isActive);
		}

		public void ZoomOut()
		{
			anim.Play("anim_player_zoomout");
		}

		public void Die()
		{
			anim.Play("anim_player_die");
			_isDead = true;
		}

		public override void SetShooting(bool isPlay)
		{
			if (isPlay)
			{
				if (GameGlobalData.GameState == GameState.Gaming)
				{
					objFire.SetActive(value: true);
					StartCoroutine(ShootCoroutines());
				}
			}
			else
			{
				objFire.SetActive(value: false);
				StopAllCoroutines();
				Singleton<AudioManager>.Instance.StopSpeciSound("hit");
			}
		}

		public void Reset(bool isTouch)
		{
			_tran.localPosition = GameGlobalData.Player_InitPos;
			_touchPrePos = Vector3.zero;
			base.enabled = false;
			_isTouch = isTouch;
			_collider2D.enabled = true;
			objFire.SetActive(value: false);
			_approachSpeed = 60f;
			_approachSpeedFactor.Clear();
			anim.Play("anim_player_recover");
			_isDead = false;
			_approachPos = Vector3.zero;
		}

		public void StopFade()
		{
			_renderers[0].DOKill();
			_renderers[0].color = Color.white;
			_rendererShield.DOKill();
			_objShield.SetActive(value: false);
		}

		public void SetCollider2DEnable(bool enable)
		{
			_collider2D.enabled = base.enabled;
		}

		private IEnumerator ShootCoroutines()
		{
			while (true)
			{
				_shooter.Fire();
				yield return _waitForSeconds;
			}
		}

		public void SlowDown(string key, float rating = 0.1f)
		{
			if (!_approachSpeedFactor.ContainsKey(key))
			{
				_approachSpeedFactor.Add(key, rating);
			}
			_approachSpeedFactor[key] = rating;
		}

		public void SlowVenomDown(string key)
		{
			if (!_approachVenomSpeedFactor.ContainsKey(key))
			{
				_approachVenomSpeedFactor.Add(key, 0.03f);
			}
		}

		public void RecoverSpeed(string key)
		{
			_approachSpeedFactor.Remove(key);
			_approachVenomSpeedFactor.Remove(key);
		}

		public void ShowCoinBulletCoin(bool isshow)
		{
			coinBulletCoin.gameObject.SetActive(isshow);
		}
	}
}
