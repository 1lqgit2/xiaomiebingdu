using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	public class EffectManager : Singleton<EffectManager>
	{
		private Dictionary<string, List<EffectBehaviour>> _poolDic;

		private Dictionary<uint, IEffectAction> _effDic;

		private EffectManager()
		{
			_poolDic = new Dictionary<string, List<EffectBehaviour>>();
			_effDic = new Dictionary<uint, IEffectAction>();
		}

		public void AddEffAction(uint key, IEffectAction action)
		{
			if (!_effDic.ContainsKey(key))
			{
				_effDic.Add(key, action);
			}
		}

		public IEffectAction GetEffAction(uint key)
		{
			if (_effDic.ContainsKey(key))
			{
				return _effDic[key];
			}
			return null;
		}

		public void Add(string order, EffectBehaviour effect)
		{
			if (_poolDic.ContainsKey(order))
			{
				_poolDic[order].Add(effect);
				return;
			}
			List<EffectBehaviour> list = new List<EffectBehaviour>();
			list.Add(effect);
			_poolDic.Add(order, list);
		}

		public void Remove(string order)
		{
			if (_poolDic.ContainsKey(order))
			{
				_poolDic[order].Clear();
				_poolDic.Remove(order);
			}
		}

		public void Pause(string order)
		{
			if (!_poolDic.ContainsKey(order))
			{
				return;
			}
			List<EffectBehaviour> list = _poolDic[order];
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].gameObject.activeSelf)
				{
					list[i].Pause();
				}
			}
		}

		public void Play(string order)
		{
			if (!_poolDic.ContainsKey(order))
			{
				return;
			}
			List<EffectBehaviour> list = _poolDic[order];
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].gameObject.activeSelf)
				{
					list[i].Play();
				}
			}
		}

		public void Stop(string order)
		{
			if (!_poolDic.ContainsKey(order))
			{
				return;
			}
			List<EffectBehaviour> list = _poolDic[order];
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].gameObject.activeSelf)
				{
					list[i].Stop();
				}
			}
		}

		public EffectBehaviour GetEffect(string order)
		{
			if (_poolDic.ContainsKey(order))
			{
				List<EffectBehaviour> list = _poolDic[order];
				for (int i = 0; i < list.Count; i++)
				{
					if (!list[i].gameObject.activeSelf)
					{
						return list[i];
					}
				}
				return null;
			}
			return null;
		}

		public void Clean(string order)
		{
			if (!_poolDic.ContainsKey(order))
			{
				return;
			}
			List<EffectBehaviour> list = _poolDic[order];
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].gameObject.activeSelf)
				{
					list[i].Clean();
				}
			}
		}

		public void Destroy(string[] orders)
		{
			for (int i = 0; i < orders.Length; i++)
			{
				if (_poolDic.ContainsKey(orders[i]))
				{
					List<EffectBehaviour> list = _poolDic[orders[i]];
					for (int j = 0; j < list.Count; j++)
					{
						UnityEngine.Object.Destroy(list[j].gameObject);
					}
					list.Clear();
				}
			}
		}
	}
}
