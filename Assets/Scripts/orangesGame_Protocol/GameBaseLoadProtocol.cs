using System;
using UnityEngine;

namespace orangesGame.Protocol
{
	public class GameBaseLoadProtocol : IProtocol<GameBaseLoadVO>
	{
		public void Request(GameBaseLoadVO param)
		{
			UnityEngine.Debug.Log("@@@@");
		}

		public void Response()
		{
			throw new NotImplementedException();
		}
	}
}
