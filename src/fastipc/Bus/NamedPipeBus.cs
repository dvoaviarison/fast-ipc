using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using fastipc.Formatter;
using fastipc.Message;

namespace fastipc.Bus {
	public delegate void MessageReceivedHandler(Message.Message msg);

	public enum Side
	{
		In,
		Out
	}

	public class NamedPipeBus : IBus
	{
		private readonly IPipeName _pipeName;
		private readonly IMessageFormatter _formatter;
		private readonly NamedPipeServerStream _server;
		private readonly HashSet<Guid> _ignoreMe;

		private event MessageReceivedHandler MessageReceived;

		public NamedPipeBus(IPipeName pipeName) {
			_pipeName = pipeName;
			_ignoreMe = new HashSet<Guid>();
			_formatter = InitializeSerializer();
			_server = new NamedPipeServerStream(
				_pipeName.Read,
				PipeDirection.InOut,
				1,
				PipeTransmissionMode.Byte,
				PipeOptions.Asynchronous);

			Debug.WriteLine($"Listening on pipe {_pipeName.Read}...");
			_server.BeginWaitForConnection(WaitForConnectionCallBack, null);
		}

		public void Publish<T>(T msg) where T : Message.Message
		{
			if (_ignoreMe.Contains(msg.MsgId))
			{
				_ignoreMe.Remove(msg.MsgId);
				return;
			}
			var client = new NamedPipeClientStream(
				".",
				_pipeName.Write,
				PipeDirection.InOut,
				PipeOptions.None,
				System.Security.Principal.TokenImpersonationLevel.None);
			client.Connect();
			_formatter.Serialize(client, msg);
			Debug.WriteLine($"[ => ] New message of type {msg.GetType().Name} sent to pipe {_pipeName.Write}");
			client.Dispose();
		}

		public void Subscribe(IHandleMessage handler) {
			MessageReceived += handler.Handle;
		}

		private void WaitForConnectionCallBack(IAsyncResult result)
		{
			try
			{
				_server.EndWaitForConnection(result);

				var message = _formatter.Deserialize(_server);
				Debug.WriteLine($"[ <= ] New message of type {message.GetType().Name} received on pipe {_pipeName.Read}");
				_ignoreMe.Add(message.MsgId);
				OnMessageReceived(message);
				_server.Disconnect();

				_server.BeginWaitForConnection(WaitForConnectionCallBack, null);
			}
			catch
			{
				return;
			}
		}

		private IMessageFormatter InitializeSerializer()
		{
			var formatter = new ProtobufNetFormatter();
			var stream = new MemoryStream();
			formatter.Serialize(stream, new StringContentMessage(content: "Content"));

			stream.Position = 0;
			formatter.Deserialize(stream);

			return formatter;
		}

		private void OnMessageReceived(Message.Message msg)
		{
			var handler = MessageReceived;
			handler?.Invoke(msg);
		}

		public void Dispose()
		{
			_server?.Dispose();
		}
	}
}
