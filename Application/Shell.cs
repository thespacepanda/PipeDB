using System;

namespace Application {
	static class Shell {

		public static void Repl(Table table) {
			while (true) {
				DisplayPrompt();
				var command = Console.ReadLine();
				if (command == "quit") {
					break;
				}
				else {
					Console.WriteLine("command is {0}", command);
				}
			}
			Console.WriteLine("quitting...");
		}

		private static void DisplayPrompt() {
			var prompt1 = "{ ~ }";
			var prompt2 = "  » ";
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.Write(prompt1);
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write(prompt2);
			Console.ResetColor();
		}
	}
}
