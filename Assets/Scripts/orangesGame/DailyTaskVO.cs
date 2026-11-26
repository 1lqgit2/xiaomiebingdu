namespace orangesGame
{
	public class DailyTaskVO
	{
		public DailyTaskType type;

		public int progress;

		public int progressMax;

		public float rate;

		public int index;

		public int rewardState;

		public bool isFinish => progress >= progressMax;

		public DailyTaskVO(DailyTaskType type, int progress, int progressMax, float rate, int index, int reward)
		{
			this.type = type;
			this.progress = progress;
			this.progressMax = progressMax;
			this.rate = rate;
			this.index = index;
			rewardState = reward;
		}
	}
}
