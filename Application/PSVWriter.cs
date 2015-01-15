using System.IO;
using System.Text;

namespace Application {

	/// <summary>
	/// Handles the writing of the PSV file from a given table.
	/// </summary>
	class PSVWriter {

		/// <summary>
		/// Table of the writer to work with.
		/// </summary>
		private Table Table = new Table();

		/// <summary>
		/// Does the table file exist?
		/// </summary>
		private bool TableExists = false;

		/// <summary>
		/// The file that will be built from the header and entries.
		/// </summary>
		private StringBuilder BuiltFile = new StringBuilder();

		/// <summary>
		/// Default and only constructor. A table must be assigned for work.
		/// </summary>
		/// <param name="table">The table to work with.</param>
		public PSVWriter(Table table) {
			this.Table = table;
			this.TableExists = File.Exists(this.Table.TableName);
		}

		/// <summary>
		/// Writes the header row of the table.
		/// </summary>
		public void WriteHeader() {
			if(!this.TableExists) {
				using(var sw = new StreamWriter(this.Table.TableName)) {
					var headerRow = string.Empty;

					var line = string.Empty;
					foreach(var header in this.Table.Headers) {
						var record = this.Table.Headers.IndexOf(header) != this.Table.Headers.Count ? header + " | " : header;
						line += record;
					}

					this.BuiltFile.Insert(0 , line + "\n");
				}
			}
			else {
				//TODO: Prompt for file overwrite. handle console out else where.
			}
		}

		/// <summary>
		/// Writes the entries out to the PSV.
		/// </summary>
		public void WriteEntries() {
			if(!this.TableExists) {
				using(var sw = new StreamWriter(this.Table.TableName)) {
					foreach(var list in this.Table.Entries) {

						var line = string.Empty;
						foreach(var entry in list) {
							var record = list.IndexOf(entry) != list.Count ? entry + " | " : entry;
							line += record;
						}

						BuiltFile.AppendLine(line);
					}
				}
			}
			else {
				//TODO: Prompt for file overwrite. Handle console write in another location.
			}
		}
	}
}
