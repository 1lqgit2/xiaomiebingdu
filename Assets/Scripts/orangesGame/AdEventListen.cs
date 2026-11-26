using System;

namespace orangesGame
{
	public class AdEventListen
	{
		public Action<string> OnLoad;

		public Action<string> OnStart;

		public Action<string> OnShow;

		public Action<string> OnClick;

		public Action<string, string, float> OnReward;

		public Action<string> OnClose;

		public Action<string, string> OnError;
	}
}
