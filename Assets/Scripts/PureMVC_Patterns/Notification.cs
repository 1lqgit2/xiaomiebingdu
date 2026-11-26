using PureMVC.Interfaces;

namespace PureMVC.Patterns
{
	public class Notification : INotification
	{
		private string m_name;

		private string m_type;

		private object m_body;

		public virtual string Name => m_name;

		public virtual object Body
		{
			get
			{
				return m_body;
			}
			set
			{
				m_body = value;
			}
		}

		public virtual string Type
		{
			get
			{
				return m_type;
			}
			set
			{
				m_type = value;
			}
		}

		public Notification(string name)
			: this(name, null, null)
		{
		}

		public Notification(string name, object body)
			: this(name, body, null)
		{
		}

		public Notification(string name, object body, string type)
		{
			m_name = name;
			m_body = body;
			m_type = type;
		}

		public override string ToString()
		{
			string str = "Notification Name: " + Name;
			str = str + "\nBody:" + ((Body != null) ? Body.ToString() : "null");
			return str + "\nType:" + ((Type != null) ? Type : "null");
		}
	}
}
