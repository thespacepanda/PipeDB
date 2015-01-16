using System;
using System.Linq;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace Application {

	/// <summary>
	/// Represents the command types a user could enter. 
	/// </summary>
	enum Command {
		Create ,
		Read ,
		Update ,
		Delete ,
		Quit ,
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
				var message = Evaluate(command);
				// using "quitting..." as a flag, never gets shown to the user
				// seems a little strange, but if we Print before we break we will
				// show "quitting" as many times as the user enters a malformed
				// argument to a database command. In this sense, the PushToPrompt
				// method creates a transparent stack of while loops (inside the
				// nested Repl() method). Hopefully this isn't a memory overflow spot.

				// TODO possible race condition
				if (message == "quitting...") {
					break;
				}
				Print(message);
			}
		}

		/// <summary>
		/// Helper function to show the fancy prompt to the screen.
		/// </summary>
		private static void DisplayPrompt(string pushed = "") {
			var prompt1 = "{ ~ }";
			var prompt2 = "  » ";
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.Write(prompt1);
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write(prompt2);
			Console.ResetColor();
			Console.Write(pushed);
		}




		/// <summary>
		/// Reads a string from the user and interprets it as a Command.
		/// </summary>
		/// <returns></returns>
		private static string Read(string pushed = "") {
			DisplayPrompt(pushed);
			var command = Console.ReadLine();
			return command;
		}

		/// <summary>
		/// Maps strings to Commands (matches on first word).
		/// Returns Command.Nothing if it cannot find a match.
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		private static Command Interpret(string command) {
			var words = command.ToLower().Split();
			switch (words[0]) {
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
			switch (verb) {
				case Command.Create:
					try {
						var newRow = ParseArgsCreate(args);
						if (this.Database.Create(newRow)) {
							return String.Format("Row {0} added" , newRow);
						}
						// This should be handled by the Argument parser, but just in case
						return "Rows must provide values for every column in the header.";
					}
					catch (ArgumentException) {
						return PushToPrompt("create" , Command.Create);
					}
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

		private string PushToPrompt(string pushed , Command verb) {
			var command = Read(pushed);
			var message = Evaluate(command);
			Print(message);
			Repl();
			// make sure we break out of nested Repl() call when the user quits
			return "quitting...";
		}

		/// <summary>
		/// Parses the arguments to the Create function.
		/// Throws an ArgumentException if the arguments are malformed.
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		private List<string> ParseArgsCreate(IEnumerable<string> args) {
			var newRow = new List<string>();
			if (args.First().Contains('{') && args.Last().Contains('}')) {
				if (args.Count() == this.Database.Headers.Count()) {
					// TODO handle escape sequences for '{' and '}' literals
					Func<char , bool> notCurly = c => c != '{' && c != '}';
					args
						.ToObservable()
						.Subscribe(arg => newRow.Add(arg.Where(notCurly).ToString()));
					return newRow;
				}
				throw new ArgumentException("Rows must provide values for every column in the header." , "args");
			}
			throw new ArgumentException("Rows look like this: {1, 2, 3, 4}" , "args");
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