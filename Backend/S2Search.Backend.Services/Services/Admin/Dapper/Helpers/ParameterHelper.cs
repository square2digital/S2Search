using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace S2Search.Backend.Services.Services.Admin.Dapper.Helpers
{
    public static class ParameterHelper
    {
        /// <summary>
        /// This will parse the parameters to ensure it is our accepted type and return them as a DynamicParameter type for dapper to consume.
        /// This method also handles parsing of Table value types if passed a parameter object that contains a DataTable type.
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <returns>DynamicParameters</returns>
        public static object ParseParameters(string procedureName, object parameters)
        {
            ValidateParameters(parameters);

            var dynamicParameters = ConvertToDynamicParameters(procedureName, parameters);

            return dynamicParameters;
        }

        private static DynamicParameters ConvertToDynamicParameters(string procedureName, object parameters)
        {
            var dynamicParameters = new DynamicParameters();

            foreach (var param in (Dictionary<string, object>)parameters)
            {
                var isDataTable = CheckParameterForDataTable(param);

                if (isDataTable)
                {
                    AddDynamicParameterAsTableValuedParameter(procedureName, dynamicParameters, param);
                }
                else
                {
                    AddDynamicParameter(dynamicParameters, param);
                }
            }

            return dynamicParameters;
        }

        private static void AddDynamicParameter(DynamicParameters dynamicParameters, KeyValuePair<string, object> param)
        {
            dynamicParameters.Add(param.Key, param.Value);
        }

        private static void AddDynamicParameterAsTableValuedParameter(string procedureName, DynamicParameters dynamicParameters, KeyValuePair<string, object> param)
        {
            var schemaName = CheckForSchemaName(procedureName);
            var tableTypeFullName = GetTableVariableTypeName(schemaName, param.Key);

            dynamicParameters.Add(param.Key, ((DataTable)param.Value).AsTableValuedParameter(tableTypeFullName));
        }

        private static string CheckForSchemaName(string procedureName)
        {
            if (procedureName.Contains("."))
            {
                return procedureName.Split('.').First().Replace("[", "").Replace("]", "");
            }

            return null;
        }

        private static bool CheckParameterForDataTable(KeyValuePair<string, object> parameter)
        {
            bool isDataTable = parameter.Value.GetType().IsAssignableFrom(typeof(DataTable));
            return isDataTable;
        }

        private static bool CheckParametersForDictionary(object parameters)
        {
            return parameters is Dictionary<string, object>;
        }

        private static string GetTableVariableTypeName(string schemaName, string typeName)
        {
            if (string.IsNullOrWhiteSpace(schemaName))
            {
                return $"[{typeName}]";
            }

            return $"[{schemaName}].[{typeName}]";
        }

        private static void ValidateParameters(object parameters)
        {
            bool isDictionary = CheckParametersForDictionary(parameters);

            if (!isDictionary)
            {
                throw new Exception("Parameter object is not of type Dictionary<string, object>");
            }
        }

    }
}
