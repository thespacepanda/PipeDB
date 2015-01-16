using System;

namespace Application {

	enum Command {
		Create,
		Read,
		Update,
		Delete,
		Quit,
		Nothing
	};

	static class Shell {

		public static void Repl(Table table) {
			while (true) {
				var command = Read();
				var result = Evaluate(command);
				Print(result);
				if (result == "quitting...") {
					break;
				}
			}
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

		private static Command Read() {
			DisplayPrompt();
			var command = Console.ReadLine();
			return Interpret(command);
		}

		private static Command Interpret(string command) {
			switch (command.Split()[0].ToLower()) {
				case "create":
					return Command.Create;
				case "read":
					return Command.Read;
				case "update":
					return Command.Update;
				case "delete":
					return Command.Delete;
				case "quit":
					return Command.Quit;
				default:
					return Command.Nothing;
			}
		}

		private static string Evaluate(Command command) {
			// TODO interact with Database
			switch (command) {
				case Command.Create:
					return "This will create a row.";
				case Command.Read:
					return "This will read rows from the database (query).";
				case Command.Update:
					return "This will update a row.";
				case Command.Delete:
					return "This will delete a row.";
				case Command.Quit:
					return "quitting...";
				case Command.Nothing:
					return "I don't understand that command; try Create, Read, Update, Delete, or Quit.";
				default:
					return "This should never happen.";
			}
		}

		private static void Print(string result) {
			Console.WriteLine(result);
		}
	}
}