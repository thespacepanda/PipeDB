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
		/// Parses the arguments to the Create function.
		/// Throws an ArgumentException if the arguments are malformed.
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public List<string> Create(IEnumerable<string> args) {
			var newRow = new List<string>();
			if (args.First().Contains('{') && args.Last().Contains('}')) {
				if (args.Count() == this.ValidHeaders.Count()) {
					// TODO handle escape sequences for '{' and '}' literals
					foreach (string arg in args) {
						var stripCurly = arg.Replace("{" , String.Empty);
						stripCurly = stripCurly.Replace("}" , String.Empty);
						var stripComma = stripCurly.Replace("," , String.Empty);
						newRow.Add(stripComma);
					}
					return newRow;
				}
				throw Error.ArgumentException(ErrorType.WrongNumberOfValues);
			}
			throw Error.ArgumentException(ErrorType.MalformedRow);
		}

		public Tuple<int , Predicate<List<string>>> Read(IEnumerable<string> args) {
			// we need to check if the first arg is *; if not get the column
			// then we parse the where statement to form the query
			var column = -1;
			Predicate<List<string>> query = null;
			if (args.Count() == 0) {
				return new Tuple<int , Predicate<List<string>>>(column , query);
			}
			else if (args.First() != "*") {
				if (!this.ValidHeaders.Contains(args.First())) {
					throw Error.ArgumentException(ErrorType.NoMatchForHeader);
				}
				column = this.ValidHeaders.IndexOf(args.First());
			}
			if (args.Contains("where")) {
				var whereClause = args.Skip(2);
				if (whereClause.Count() != 3) {
					throw Error.ArgumentException(ErrorType.MalformedQuery);
				}
				else if (!this.ValidHeaders.Contains(whereClause.First())) {
					throw Error.ArgumentException(ErrorType.NoMatchForHeader);
				}
				var queryHeader = this.ValidHeaders.IndexOf(whereClause.First());
				query = row => row[queryHeader] == whereClause.Last();
			}
			return new Tuple<int , Predicate<List<string>>>(column , query);
		}

		public Tuple<Tuple<string , string> , Predicate<List<string>>> Update(IEnumerable<string> args) {
			throw new NotImplementedException();
		}

		public Predicate<List<string>> Delete(IEnumerable<string> args) {
			throw new NotImplementedException();
		}
	}
}
