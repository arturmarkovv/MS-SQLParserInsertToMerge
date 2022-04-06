using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_SQL_Parser___Insert_To_Merge
{
    class MergeScriptBuilder
    {
        private string _result;
        public void AddData(IDictionary<string, string> dataDict)
        {
            if (dataDict.Count > 0)
            {
                _result += ConvertToMerge(dataDict);
            }
            else
            {
                _result += "GO\n";
            }
        }
        public string GetResult()
        {
            return _result;
        }
        private string ConvertToMerge(IDictionary<string, string> dataDict)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"MERGE {dataDict["tableName"]}  WITH(HOLDLOCK) AS tgt");
            sb.AppendLine(GenerateUsingPart(dataDict));
            sb.AppendLine(GenerateSourcePart(dataDict));
            sb.AppendLine($"\tON tgt.[Id] = src.[Id]");
            sb.AppendLine($"WHEN MATCHED THEN");
            sb.AppendLine($"\tUPDATE");
            sb.AppendLine($"\tSET "+GenerateSetPart(dataDict));
            sb.AppendLine("WHEN NOT MATCHED THEN");
            sb.AppendLine("\t"+GenerateInsertPartFields(dataDict));
            sb.AppendLine("\t"+GenerateInsertPartValues(dataDict));
            return sb.ToString();
        }

        private string GenerateInsertPartValues(IDictionary<string, string> dataDict)
        {
            var sb = new StringBuilder();
            sb.Append($"VALUES (");
            foreach (var data in dataDict)
            {
                if (data.Key != "tableName")
                {
                    sb.Append(data.Value + ",  ");
                }
            }
            var result = sb.ToString().TrimEnd(',', ' ');
            result += ");";
            return result;
        }

        private string GenerateInsertPartFields(IDictionary<string, string> dataDict)
        {
            var sb = new StringBuilder();
            sb.Append($"INSERT (");
            foreach (var data in dataDict)
            {
                if (data.Key != "tableName")
                {
                    sb.Append(data.Key + ", ");
                }
            }
            var result = sb.ToString().TrimEnd(',', ' ');
            result += ")";
            return result;
        }

        private string GenerateSetPart(IDictionary<string, string> dataDict)
        {
            var sb = new StringBuilder();
            var i = 0;
            foreach (var data in dataDict)
            {
                i += 1;
                if (data.Key != "tableName" && data.Key != "[Id]")
                {
                    if (i < dataDict.Keys.Count)
                    {
                        sb.AppendLine($"\t{data.Key} = {data.Value},");
                    }
                    else
                    {
                        sb.Append($"\t{data.Key} = {data.Value}");
                    }
                }
               
            }
            var result = sb.ToString();
            return result;
        }

        private string GenerateSourcePart(IDictionary<string, string> dataDict)
        {
            var sb = new StringBuilder();
            sb.Append($"\tAS src (");
            foreach (var data in dataDict)
            {
                if (data.Key != "tableName")
                {
                    sb.Append(data.Key + ", ");
                }
            }
            var result = sb.ToString().TrimEnd(',', ' ');
            result += ")";
            return result;
        }

        private string GenerateUsingPart(IDictionary<string, string> dataDict)
        {
            var sb = new StringBuilder();
            sb.Append($"USING (VALUES (");
            foreach (var data in dataDict)
            {
                if (data.Key!= "tableName")
                {
                    sb.Append(data.Value + ", ");
                }
            }
            var result = sb.ToString().TrimEnd(',', ' ');
            result += "))";
            return result;
        }
    }
}
/* EXAMPLE
MERGE [SysAdminUnitInRole] AS tgt
USING (VALUES (N'abfe2227-0503-48a3-9352-cd8479428259',N'491f5785-bf17-4e7b-91ef-11989a7c2033',N'040e7d59-d80e-4c98-9364-be3b16a16939', CAST(N'2022-04-04T06:16:08.5470000' AS DateTime2), NULL, CAST(N'2022-04-04T06:16:08.5470000' AS DateTime2), NULL, 0, NULL, 50))
	AS src ([Id], [SysAdminUnitId], [SysAdminUnitRoleId], [CreatedOn], [CreatedById], [ModifiedOn], [ModifiedById], [ProcessListeners], [SourceAdminUnitId], [Source])
	ON tgt.[Id] = src.[Id]
WHEN MATCHED THEN
	UPDATE
	SET tgt.[SysAdminUnitId] = src.[SysAdminUnitId]
WHEN NOT MATCHED THEN 
	INSERT ([Id], [SysAdminUnitId], [SysAdminUnitRoleId], [CreatedOn], [CreatedById], [ModifiedOn], [ModifiedById], [ProcessListeners], [SourceAdminUnitId], [Source]) VALUES (N'abfe2227-0503-48a3-9352-cd8479428259', N'491f5785-bf17-4e7b-91ef-11989a7c2033', N'040e7d59-d80e-4c98-9364-be3b16a16939', CAST(N'2022-04-04T06:16:08.5470000' AS DateTime2), NULL, CAST(N'2022-04-04T06:16:08.5470000' AS DateTime2), NULL, 0, NULL, 50);
*/
