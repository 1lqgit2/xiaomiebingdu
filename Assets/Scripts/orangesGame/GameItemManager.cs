using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	public class GameItemManager : Singleton<GameItemManager>
	{
		private Dictionary<string, List<GameItem>> _poolDic;

		private Transform _parent;

		private GameItemManager()
		{
			_poolDic = new Dictionary<string, List<GameItem>>();
			_parent = Singleton<UIViewManager>.Instance.GetLayerTran(40);
		}

		public T Spawn<T>(string key, string resPath) where T : GameItem, new()
		{
			return Spawn<T>(key, resPath, _parent);
		}

		public T Spawn<T>(string key, string resPath, Transform parent) where T : GameItem, new()
		{
			if (!_poolDic.ContainsKey(key))
			{
				_poolDic.Add(key, new List<GameItem>());
			}
			List<GameItem> list = _poolDic[key];
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].IsDirty)
				{
					list[i].SetParent(parent);
					return list[i] as T;
				}
			}
			T val = Object.Instantiate(Resources.Load<T>(resPath));
			val.SetParent(parent);
			list.Add(val);
			return val;
		}

		public void Clean(string key)
		{
			if (!_poolDic.ContainsKey(key))
			{
				_poolDic.Add(key, new List<GameItem>());
			}
			List<GameItem> list = _poolDic[key];
			for (int i = 0; i < list.Count; i++)
			{
				if (!list[i].IsDirty)
				{
					list[i].Clean();
				}
			}
		}

		public void Destroy(string key)
		{
			if (_poolDic.ContainsKey(key))
			{
				List<GameItem> list = _poolDic[key];
				for (int i = 0; i < list.Count; i++)
				{
					list[i].Dispose();
				}
				list.Clear();
			}
		}

		public void CleanAllItems()
		{
			string[] iTEM_LIST = ResPath.ITEM_LIST;
			foreach (string key in iTEM_LIST)
			{
				Clean(key);
			}
		}

		public void PauseAllItems()
		{
			string[] iTEM_LIST = ResPath.ITEM_LIST;
			foreach (string key in iTEM_LIST)
			{
				Pause(key);
			}
		}

		public void ContinueAllItems()
		{
			string[] iTEM_LIST = ResPath.ITEM_LIST;
			foreach (string key in iTEM_LIST)
			{
				Continue(key);
			}
		}

		public void Pause(string key)
		{
			if (!_poolDic.ContainsKey(key))
			{
				_poolDic.Add(key, new List<GameItem>());
			}
			List<GameItem> list = _poolDic[key];
			for (int i = 0; i < list.Count; i++)
			{
				if (!list[i].IsDirty)
				{
					list[i].Pause();
				}
			}
		}

		public void Continue(string key)
		{
			if (!_poolDic.ContainsKey(key))
			{
				_poolDic.Add(key, new List<GameItem>());
			}
			List<GameItem> list = _poolDic[key];
			for (int i = 0; i < list.Count; i++)
			{
				if (!list[i].IsDirty)
				{
					list[i].Continue();
				}
			}
		}
	}
}
