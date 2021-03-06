using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MS_SQL_Parser___Insert_To_Merge
{
    public static class Parser
    {
        public static IDictionary<string, string> GetNodes(string inputData)
        {
            var nodesDict = new Dictionary<string, string>();

            if (inputData.Contains("GO"))
            {
                return nodesDict;
            }
            string namePattern = @"INSERT (\[\w+\])";
            string fieldPattern = @"(\[\w+\])(\,|\))";
            string valuesPattern = @"VALUES \((.*)\)";

            
            var nameMatch = Regex.Match(inputData, namePattern);
            var tableName = nameMatch.Groups[1].Value;

            var fieldMatches = Regex.Matches(inputData, fieldPattern);

            var valuesStr = Regex.Match(inputData, valuesPattern).Groups[1].Value;
            var valueMatches = valuesStr.Split(", ");

            var fields = new List<string>();
            var values = new List<string>();

            for (int i = 0; i < fieldMatches.Count; i++)
            {
                try
                {
                    fields.Add(fieldMatches[i].Groups[1].Value); //из группы без запятых
                    values.Add(valueMatches[i]);
                }
                catch 
                {
                    throw new Exception("Mismatch between the number of values and fields");
                }
            }
            if (tableName == string.Empty)
            {
                throw new Exception("Missed Table Name");
            }
            if (values.Count != fields.Count)
            {
                throw new Exception("Missed Data");
            }
            nodesDict.Add("tableName", tableName);
            for (int i = 0; i < fields.Count; i++)
            {
                nodesDict.Add(fields[i], values[i]);
            }
            return nodesDict;
        }
    }
}