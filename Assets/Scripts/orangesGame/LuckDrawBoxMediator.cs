using DG.Tweening;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	public class LuckDrawBoxMediator : Mediator, IMediator
	{
		private LuckyDrawVo _drawVo;

		private float _itemWide = 420f;

		private LuckDrawBoxView View => (LuckDrawBoxView)ViewComponent;

		public LuckDrawBoxMediator(string name, LuckDrawBoxView viewComponent)
			: base(name, viewComponent)
		{
		}

		public override void OnRegister()
		{
			base.OnRegister();
			View.btnConfirm.onClick.AddListener(ClickConfirm);
		}

		public override IList<string> ListNotificationInterests()
		{
			List<string> list = new List<string>();
			list.Add("LUCK_DRAW_BOX_INIT");
			return list;
		}

		public override void HandleNotification(INotification notification)
		{
			string name = notification.Name;
			if (name != null && name == "LUCK_DRAW_BOX_INIT")
			{
				LuckDrawInit();
			}
		}

		private void LuckDrawInit()
		{
			View.TreasureBox.gameObject.SetActive(value: true);
			View.BoxItemTransform.gameObject.SetActive(value: false);
			View.btnConfirm.gameObject.SetActive(value: false);
			View.AnimationTransform.localPosition = Vector3.zero;
			LuckyDrawStruct[] luckyDrawJson = Singleton<GameJson>.Instance.LuckyDrawJson;
			for (int i = 0; i < luckyDrawJson.Length; i++)
			{
				if (luckyDrawJson[i].Type == 7 || luckyDrawJson[i].Type == 6)
				{
					float fireCost = GameGlobalData.GetFireCost(GameGlobalData.GetFirePowerData());
					View.ItemNumsTexts[i].text = "+ " + GameUtils.FloatToUnit((float)luckyDrawJson[i].AddNum * fireCost);
				}
			}
			_drawVo = Singleton<LuckyDrawManager>.Instance.LuckyDraw();
			Timer.Register(3f, TreasureBoxOpenFinish);
		}

		private void TreasureBoxOpenFinish()
		{
			View.TreasureBox.gameObject.SetActive(value: false);
			View.BoxItemTransform.gameObject.SetActive(value: true);
			View.AnimationTransform.DOLocalMoveX((0f - _itemWide) * 8f, 0.6f).SetLoops(5).SetAutoKill(autoKillOnCompletion: true)
				.SetEase(Ease.Linear)
				.OnComplete(delegate
				{
					View.AnimationTransform.DOLocalMoveX(-8f, 0f);
					View.AnimationTransform.DOLocalMoveX((0f - _itemWide) * 8f, 0.8f).SetAutoKill(autoKillOnCompletion: true).SetEase(Ease.Linear)
						.OnComplete(delegate
						{
							View.AnimationTransform.DOLocalMoveX(-8f, 0f);
							View.AnimationTransform.DOLocalMoveX((0f - _itemWide) * (float)(_drawVo.GetDrawType() - 1), 1f).SetAutoKill(autoKillOnCompletion: true).SetEase(Ease.Linear)
								.OnComplete(BoxItemDrawFinish);
						});
				});
		}

		private void BoxItemDrawFinish()
		{
			View.btnConfirm.gameObject.SetActive(value: true);
		}

		private void ClickConfirm()
		{
			Singleton<LuckyDrawManager>.Instance.AddLuckyDraw(_drawVo);
			Singleton<UIViewManager>.Instance.HideView("LUCK_BOX_DRAW_VIEW");
		}
	}
}
