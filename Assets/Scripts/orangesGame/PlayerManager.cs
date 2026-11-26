using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	public class PlayerManager : Singleton<PlayerManager>
	{
		private Dictionary<string, RoleBase> _Dic;

		public List<ItemBase> ItemsInEffect = new List<ItemBase>();

		private Player _leader;

		private Wingman _wingman;

		public Wingman GetWingman => _wingman;

		private PlayerManager()
		{
			_Dic = new Dictionary<string, RoleBase>();
		}

		public Player LeaderSpawn()
		{
			PlayerVO vo = CreatePlayerVO();
			Transform layerTran = Singleton<UIViewManager>.Instance.GetLayerTran(40);
			_leader = (Spawn(vo, layerTran) as Player);
			_leader.Init(vo, layerTran);
			_leader.SetShooter(vo.shooter);
			return _leader;
		}

		public Wingman WingmanSpawn()
		{
			PlayerVO getPlayerVO = _leader.GetPlayerVO;
			Transform layerTran = Singleton<UIViewManager>.Instance.GetLayerTran(40);
			_wingman = SpawnWingman(getPlayerVO, layerTran);
			_wingman.Init(getPlayerVO, layerTran);
			_wingman.SetShooter(getPlayerVO.shooter);
			return _wingman;
		}

		public void WingmanOut()
		{
			if (_wingman != null)
			{
				_wingman.Stop();
			}
		}

		public void WingmanPause()
		{
			if (_wingman != null)
			{
				_wingman.SetShooting(isPlay: false);
			}
		}

		public void WingmanPlay()
		{
			if (_wingman != null)
			{
				_wingman.SetShooting(isPlay: true);
			}
		}

		public void LeaderScale()
		{
			_leader.ZoomOut();
		}

		public void LeaderSetVip()
		{
			UnityEngine.Object.Destroy(_leader.gameObject);
			LeaderSpawn();
		}

		public void LeaderRest()
		{
			PlayerVO playerVO = CreatePlayerVO();
			_leader.SetShooter(playerVO.shooter);
			_leader.SetWeapon(playerVO.weapons);
			ContinueLeader(protect: false);
		}

		public void SetWeaponsShooting(bool isShoot)
		{
			_leader.SetShootWeaponState(state: false);
			_leader.SetWeaponsSetShooting(isShoot);
		}

		public void LeaderDie()
		{
			_leader.Die();
		}

		public void ChangeLeaderWeapon()
		{
			ShooterVO[] vos = CreateWeaponVO();
			_leader.ChangeWeapon(vos);
		}

		public void LeaderHurt()
		{
			_leader.Hurt();
		}

		public void ChangeLeaderShooter(ShooterType type, bool isEffect = true, bool isRestShoot = true)
		{
			ShooterVO vo = CreateShooterVO();
			vo.Type = type;
			_leader.SetShooter(vo, isChange: true);
			_leader.SetShooting(isPlay: false);
			if (isRestShoot)
			{
				_leader.ResetShoot();
			}
			if (isEffect)
			{
			}
			ChangeWingmanShooter(type);
		}

		public void ChangeWingmanShooter(ShooterType type)
		{
			if (_wingman != null && _wingman.gameObject.activeSelf)
			{
				ShooterVO vo = CreateShooterVO();
				vo.Type = type;
				_wingman.SetShooter(vo, isChange: true);
			}
		}

		public Transform GetLeader()
		{
			return _leader.tran;
		}

		public Player GetLeaderObj()
		{
			return _leader;
		}

		public void PauseLeader(bool isMove = false, bool isUnActiveCollider = false)
		{
			_leader.SetShooting(isPlay: false);
			_leader.enabled = isMove;
			_leader.StopFade();
			_leader.SetWeaponsSetShooting(isShoot: false);
			if (isUnActiveCollider)
			{
				_leader.SetCollider2DEnable(enable: false);
			}
		}

		public void BattleItemSetSheild(bool isActive)
		{
			_leader.ShieldItemActive(isActive);
		}

		public void PauseLeaderWeapon()
		{
			_leader.SetWeaponsSetShooting(isShoot: false);
		}

		public void LeaderDropItems(CoinManager.Type type)
		{
			Singleton<CoinManager>.Instance.Sqawn(_leader.transform.localPosition, type, Vector2.up, isPlayer: true);
		}

		public void ContinueLeader(bool protect = true, bool isShoot = true)
		{
			bool enabled = _leader.enabled;
			if (!enabled)
			{
				_leader.enabled = true;
			}
			if (protect)
			{
				_leader.Protect();
				return;
			}
			if (!enabled && isShoot)
			{
				_leader.ResetShoot();
			}
			if (!isShoot)
			{
				_leader.SetTouchState(isState: false);
			}
		}

		public bool IsLeaderTouch()
		{
			return _leader.isTouch;
		}

		private RoleBase Spawn(PlayerVO vo, Transform parent)
		{
			RoleBase roleBase = null;
			if (!_Dic.ContainsKey(vo.name))
			{
				roleBase = Object.Instantiate(Resources.Load<RoleBase>(vo.name));
				_Dic.Add(vo.name, roleBase);
			}
			else
			{
				roleBase = _Dic[vo.name];
				roleBase.gameObject.SetActive(value: true);
			}
			return roleBase;
		}

		private Wingman SpawnWingman(PlayerVO vo, Transform parent)
		{
			Wingman wingman = null;
			string text = "GameObject/Player/player_wingman";
			if (!_Dic.ContainsKey(text))
			{
				wingman = Object.Instantiate(Resources.Load<Wingman>(text));
				_Dic.Add(text, wingman);
			}
			else
			{
				wingman = (_Dic[text] as Wingman);
				wingman.gameObject.SetActive(value: true);
			}
			return wingman;
		}

		public void Reset(bool isTouch)
		{
			_leader.Reset(isTouch);
		}

		private PlayerVO CreatePlayerVO()
		{
			PlayerVO result = default(PlayerVO);
			result.name = ((!Singleton<VipManager>.Instance.VipStatus) ? "GameObject/Player/player_plane" : "GameObject/Player/player_plane_2");
			result.shooter = CreateShooterVO();
			if (GameGlobalData.WeaponID >= 0)
			{
				result.weapons = CreateWeaponVO();
			}
			return result;
		}

		private ShooterVO CreateShooterVO()
		{
			FireSpeedStruct fireSpeedData = GameGlobalData.GetFireSpeedData();
			FirePowerStruct firePowerData = GameGlobalData.GetFirePowerData();
			ShooterVO result = default(ShooterVO);
			result.FireBulletCount = fireSpeedData.Count;
			result.FirePower = GameUtils.UnitToFloat(firePowerData.Power);
			result.FireSpeedMax = ((!(result.FireBulletCount >= 70f)) ? 0.05f : 0.05f);
			if (result.FireBulletCount < 20f)
			{
				result.FireSpeedMax = 0.0769f;
			}
			else if (result.FireBulletCount < 40f)
			{
				result.FireSpeedMax = 0.0714f;
			}
			else if (result.FireBulletCount < 60f)
			{
				result.FireSpeedMax = 0.0667f;
			}
			else if (result.FireBulletCount < 80f)
			{
				result.FireSpeedMax = 0.0625f;
			}
			else if (result.FireBulletCount < 100f)
			{
				result.FireSpeedMax = 0.0588f;
			}
			else if (result.FireBulletCount < 120f)
			{
				result.FireSpeedMax = 0.0556f;
			}
			else if (result.FireBulletCount < 140f)
			{
				result.FireSpeedMax = 0.0526f;
			}
			else if (result.FireBulletCount < 160f)
			{
				result.FireSpeedMax = 0.05f;
			}
			else if (result.FireBulletCount < 180f)
			{
				result.FireSpeedMax = 0.0476f;
			}
			else if (result.FireBulletCount < 200f)
			{
				result.FireSpeedMax = 0.0455f;
			}
			else if (result.FireBulletCount < 220f)
			{
				result.FireSpeedMax = 0.04f;
			}
			else
			{
				result.FireSpeedMax = 0.0357f;
			}
			result.BulletMoveSpeed = 0.35f;
			result.Type = ShooterType.Change;
			return result;
		}

		private ShooterVO[] CreateWeaponVO()
		{
			ShooterVO[] array = new ShooterVO[2];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = CreateWeaponVO(i);
			}
			return array;
		}

		private ShooterVO CreateWeaponVO(int index)
		{
			ShooterVO result = default(ShooterVO);
			result.Dir = GameUtils.GetDir(index);
			int weaponID = GameGlobalData.WeaponID;
			WeaponStruct weaponStruct = Singleton<GameJson>.Instance.WeaponJson[weaponID];
			MaxWeaponState maxWeaponState = (MaxWeaponState)GameGlobalData.MaxWeaponState;
			bool flag = (maxWeaponState == MaxWeaponState.Free || maxWeaponState == MaxWeaponState.HasBeenFree || maxWeaponState == MaxWeaponState.HasBeenAdUse) && GameGlobalData.TempWeaponID == GameGlobalData.WeaponID;
			int num = (!flag) ? GameGlobalData.GetWeaponLevel(weaponID * 2, 0) : (weaponStruct.LevelMax[0] - 1);
			int num2 = (!flag) ? GameGlobalData.GetWeaponLevel(weaponID * 2, 1) : (weaponStruct.LevelMax[1] - 1);
			WeaponLevelStruct[] weaponLevelJson = Singleton<GameJson>.Instance.WeaponLevelJson;
			WeaponLevelStruct weaponLevelStruct = weaponLevelJson[num];
			WeaponLevelStruct weaponLevelStruct2 = weaponLevelJson[num2];
			switch (weaponID)
			{
			case 0:
			{
				float fireScale4 = (float)weaponLevelStruct.FireScale_1;
				float firePower5 = GameUtils.UnitToFloat(weaponLevelStruct2.FirePower_1);
				result.Type = ShooterType.GreenBall;
				result.FireScale = fireScale4;
				result.BulletMoveSpeed = (float)weaponStruct.BulletSpeed;
				result.FirePower = firePower5;
				result.FireDistance = (float)weaponStruct.FireDistance;
				break;
			}
			case 1:
			{
				float fireDistance = (float)weaponLevelStruct.FireDistance_2;
				float firePower4 = GameUtils.UnitToFloat(weaponLevelStruct2.FirePower_2);
				result.Type = ShooterType.Angle;
				result.BulletMoveSpeed = (float)weaponStruct.BulletSpeed;
				result.FirePower = firePower4;
				result.FireDistance = fireDistance;
				break;
			}
			case 2:
			{
				float fireScale3 = (float)weaponLevelStruct.FireWide_3;
				float firePower3 = GameUtils.UnitToFloat(weaponLevelStruct2.FirePower_3);
				result.Type = ShooterType.Defense;
				result.FirePower = firePower3;
				result.FireScale = fireScale3;
				result.FireDistance = (float)weaponStruct.FireDistance;
				break;
			}
			case 3:
			{
				float fireScale2 = (float)weaponLevelStruct.FireScale_4;
				float firePower2 = GameUtils.UnitToFloat(weaponLevelStruct2.FirePower_4);
				result.Type = ShooterType.Follow;
				result.FirePower = firePower2;
				result.FireDistance = (float)weaponStruct.FireDistance;
				result.FireScale = fireScale2;
				result.BulletMoveSpeed = (float)weaponStruct.BulletSpeed;
				break;
			}
			case 4:
			{
				float fireScale = (float)weaponLevelStruct.FireScale_5;
				float firePower = GameUtils.UnitToFloat(weaponLevelStruct2.FirePower_5);
				float netScale = (float)weaponLevelStruct.FireNetScale_5;
				result.Type = ShooterType.Thunder;
				result.FirePower = firePower;
				result.FireScale = fireScale;
				result.NetScale = netScale;
				result.FireDistance = (float)weaponStruct.FireDistance;
				result.BulletMoveSpeed = (float)weaponStruct.BulletSpeed;
				break;
			}
			}
			result.FireDistanceOffset = result.FireDistance / 2f * (float)index;
			result.IsInterval = (weaponStruct.FireDistanceOffset == 1);
			return result;
		}

		public double GetOutputFire()
		{
			FireSpeedStruct fireSpeedData = GameGlobalData.GetFireSpeedData();
			FirePowerStruct firePowerData = GameGlobalData.GetFirePowerData();
			return (float)fireSpeedData.Count * GameUtils.UnitToFloat(firePowerData.Power);
		}
	}
}
