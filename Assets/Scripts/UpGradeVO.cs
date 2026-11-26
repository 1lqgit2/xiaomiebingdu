using System;

namespace orangesGame
{
	public struct UpGradeVO
	{
		public float cost;

		public Action<int> onUpGrade;

		public int weaponIndex;
	}
}
