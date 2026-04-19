using DbManager.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DbManager
{
    public class MiniSQLParser
    {
        public static MiniSqlQuery Parse(string miniSQLQuery)
        {
            //TODO DEADLINE 2
            const string selectPattern = @"^SELECT\s+(.+?)\s+FROM\s+(\w+)(?:\s+WHERE\s+(.*)\s*)?$";

            const string insertPattern = @"^INSERT\s+INTO\s+(\w+)\s+VALUES\s+\(\s*('[^']*'(?:\s*,\s*'[^']*')*)\s*\)\s?$";

            const string dropTablePattern = @"^DROP\s+TABLE\s+(\w+)\s*$";

            //Note: The parsing of CREATE TABLE should accept empty columns "()"
            //And then, an execution error should be given if a CreateTable without columns is executed
            const string createTablePattern = @"^CREATE\s+TABLE\s+(\w+)\s*\((.*)\)\s*$";

            const string updateTablePattern = @"^UPDATE\s+(\w+)\s+SET\s+(.+?)(?:\s+WHERE\s+(\w+)(=|<>|<|>|<=|>=)('[^']*'|\d+(?:\.\d+)?))?\s*$";

            const string deletePattern = @"^DELETE\s+FROM\s+(\w+)\s+WHERE\s+(\w+)\s*(=|<>|!=|<=|>=|<|>)\s*'?([^']+)'?\s*$";

            //TODO DEADLINE 4 
            const string createSecurityProfilePattern = null;
            
            const string dropSecurityProfilePattern = null;
            
            const string grantPattern = @"^GRANT\s+(\w+)\s+ON\s+(\w+)\s+TO\s+(\w+)\s*$";
            
            const string revokePattern = @"^REVOKE\s+([A-Z]+)\s+ON\s+(\w+)\s+TO\s+([a-zA-Z]+)\s*$";
            
            const string addUserPattern = @"^ADD\s+USER\s*\(([a-zA-Z]+),([^,]+),([a-zA-Z]+)\)\s*$";
            
            const string deleteUserPattern = @"^DELETE\s+USER\s+([a-zA-Z]+)\s*$";


            //TODO DEADLINE 2
            //Parse query using the regular expressions above one by one. If there is a match, create an instance of the query with the parsed parameters
            //For example, if the query is a "SELECT ...", there should be a match with selectPattern. We would create and return an instance of Select
            //initialized with the table name, the columns, and (possibly) an instance of Condition.
            //If there is no match, it means there is a syntax error. We will return null.

            Match match;

            //SELECT
            match = Regex.Match(miniSQLQuery, selectPattern);
            var newSelect = new Select();
            if (match.Success)
            {
                string columnNames = match.Groups[1].Value;
                string[] columnsSelect = columnNames.Split(',').Select(c => c.Trim()).ToArray();
                
                string tableSelect = match.Groups[2].Value;

                Condition conditionsParse = null;

                if (match.Groups[3].Success && !string.IsNullOrWhiteSpace(match.Groups[3].Value)) 
                {
                    string conditions = match.Groups[3].Value;
                    string[] eachCondition = conditions.Split(",");

                    foreach (var condition in eachCondition)
                    {
                        var conditionMatch = Regex.Match(condition.Trim(), @"(\w+)\s*(<=|>=|=|<|>)\s*['""]?([^'""]+)['""]?$");
                        if (conditionMatch.Success)
                        {
                            string col = conditionMatch.Groups[1].Value;
                            string op = conditionMatch.Groups[2].Value;
                            string val = conditionMatch.Groups[3].Value;

                            conditionsParse = new Condition(col, op, val);
                        }
                    }
                }

                return new Select(tableSelect, columnsSelect.ToList(), conditionsParse);
            }

            //INSERT
            match = Regex.Match(miniSQLQuery, insertPattern);
            if (match.Success)
            {
                string tableName = match.Groups[1].Value;

                string literalValues = match.Groups[2].Value;
                List<string> values = literalValues.Split(',').Select(v => v.Trim().Trim('\'')).ToList();
                if(!literalValues.Contains("'"))
                {
                    return null;
                }

                return new Insert(tableName, values);
            }

            //DROPTABLE
            match = Regex.Match(miniSQLQuery, dropTablePattern);
            if (match.Success)
            {
                string table = match.Groups[1].Value;
                return new DropTable(table);
            }

            //CREATETABLE
            match = Regex.Match(miniSQLQuery, createTablePattern);
            if (match.Success)
            {
                const string textType = "TEXT";
                const string intType = "INT";
                const string doubleType = "DOUBLE";

                string table = match.Groups[1].Value;
                string columnsText = match.Groups[2].Value;

                List<ColumnDefinition> columns = new List<ColumnDefinition>();

                if (columnsText.Trim().Length != 0)
                {
                    string[] parts = columnsText.Split(',');

                    foreach (string part in parts)
                    {
                        string[] columnParts = Regex.Split(part.Trim(), @"\s+");

                        if (columnParts.Length != 2)
                            return null;

                        string columnName = columnParts[0].Trim();
                        string columnType = columnParts[1].Trim();

                        ColumnDefinition.DataType type;

                        if (columnType == textType)
                            type = ColumnDefinition.DataType.String;
                        else if (columnType == intType)
                            type = ColumnDefinition.DataType.Int;
                        else if (columnType == doubleType)
                            type = ColumnDefinition.DataType.Double;
                        else
                            return null;

                        columns.Add(new ColumnDefinition(type, columnName));
                    }
                }

                return new CreateTable(table, columns);
            }


            //UPDATE
            match = Regex.Match(miniSQLQuery, updateTablePattern);
            if (match.Success)
            {
                if (miniSQLQuery.Contains("  "))
                    return null;

                string table = match.Groups[1].Value;
                string setText = match.Groups[2].Value;

                List<SetValue> values = new List<SetValue>();
                string[] assignments = setText.Split(',');

                foreach (string assignment in assignments)
                {
                    var setMatch = Regex.Match(assignment.Trim(), @"^(\w+)=('[^']*'|\d+(?:\.\d+)?)$");

                    if (!setMatch.Success) return null;

                    string column = setMatch.Groups[1].Value;
                    string value = setMatch.Groups[2].Value.Trim('\'');

                    values.Add(new SetValue(column, value));
                }

                Condition condition = null;

                if (match.Groups[3].Success)
                {
                    string col = match.Groups[3].Value;
                    string op = match.Groups[4].Value;
                    string val = match.Groups[5].Value.Trim('\'');

                    condition = new Condition(col, op, val);
                }

                return new Update(table, values, condition);
            }

            //DELETE
            match = Regex.Match(miniSQLQuery, deletePattern, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                string table = match.Groups[1].Value;
                Condition where = null;
                if (match.Groups[2].Success)
                {
                    where = new Condition(
                        match.Groups[2].Value,
                        match.Groups[3].Value,
                        match.Groups[4].Value
                    );
                }
                return new Delete(table, where);
            }

            //TODO DEADLINE 4
            //Do the same for the security queries (CREATE SECURITY PROFILE, ...)

            //ADDUSER
            match = Regex.Match(miniSQLQuery, addUserPattern, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                string user = match.Groups[1].Value;
                string pass = match.Groups[2].Value;
                string profile = match.Groups[3].Value;
                return new AddUser(user, pass, profile);
            }

            //CREATESECURITYPROFILE

            //DELETEUSER

            match = Regex.Match(miniSQLQuery, deleteUserPattern);
            if (match.Success)
            {
                string username = match.Groups[1].Value;
                return new DeleteUser(username);
            }

            //DROPSECURITYPROFILE

            //GRANT
            match = Regex.Match(miniSQLQuery, grantPattern, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                string privilege = match.Groups[1].Value;
                string table = match.Groups[2].Value;
                string profile = match.Groups[3].Value;

                return new Grant(privilege, table, profile);
            }

            //REVOKE
            match = Regex.Match(miniSQLQuery, revokePattern);
            if (match.Success)
            {
                string privilege = match.Groups[1].Value;
                string table = match.Groups[2].Value;
                string profile = match.Groups[3].Value;

                return new Revoke(privilege, table, profile);
            }

            return null;
        }

        static List<string> CommaSeparatedNames(string text)
        {
            string[] textParts = text.Split(",", System.StringSplitOptions.RemoveEmptyEntries);
            List<string> commaSeparator = new List<string>();
            for(int i=0; i < textParts.Length; i++)
            {
                commaSeparator.Add(textParts[i]);
            }
            return commaSeparator;
        }
        
    }
}

