using System;

namespace fastipc.Bus {
	public interface IBus : IDisposable
	{
		void Publish<T>(T msg) where T: Message.Message;

		void Subscribe(IHandleMessage handler);
	}
}
