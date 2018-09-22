using System;
using System.Threading;
using fastipc.Bus;
using fastipc.Message;
using ProtoBuf;
using Xunit;
using Xunit.Abstractions;

namespace fastipc.Tests
{
	public class NamedPipeListenerTests
	{
		private readonly ITestOutputHelper _logger;

		public NamedPipeListenerTests(ITestOutputHelper logger)
		{
			_logger = logger;
		}

		[Fact]
		private void can_exchange_messages()
		{
			var timeout = TimeSpan.FromSeconds(3);
			var processADone = new ManualResetEventSlim(false);
			var processBDone = new ManualResetEventSlim(false);

			// Process A Simulation
			new Thread(() =>
					{
						var pipeName = new SimpleStringPipeName(
							name: "UnitTest",
							side: Side.Out);
						var bus = new NamedPipeBus(pipeName: pipeName);
						var inHost = new TestHandler(
							handleStringContent : msg => {
								_logger.WriteLine($"[ProcessA] Received new message of type {msg.GetType().Name}: {msg.Content}");
							},
							handleTestMessage : msg => {
								_logger.WriteLine($"[ProcessA] Received new message of type {msg.GetType().Name}");
								processADone.Set();
							}
						);

						bus.Subscribe(inHost);
						bus.Publish(new StringContentMessage("Message from In"));
						bus.Publish(new TestMessage());
					})
					{ IsBackground = true }
				.Start();

			// Process B Simulation
			new Thread(() =>
					{
						var pipeName = new SimpleStringPipeName(name: "UnitTest");
						var bus = new NamedPipeBus(pipeName: pipeName);

						var outHost = new TestHandler(
							handleStringContent: msg => {
								_logger.WriteLine($"[ProcessB] Received new message of type {msg.GetType().Name}: {msg.Content}");
							},
							handleTestMessage: msg => {
								_logger.WriteLine($"[ProcessB] Received new message of type {msg.GetType().Name}");
								processBDone.Set();
							}
						);

						bus.Subscribe(outHost);
						bus.Publish(new StringContentMessage("Message from Out"));
						bus.Publish(new TestMessage());
					})
					{ IsBackground = true }
				.Start();

			processBDone.Wait(timeout: timeout);
			processADone.Wait(timeout: timeout);

			Assert.True(
				condition: processBDone.IsSet,
				userMessage: "Failed to receive message from 'ProcessA'"
			);

			Assert.True(
				condition: processADone.IsSet,
				userMessage: "Failed to receive message from 'ProcessB'"
			);
		}
	}

	[ProtoContract]
	public class TestMessage : Message.Message { }

	public class TestHandler : IHandleMessage {
		private readonly Action<StringContentMessage> _handleStringContent;
		private readonly Action<TestMessage> _handleTestMessage;

		public TestHandler(
		Action<StringContentMessage> handleStringContent,
		Action<TestMessage> handleTestMessage) {
			_handleStringContent = handleStringContent;
			_handleTestMessage = handleTestMessage;
		}

		public void Handle(Message.Message msg) {
			HandleInternal((dynamic)msg);
		}

		private void HandleInternal(StringContentMessage msg) {
			_handleStringContent?.Invoke(msg);
		}

		private void HandleInternal(TestMessage msg) {
			_handleTestMessage?.Invoke(msg);
		}
	}
}
