using DG.Tweening;
using UnityEngine;

namespace orangesGame
{
	public class LaserBullet : Bullet
	{
		public SpriteRenderer sprRenderer;

		public BoxCollider2D collider2d;

		private float _zoomOutTime = 0.3f;

		private float _zoomInTime = 0.1f;

		private Vector3 _origin = new Vector3(0f, 1f, 1f);

		private void Awake()
		{
			AwakeInit();
		}

		public void SetSize(float wide)
		{
			Vector3 localPosition = _tran.localPosition;
			float y = localPosition.y;
			y = ((!(y <= 0f)) ? (9.6f - y) : (y * -1f + 9.6f));
			sprRenderer.transform.localScale = _origin;
			sprRenderer.size = new Vector2(wide, y);
			collider2d.size = new Vector2(wide - 0.5f, y);
			collider2d.offset = new Vector2(0f, y / 2f);
			ZoomIn();
		}

		public void SetProperty(float atk)
		{
			if (_atk != atk)
			{
				_atk = atk;
			}
		}

		public override void Clean()
		{
			sprRenderer.transform.DOKill();
			base.Clean();
		}

		private void ZoomIn()
		{
			sprRenderer.transform.DOScaleX(1f, _zoomInTime).SetAutoKill(autoKillOnCompletion: true).OnComplete(ZoomOut);
		}

		private void ZoomOut()
		{
			sprRenderer.transform.DOScaleX(0f, _zoomOutTime).SetAutoKill(autoKillOnCompletion: true).OnComplete(((Bullet)this).Clean);
		}

		private void OnStart()
		{
			collider2d.enabled = false;
		}
	}
}
