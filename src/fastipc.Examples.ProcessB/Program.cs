using System;
using fastipc.Bus;
using fastIpc.Examples.Messages;

namespace fastipc.Examples.ProcessB {
	static class Program {
		static void Main(string[] args) {
			var pipeName = new SimpleStringPipeName(
				name: "Example", 
				side: Side.Out);
			var bus = new NamedPipeBus(pipeName: pipeName);
			new ProcessBHost(bus);

			Console.WriteLine($"Process B Host Runing");
			Console.WriteLine("type exit to close");
			Console.WriteLine("type help for command list");
			var exit = false;
			do {
				var cmd = Console.ReadLine();
				if (string.IsNullOrWhiteSpace(cmd)) {
					continue;
				}

				switch (cmd.ToLower()) {
					case "exit":
						exit = true;
						break;
					case "ping":
						bus.Publish(new Ping());
						break;
					case "help":
					case "?":
						Console.WriteLine("exit: exit program");
						Console.WriteLine("ping: publish ping message");
						Console.WriteLine("cls: Clear Screen");
						break;
					case "cls":
						Console.Clear();
						break;
				}
			} while (!exit);

			bus.Dispose();
		}
	}
}
