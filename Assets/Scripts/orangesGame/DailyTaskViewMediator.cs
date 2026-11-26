using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	public class DailyTaskViewMediator : Mediator, IMediator
	{
		private Timer _timer;

		private int _curTime;

		private List<UITaskItem> _items;

		private DailyTaskView View => (DailyTaskView)ViewComponent;

		public DailyTaskViewMediator(string name, DailyTaskView viewComponent)
			: base(name, viewComponent)
		{
		}

		public override void OnRegister()
		{
			base.OnRegister();
			View.btnClose.onClick.AddListener(OnClickClose);
			_items = new List<UITaskItem>();
			View.txtTitle.text = LanguageConfig.DIALY_TASK_TITLE;
			View.txtTimeTitle.text = LanguageConfig.DIALY_TASK_TIME_TITLE;
		}

		public override void OnRemove()
		{
			base.OnRemove();
		}

		public override IList<string> ListNotificationInterests()
		{
			List<string> list = new List<string>();
			list.Add("TASK_INIT");
			list.Add("TASK_REWARD");
			list.Add("TASK_ITEM_REFRESH");
			return list;
		}

		public override void HandleNotification(INotification notification)
		{
			string name = notification.Name;
			if (name == null)
			{
				return;
			}
			if (!(name == "TASK_INIT"))
			{
				if (!(name == "TASK_REWARD"))
				{
					if (name == "TASK_ITEM_REFRESH")
					{
						List<DailyTaskVO> list = Singleton<DailyTaskHandle>.Instance.GetList();
						int index = (int)notification.Body;
						_items[index].SetTask(list[index], GameGlobalData.TaskFirePowerCost);
					}
				}
				else
				{
					_items[(int)notification.Body].SetBtnRewardActive(isActive: false);
					List<DailyTaskVO> list2 = Singleton<DailyTaskHandle>.Instance.GetList();
					int index2 = _items.Count - 1;
					_items[index2].SetSpecialTask(list2[index2], GameGlobalData.TaskFirePowerCost);
				}
			}
			else
			{
				if (_items.Count == 0)
				{
					InitTaskItems();
				}
				SetTaskItems();
				SetTimer();
			}
		}

		private void SetTimer()
		{
			_curTime = Singleton<DailyTaskHandle>.Instance.GetTime();
			View.txtTime.text = GameUtils.ParseSecondToHHMMSS(_curTime);
			_timer = Timer.Register(1f, TimerCallback, null, isLooped: true);
		}

		private void InitTaskItems()
		{
			List<DailyTaskVO> list = Singleton<DailyTaskHandle>.Instance.GetList();
			int num = list.Count - 1;
			for (int i = 0; i < num; i++)
			{
				UITaskItem uITaskItem = Singleton<ResLoadManager>.Instance.ResLoad<UITaskItem>("GameUI/DailyTask/ui_dailytaskview_item_task", isPreLoad: false);
				uITaskItem.Init(View.Root.transform, new Vector2(0f, 450 - i * 185));
				_items.Add(uITaskItem);
			}
			UITaskItem uITaskItem2 = Singleton<ResLoadManager>.Instance.ResLoad<UITaskItem>("GameUI/DailyTask/ui_dailytaskview_item_task", isPreLoad: false);
			uITaskItem2.Init(View.Root.transform, new Vector2(0f, 450 - num * 185));
			_items.Add(uITaskItem2);
		}

		private void SetTaskItems()
		{
			List<DailyTaskVO> list = Singleton<DailyTaskHandle>.Instance.GetList();
			int num = _items.Count - 1;
			for (int i = 0; i < num; i++)
			{
				_items[i].SetTask(list[i], GameGlobalData.TaskFirePowerCost);
			}
			_items[num].SetSpecialTask(list[num], GameGlobalData.TaskFirePowerCost);
		}

		private void TimerCallback()
		{
			if (_curTime <= 0)
			{
				Singleton<DailyTaskHandle>.Instance.Init();
				SetTaskItems();
			}
			_curTime--;
			View.txtTime.text = GameUtils.ParseSecondToHHMMSS(_curTime);
		}

		private void OnClickClose()
		{
			if (_timer != null)
			{
				_timer.Cancel();
			}
			Singleton<UIViewManager>.Instance.HideView("TASK_VIEW");
		}
	}
}
