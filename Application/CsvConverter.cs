using System.IO;

namespace Application {
	class CsvConverter {
		private string Path { get; set; }
		public CsvConverter(string path) {
			this.Path = path;
		}
		public void ConvertToPsv() {
			var psvPath = new FileInfo(this.Path).Directory +
				"\\" +
				System.IO.Path.GetFileNameWithoutExtension(this.Path) + ".psv";

			var sw = File.Exists(psvPath) ? new StreamWriter(psvPath , true) : new StreamWriter(psvPath);

			using(sw)
			using(var sr = new StreamReader(Path)) {
				var line = string.Empty;
				while((line = sr.ReadLine()) != null) {
					sw.WriteLine(line.Replace(',' , '|'));
				}
			}
		}
	}
}
