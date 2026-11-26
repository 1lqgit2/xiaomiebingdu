using System;
using System.Collections.Generic;

namespace Waiting.UGUI.Collections
{
	internal class Pool<T> where T : new()
	{
		private readonly Stack<T> _stack = new Stack<T>();

		private readonly Action<T> _actionOnGet;

		private readonly Action<T> _actionOnRecycle;

		public int count
		{
			get;
			private set;
		}

		public int activeCount => count - inactiveCount;

		public int inactiveCount => _stack.Count;

		public Pool(Action<T> actionOnGet, Action<T> actionOnRecycle)
		{
			_actionOnGet = actionOnGet;
			_actionOnRecycle = actionOnRecycle;
		}

		public T Get()
		{
			T val;
			if (_stack.Count == 0)
			{
				val = new T();
				count++;
			}
			else
			{
				val = _stack.Pop();
			}
			if (_actionOnGet != null)
			{
				_actionOnGet(val);
			}
			return val;
		}

		public void Recycle(T element)
		{
			if (_stack.Count > 0 && object.ReferenceEquals(_stack.Peek(), element))
			{
				throw new Exception("Internal error. Trying to destroy object that is already released to pool.");
			}
			if (_actionOnRecycle != null)
			{
				_actionOnRecycle(element);
			}
			_stack.Push(element);
		}

		public void Clear()
		{
			_stack.Clear();
			count = 0;
		}
	}
}
