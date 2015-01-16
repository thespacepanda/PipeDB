using System;

namespace Application {

	/// <summary>
	/// Represents the command types a user could enter. 
	/// </summary>
	enum Command {
		Create,
		Read,
		Update,
		Delete,
		Quit,
		Nothing
	};

	/// <summary>
	/// Interprets and executes CRUD commands on the given table.
	/// </summary>
	class Shell {

		/// <summary>
		/// The Database the user will be interacting with.
		/// </summary>
		private Table Database;

		public Shell(Table table) {
			this.Database = table;
		}

		/// <summary>
		/// Main entry point for the Read, Eval, Print, Loop.
		/// </summary>
		/// <param name="table"></param>
		public void Repl() {
			while (true) {
				var command = Read();
				var result = Evaluate(command);
				Print(result);
				if (result == "quitting...") {
					break;
				}
			}
		}

		/// <summary>
		/// Helper function to show the fancy prompt to the screen.
		/// </summary>
		private static void DisplayPrompt() {
			var prompt1 = "{ ~ }";
			var prompt2 = "  » ";
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.Write(prompt1);
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write(prompt2);
			Console.ResetColor();
		}

		/// <summary>
		/// Reads a string from the user and interprets it as a Command.
		/// </summary>
		/// <returns></returns>
		private static Command Read() {
			DisplayPrompt();
			var command = Console.ReadLine();
			return Interpret(command);
		}

		/// <summary>
		/// Maps strings to Commands (matches on first word).
		/// Returns Command.Nothing if it cannot find a match.
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
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

		/// <summary>
		/// Executes supplied command on the shell's table.
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
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

		/// <summary>
		/// Prints the output from Evaluate to the console.
		/// </summary>
		/// <param name="result"></param>
		private static void Print(string result) {
			Console.WriteLine(result);
		}
	}
}