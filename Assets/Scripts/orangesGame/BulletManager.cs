using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	public class BulletManager : Singleton<BulletManager>
	{
		private Dictionary<BulletType, List<Bullet>> _poolDic;

		private Transform _parent;

		private BulletManager()
		{
			_poolDic = new Dictionary<BulletType, List<Bullet>>();
			_parent = Singleton<UIViewManager>.Instance.GetLayerTran(40);
		}

		public T Spawn<T>(BulletType key, string resPath) where T : Bullet, new()
		{
			if (!_poolDic.ContainsKey(key))
			{
				_poolDic.Add(key, new List<Bullet>());
			}
			List<Bullet> list = _poolDic[key];
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].isDirty)
				{
					list[i].Init();
					return list[i] as T;
				}
			}
			T val = Object.Instantiate(Resources.Load<T>(resPath));
			val.Init();
			val.SetParent(_parent);
			list.Add(val);
			return val;
		}

		public void Clean()
		{
			foreach (List<Bullet> value in _poolDic.Values)
			{
				for (int i = 0; i < value.Count; i++)
				{
					if (!value[i].isDirty)
					{
						value[i].Clean();
					}
				}
			}
		}

		public void Destroy()
		{
			foreach (List<Bullet> value in _poolDic.Values)
			{
				for (int i = 0; i < value.Count; i++)
				{
					UnityEngine.Object.Destroy(value[i].gameObject);
				}
				value.Clear();
			}
		}

		public void Clean(BulletType type)
		{
			if (!_poolDic.ContainsKey(type))
			{
				return;
			}
			List<Bullet> list = _poolDic[type];
			for (int i = 0; i < list.Count; i++)
			{
				if (!list[i].isDirty)
				{
					list[i].Clean();
				}
			}
		}
	}
}
