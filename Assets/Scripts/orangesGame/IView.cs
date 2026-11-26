using System;

namespace orangesGame
{
	public interface IView
	{
		void OnInit();

		void OnLoad();

		void OnShow(Action callback);

		void OnHide(Action callback);

		void OnDestory();

		void OnRegister();

		void OnRemove();
	}
}
