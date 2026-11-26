using System.Collections.Generic;

namespace orangesGame
{
	internal class AdjustSdk : Singleton<AdjustSdk>
	{
		public void TrackEvent(string eventName)
		{
			Singleton<SdkUtils>.Instance.AdjustTrackEvent(eventName);
		}

		public void TrackEventParam(string eventName, Dictionary<string, string> dic)
		{
			Singleton<SdkUtils>.Instance.AdjustTrackEventParam(eventName, dic);
		}
	}
}
