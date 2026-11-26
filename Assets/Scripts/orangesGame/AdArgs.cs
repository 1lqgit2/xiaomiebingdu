namespace orangesGame
{
	public class AdArgs
	{
		public string Appkey;

		public string Appsecret;

		public string Appchannel;

		public string UMKey;

		public string[] BannerIDs;

		public string[] InterstitialIDs;

		public string[] VideoIDs;

		public AdEventListen BannerEvent = new AdEventListen();

		public AdEventListen InterstitialEvent = new AdEventListen();

		public AdEventListen VideoEvent = new AdEventListen();
	}
}
