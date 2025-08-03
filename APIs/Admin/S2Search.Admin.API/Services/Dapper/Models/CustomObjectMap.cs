using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Dapper.Models
{
    public class CustomObjectMap
    {
        public Type Type { get; private set; }
        public bool IsList { get; private set; }
        public string PropertyName { get; private set; }

        public CustomObjectMap(Type type, bool isList, string propertyName)
        {
            Type = type;
            IsList = isList;
            PropertyName = propertyName;
        }
    }
}
