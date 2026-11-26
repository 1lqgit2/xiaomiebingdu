using System;

namespace orangesGame
{
	public class AdAction
	{
		protected bool _isLoade;

		protected float _reloadTime = 10f;

		protected AdEventListen _event;

		protected Action<bool> _onComplete;

		public AdEventListen Adevent => _event;

		public Action<bool> onComplete
		{
			set
			{
				_onComplete = value;
			}
		}

		public virtual void OnLoad(string id)
		{
		}

		public virtual void OnClick(string id)
		{
		}

		public virtual void OnClose(string id)
		{
		}

		public virtual void OnShow(string id)
		{
		}

		public virtual void OnReward(string id, string type, float cost)
		{
		}

		public virtual void OnError(string id, string msg)
		{
		}
	}
}
