namespace orangesGame
{
	public class AdVideoAction : AdAction
	{
		private bool _isReward;

		public AdVideoAction()
		{
			_event = new AdEventListen();
			_event.OnLoad = ((AdAction)this).OnLoad;
			_event.OnClick = OnClick;
			_event.OnShow = OnShow;
			_event.OnClose = ((AdAction)this).OnClose;
			_event.OnError = ((AdAction)this).OnError;
			_event.OnReward = ((AdAction)this).OnReward;
		}

		public override void OnLoad(string id)
		{
			base.OnLoad(id);
		}

		public override void OnClose(string id)
		{
			base.OnClose(id);
			if (_onComplete != null)
			{
				bool temp = _isReward;
				UnityMainThreadDispatcher.Instance().Enqueue(delegate
				{
					_onComplete(temp);
				});
			}
			_isReward = false;
			Singleton<AdManager>.Instance.LoadVideo(id);
		}

		public override void OnReward(string id, string type, float cost)
		{
			base.OnReward(id, type, cost);
			_isReward = true;
			UnityMainThreadDispatcher.Instance().Enqueue(delegate
			{
				GameGlobalData.UpdateADSTime(isVedio: true);
				Singleton<AdjustConfig>.Instance.FinishVideoTrack();
			});
		}

		public override void OnError(string id, string msg)
		{
			base.OnError(id, msg);
			Timer.Register(_reloadTime, delegate
			{
				Singleton<AdManager>.Instance.LoadVideo(id);
			});
		}
	}
}
