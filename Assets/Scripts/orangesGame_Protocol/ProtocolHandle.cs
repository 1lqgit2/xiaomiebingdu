using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace orangesGame.Protocol
{
	public class ProtocolHandle : Singleton<ProtocolHandle>
	{
		private ArrayList aaa = new ArrayList();

		private Dictionary<uint, object> _dic = new Dictionary<uint, object>();

		public void Register<T, K>(uint key) where T : IProtocol<K>, new()
		{
			if (!_dic.ContainsKey(key))
			{
				_dic.Add(key, new T());
			}
		}

		public void Request<T>(uint key, T param)
		{
			if (_dic.ContainsKey(key))
			{
				IProtocol<T> protocol = _dic[key] as IProtocol<T>;
				if (protocol != null)
				{
					protocol.Request(param);
				}
				else
				{
					UnityEngine.Debug.LogError($"Key:{key} does not match T:{param.GetType()}");
				}
			}
		}
	}
}
