using LitJson;
using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	public class NpcPrefsData
	{
		private List<int> _npcLevelIDList;

		public NpcPrefsData()
		{
			_npcLevelIDList = new List<int>();
		}

		public void LoadNpcLevelID()
		{
			string @string = PlayerPrefs.GetString("NpcLevelID", string.Empty);
			_npcLevelIDList = new List<int>();
			if (!string.IsNullOrEmpty(@string))
			{
				_npcLevelIDList = JsonMapper.ToObject<List<int>>(@string);
			}
		}

		public void SaveNpcLevelID(int id)
		{
			if (!_npcLevelIDList.Contains(id))
			{
				_npcLevelIDList.Add(id);
				string value = JsonMapper.ToJson(_npcLevelIDList);
				PlayerPrefs.SetString("NpcLevelID", value);
			}
		}

		public bool Contains(int id)
		{
			return _npcLevelIDList.Contains(id);
		}
	}
}
