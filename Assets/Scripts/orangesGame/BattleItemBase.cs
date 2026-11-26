using System;

namespace orangesGame
{
	public abstract class BattleItemBase
	{
		public enum BattleItemState
		{
			None = -1,
			Ready,
			InUse
		}

		public string ItemName;

		public int ItemPrice;

		public int ItemTime;

		public int ItemId;

		public string ItemIconName;

		public BattleItemState CurrentBattleItemState = BattleItemState.None;

		protected Timer CloseTimer;

		public Action<float, float> ActionItemCountDownUpdate;

		public Action ActionItemTimeOut;

		protected BattleItemBase(string itemName, int itemPrice, int itemTime, int itemId)
		{
			ItemName = itemName;
			ItemPrice = itemPrice;
			ItemTime = itemTime;
			ItemId = itemId;
			CurrentBattleItemState = BattleItemState.Ready;
		}

		protected virtual void TimeOut()
		{
			CloseTimer = null;
			CurrentBattleItemState = BattleItemState.Ready;
			ApplicationFacade.Instance.SendNotification("BATTLE_ITEM_STATE_TIMEOUT", ItemId);
			if (ActionItemTimeOut != null)
			{
				ActionItemTimeOut();
			}
		}

		protected virtual void Pause()
		{
			if (CloseTimer != null)
			{
				CloseTimer.Pause();
			}
		}

		protected virtual void Continue()
		{
			if (CloseTimer != null)
			{
				CloseTimer.Resume();
			}
		}

		public virtual void Employ()
		{
			ActionItemCountDownUpdate = null;
			ActionItemTimeOut = null;
			StartTimer();
		}

		protected void StartTimer()
		{
			if (CloseTimer != null)
			{
				CloseTimer.Cancel();
			}
			CurrentBattleItemState = BattleItemState.InUse;
			ApplicationFacade.Instance.SendNotification("BATTLE_ITEM_STATE_START", this);
			CloseTimer = Timer.Register(ItemTime, TimeOut, delegate(float delta)
			{
				if (ActionItemCountDownUpdate != null)
				{
					ActionItemCountDownUpdate(delta, ItemTime);
				}
			});
		}
	}
}
