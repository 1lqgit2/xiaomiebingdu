using UnityEngine;

namespace orangesGame
{
	public class DefenseBullet : Bullet
	{
		public GameObject trail;

		private float _dir;

		private float _angle;

		private const float MAX_RADIUS = 3.6f;

		private const float MIN_RADIUS = 0.8f;

		private const float MAX_ANGLE = 360f;

		private const float MIN_ANGLE = 90f;

		private float _curAngle = 90f;

		private float _curRadius;

		private float _time;

		private Transform _target;

		private void Awake()
		{
			AwakeInit();
		}

		private void Start()
		{
			_speed = 3f;
		}

		public void SetProperty(float atk, float speed, float dir)
		{
			if (_atk != atk)
			{
				_atk = atk;
				_angle = speed;
				_dir = dir;
			}
			trail.gameObject.SetActive(value: true);
			_target = Singleton<PlayerManager>.Instance.GetLeader();
		}

		public override void Clean()
		{
			base.Clean();
			_curAngle = 90f;
			_curRadius = 0f;
			_time = 0f;
			trail.gameObject.SetActive(value: false);
		}

		private void Update()
		{
			if (!isDirty)
			{
				Moving();
			}
		}

		private void Moving()
		{
			if (_target != null)
			{
				if (_curRadius < 3.6f)
				{
					_curRadius = Mathf.Lerp(0.8f, 3.6f, _time);
					_time += Time.deltaTime;
				}
				base.tran.position = _target.position + Quaternion.Euler(0f, 0f, _curAngle) * new Vector3(0f, _curRadius * _dir, 0f);
				Vector2 vector = _target.position - base.tran.position;
				float angle = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
				base.tran.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
				_curAngle -= _angle * Time.deltaTime;
				if (_curAngle >= 360f)
				{
					_curAngle = 0f;
				}
			}
		}
	}
}
