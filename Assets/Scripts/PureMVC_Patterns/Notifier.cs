using PureMVC.Interfaces;

namespace PureMVC.Patterns
{
	public class Notifier : INotifier
	{
		private IFacade m_facade = PureMVC.Patterns.Facade.Instance;

		protected IFacade Facade => m_facade;

		public virtual void SendNotification(string notificationName)
		{
			m_facade.SendNotification(notificationName);
		}

		public virtual void SendNotification(string notificationName, object body)
		{
			m_facade.SendNotification(notificationName, body);
		}

		public virtual void SendNotification(string notificationName, object body, string type)
		{
			m_facade.SendNotification(notificationName, body, type);
		}
	}
}
