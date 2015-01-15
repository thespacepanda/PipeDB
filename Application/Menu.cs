using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application {

	static class Menu {
		public static void PresentMenu(string[] args) {
			var options = new List<string>() {
				"1. Import Database",
				"2. Create New Database",
				"3. Exit The Program"
			};
			options.ForEach(option => Console.WriteLine(option));
			DisplayPrompt();
			var choice = Convert.ToInt32(Console.ReadLine());
			switch (choice) {
				case 1:
					//ImportDatabase(); TODO
				case 2:
					//NewDatabase(); TODO
				case 3:
					Environment.Exit(0);
					break;
				default:
					PresentMenu(args);
					Console.WriteLine("Please enter a valid option.");
					break;
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
	}
}