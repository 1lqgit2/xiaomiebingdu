using PureMVC.Interfaces;
using System.Collections.Generic;

namespace PureMVC.Patterns
{
	public class Mediator : Notifier, IMediator, INotifier
	{
		public const string NAME = "Mediator";

		protected string m_mediatorName;

		protected object m_viewComponent;

		public virtual string MediatorName => m_mediatorName;

		public virtual object ViewComponent
		{
			get
			{
				return m_viewComponent;
			}
			set
			{
				m_viewComponent = value;
			}
		}

		public Mediator()
			: this("Mediator", null)
		{
		}

		public Mediator(string mediatorName)
			: this(mediatorName, null)
		{
		}

		public Mediator(string mediatorName, object viewComponent)
		{
			m_mediatorName = ((mediatorName == null) ? "Mediator" : mediatorName);
			m_viewComponent = viewComponent;
		}

		public virtual IList<string> ListNotificationInterests()
		{
			return new List<string>();
		}

		public virtual void HandleNotification(INotification notification)
		{
		}

		public virtual void OnRegister()
		{
		}

		public virtual void OnRemove()
		{
		}
	}
}
