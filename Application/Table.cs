using System.Collections.Generic;

namespace Application {

	/// <summary>
	/// Generic data class for data entry.
	/// </summary>
	class Table {

		/// <summary>
		/// Name of the database or file name.
		/// </summary>
		public string TableName { get; set; }

		/// <summary>
		/// Header or column names for the database.
		/// </summary>
		public List<string> Headers { get; private set; }

		/// <summary>
		/// Entries under the header.
		/// </summary>
		public List<List<string>> Entries { get; private set; }

		/// <summary>
		/// Sets database or file headers. 
		/// </summary>
		/// <param name="headers"></param>
		public void SetTableHeaders(List<string> headers) {
			this.Headers = headers;
		}

		/// <summary>
		/// Sets the table entries, essentially a lines and their entries.
		/// </summary>
		/// <param name="entries">The list of list or lines of entries.</param>
		public void SetTableEntries(List<List<string>> entries) {
			this.Entries = entries;
		}
	}
}
