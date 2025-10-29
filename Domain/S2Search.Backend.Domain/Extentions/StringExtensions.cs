using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace S2Search.Backend.Domain.Extentions;

public static class StringExtensions
{
    public static string FirstCharToUpper(this string input)
    {
        if (input.Contains(" "))
        {
            return input;
        }
        else if (string.IsNullOrEmpty(input))
        {
            return input;
        }
        else if (input.All(char.IsDigit))
        {
            return input;
        }
        else
        {
            return input.ToLower().First().ToString().ToUpper() + input.Substring(1);
        }
    }
}