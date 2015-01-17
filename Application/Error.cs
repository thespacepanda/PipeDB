using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application {

	enum ErrorType {
		MalformedRow ,
		MalformedQuery ,
		MalformedAssignment ,
		WrongNumberOfValues ,
		NoMatchForHeader ,
		NoArgument
	}

	/// <summary>
	/// Utility class for pretty printing errors.
	/// </summary>
	static class Error {

		/// <summary>
		/// Gives the output string for the provided error type.
		/// </summary>
		/// <param name="errorType"></param>
		/// <returns></returns>
		public static Exception ArgumentException(ErrorType errorType) {
			var message = String.Empty;
			switch (errorType) {
				case ErrorType.MalformedRow:
					message = "Rows look like this: {1, 2, 3, 4}";
					break;
				case ErrorType.MalformedQuery:
					message = "Queries must be comprised of a single header name, a =, and a value.";
					break;
				case ErrorType.MalformedAssignment:
					message = "The column to update must be presented as: 'update column_name = new_value'";
					break;
				case ErrorType.WrongNumberOfValues:
					message = "Rows must provide values for every column in the header.";
					break;
				case ErrorType.NoMatchForHeader:
					message = "Couldn't find a header by that name; you can see the current headers by typing 'headers'";
					break;
				case ErrorType.NoArgument:
					message = "Must supply an argument.";
					break;
				default:
					// Defaults to saying nothing, maybe change to generic message?
					break;
			}
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(message);
			Console.ResetColor();
			return new ArgumentException(message , "args");
		}
	}
}
