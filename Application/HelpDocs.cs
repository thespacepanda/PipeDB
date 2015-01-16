using System;

namespace Application {
	class HelpDocs {
		/// <summary>
		/// Displays usage, arguments, and program info.
		/// </summary>
		/// Commented numbers represent string position index for format debugging.
		public static void DisplayHelp() {
			Console.WriteLine("{0}\n{1}\n{2, -15}{3, -19}\n{4}\n\n{5, -15}{6, -19}\n{7}\n{8,-15}{9,-19}\n{10}\n" ,
				"usage:  [args] \"path.example\" /?\n" , //0
				"Author: We Know Work\tDate: 01.16.2014\nDescription: Basic CRUD database manipulator \n" , //1
				"1 : Import" , //2
				"Pipe \"fileName.psv\"" , //3
				" -Imports a PSV file to the database from the specified path." , //4
				"2 : Convert" , //5
				"Pipe \"filename.csv\"" , //6
				" -Converts a CSV file to PSV and stores it in the same location.\n" , //7
				"3 : Exit" , //8
				"Pipe \"Exit\"" , //9
				" -Exits the Pipe program \n" //10
			);
		}

		public static void DisplayReplHelp(Command command) {
			switch(command) {
				case Command.Create:
					break;
				case Command.Read:
					break;
				case Command.Update:
					break;
				case Command.Delete:
					break;
				case Command.Quit:
					break;
				case Command.Nothing:
					break;
				default:
					break;
			}
		}
	}
}
