using System;

namespace Application {
	static class Shell {

		public static void Repl(Table table) {
			Console.WriteLine("REPL");
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
