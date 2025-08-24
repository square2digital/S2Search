using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace S2.Test.Importer.Helpers
{
    public static class StringHelper
    {
        public static string FirstCharToUpperAndLower(string input)
        {
            switch (input)
            {
                case null: return "";
                case "": return input;
                default: return (input.ToLower().Trim().First().ToString().ToUpper() + input.Substring(1)).Trim();
            }
        }
    }
}
