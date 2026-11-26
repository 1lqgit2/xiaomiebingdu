using PureMVC.Interfaces;
using System;
using System.Reflection;

namespace PureMVC.Patterns
{
	public class Observer : IObserver
	{
		private string m_notifyMethod;

		private object m_notifyContext;

		protected readonly object m_syncRoot = new object();

		public string NotifyMethod
		{
			private get
			{
				return m_notifyMethod;
			}
			set
			{
				m_notifyMethod = value;
			}
		}

		public object NotifyContext
		{
			private get
			{
				return m_notifyContext;
			}
			set
			{
				m_notifyContext = value;
			}
		}

		public Observer(string notifyMethod, object notifyContext)
		{
			m_notifyMethod = notifyMethod;
			m_notifyContext = notifyContext;
		}

		public virtual void NotifyObserver(INotification notification)
		{
			object notifyContext;
			string notifyMethod;
			lock (m_syncRoot)
			{
				notifyContext = NotifyContext;
				notifyMethod = NotifyMethod;
			}
			Type type = notifyContext.GetType();
			BindingFlags bindingAttr = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public;
			MethodInfo method = type.GetMethod(notifyMethod, bindingAttr);
			method.Invoke(notifyContext, new object[1]
			{
				notification
			});
		}

		public virtual bool CompareNotifyContext(object obj)
		{
			lock (m_syncRoot)
			{
				return NotifyContext.Equals(obj);
			}
		}
	}
}
