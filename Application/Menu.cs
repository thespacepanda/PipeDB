using System;
using System.Collections.Generic;
using System.IO;

namespace Application {

	static class Menu {
		public static void PresentMenu() {
			var options = new List<string>() {
				"1. Import Database",
				"2. Create New Database",
				"3. Exit The Program"
			};
			options.ForEach(option => Console.WriteLine(option));
			Console.Write("$> ");
			var choice = Convert.ToInt32(Console.ReadLine());
			switch (choice) {
				case 1:
					ImportDatabase();
					break;
				case 2:
					//NewDatabase(); TODO
					break;
				case 3:
					Environment.Exit(0);
					break;
				default:
					PresentMenu();
					Console.WriteLine("Please enter a valid option.");
					break;
			}
		}

		private static void ImportDatabase(string filePath = null) {
			if (String.IsNullOrEmpty(filePath)) {
				Console.Write("Please specify the location of your .psv file: ");
				filePath = Console.ReadLine();
			}
			Table table;
			if (!File.Exists(filePath)) {
				Console.WriteLine("That file doesn't exist, I'm making you a new database with that filename");
				table = CreateDatabase(filePath);
			}
			else {
				try {
					table = ParsePSV.GetTable(filePath);
				}
				catch (NullReferenceException) {
					// file is empty.
					Console.WriteLine("That file looks empty; let's create a database there.");
					table = CreateDatabase(filePath);
				}
			}
			var shell = new Shell(table);
			shell.Repl();
		}

		private static Table CreateDatabase(string filePath = null) {
			if (String.IsNullOrEmpty(filePath)) {
				Console.WriteLine("What would you like to call your database?");
				Console.Write("$> ");
				filePath = Console.ReadLine();
				if (!filePath.Contains("psv")) {
					filePath += ".psv";
				}
			}
			Console.WriteLine("Creating a new database at {0}...", filePath);
			var headers = new List<string>();
			Console.WriteLine("Start entering the names of the columns you would like.");
			Console.WriteLine("When you're done, type 'done'");
			var done = false;
			do {
				Console.Write("$> ");
				var response = Console.ReadLine();
				if (response.ToLower() == "done") {
					done = true;
				}
				else {
					headers.Add(response.ToLower());
				}
			} while (!done);
			Console.WriteLine("Your headers are: {0}", headers);
			Console.WriteLine("We'll now import your new database and you can interact with it.");
			var table = new Table(new Session(filePath));
			table.SetTableHeaders(headers);
			table.SetTableEntries(new List<List<string>>());
			return table;
		}
	}
}