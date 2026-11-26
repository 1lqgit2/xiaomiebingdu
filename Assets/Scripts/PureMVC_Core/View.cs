using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections.Generic;

namespace PureMVC.Core
{
	public class View : IView
	{
		protected IDictionary<string, IMediator> m_mediatorMap;

		protected IDictionary<string, IList<IObserver>> m_observerMap;

		protected static volatile IView m_instance;

		protected readonly object m_syncRoot = new object();

		protected static readonly object m_staticSyncRoot;

		public static IView Instance
		{
			get
			{
				if (m_instance == null)
				{
					lock (m_staticSyncRoot)
					{
						if (m_instance == null)
						{
							m_instance = new View();
						}
					}
				}
				return m_instance;
			}
		}

		protected View()
		{
			m_mediatorMap = new Dictionary<string, IMediator>();
			m_observerMap = new Dictionary<string, IList<IObserver>>();
			InitializeView();
		}

		static View()
		{
			m_staticSyncRoot = new object();
		}

		public virtual void RegisterObserver(string notificationName, IObserver observer)
		{
			lock (m_syncRoot)
			{
				if (!m_observerMap.ContainsKey(notificationName))
				{
					m_observerMap[notificationName] = new List<IObserver>();
				}
				m_observerMap[notificationName].Add(observer);
			}
		}

		public virtual void NotifyObservers(INotification notification)
		{
			IList<IObserver> list = null;
			lock (m_syncRoot)
			{
				if (m_observerMap.ContainsKey(notification.Name))
				{
					IList<IObserver> collection = m_observerMap[notification.Name];
					list = new List<IObserver>(collection);
				}
			}
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					IObserver observer = list[i];
					observer.NotifyObserver(notification);
				}
			}
		}

		public virtual void RemoveObserver(string notificationName, object notifyContext)
		{
			lock (m_syncRoot)
			{
				if (m_observerMap.ContainsKey(notificationName))
				{
					IList<IObserver> list = m_observerMap[notificationName];
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].CompareNotifyContext(notifyContext))
						{
							list.RemoveAt(i);
							break;
						}
					}
					if (list.Count == 0)
					{
						m_observerMap.Remove(notificationName);
					}
				}
			}
		}

		public virtual void RegisterMediator(IMediator mediator)
		{
			lock (m_syncRoot)
			{
				if (m_mediatorMap.ContainsKey(mediator.MediatorName))
				{
					return;
				}
				m_mediatorMap[mediator.MediatorName] = mediator;
				IList<string> list = mediator.ListNotificationInterests();
				if (list.Count > 0)
				{
					IObserver observer = new Observer("handleNotification", mediator);
					for (int i = 0; i < list.Count; i++)
					{
						RegisterObserver(list[i].ToString(), observer);
					}
				}
			}
			mediator.OnRegister();
		}

		public virtual IMediator RetrieveMediator(string mediatorName)
		{
			lock (m_syncRoot)
			{
				if (!m_mediatorMap.ContainsKey(mediatorName))
				{
					return null;
				}
				return m_mediatorMap[mediatorName];
			}
		}

		public virtual IMediator RemoveMediator(string mediatorName)
		{
			IMediator mediator = null;
			lock (m_syncRoot)
			{
				if (!m_mediatorMap.ContainsKey(mediatorName))
				{
					return null;
				}
				mediator = m_mediatorMap[mediatorName];
				IList<string> list = mediator.ListNotificationInterests();
				for (int i = 0; i < list.Count; i++)
				{
					RemoveObserver(list[i], mediator);
				}
				m_mediatorMap.Remove(mediatorName);
			}
			mediator?.OnRemove();
			return mediator;
		}

		public virtual bool HasMediator(string mediatorName)
		{
			lock (m_syncRoot)
			{
				return m_mediatorMap.ContainsKey(mediatorName);
			}
		}

		protected virtual void InitializeView()
		{
		}
	}
}
