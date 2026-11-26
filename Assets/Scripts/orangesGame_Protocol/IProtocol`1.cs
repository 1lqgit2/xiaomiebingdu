namespace orangesGame.Protocol
{
	public interface IProtocol<T>
	{
		void Request(T param);

		void Response();
	}
}
