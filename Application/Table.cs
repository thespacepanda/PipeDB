using System;
using System.Linq;
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

		/// <summary>
		/// Adds a row to the table; must be the same length as the headers.
		/// Returns success status.
		/// </summary>
		/// <param name="newRow"></param>
		/// <returns></returns>
		public bool Create(List<string> newRow) {
			if (newRow.Count != this.Headers.Count) {
				return false;
			}
			this.Entries.Add(newRow);
			return true;
		}

		/// <summary>
		/// Returns all the rows which match the given query delegate.
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public List<List<string>> Read(Predicate<List<string>> query = null) {
			if (query == null) {
				query = _ => true;
			}
			return this.Entries
				.FindAll(query)
				.ToList();
		}

		/// <summary>
		/// Returns the values of every matching row at the provided column.
		/// </summary>
		/// <param name="column"></param>
		/// <param name="query"></param>
		/// <returns></returns>
		public List<string> Read(int column , Predicate<List<string>> query = null) {
			if (column < this.Headers.Count) {
				return new List<string>();
			}
			return Read(query)
				.Select(row => row[column])
				.ToList();
		}

		/// <summary>
		/// Modifies the rows which match the given query delegate.
		/// Unlike read, this is required because it's very rare to want to change
		/// every column to a single value globally.
		/// </summary>
		/// <param name="diff"></param>
		/// <param name="query"></param>
		/// <returns></returns>
		public bool Update(Tuple<string , string> diff, Predicate<List<string>> query) {
			if (this.Headers.Contains(diff.Item1)) {
				Read(query)
					.Select(row => row[this.Headers.IndexOf(diff.Item1)] = diff.Item2);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Deletes rows which match a query, but I don't think asr
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public bool Delete(Predicate<List<string>> query) {
			bool e;
			try {
				this.Entries.RemoveAll(query);
				e = true;
			}
			finally {
				e = false;
			}
			return e; 
		}
	}
}
