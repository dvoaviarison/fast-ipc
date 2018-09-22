using System;
using fastipc.Bus;
using fastIpc.Example.Messages;

namespace fastipc.Examples.InProcess
{
	public class ProcessBHost : IHandleMessage {
		private readonly IBus _bus;

		public ProcessBHost(IBus bus) {
			_bus = bus;
			_bus.Subscribe(this);
		}

		public void Handle(Message.Message msg) {
			HandleInternal((dynamic)msg);
		}

		private void HandleInternal(Ping msg) {
			Console.WriteLine("Received a ping, sending a pong");
			_bus.Publish(new Pong());
		}

		private void HandleInternal(Pong msg) {
			Console.WriteLine("Received a pong");
		}
	}
}
