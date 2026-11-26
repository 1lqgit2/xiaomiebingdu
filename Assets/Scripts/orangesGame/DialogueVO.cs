using System;

namespace orangesGame
{
	public class DialogueVO
	{
		public string content;

		public int npcID;

		public DialogueType type;

		public Action callback;

		public bool firstClickStart;

		public float moveSpeed = 0.5f;
	}
}
