﻿using System.IO;
using System.Linq;
using System.Collections.Generic;

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
					//TODO: Convert a CSV to PSV.
				}
				else if(this.OrigFileLocation.Contains(SupportedFileTypes[1])) {
					//. Handle importing the PSV file type.
					using(var sr = new StreamReader(this.OrigFileLocation)) {
						var table = ParsePSV.GetTable(sr , this.OrigFileLocation);
						Shell.Repl(table);
					}
				}
				else if(this.OrigFileLocation == "-h") {
					//TODO: Display the help menue.
				} 
			}
		}
	}
}
