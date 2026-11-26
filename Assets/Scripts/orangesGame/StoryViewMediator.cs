using DG.Tweening;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections.Generic;

namespace orangesGame
{
	public class StoryViewMediator : Mediator, IMediator
	{
		private int _clickIndex;

		private Timer _timer;

		private StoryView View => (StoryView)ViewComponent;

		public StoryViewMediator(string name, StoryView viewComponent)
			: base(name, viewComponent)
		{
		}

		public override void OnRegister()
		{
			base.OnRegister();
			View.btnClick.onClick.AddListener(OnClick);
			View.txtContent.text = string.Empty;
			View.txtClick.text = string.Empty;
		}

		public override IList<string> ListNotificationInterests()
		{
			List<string> list = new List<string>();
			list.Add("STORY_VIEW_SHOW");
			return list;
		}

		public override void HandleNotification(INotification notification)
		{
			string name = notification.Name;
			if (name != null && name == "STORY_VIEW_SHOW")
			{
				View.txtContent.DOText(LanguageConfig.story, 2f).OnComplete(OnClick);
			}
		}

		private void OnClick()
		{
			if (_clickIndex == 0)
			{
				_clickIndex++;
				View.txtContent.DOKill();
				View.txtContent.text = LanguageConfig.story;
				View.txtClick.text = LanguageConfig.clickAndContinues;
				_timer = Timer.Register(2f, OnClick);
				return;
			}
			if (_timer != null)
			{
				_timer.Cancel();
				_timer = null;
			}
			View.txtContent.text = string.Empty;
			View.txtClick.text = string.Empty;
			Singleton<DialogueManager>.Instance.ShowFirstNpcTip();
			Singleton<DialogueManager>.Instance.SetShowFirstNpc();
		}
	}
}
