using System;

namespace orangesGame
{
	public class Singleton<T> where T : class
	{
		private static readonly T instance;

		public static T Instance => instance;

		static Singleton()
		{
			instance = (T)Activator.CreateInstance(typeof(T), nonPublic: true);
		}
	}
}
