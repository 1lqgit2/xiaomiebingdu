namespace orangesGame
{
	public class LuckyDrawVo
	{
		private int type;

		private float rate;

		private int itemId;

		private float addNum;

		public LuckyDrawVo(int type, int itemId, float addNum, float rate)
		{
			this.type = type;
			this.itemId = itemId;
			this.rate = rate;
			this.addNum = addNum;
		}

		public int GetItemId()
		{
			return itemId;
		}

		public int GetDrawType()
		{
			return type;
		}

		public float GetRate()
		{
			return rate;
		}

		public float GetAddNum()
		{
			return addNum;
		}
	}
}
