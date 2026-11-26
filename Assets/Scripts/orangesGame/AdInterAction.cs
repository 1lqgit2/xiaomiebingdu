namespace orangesGame
{
	public class AdInterAction : AdAction
	{
		public AdInterAction()
		{
			_event = new AdEventListen();
			_event.OnLoad = ((AdAction)this).OnLoad;
			_event.OnClick = OnClick;
			_event.OnShow = OnShow;
			_event.OnClose = ((AdAction)this).OnClose;
			_event.OnError = ((AdAction)this).OnError;
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
				_onComplete(obj: true);
				UnityMainThreadDispatcher.Instance().Enqueue(delegate
				{
					GameGlobalData.UpdateADSTime(isVedio: false);
				});
			}
			Singleton<AdManager>.Instance.LoadInterstitial("35eeefa8-c011-4112-bc9b-4a04fe3c413c");
		}

		public override void OnError(string id, string msg)
		{
			base.OnError(id, msg);
			Timer.Register(_reloadTime, delegate
			{
				Singleton<AdManager>.Instance.LoadInterstitial("35eeefa8-c011-4112-bc9b-4a04fe3c413c");
			});
		}
	}
}
