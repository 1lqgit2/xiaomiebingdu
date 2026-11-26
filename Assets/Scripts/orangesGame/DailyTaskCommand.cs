using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine;

namespace orangesGame
{
	public class DailyTaskCommand : SimpleCommand, ICommand
	{
		private int _finalIndex;

		public override void Execute(INotification notification)
		{
			string name = notification.Name;
			if (name == null)
			{
				return;
			}
			if (!(name == "TASK_GETREWARD"))
			{
				if (!(name == "TASK_UPGRADE"))
				{
					if (!(name == "TASK_GETREWARD_FINAL"))
					{
						if (name == "TASK_COIN_MULT")
						{
							Singleton<DailyTaskHandle>.Instance.AddProgress(DailyTaskType.CoinReward_3, 1);
							int index = Singleton<DailyTaskHandle>.Instance.GetIndex(DailyTaskType.CoinReward_3);
							if (index >= 0)
							{
								SendNotification("TASK_ITEM_REFRESH", index);
								SendNotification("MAIN_TASK_TIP");
							}
						}
					}
					else
					{
						_finalIndex = (int)notification.Body;
						if (Singleton<DailyTaskHandle>.Instance.IsReward(_finalIndex) && Singleton<AdManager>.Instance.IsLoad("0a2cad70-9e1c-450f-bcdc-f3798b324b95", "OnClickTask"))
						{
							Singleton<AdManager>.Instance.ShowVideo("0a2cad70-9e1c-450f-bcdc-f3798b324b95", "taskgold", AdsTaskReward);
						}
					}
				}
				else
				{
					Singleton<DailyTaskHandle>.Instance.AddProgress(DailyTaskType.UpGrade_5, 1);
					SendNotification("MAIN_TASK_TIP");
				}
			}
			else
			{
				int num = (int)notification.Body;
				if (Singleton<DailyTaskHandle>.Instance.IsReward(num))
				{
					Singleton<UIViewManager>.Instance.ShowView("DIALOG_VIEW");
					SendNotification("DIALOG_TASK", num);
				}
			}
		}

		private void AdsTaskReward(bool isReward)
		{
			if (isReward)
			{
				Singleton<DailyTaskHandle>.Instance.SetRewardState(_finalIndex);
				SendNotification("TASK_REWARD", _finalIndex);
				SendNotification("MAIN_TASK_TIP");
				Transform layerTran = Singleton<UIViewManager>.Instance.GetLayerTran(110);
				Singleton<CoinManager>.Instance.FloatByCircle<FloatGold>("GameObject/Item/FloatCoin", 20, Vector2.zero, GameGlobalData.CoinRecoverTopLeftPos, 0.25f, delegate
				{
					GameGlobalData.AddCurCoin(Singleton<DailyTaskHandle>.Instance.GetRewardCoin(_finalIndex));
				}, layerTran);
				GameAnalysisData.SendShowVedio("taskgold");
				GameAnalysisData.GetMissionReward(8, 1);
			}
		}
	}
}
