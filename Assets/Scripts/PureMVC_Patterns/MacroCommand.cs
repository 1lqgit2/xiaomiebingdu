using PureMVC.Interfaces;
using System;
using System.Collections.Generic;

namespace PureMVC.Patterns
{
	public class MacroCommand : Notifier, ICommand, INotifier
	{
		private IList<Type> m_subCommands;

		public MacroCommand()
		{
			m_subCommands = new List<Type>();
			InitializeMacroCommand();
		}

		public virtual void Execute(INotification notification)
		{
			while (m_subCommands.Count > 0)
			{
				Type type = m_subCommands[0];
				object obj = Activator.CreateInstance(type);
				if (obj is ICommand)
				{
					((ICommand)obj).Execute(notification);
				}
				m_subCommands.RemoveAt(0);
			}
		}

		protected virtual void InitializeMacroCommand()
		{
		}

		protected void AddSubCommand(Type commandType)
		{
			m_subCommands.Add(commandType);
		}
	}
}
