using System;

namespace Application {
    class HelpDocs {
        public static void DisplayHelp() {
            Console.WriteLine("{0}\n{1}\n{2}\n{3}\n{4}",
                "usage:  [args] /?",
                "Author: Date: Description: ",
                "1 : Import\tPipe \"fileName.psv\" \n\tImport a PSV file from the specified path.",
                "2 : Convert\t Pipe \"filename.csv\"\n\tConverts a CSV file to PSV and stores it in the same location.",
                "3 : Exit\t Pipe Exit\n\tExits the Pipe program "
                );
        }
    }
}
