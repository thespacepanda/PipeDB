using System.IO;

namespace Application {
	class CommandOptions {

		/// <summary>
		/// File location of the PSV or CSV.
		/// </summary>
		public string OrigFileLocation { get; set; }

		/// <summary>
		/// Supported file types.
		/// </summary>
		private string[] SupportedFileTypes = { ".csv" , ".psv" };

		/// <summary>
		/// Parse out the command line options
		/// </summary>
		/// <param name="args">The command line arguments to parse.</param>
		public CommandOptions(string[] args) {
			if(args.Length == 1) {
				this.OrigFileLocation = args[0];
			}

			if(!string.IsNullOrEmpty(this.OrigFileLocation)) {
				if(this.OrigFileLocation.Contains(SupportedFileTypes[0])) {
					//.Convert a CSV to PSV in the same directory.

					var cc = new CsvConverter(this.OrigFileLocation);
					cc.ConvertToPsv();
				}
				else if(this.OrigFileLocation.Contains(SupportedFileTypes[1])) {
					Table table;
					//. Handle importing the PSV file type.
					table = ParsePSV.GetTable(this.OrigFileLocation);
					var shell = new Shell(table);
					shell.Repl();
				}
				else if(this.OrigFileLocation == "/?") {
					//TODO: Display the help menu.
					HelpDocs.DisplayHelp();
				}
			}
			else {
				//. Present the Menu upon entry of the program.
				Menu.PresentMenu();
			}
		}
	}
}
