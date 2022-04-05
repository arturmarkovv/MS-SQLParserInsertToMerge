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
            sb.AppendLine($"MERGE {dataDict["tableName"]}  WITH(HOLDLOCK) AS target");
            sb.AppendLine(GenerateUsingPart(dataDict));
            sb.AppendLine(GenerateSourcePart(dataDict));
            sb.AppendLine($"\tON target.Id = {dataDict["[Id]"]}");
            sb.AppendLine($"WHEN MATCHED THEN");
            sb.AppendLine($"\tUPDATE");
            sb.AppendLine($"\tSET "+GenerateSetPart(dataDict));
            sb.AppendLine("WHEN NOT MATHCED THEN");
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
                    sb.Append(data.Value + ", ");
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
                if (data.Key != "tableName")
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
            var result = sb.ToString().TrimEnd(',','\n');
            return result;
        }

        private string GenerateSourcePart(IDictionary<string, string> dataDict)
        {
            var sb = new StringBuilder();
            sb.Append($"\tAS source (");
            foreach (var data in dataDict)
            {
                if (data.Key != "tableName" || data.Key != "[Id]")
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
                if (data.Key!= "tableName" || data.Key!="[Id]")
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
merge [SysAdminUnit] with(HOLDLOCK) as target
using (VALUES (CAST(N'2022-03-30T05:31:17.5590000' AS DateTime2), N'410006e1-ca4e-4502-a9ec-e54d922d2c00', CAST(N'2022-04-04T09:01:10.6090000' AS DateTime2), N'410006e1-ca4e-4502-a9ec-e54d922d2c00', N'Test4', N'', N'a29a3ba5-4b0d-de11-9a51-005056c00008', NULL, N'', N'', 0, NULL, 1, 0, 0, N'', N'', N'', 0, 0, N'1a778e3f-0a8e-e111-84a3-00155d054c03', 0, N'', N'', NULL, NULL, 0, NULL, 0, NULL, NULL, 0, NULL, N'', N''))
    as source ( [CreatedOn], [CreatedById], [ModifiedOn], [ModifiedById], [Name], [Description], [ParentRoleId], [ContactId], [TimeZoneId], [UserPassword], [SysAdminUnitTypeValue], [AccountId], [Active], [LoggedIn], [SynchronizeWithLDAP], [LDAPEntry], [LDAPEntryId], [LDAPEntryDN], [IsDirectoryEntry], [ProcessListeners], [SysCultureId], [LoginAttemptCount], [SourceControlLogin], [SourceControlPassword], [PasswordExpireDate], [HomePageId], [ConnectionType], [UnblockTime], [ForceChangePassword], [LDAPElementId], [DateTimeFormatId], [SessionTimeout], [PortalAccountId], [Email], [OpenIDSub])
    on target.Id = N'522cdd4d-5f68-4b87-8201-a8ba5c7559de'
when matched then
    update
    set [Name] = source.[Name]
when not matched then
    insert ( [Id], [CreatedOn], [CreatedById], [ModifiedOn], [ModifiedById], [Name], [Description], [ParentRoleId], [ContactId], [TimeZoneId], [UserPassword], [SysAdminUnitTypeValue], [AccountId], [Active], [LoggedIn], [SynchronizeWithLDAP], [LDAPEntry], [LDAPEntryId], [LDAPEntryDN], [IsDirectoryEntry], [ProcessListeners], [SysCultureId], [LoginAttemptCount], [SourceControlLogin], [SourceControlPassword], [PasswordExpireDate], [HomePageId], [ConnectionType], [UnblockTime], [ForceChangePassword], [LDAPElementId], [DateTimeFormatId], [SessionTimeout], [PortalAccountId], [Email], [OpenIDSub] )
    values (N'522cdd4d-5f68-4b87-8201-a8ba5c7559de', CAST(N'2022-03-30T05:31:17.5590000' AS DateTime2), N'410006e1-ca4e-4502-a9ec-e54d922d2c00', CAST(N'2022-04-04T09:01:10.6090000' AS DateTime2), N'410006e1-ca4e-4502-a9ec-e54d922d2c00', N'Test4', N'', N'a29a3ba5-4b0d-de11-9a51-005056c00008', NULL, N'', N'', 0, NULL, 1, 0, 0, N'', N'', N'', 0, 0, N'1a778e3f-0a8e-e111-84a3-00155d054c03', 0, N'', N'', NULL, NULL, 0, NULL, 0, NULL, NULL, 0, NULL, N'', N'');

*/
