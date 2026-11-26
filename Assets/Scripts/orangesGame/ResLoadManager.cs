using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	public class ResLoadManager : Singleton<ResLoadManager>
	{
		private Dictionary<string, GameObject> _pool;

		private GameObject _preResContainer;

		private ResLoadManager()
		{
			_pool = new Dictionary<string, GameObject>();
		}

		public void PreLoad(params string[] resPaths)
		{
			if (_preResContainer == null)
			{
				_preResContainer = new GameObject("PreResContainer");
				_preResContainer.SetActive(value: false);
			}
			for (int i = 0; i < resPaths.Length; i++)
			{
				if (!_pool.ContainsKey(resPaths[i]))
				{
					GameObject value = UnityEngine.Object.Instantiate(Resources.Load(resPaths[i]), _preResContainer.transform) as GameObject;
					_pool.Add(resPaths[i], value);
				}
				else
				{
					UnityEngine.Debug.LogError("PreLoad Key is exist:" + resPaths[i]);
				}
			}
		}

		public T ResLoad<T>(string resPath, bool isPreLoad) where T : MonoBehaviour
		{
			if (isPreLoad && _pool.ContainsKey(resPath))
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(_pool[resPath]);
				T component = gameObject.GetComponent<T>();
				if ((Object)component != (Object)null)
				{
					return component;
				}
			}
			return UnityEngine.Object.Instantiate(Resources.Load<T>(resPath));
		}

		public GameObject ResLoad(string resPath, bool isPreLoad)
		{
			if (isPreLoad && _pool.ContainsKey(resPath))
			{
				return UnityEngine.Object.Instantiate(_pool[resPath]);
			}
			return UnityEngine.Object.Instantiate(Resources.Load(resPath)) as GameObject;
		}
	}
}
