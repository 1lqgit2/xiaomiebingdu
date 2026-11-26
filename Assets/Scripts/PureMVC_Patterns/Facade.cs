using PureMVC.Core;
using PureMVC.Interfaces;
using System;

namespace PureMVC.Patterns
{
	public class Facade : IFacade, INotifier
	{
		protected IController m_controller;

		protected IModel m_model;

		protected IView m_view;

		protected static volatile IFacade m_instance;

		protected static readonly object m_staticSyncRoot;

		public static IFacade Instance
		{
			get
			{
				if (m_instance == null)
				{
					lock (m_staticSyncRoot)
					{
						if (m_instance == null)
						{
							m_instance = new Facade();
						}
					}
				}
				return m_instance;
			}
		}

		protected Facade()
		{
			InitializeFacade();
		}

		static Facade()
		{
			m_staticSyncRoot = new object();
		}

		public virtual void RegisterProxy(IProxy proxy)
		{
			m_model.RegisterProxy(proxy);
		}

		public virtual IProxy RetrieveProxy(string proxyName)
		{
			return m_model.RetrieveProxy(proxyName);
		}

		public virtual IProxy RemoveProxy(string proxyName)
		{
			return m_model.RemoveProxy(proxyName);
		}

		public virtual bool HasProxy(string proxyName)
		{
			return m_model.HasProxy(proxyName);
		}

		public virtual void RegisterCommand(string notificationName, Type commandType)
		{
			m_controller.RegisterCommand(notificationName, commandType);
		}

		public virtual void RemoveCommand(string notificationName)
		{
			m_controller.RemoveCommand(notificationName);
		}

		public virtual bool HasCommand(string notificationName)
		{
			return m_controller.HasCommand(notificationName);
		}

		public virtual void RegisterMediator(IMediator mediator)
		{
			m_view.RegisterMediator(mediator);
		}

		public virtual IMediator RetrieveMediator(string mediatorName)
		{
			return m_view.RetrieveMediator(mediatorName);
		}

		public virtual IMediator RemoveMediator(string mediatorName)
		{
			return m_view.RemoveMediator(mediatorName);
		}

		public virtual bool HasMediator(string mediatorName)
		{
			return m_view.HasMediator(mediatorName);
		}

		public virtual void NotifyObservers(INotification notification)
		{
			m_view.NotifyObservers(notification);
		}

		public virtual void SendNotification(string notificationName)
		{
			NotifyObservers(new Notification(notificationName));
		}

		public virtual void SendNotification(string notificationName, object body)
		{
			NotifyObservers(new Notification(notificationName, body));
		}

		public virtual void SendNotification(string notificationName, object body, string type)
		{
			NotifyObservers(new Notification(notificationName, body, type));
		}

		protected virtual void InitializeFacade()
		{
			InitializeModel();
			InitializeController();
			InitializeView();
		}

		protected virtual void InitializeController()
		{
			if (m_controller == null)
			{
				m_controller = Controller.Instance;
			}
		}

		protected virtual void InitializeModel()
		{
			if (m_model == null)
			{
				m_model = Model.Instance;
			}
		}

		protected virtual void InitializeView()
		{
			if (m_view == null)
			{
				m_view = View.Instance;
			}
		}
	}
}
