using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Services.Mappers
{
    public class ObjectToDataTableMapper
    {
        /// <summary>
        /// The order of the class properties you pass to this method must match the order of the Table Type in SQL.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToConvert"></param>
        /// <returns></returns>
        public static DataTable CreateDataTable<T>(T objectToConvert)
        {
            if (objectToConvert is IEnumerable)
            {
                var typeOfObject = GetUnderlyingTypeOfEnumerable(objectToConvert);
                var properties = typeOfObject.GetProperties();
                var dataTable = CreateDataTableForType(properties);

                foreach(var itemAsObject in objectToConvert as IEnumerable)
                {
                    AddPropertyValuesToDataTable(dataTable, itemAsObject, properties);
                }

                return dataTable;
            }
            else
            {
                var properties = typeof(T).GetProperties();
                var dataTable = CreateDataTableForType(properties);

                AddPropertyValuesToDataTable(dataTable, objectToConvert, properties);

                return dataTable;
            }
        }

        private static void AddPropertyValuesToDataTable(DataTable dataTable, object itemAsObject, PropertyInfo[] itemProperties)
        {
            object[] itemValues = new object[itemProperties.Length];
            for (int i = 0; i < itemProperties.Length; i++)
            {
                itemValues[i] = itemProperties[i].GetValue(itemAsObject);
            }

            dataTable.Rows.Add(itemValues);
        }

        private static Type GetUnderlyingTypeOfEnumerable<T>(T objectToConvert)
        {
            Type[] interfaces = objectToConvert.GetType().GetInterfaces();

            var typeOfObject = interfaces.Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                                         .Select(y => y.GetGenericArguments()[0])
                                         .FirstOrDefault();
            return typeOfObject;
        }

        private static DataTable CreateDataTableForType(PropertyInfo[] properties)
        {
            var dataTable = new DataTable();

            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            return dataTable;
        }
    }
}
