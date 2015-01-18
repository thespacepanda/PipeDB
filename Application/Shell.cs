using System;
using System.Collections.Generic;
using System.Linq;

namespace Application {

	/// <summary>
	/// Interprets and executes CRUD commands on the given table.
	/// </summary>
	class Shell {

		/// <summary>
		/// The Database the user will be interacting with.
		/// </summary>
		private Table Database;

		private Parser Parse;

		public Shell(Table table) {
			this.Database = table;
			this.Parse = new Parser(this.Database.Headers);
		}


		/// <summary>
		/// Main entry point for the Read, Eval, Print, Loop.
		/// </summary>
		/// <param name="table"></param>
		public void Repl() {
			var result = string.Empty;
			do {
				var command = Read();
				result = Evaluate(command);
				Print(result);
			} while (!(result == "quitting..."));
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
		private static string Read(string pushed = "") {
			DisplayPrompt();
			var command = Console.ReadLine();
			return pushed + command;
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
		private string Evaluate(string command) {
			var verb = Interpret(command);
			var args = command.ToLower().Split().Skip(1);
			if (args.Count() != 0) {
				if (args.First() == "/?") {
					HelpDocs.DisplayReplHelp(verb);
					return String.Empty;
				}
			}
			switch (verb) {
				case Command.Create:
					try {
						var newRow = this.Parse.Create(args);
						if (this.Database.Create(newRow)) {
							var stringOfRow = String.Join(" | " , newRow.ToArray());
							return String.Format("Row {0} added" , stringOfRow);
						}
						// This should be handled by the Argument parser, but just in case
						return "Rows must provide values for every column in the header.";
					}
					catch (ArgumentException) {
						return String.Empty;
					}
				case Command.Read:
					try {
						var columnAndQuery = this.Parse.Read(args);
						var column = columnAndQuery.Item1;
						var query = columnAndQuery.Item2;
						var rowsThatMatch = this.Database.Read(column , query);
						var rowString = String.Empty;
						foreach (List<string> row in rowsThatMatch) {
							rowString += String.Join(" | " , row.ToArray());
							rowString += "\n";
						}
						return rowString;
					}
					catch (ArgumentException) {
						return String.Empty;
					}
				case Command.Update:
					try {
						var diffAndQuery = this.Parse.Update(args);
						var diff = diffAndQuery.Item1;
						var query = diffAndQuery.Item2;
						this.Database.Update(diff , query);
						return "Updated Database";
					}
					catch (ArgumentException) {
						return String.Empty;
					}
				case Command.Delete:
					try {
						var query = this.Parse.Delete(args);
						this.Database.Delete(query);
						return "Deleted all that matched.";
					}
					catch (ArgumentException) {
						return String.Empty;
					}
				case Command.Quit:
					return "quitting...";

				//. The below case Command.Nothing could be handled in the default case.
				case Command.Nothing:
					//return "I don't understand that command; try Create, Read, Update, Delete, or Quit.";
					var showHeaders = String.Empty;
					foreach (string header in this.Database.Headers) {
						showHeaders += header;
						showHeaders += "\n";
					}
					return showHeaders;
				default:
					return "This should never happen.";
			}
		}

		/// <summary>
		/// Prints the output from Evaluate to the console.
		/// </summary>
		/// <param name="message"></param>
		private static void Print(string message) {
			Console.WriteLine(message);
		}
	}
}