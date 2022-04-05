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
            //string pattern = @"(?'Name'\[\w+\]\.\[\w+\])|(?'Fields'\[\w+\]\,?)|(?'Values'N\'.*?\'\,?|CAST\(N\'.*?\' AS DateTime2\)\,?|NULL\,?|\d+?\,?)";
            string namePattern = @"INSERT (\[\w+\])";
            string fieldPattern = @"\[\w+\]\,?";
            string valuePattern = @"N\'.*?\'\,?|CAST\(N\'.*?\' AS DateTime2\)\,?|NULL\,?|\d+?\,?";
            //Regex regex = new(pattern);

            var nameMatch = Regex.Match(inputData, namePattern);
            var tableName = nameMatch.Groups[1].Value;

            var fieldMatches = Regex.Matches(inputData, fieldPattern);
            var valueMatches = Regex.Matches(inputData, valuePattern);

            var fields = new List<string>();
            var values = new List<string>();

            for (int i = 1; i < fieldMatches.Count; i++)
            {
                //Сдвиг из-за того, что имя таблицы тоже добавляется в поля
                try
                {
                    fields.Add(fieldMatches[i].Value.TrimEnd(','));
                    values.Add(valueMatches[i-1].Value.TrimEnd(','));
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