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

		private static void ImportDatabase() {
			Console.Write("Please specify the location of your .psv file: ");
			var filename = Console.ReadLine();
			Table table;
			if (!File.Exists(filename)) {
				Console.WriteLine("That file doesn't exist, I'm making you a new database with that filename");
				File.Create(filename);
			}
			using(StreamReader psv = new StreamReader(filename)) {
				table = ParsePSV.GetTable(psv, filename);
			}
			if(table != null) {
				var shell = new Shell(table);
				shell.Repl();
			}
			else {
				ImportDatabase();
			}
		}
	}
}