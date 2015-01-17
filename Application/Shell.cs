using System;
using System.Collections.Generic;
using System.Linq;

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
			var result = string.Empty;
			do {
				var command = Read();
				result = Evaluate(command);
				Print(result);
			} while(!(result == "quitting..."));
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
			return pushed + command;
		}

		/// <summary>
		/// Maps strings to Commands (matches on first word).
		/// Returns Command.Nothing if it cannot find a match.
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		private static Command Interpret(string command) {
			switch(command.Split()[0].ToLower()) {
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
			switch(verb) {
				case Command.Create:
					try {
						var newRow = ParseArgsCreate(args);
						if(this.Database.Create(newRow)) {
							var stringOfRow = String.Join(" | " , newRow.ToArray());
							return String.Format("Row {0} added" , stringOfRow);
						}
						// This should be handled by the Argument parser, but just in case
						return "Rows must provide values for every column in the header.";
					}
					catch(ArgumentException) {
						return PushToPrompt("create" , Command.Create);
					}
				case Command.Read:
					try {
						var columnAndQuery = ParseArgsRead(args);
						var column = columnAndQuery.Item1;
						var query = columnAndQuery.Item2;
						if(column == -1) {
							var rowsThatMatch = this.Database.Read(query);
							var rowString = String.Empty;
							foreach(List<string> row in rowsThatMatch) {
								rowString += String.Join(" | " , row.ToArray());
								rowString += "\n";
							}
							return rowString;
						}
						var columnsThatMatch = this.Database.Read(column , query);
						var columnString = String.Empty;
						foreach(string value in columnsThatMatch) {
							columnString += value;
							columnString += "\n";
						}
						return columnString;
					}
					catch(ArgumentException) {
						return PushToPrompt("read" , Command.Read);
					}
				case Command.Update:
					try {
						var diffAndQuery = ParseArgsUpdate(args);
						var diff = diffAndQuery.Item1;
						var query = diffAndQuery.Item2;
						if(this.Database.Update(diff , query)) {
							return "Updated Database";
						}
						return "Type not in Headers.";
					}
					catch(ArgumentException) {
						return PushToPrompt("update" , Command.Read);
					}
				case Command.Delete:
					try {
						var query = ParseArgsDelete(args);
						if(this.Database.Delete(query)) {
							return "Deleted all that matched";
						}
						return "Unexpected Deletion error";
					}
					catch(ArgumentException) {
						return PushToPrompt("delete" , Command.Delete);
					}
				case Command.Quit:
					return "quitting...";

				//. The below case Command.Nothing could be handled in the default case.
				case Command.Nothing:
					//return "I don't understand that command; try Create, Read, Update, Delete, or Quit.";
					var showHeaders = String.Empty;
					foreach(string header in this.Database.Headers) {
						showHeaders += header;
						showHeaders += "\n";
					}
					return showHeaders;
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
		/// Message that's printed out when user passes an argument which isn't the
		/// same length as the table's header.
		/// </summary>
		private string WrongNumberOfValues = "Rows must provide values for every column in the header.";

		/// <summary>
		/// Message that's printed when user passes a malformed row.
		/// </summary>
		private string MalformedRow = "Rows look like this: {1, 2, 3, 4}";

		/// <summary>
		/// Parses the arguments to the Create function.
		/// Throws an ArgumentException if the arguments are malformed.
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		private List<string> ParseArgsCreate(IEnumerable<string> args) {
			var newRow = new List<string>();
			if(args.First().Contains('{') && args.Last().Contains('}')) {
				if(args.Count() == this.Database.Headers.Count()) {
					// TODO handle escape sequences for '{' and '}' literals
					foreach(string arg in args) {
						var stripCurly = arg.Replace("{" , String.Empty);
						stripCurly = stripCurly.Replace("}" , String.Empty);
						var stripComma = stripCurly.Replace("," , String.Empty);
						newRow.Add(stripComma);
					}
					return newRow;
				}
				throw Error(WrongNumberOfValues , "args");
			}
			throw Error(MalformedRow , "args");
		}

		private string MalformedQuery = "Queries must be comprised of a single header name, a =, and a value.";

		private string NoMatchForHeader = "Couldn't find a header with that name.";

		private Tuple<int , Predicate<List<string>>> ParseArgsRead(IEnumerable<string> args) {
			if(args.Contains("where")) {
				var query = args.Skip(2);
				if(query.Count() == 3) {
					if(this.Database.Headers.Contains(query.First())) {
						var headerLocation = this.Database.Headers.IndexOf(query.First());
						Predicate<List<string>> matchQuery = s => s[headerLocation] == query.Last();
						return new Tuple<int , Predicate<List<string>>>(headerLocation , matchQuery);
					}
					throw Error(NoMatchForHeader , "args");
				}
				throw Error(MalformedQuery , "args");
			}
			if(args.Count() == 0) {
				throw Error("Must give an argument." , "args");
			}
			if(args.First() == "*") {
				return new Tuple<int , Predicate<List<string>>>(-1 , null);
			}
			else if(this.Database.Headers.Contains(args.First())) {
				return new Tuple<int , Predicate<List<string>>>(this.Database.Headers.IndexOf(args.First()) , null);
			}
			throw Error(NoMatchForHeader , "args");
		}
		private Tuple<Tuple<string , string> , Predicate<List<string>>> ParseArgsUpdate(IEnumerable<string> args) {
			throw new NotImplementedException();
		}

		private Predicate<List<string>> ParseArgsDelete(IEnumerable<string> args) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Prints the output from Evaluate to the console.
		/// </summary>
		/// <param name="message"></param>
		private static void Print(string message) {
			Console.WriteLine(message);
		}

		private static Exception Error(string errorMessage , string variable) {
			Console.ForegroundColor = ConsoleColor.Red;
			Print(errorMessage);
			Console.ResetColor();
			return new ArgumentException(errorMessage , variable);
		}
	}
}