using System;
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
		/// Parses the arguments to the Create function, and returns an object
		/// which can then be passed to Table.Create().
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
		/// Parses the arguments to Read, and returns an object which can then
		/// be passed to Table.Read().
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
		/// If the given string is a valid header column, returns the index of 
		/// said column. Throws and ArgumentException if not.
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
			if (ValidClause(whereClause , ErrorType.MalformedQuery)) {
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

		/// <summary>
		/// Parses the provided arguments to Update and returns a tuple object
		/// which can then be passed into Table.Update(). Throws an ArgumentException
		/// if they are invalid.
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public Tuple<Tuple<int , string> , Predicate<List<string>>> Update(IEnumerable<string> args) {
			if (args.Count() != 7) {
				throw Error.ArgumentException(ErrorType.MalformedUpdate);
			}
			var clauses = ClauseSeparator(args);
			var assignmentClause = clauses.First();
			var whereClause = clauses.Last();
			var query = WhereToQuery(whereClause);
			var diff = AssignmentToDiff(assignmentClause);
			return new Tuple<Tuple<int , string> , Predicate<List<string>>>(diff , query);
		}

		/// <summary>
		/// Splits the IEnumerable<string> on the value "where", and returns a List
		/// of the two resultant IEnumerable<string>s.
		/// </summary>
		/// <param name="tiedClauses"></param>
		/// <returns></returns>
		private List<IEnumerable<string>> ClauseSeparator(IEnumerable<string> tiedClauses) {
			var assignmentClause = tiedClauses.Take(3);
			var whereClause = tiedClauses.Skip(4);
			var clauses = new List<IEnumerable<string>>();
			clauses.Add(assignmentClause);
			clauses.Add(whereClause);
			return clauses;
		}

		public Predicate<List<string>> Delete(IEnumerable<string> args) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Parses the provided clause into a tuple which represents a diff from the
		/// column name (represented by the index) and the new value. Throws an
		/// ArgumentException if the clause is malformed.
		/// </summary>
		/// <param name="assignmentClause"></param>
		/// <returns></returns>
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
