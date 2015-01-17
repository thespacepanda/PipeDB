﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application {

	/// <summary>
	/// Represents the command types a user could enter. 
	/// </summary>
	enum Command {
		Create ,
		Read ,
		Update ,
		Delete ,
		Quit ,
		Nothing
	};

	/// <summary>
	/// Utility class for parsing REPL statements.
	/// </summary>
	class Parser {

		/// <summary>
		/// The list of headers from the table.
		/// </summary>
		private List<string> ValidHeaders;

		/// <summary>
		/// Takes the list of headers from the table.
		/// </summary>
		/// <param name="headers"></param>
		public Parser(List<string> headers) {
			this.ValidHeaders = headers;
		}

		/// <summary>
		/// Parses the arguments to the Create function.
		/// Throws an ArgumentException if the arguments are malformed.
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public List<string> Create(IEnumerable<string> args) {
			if (args.Count() == 0) {
				throw Error.ArgumentException(ErrorType.NoArgument);
			}
			else {
				return ValidRow(args);
			}
		}

		/// <summary>
		/// Creates a new row from the supplied string representation.
		/// </summary>
		/// <param name="maybeRow"></param>
		/// <returns></returns>
		private List<string> ValidRow(IEnumerable<string> maybeRow) {
			if (!maybeRow.First().Contains('{') && maybeRow.Last().Contains('}')) {
				throw Error.ArgumentException(ErrorType.MalformedRow);
			}
			else if (maybeRow.Count() != this.ValidHeaders.Count()) {
				throw Error.ArgumentException(ErrorType.WrongNumberOfValues);
			}
			var newRow = new List<string>();
			foreach (string value in maybeRow) {
				var newValue = value.Replace("{" , String.Empty);
				newValue = newValue.Replace("}" , String.Empty);
				newValue = newValue.Replace("," , String.Empty);
				newRow.Add(newValue);
			}
			return newRow;
		}

		/// <summary>
		/// Parses the arguments to Read.
		/// Throws an ArgumentException if they are malformed.
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public Tuple<int , Predicate<List<string>>> Read(IEnumerable<string> args) {
			// we need to check if the first arg is *; if not get the column
			// then we parse the where statement to form the query
			var column = -1;
			Predicate<List<string>> query = null;
			if (args.Count() == 0) {
				return new Tuple<int , Predicate<List<string>>>(column , query);
			}
			else {
				column = ColumnToIndex(args.First());
				if (args.Contains("where")) {
					var whereClause = args.Skip(2);
					query = WhereToQuery(whereClause);
				}
			}
			return new Tuple<int , Predicate<List<string>>>(column , query);
		}

		/// <summary>
		/// If the given string is a valid header column, returns the index of said column.
		/// Throws and ArgumentException if not.
		/// </summary>
		/// <param name="column"></param>
		/// <returns></returns>
		private int ColumnToIndex(string column) {
			if (column == "*") {
				// Displays all
				return -1;
			}
			else if (this.ValidHeaders.Contains(column)) {
				return this.ValidHeaders.IndexOf(column);
			}
			else {
				throw Error.ArgumentException(ErrorType.NoMatchForHeader);
			}
		}

		/// <summary>
		/// If the given clause is valid, returns the Predicate parsed out of the clause.
		/// Throws an ArgumentException if not.
		/// </summary>
		/// <param name="whereClause"></param>
		/// <returns></returns>
		private Predicate<List<string>> WhereToQuery(IEnumerable<string> whereClause) {
			if (ValidClause(whereClause, ErrorType.MalformedQuery)) {
				var queryHeader = this.ValidHeaders.IndexOf(whereClause.First());
				return row => row[queryHeader] == whereClause.Last();
			}
			// should never happen since ValidClause will throw a descriptive error.
			return null;
		}

		/// <summary>
		/// Predicate that detects whether the passed clause is valid. Must also be
		/// passed an ErrorType which represents the meaning of the equals sign in
		/// said clause. Throws and ArgumentException if the clause is invalid.
		/// </summary>
		/// <param name="clause"></param>
		/// <param name="equalsMeaning"></param>
		/// <returns></returns>
		private bool ValidClause(IEnumerable<string> clause , ErrorType equalsMeaning) {
			if (clause.Count() != 3) {
				throw Error.ArgumentException(equalsMeaning);
			}
			else if (!this.ValidHeaders.Contains(clause.First())) {
				throw Error.ArgumentException(ErrorType.NoMatchForHeader);
			}
			else {
				return true;
			}
		}

		public Tuple<Tuple<string , string> , Predicate<List<string>>> Update(IEnumerable<string> args) {
			throw new NotImplementedException();
		}

		public Predicate<List<string>> Delete(IEnumerable<string> args) {
			throw new NotImplementedException();
		}

		private Tuple<int , string> AssignmentToDiff(IEnumerable<string> assignmentClause) {
			if (ValidClause(assignmentClause , ErrorType.MalformedAssignment)) {
				var columnName = assignmentClause.First();
				var newValue = assignmentClause.Last();
				var column = this.ValidHeaders.IndexOf(columnName);
				return new Tuple<int , string>(column , newValue);
			}
			// should never happen since ValidClause will throw a descriptive error.
			return null;
		}


	}
}