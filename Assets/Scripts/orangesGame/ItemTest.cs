using UnityEngine;

namespace orangesGame
{
	public class ItemTest : MonoBehaviour
	{
		public CoinManager.Type TestItem;

		public int MinVenomLevel = 3;

		public float Chance = 1f;

		public static ItemTest Instance
		{
			get;
			private set;
		}

		private void Awake()
		{
			Instance = this;
		}
	}
}
