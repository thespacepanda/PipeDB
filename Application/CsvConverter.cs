using System.IO;

namespace Application {
    class CsvConverter {
        private string Path { get; set; }
        public CsvConverter(string path) {
            this.Path = path;
        }
        public void ConvertToPsv() {
            var csvPath = System.IO.Path.GetDirectoryName(this.Path) +
                "\\" +
                System.IO.Path.GetFileNameWithoutExtension(this.Path) + ".psv";

            using (var sw = new StreamWriter(csvPath))
            using (var sr = new StreamReader(Path)) {
                var line = string.Empty;
                while ((line = sr.ReadLine()) != null) {
                    sw.WriteLine(line.Replace(',', '|'));
                }
            }
        }
    }
}
