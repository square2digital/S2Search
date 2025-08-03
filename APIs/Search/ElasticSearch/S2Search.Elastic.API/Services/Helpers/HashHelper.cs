﻿using HashDepot;
using System.Text;

namespace Services.Helpers
{
    public static class HashHelper
    {
        public static string GetXXHashString(string inputString)
        {
            // see for details -> https://github.com/ssg/HashDepot#xxhash
            var buffer = Encoding.UTF8.GetBytes(inputString);
            ulong result = XXHash.Hash64(buffer);

            return result.ToString();
        }
    }
}
