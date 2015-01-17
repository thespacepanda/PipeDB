using System;

namespace Application {
    class HelpDocs {
        /// <summary>
        /// Displays usage, arguments, and program info.
        /// </summary>
        /// Commented numbers represent string position index for format debugging.
        public static void DisplayHelp() {
            Console.WriteLine("{0}\n{1}\n{2, -15}{3, -19}\n{4}\n\n{5, -15}{6, -19}\n{7}\n{8,-15}{9,-19}\n{10}\n",
                "usage:  [args] \"path.example\" /?\n", //0
                "Author: We Know Work\tDate: 01.16.2014\nDescription: Basic CRUD database manipulator \n", //1
                "1 : Import", //2
                "Pipe \"fileName.psv\"", //3
                " -Imports a PSV file to the database from the specified path.", //4
                "2 : Convert", //5
                "Pipe \"filename.csv\"", //6
                " -Converts a CSV file to PSV and stores it in the same location.\n", //7
                "3 : Exit", //8
                "Pipe \"Exit\"", //9
                " -Exits the Pipe program \n" //10
            );
        }

        /// <summary>
        /// Displays help for individual REPL commands.
        /// </summary>
        /// <param name="command"></param>
        /// Waiting on more information on how the commands are implemented.
        public static void DisplayReplHelp(Command command) {
            switch (command) {
                case Command.Create:
                    Console.WriteLine("{0}\n{1}\n{2}",
                        "usage: [args] \"Row\" /?",
                        "ex. Create \"Row\"",
                        "Creates a row in the table"
                        );
                    break;
                case Command.Read:
                    Console.WriteLine("{0}\n{1}\n{2}",
                        "usage: [args] \"Row\" \"Column_Name\" /?",
                        "ex. Read \"Row\"\t OR Read \"Row\" \"Coloumn_Name\"",
                        "Reads row or specified cell from the database (query)."
                        );
                    break;
                case Command.Update:
                    Console.WriteLine("{0}\n{1}\n{2}",
                        "usage: [args] \"Row\" \"Coloumn_Name\" \"Data_To_Insert\" /?",
                        "ex. Update \"Row\"\t OR Update \"Row\" \"First_Name\" \"Bob Jackson\"",
                        "Updates a row or the specified cell."
                        );
                    break;
                case Command.Delete:
                    Console.WriteLine("{0}\n{1}\n{2}",
                        "usage: [args] \"Row #\" \"Column_Name\" /?",
                        "ex. Delete \"1\"\t OR Delete \"1\" \"First_Name\"",
                        "Deletes a whole row or the specified cell."
                        );
                    break;
                case Command.Quit:
                    Console.WriteLine("{0}\n{1}\n{2}",
                       "usage: [args] /?",
                       "ex. Quit",
                       "This will quit the program."
                       );
                    break;
                case Command.Nothing:
                    break;
                default:
                    break;
            }
        }
    }
}
