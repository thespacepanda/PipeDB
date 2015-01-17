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
			this.OrigFileLocation = args[1];
			if(args.Length > 1) {
				if(this.OrigFileLocation.Contains(SupportedFileTypes[0])) {
					//TODO: Convert a CSV to PSV in the same directory.
					//	Waiting on file to be completed
					/*
					 * CSVtoPSV ctp = new CSVtoPSV(this.OrigFileLocation);
					 * ctp.Start();
					 */
				}
				else if(this.OrigFileLocation.Contains(SupportedFileTypes[1])) {
					//. Handle importing the PSV file type.
					using(var sr = new StreamReader(this.OrigFileLocation)) {
						var table = ParsePSV.GetTable(sr , this.OrigFileLocation);
						var shell = new Shell(table);
						shell.Repl();
					}
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
