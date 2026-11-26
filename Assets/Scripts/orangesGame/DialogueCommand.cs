using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;

namespace orangesGame
{
	public class DialogueCommand : SimpleCommand, ICommand
	{
		public override void Execute(INotification notification)
		{
			string name = notification.Name;
			if (name == null)
			{
				return;
			}
			if (!(name == "DIALOGUE_CHECK_START"))
			{
				if (!(name == "DIALOGUE_CHECK"))
				{
					return;
				}
				int num = GameGlobalData.ActualLevel - 1;
				NpcLevelStruct[] npcLevelStruct = Singleton<GameJson>.Instance.NpcLevelStruct;
				int num2 = 1;
				while (true)
				{
					if (num2 >= npcLevelStruct.Length)
					{
						return;
					}
					if (num == npcLevelStruct[num2].Level)
					{
						if (npcLevelStruct[num2].Repeat == 1)
						{
							SetDialogue(npcLevelStruct[num2]);
							return;
						}
						if (npcLevelStruct[num2].Repeat == 0 && !GameGlobalData.NpcData.Contains(npcLevelStruct[num2].ID))
						{
							break;
						}
					}
					num2 += 2;
				}
				SetDialogue(npcLevelStruct[num2]);
				GameGlobalData.NpcData.SaveNpcLevelID(npcLevelStruct[num2].ID);
				return;
			}
			NpcLevelStruct[] npcLevelStruct2 = Singleton<GameJson>.Instance.NpcLevelStruct;
			for (int i = 0; i < npcLevelStruct2.Length; i += 2)
			{
				if (GameGlobalData.ActualLevel == npcLevelStruct2[i].Level)
				{
					if (npcLevelStruct2[i].Repeat == 1)
					{
						SetDialogue(npcLevelStruct2[i]);
						return;
					}
					if (npcLevelStruct2[i].Repeat == 0 && !GameGlobalData.NpcData.Contains(npcLevelStruct2[i].ID))
					{
						SetDialogue(npcLevelStruct2[i], notification.Body as Action);
						GameGlobalData.NpcData.SaveNpcLevelID(npcLevelStruct2[i].ID);
						return;
					}
				}
			}
			Action action = notification.Body as Action;
			action();
		}

		private void SetDialogue(NpcLevelStruct data, Action action)
		{
			Singleton<UIViewManager>.Instance.ShowView("DIALOGUE_VIEW");
			DialogueVO dialogueVO = new DialogueVO();
			dialogueVO.npcID = data.NpcID;
			dialogueVO.content = Singleton<LanguageManager>.Instance.GetLanguage().GetTxt(data.LanguageID);
			dialogueVO.type = (DialogueType)data.LocationType;
			dialogueVO.callback = action;
			SendNotification("SHOW_DIALOGUE", new DialogueVO[1]
			{
				dialogueVO
			});
		}

		private void SetDialogue(NpcLevelStruct data)
		{
			Singleton<UIViewManager>.Instance.ShowView("DIALOGUE_VIEW");
			DialogueVO dialogueVO = new DialogueVO();
			dialogueVO.npcID = data.NpcID;
			dialogueVO.content = Singleton<LanguageManager>.Instance.GetLanguage().GetTxt(data.LanguageID);
			dialogueVO.type = (DialogueType)data.LocationType;
			SendNotification("SHOW_DIALOGUE", new DialogueVO[1]
			{
				dialogueVO
			});
		}
	}
}
