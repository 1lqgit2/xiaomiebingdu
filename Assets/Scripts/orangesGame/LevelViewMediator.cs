using DG.Tweening;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	public class LevelViewMediator : Mediator, IMediator
	{
		private bool _isInit;

		private LevelView View => (LevelView)ViewComponent;

		public LevelViewMediator(string name, LevelView viewComponent)
			: base(name, viewComponent)
		{
		}

		public override void OnRegister()
		{
			base.OnRegister();
			View.btnClose.onClick.AddListener(OnClose);
		}

		public override void OnRemove()
		{
			base.OnRemove();
		}

		public override IList<string> ListNotificationInterests()
		{
			List<string> list = new List<string>();
			list.Add("LEVEL_VIEW_SHOW");
			list.Add("LEVEL_VIEW_ITEM_HIDE");
			list.Add("LEVEL_VIEW_ITEM_SHOW");
			return list;
		}

		public override void HandleNotification(INotification notification)
		{
			string name = notification.Name;
			if (name == null)
			{
				return;
			}
			if (!(name == "LEVEL_VIEW_SHOW"))
			{
				if (!(name == "LEVEL_VIEW_ITEM_HIDE"))
				{
					if (name == "LEVEL_VIEW_ITEM_SHOW")
					{
						SetItemShow();
					}
				}
				else
				{
					SetItemHide();
				}
			}
			else
			{
				SetView();
				_isInit = true;
			}
		}

		private void SetView()
		{
			int num = -1;
			int[] array = new int[13]
			{
				5,
				10,
				20,
				40,
				60,
				80,
				100,
				120,
				140,
				160,
				180,
				200,
				999
			};
			for (int i = 0; i < View.levelItems.Length; i++)
			{
				LevelNewItemComponent levelNewItemComponent = View.levelItems[i];
				SetItemText(levelNewItemComponent, i);
				levelNewItemComponent.SetBtnClick();
				int num2 = array[i];
				if (GameGlobalData.ActualLevel <= num2)
				{
					levelNewItemComponent.txtProgress.text = string.Format("({0}{1}</color>/{2})", "<color=#00ffffff>", GameGlobalData.ActualLevel, num2);
					levelNewItemComponent.SetImageActive();
					num = i + 1;
					break;
				}
				levelNewItemComponent.txtProgress.text = string.Format("({0}/{0})", num2);
				levelNewItemComponent.SetImageDark();
			}
			if (num >= 0)
			{
				Material grayMaterial = GameUtils.GetGrayMaterial();
				for (int j = num; j < View.levelItems.Length; j++)
				{
					LevelNewItemComponent levelNewItemComponent2 = View.levelItems[j];
					SetItemText(levelNewItemComponent2, j);
					levelNewItemComponent2.txtProgress.text = $"(0/{array[j]})";
					levelNewItemComponent2.SetImageGray(grayMaterial);
				}
				if (num >= 3)
				{
					float verticalNormalizedPosition = (float)num / (float)array.Length;
					View.srlRect.verticalNormalizedPosition = verticalNormalizedPosition;
				}
			}
			int num3 = (num >= 3) ? (num - 2) : 0;
			for (int k = num3; k < View.levelItems.Length; k++)
			{
				RectTransform rectTransform = View.levelItems[k].transform as RectTransform;
				RectTransform rectTransform2 = rectTransform;
				Vector2 anchoredPosition = rectTransform.anchoredPosition;
				rectTransform2.anchoredPosition = new Vector2(2200f, anchoredPosition.y);
				rectTransform.DOAnchorPosX(1080f, 0.6f).SetDelay(0.05f * (float)k).SetAutoKill()
					.SetEase(Ease.OutQuad);
			}
		}

		private void SetItemShow()
		{
			View.btnClose.gameObject.SetActive(value: true);
			for (int i = 0; i < View.levelItems.Length; i++)
			{
				RectTransform rectTransform = View.levelItems[i].transform as RectTransform;
				RectTransform rectTransform2 = rectTransform;
				Vector2 anchoredPosition = rectTransform.anchoredPosition;
				rectTransform2.anchoredPosition = new Vector2(2200f, anchoredPosition.y);
				rectTransform.DOAnchorPosX(1080f, 0.6f).SetDelay(0.05f * (float)i).SetAutoKill()
					.SetEase(Ease.OutQuad);
			}
		}

		private void SetItemHide()
		{
			View.btnClose.gameObject.SetActive(value: false);
			for (int i = 0; i < View.levelItems.Length; i++)
			{
				RectTransform target = View.levelItems[i].transform as RectTransform;
				target.DOAnchorPosX(2200f, 0.6f).SetDelay(0.05f * (float)i).SetAutoKill()
					.SetEase(Ease.OutQuad);
			}
		}

		private void SetItemText(LevelNewItemComponent item, int index)
		{
			item.index = index;
			item.txtTitle.text = LanguageConfig.constellation_names[index];
			if (item.txtContent != null)
			{
				item.txtContent.text = LanguageConfig.level_unlock_weapon;
			}
		}

		private void OnClose()
		{
			Singleton<UIViewManager>.Instance.HideView("LEVEL_VIEW");
		}
	}
}
