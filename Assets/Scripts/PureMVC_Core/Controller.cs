using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;

namespace PureMVC.Core
{
	public class Controller : IController
	{
		protected IView m_view;

		protected IDictionary<string, Type> m_commandMap;

		protected static volatile IController m_instance;

		protected readonly object m_syncRoot = new object();

		protected static readonly object m_staticSyncRoot;

		public static IController Instance
		{
			get
			{
				if (m_instance == null)
				{
					lock (m_staticSyncRoot)
					{
						if (m_instance == null)
						{
							m_instance = new Controller();
						}
					}
				}
				return m_instance;
			}
		}

		protected Controller()
		{
			m_commandMap = new Dictionary<string, Type>();
			InitializeController();
		}

		static Controller()
		{
			m_staticSyncRoot = new object();
		}

		public virtual void ExecuteCommand(INotification note)
		{
			Type type = null;
			lock (m_syncRoot)
			{
				if (!m_commandMap.ContainsKey(note.Name))
				{
					return;
				}
				type = m_commandMap[note.Name];
			}
			object obj = Activator.CreateInstance(type);
			if (obj is ICommand)
			{
				((ICommand)obj).Execute(note);
			}
		}

		public virtual void RegisterCommand(string notificationName, Type commandType)
		{
			lock (m_syncRoot)
			{
				if (!m_commandMap.ContainsKey(notificationName))
				{
					m_view.RegisterObserver(notificationName, new Observer("executeCommand", this));
				}
				m_commandMap[notificationName] = commandType;
			}
		}

		public virtual bool HasCommand(string notificationName)
		{
			lock (m_syncRoot)
			{
				return m_commandMap.ContainsKey(notificationName);
			}
		}

		public virtual void RemoveCommand(string notificationName)
		{
			lock (m_syncRoot)
			{
				if (m_commandMap.ContainsKey(notificationName))
				{
					m_view.RemoveObserver(notificationName, this);
					m_commandMap.Remove(notificationName);
				}
			}
		}

		protected virtual void InitializeController()
		{
			m_view = View.Instance;
		}
	}
}
