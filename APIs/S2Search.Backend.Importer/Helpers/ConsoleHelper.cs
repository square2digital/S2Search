using System;

namespace S2.Test.Importer.Helpers
{
    public static class ConsoleHelper
    {
        public static void WriteIndicatorMessage(string message)
        {
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
        }

        public static void WriteInformationMessage(string message)
        {
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
        }

        public static void WriteErrorMessage(string message)
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
        }
    }
}