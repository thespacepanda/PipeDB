using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application {
	class ParsePSV {

		public static Table GetTable(StreamReader psv , string filename) {
			var table = new Table();
			table.SetTableHeaders(GetHeaders(psv));
			table.SetTableEntries(GetEntries(psv));
			psv.Close();
			return table;
		}

		private static List<string> GetHeaders(StreamReader psv) {
			return SplitOnPipe(psv.ReadLine());
		}

		private static List<string> SplitOnPipe(string line) {
			return line.Split('|')
				.Select(element => element.Trim())
				.ToList();
		}

		private static List<List<string>> GetEntries(StreamReader psv) {
			return psv.ReadToEnd()
				.Split(new string[] { Environment.NewLine } , StringSplitOptions.None)
				.Select(line => (List<string>) SplitOnPipe(line))
				.ToList();
		}
	}
}
