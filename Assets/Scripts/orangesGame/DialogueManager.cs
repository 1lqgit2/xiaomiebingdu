using UnityEngine;

namespace orangesGame
{
	internal class DialogueManager : Singleton<DialogueManager>
	{
		private int ShowFirstNpc;

		public void InitShowFirshNpc()
		{
			ShowFirstNpc = PlayerPrefs.GetInt("ShowFirstNpc", 0);
		}

		public void SetShowFirstNpc()
		{
			ShowFirstNpc = 1;
			PlayerPrefs.SetInt("ShowFirstNpc", ShowFirstNpc);
		}

		public bool CanShowFirstNpc()
		{
			return ShowFirstNpc == 0;
		}

		public void ShowFirstNpcTip()
		{
			Singleton<UIViewManager>.Instance.ShowView("DIALOGUE_VIEW");
			DialogueVO dialogueVO = new DialogueVO();
			dialogueVO.npcID = 0;
			dialogueVO.type = DialogueType.GameTip;
			dialogueVO.content = LanguageConfig.level_start_npc;
			dialogueVO.firstClickStart = true;
			dialogueVO.callback = delegate
			{
				Singleton<UIViewManager>.Instance.HideView("STORY_VIEW");
			};
			ApplicationFacade.Instance.SendNotification("SHOW_DIALOGUE_TIP", dialogueVO);
		}
	}
}
