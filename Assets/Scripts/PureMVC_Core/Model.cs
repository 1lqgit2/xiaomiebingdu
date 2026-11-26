using PureMVC.Interfaces;
using System.Collections.Generic;

namespace PureMVC.Core
{
	public class Model : IModel
	{
		protected IDictionary<string, IProxy> m_proxyMap;

		protected static volatile IModel m_instance;

		protected readonly object m_syncRoot = new object();

		protected static readonly object m_staticSyncRoot;

		public static IModel Instance
		{
			get
			{
				if (m_instance == null)
				{
					lock (m_staticSyncRoot)
					{
						if (m_instance == null)
						{
							m_instance = new Model();
						}
					}
				}
				return m_instance;
			}
		}

		protected Model()
		{
			m_proxyMap = new Dictionary<string, IProxy>();
			InitializeModel();
		}

		static Model()
		{
			m_staticSyncRoot = new object();
		}

		public virtual void RegisterProxy(IProxy proxy)
		{
			lock (m_syncRoot)
			{
				m_proxyMap[proxy.ProxyName] = proxy;
			}
			proxy.OnRegister();
		}

		public virtual IProxy RetrieveProxy(string proxyName)
		{
			lock (m_syncRoot)
			{
				if (!m_proxyMap.ContainsKey(proxyName))
				{
					return null;
				}
				return m_proxyMap[proxyName];
			}
		}

		public virtual bool HasProxy(string proxyName)
		{
			lock (m_syncRoot)
			{
				return m_proxyMap.ContainsKey(proxyName);
			}
		}

		public virtual IProxy RemoveProxy(string proxyName)
		{
			IProxy proxy = null;
			lock (m_syncRoot)
			{
				if (m_proxyMap.ContainsKey(proxyName))
				{
					proxy = RetrieveProxy(proxyName);
					m_proxyMap.Remove(proxyName);
				}
			}
			proxy?.OnRemove();
			return proxy;
		}

		protected virtual void InitializeModel()
		{
		}
	}
}
