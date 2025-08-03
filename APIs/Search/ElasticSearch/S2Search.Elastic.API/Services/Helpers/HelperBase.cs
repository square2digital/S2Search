﻿using HashDepot;
using System.Text;

namespace Services.Helpers
{
    public static class HelperBase
    {
        public static bool DoesKeyMatchDefaultProperties(string key)
        {
            return (key.ToLower() == "Id".ToLower()
                 || key.ToLower() == "Title".ToLower()
                 || key.ToLower() == "Subtitle".ToLower()
                 || key.ToLower() == "Price".ToLower()
                 || key.ToLower() == "City".ToLower()
                 || key.ToLower() == "ImageURL".ToLower()
                 || key.ToLower() == "LinkUrl".ToLower());
        }
    }
}
