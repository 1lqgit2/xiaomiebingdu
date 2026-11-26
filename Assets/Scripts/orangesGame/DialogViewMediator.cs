using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace orangesGame
{
	public class DialogViewMediator : Mediator, IMediator
	{
		private const string UPGRADE_INACTIVE = "anniu_b_3";

		private const string UPGRADE_GREEN = "anniu_b_2";

		private int _state;

		private int _taskIndex;

		private DialogData _dialogData;

		private DialogView View => (DialogView)ViewComponent;

		public DialogViewMediator(string name, DialogView viewComponent)
			: base(name, viewComponent)
		{
		}

		public override void OnRegister()
		{
			base.OnRegister();
			View.btnLeft.onClick.AddListener(OnClickOfflineReward);
			View.btnRight.onClick.AddListener(OnClickOfflineAdReward);
			View.dialogBlackGround.onClick.AddListener(OnClickHideView);
			View.btnClose.onClick.AddListener(OnClickHideView);
			View.txtLeftCollect.text = LanguageConfig.COLLECT;
			View.btnDialogLeft.onClick.AddListener(OnClickDialogLeft);
			View.btnDialogRight.onClick.AddListener(OnClickDialogRight);
		}

		public override void OnRemove()
		{
			base.OnRemove();
		}

		public override IList<string> ListNotificationInterests()
		{
			List<string> list = new List<string>();
			list.Add("DIALOG_OFFLINE");
			list.Add("DIALOG_TASK");
			list.Add("DIALOG_COMMON");
			list.Add("DIALOG_DOWNLOAD");
			return list;
		}

		public override void HandleNotification(INotification notification)
		{
			string name = notification.Name;
			if (name == null)
			{
				return;
			}
			if (!(name == "DIALOG_OFFLINE"))
			{
				if (!(name == "DIALOG_TASK"))
				{
					if (!(name == "DIALOG_COMMON"))
					{
						if (name == "DIALOG_DOWNLOAD")
						{
							SetDownloadDialog();
						}
					}
					else
					{
						SetCommonDialog(notification.Body as DialogData);
					}
				}
				else
				{
					_state = 1;
					SetTask((int)notification.Body);
				}
			}
			else
			{
				_state = 0;
				SetOffLine();
			}
		}

		private void SetTask(int index)
		{
			View.tranReward.gameObject.SetActive(value: true);
			string str = (!Singleton<VipManager>.Instance.VipStatus) ? " x3" : " x4";
			View.txtRightCollect.text = LanguageConfig.COLLECT + str;
			float rewardCoin = Singleton<DailyTaskHandle>.Instance.GetRewardCoin(index);
			View.txtTitle.text = string.Format(LanguageConfig.DIALOG_TASK, GameUtils.FloatToUnit(rewardCoin));
			bool flag = Singleton<AdManager>.Instance.IsLoad("0a2cad70-9e1c-450f-bcdc-f3798b324b95", "SetTask");
			SpriteAtlas spriteAtlas = Resources.Load<SpriteAtlas>("GameAtlas/MainAtlas");
			if (flag)
			{
				View.btnRight.image.sprite = spriteAtlas.GetSprite("anniu_b_2");
			}
			else
			{
				View.btnRight.image.sprite = spriteAtlas.GetSprite("anniu_b_3");
			}
			View.btnRight.interactable = flag;
			_taskIndex = index;
		}

		private void SetOffLine()
		{
			string str = (!Singleton<VipManager>.Instance.VipStatus) ? " x3" : " x4";
			View.txtRightCollect.text = LanguageConfig.COLLECT + str;
			View.tranReward.gameObject.SetActive(value: true);
			View.txtTitle.text = string.Format(LanguageConfig.DIALOG_OFFLINE, GameUtils.CoinParse(GameGlobalData.OffLineCoin));
			bool flag = Singleton<AdManager>.Instance.IsLoad("0a2cad70-9e1c-450f-bcdc-f3798b324b95", "OnClickOffline");
			SpriteAtlas spriteAtlas = Resources.Load<SpriteAtlas>("GameAtlas/MainAtlas");
			if (flag)
			{
				View.btnRight.image.sprite = spriteAtlas.GetSprite("anniu_b_2");
			}
			else
			{
				View.btnRight.image.sprite = spriteAtlas.GetSprite("anniu_b_3");
			}
			View.btnRight.interactable = flag;
		}

		private void SetCommonDialog(DialogData data)
		{
			View.tranDialog.gameObject.SetActive(value: true);
			_dialogData = data;
			View.txtDialogContent.text = data.content;
		}

		private void SetDownloadDialog()
		{
			View.tranDownload.gameObject.SetActive(value: true);
			View.dialogBlackGround.enabled = false;
			View.btnDonwload.onClick.AddListener(delegate
			{
				Application.OpenURL(GameGlobalData.GARDE_URL);
			});
		}

		private void OnClickOfflineReward()
		{
			if (_state == 0)
			{
				GetReward(isAds: false);
			}
			else if (_state == 1)
			{
				GetTaskReward(isAds: false);
			}
		}

		private void GetReward(bool isAds)
		{
			Singleton<UIViewManager>.Instance.HideView("DIALOG_VIEW");
			SendNotification("MAIN_GET_OFFLINE", isAds);
		}

		private void GetTaskReward(bool isAds)
		{
			Singleton<UIViewManager>.Instance.HideView("DIALOG_VIEW");
			Singleton<DailyTaskHandle>.Instance.SetRewardState(_taskIndex);
			SendNotification("TASK_REWARD", _taskIndex);
			SendNotification("MAIN_TASK_TIP");
			Transform layerTran = Singleton<UIViewManager>.Instance.GetLayerTran(110);
			if (isAds)
			{
				bool isVip = Singleton<VipManager>.Instance.VipStatus;
				Singleton<CoinManager>.Instance.FloatByCircle<FloatGold>("GameObject/Item/FloatCoin", 20, Vector2.zero, GameGlobalData.CoinRecoverTopLeftPos, 0.25f, delegate
				{
					GameGlobalData.AddCurCoin((!isVip) ? (Singleton<DailyTaskHandle>.Instance.GetRewardCoin(_taskIndex) * 3f) : (Singleton<DailyTaskHandle>.Instance.GetRewardCoin(_taskIndex) * 4f));
				}, layerTran);
				GameAnalysisData.SendShowVedio("taskgold");
				DailyTaskVO dailyTaskVO = Singleton<DailyTaskHandle>.Instance.GetList()[_taskIndex];
				GameAnalysisData.GetMissionReward((int)dailyTaskVO.type, 1);
			}
			else
			{
				Singleton<CoinManager>.Instance.FloatByCircle<FloatGold>("GameObject/Item/FloatCoin", 20, Vector2.zero, GameGlobalData.CoinRecoverTopLeftPos, 0.25f, delegate
				{
					GameGlobalData.AddCurCoin(Singleton<DailyTaskHandle>.Instance.GetRewardCoin(_taskIndex));
				}, layerTran);
				DailyTaskVO dailyTaskVO2 = Singleton<DailyTaskHandle>.Instance.GetList()[_taskIndex];
				GameAnalysisData.GetMissionReward((int)dailyTaskVO2.type, 0);
			}
			Timer.Register(0.8f, delegate
			{
				for (int i = 0; i < 4; i++)
				{
					Timer.Register((float)i * 0.2f, delegate
					{
						Singleton<AudioManager>.Instance.PlaySound("coin");
					});
				}
			});
		}

		private void OnClickOfflineAdReward()
		{
			if (_state == 0)
			{
				Singleton<AdManager>.Instance.ShowVideo("0a2cad70-9e1c-450f-bcdc-f3798b324b95", "offlinegold", AdOfflineAdReward);
			}
			else if (_state == 1)
			{
				Singleton<AdManager>.Instance.ShowVideo("0a2cad70-9e1c-450f-bcdc-f3798b324b95", "taskgold", AdsTaskReward);
			}
		}

		private void OnClickHideView()
		{
			Singleton<UIViewManager>.Instance.HideView("DIALOG_VIEW");
			SendNotification("MAIN_SET_CLICK_OFFLINE_STATE", false);
		}

		private void AdOfflineAdReward(bool isReward)
		{
			if (isReward)
			{
				if (Singleton<VipManager>.Instance.VipStatus)
				{
					GameGlobalData.OffLineCoin *= 4;
				}
				else
				{
					GameGlobalData.OffLineCoin *= 3;
				}
				GetReward(isAds: true);
				Singleton<DailyTaskHandle>.Instance.AddProgress(DailyTaskType.CoinReward_3, 1);
			}
			else
			{
				GetReward(isAds: false);
			}
		}

		private void AdsTaskReward(bool isReward)
		{
			if (isReward)
			{
				GetTaskReward(isAds: true);
				SendNotification("TASK_COIN_MULT");
			}
			else
			{
				GetTaskReward(isAds: false);
			}
		}

		private void OnClickDialogLeft()
		{
			if (_dialogData != null)
			{
				_dialogData.callback();
			}
			OnClickDialogRight();
		}

		private void OnClickDialogRight()
		{
			Singleton<UIViewManager>.Instance.HideView("DIALOG_VIEW");
		}
	}
}
