using DG.Tweening;
using UnityEngine;

namespace orangesGame
{
	public class SmallBullet : Bullet
	{
		public Sprite NormalStateSp;

		public Sprite EnhancedStateSp;

		public Sprite CoinBulletStateSp;

		public Sprite EnhancedCoinBulletStateSp;

		private SpriteRenderer _renderer;

		private float _wide;

		private float _wideHalf;

		private const string TAG_BARR = "Player";

		public float wideHalf => _wideHalf;

		public float wide => _wide;

		private void Awake()
		{
			AwakeInit();
			type = BulletType.SmallBullet;
			_renderer = _renderers[0];
			_wide = _renderer.sprite.textureRect.width / 100f;
			_wideHalf = _wide / 2f;
			isDirty = false;
		}

		private void Start()
		{
		}

		public void SetPropter(float atk)
		{
			if (_atk != atk)
			{
				_atk = atk;
			}
			if (GameGlobalData.IsPlayerBulletEnhanced || GameGlobalData.BattleItemData.BattleIsPlayerBulletEnhanced)
			{
				if (GameGlobalData.IsPlayerCoinBullet)
				{
					_renderer.sprite = EnhancedCoinBulletStateSp;
				}
				else
				{
					_renderer.sprite = EnhancedStateSp;
				}
			}
			else if (GameGlobalData.IsPlayerCoinBullet)
			{
				_renderer.sprite = CoinBulletStateSp;
			}
			else
			{
				_renderer.sprite = NormalStateSp;
			}
		}

		public override void OnTriggerLogic(float ballValue = 0f)
		{
			base.OnTriggerLogic(ballValue);
			Clean();
		}

		public override void PlaySound()
		{
			Singleton<AudioManager>.Instance.PlaySpeciSound("hit", isLoop: true);
		}

		public override void Clean()
		{
			_tran.DOKill();
			base.Clean();
		}
	}
}
