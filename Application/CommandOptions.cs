using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Application {
	class CommandOptions {

		/// <summary>
		/// File location of the PSV or CSV.
		/// </summary>
		public Path OrigFileLocation { get; set; }
		
		/// <summary>
		/// Supported file types.
		/// </summary>
		private string[] SupportedFileTypes = { ".csv" , ".psv" };

		/// <summary>
		/// Parse out the command line options
		/// </summary>
		/// <param name="args">The command line arguments to parse.</param>
		public CommandOptions(string[] args) {
			if(args[1].Contains(SupportedFileTypes[0])) {
				//TODO: Convert a CSV to PSV.
			}
			else if(args[1].Contains(SupportedFileTypes[1])) {
				//TODO: Handle importing the PSV file type.
			}
			else if(args[1] == "-h") {
				//TODO: Display the help menue.
			}
		}
	}
}
