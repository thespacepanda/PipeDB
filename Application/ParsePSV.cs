using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Application {

	/// <summary>
	/// Parses a psv file with a header top row.
	/// </summary>
	class ParsePSV {

		/// <summary>
		/// Creates a table from a given file path and file stream.
		/// </summary>
		/// <param name="psv">The file stream to parse for PSV</param>
		/// <param name="filePath">The file path.</param>
		/// <returns></returns>
		public static Table GetTable(StreamReader psv , string filePath) {
			var table = new Table();
			table.TableName = filePath;
			table.SetTableHeaders(GetHeaders(psv));
			table.SetTableEntries(GetEntries(psv));
			return table;
		}

		/// <summary>
		/// Gets the header from the first row of the file stream.
		/// </summary>
		/// <param name="psv">The PSV file stream to read from.</param>
		/// <returns>Returns a list of type string with the header entries.</returns>
		private static List<string> GetHeaders(StreamReader psv) {
			return SplitOnPipe(psv.ReadLine());
		}

		/// <summary>
		/// Splits the given line through pipe delineation. 
		/// </summary>
		/// <param name="line">The line to parse.</param>
		/// <returns>Returns a parsed list of elements.</returns>
		private static List<string> SplitOnPipe(string line) {
			return line.Split('|')
				.Select(element => element.Trim())
				.ToList();
		}

		/// <summary>
		/// Gets the entries from a given stream starting at line 2 or position 1
		/// </summary>
		/// <param name="psv">The PSV file stream to read from</param>
		/// <returns>Returns a list of list, essentially the lines and their entries.</returns>
		private static List<List<string>> GetEntries(StreamReader psv) {
			return psv.ReadToEnd()
				.Split(new string[] { Environment.NewLine } , StringSplitOptions.None)
				.Select(line => (List<string>) SplitOnPipe(line))
				.ToList();
		}
	}
}
