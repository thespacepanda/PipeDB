using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Application {

	/// <summary>
	/// Handles file access by the program.
	/// </summary>
	class Session {

		/// <summary>
		/// Path to the file Session consumes.
		/// </summary>
		public string FilePath;

		/// <summary>
		/// The first line of the file; used for saving between StreamReader calls.
		/// </summary>
		private string FirstLine;

		/// <summary>
		/// The contents of the file minus the first line; used for saving between calls.
		/// </summary>
		private string RestOfFile;

		/// <summary>
		/// Constructor takes the path to our PSV.
		/// </summary>
		/// <param name="filePath"></param>
		public Session(string filePath) {
			this.FilePath = filePath;
		}

		/// <summary>
		/// Opens a new StreamReader and returns the contents of the file.
		/// </summary>
		/// <returns></returns>
		public string GetFileContents() {
			var contents = String.Empty;
			using (StreamReader sr = new StreamReader(this.FilePath)) {
				contents = sr.ReadToEnd();
			}
			return contents;
		}

		/// <summary>
		/// Writes the given string to the file.
		/// </summary>
		/// <param name="contents"></param>
		public void WriteToFile(string contents) {
			using (StreamWriter sw = new StreamWriter(this.FilePath)) {
				sw.Write(contents);
				sw.Flush();
			}
		}

		/// <summary>
		/// Loads the whole file but separates the first line from the rest.
		/// </summary>
		/// <returns></returns>
		private Tuple<string, string> GetLineAndRest() {
			var firstLine = String.Empty;
			var rest = String.Empty;
			using (StreamReader sr = new StreamReader(this.FilePath)) {
				firstLine = sr.ReadLine();
				rest = sr.ReadToEnd();
			}
			return new Tuple<string , string>(firstLine , rest);
		}

		/// <summary>
		/// Returns the first line of the file.
		/// </summary>
		/// <returns></returns>
		public string GetFirstLine() {
			if (String.IsNullOrEmpty(this.FirstLine)) {
				var wholeFile = GetLineAndRest();
				this.FirstLine = wholeFile.Item1;
				this.RestOfFile = wholeFile.Item2;
			}
			return this.FirstLine;
		}

		/// <summary>
		/// Returns the whole file minus the first line.
		/// </summary>
		/// <returns></returns>
		public string GetRestOfFile() {
			if (String.IsNullOrEmpty(this.RestOfFile)) {
				var wholeFile = GetLineAndRest();
				this.FirstLine = wholeFile.Item1;
				this.RestOfFile = wholeFile.Item2;
			}
			return this.RestOfFile;
		}

		public void WriteTable(Table table) {
			var contents = String.Empty;
			foreach (var str in Intersperse(table.Headers))
			{
				contents += str;		 
			}
			contents += "\n";
			foreach (var row in table.Entries) {
				foreach (var str in Intersperse(row)) {
					contents += str;	
				}
				contents += "\n";
			}
			WriteToFile(contents);
		}

		private IEnumerable<string> Intersperse(IEnumerable<string> headers) {
			var first = true;
			foreach (var header in headers) {
				if (first) {
					first = false;
				}
				else {
					yield return " | ";
				}
				yield return header;
			}
		}
	}
}
