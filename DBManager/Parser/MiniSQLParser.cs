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
            const string selectPattern = @"^SELECT\s+(\*|[a-zA-Z]\w*(,[a-zA-Z]\w*)*)\s+FROM\s+(\w+)(?:\s+WHERE\s+(\w+)(=|<>|<|>|<=|>=)('[^']*'|\d+))?$";

            const string insertPattern = @"^INSERT\s+INTO\s+(\w+)\s+VALUES\s+\(\s*('[^']*'(?:\s*,\s*'[^']*')*)\s*\)\s?$";

            const string dropTablePattern = @"^DROP\s+TABLE\s+(\w+)\s*$";

            //Note: The parsing of CREATE TABLE should accept empty columns "()"
            //And then, an execution error should be given if a CreateTable without columns is executed
            const string createTablePattern = @"^CREATE\s+TABLE\s+([a-zA-Z]\w*)\s*\((.*)\)$";

            const string updateTablePattern = @"^UPDATE\s+(\w+)\s+SET\s+([a-zA-Z]\w*=(?:'[^']*'|\d+)(?:,[a-zA-Z]\w*=(?:'[^']*'|\d+))*)(?:\s+WHERE\s+(\w+)(=|<>|<|>|<=|>=)('[^']*'|\d+))?$";

            const string deletePattern = @"^DELETE\s+FROM\s+(\w+)(?:\s+WHERE\s+(\w+)(=|<>|<=|>=|<|>)('[^']*'|\d+))?$";

            const string createSecurityProfilePattern = @"^CREATE\s+SECURITY\s+PROFILE\s+([a-zA-Z0-9]+)\s*$";
            
            const string dropSecurityProfilePattern = @"^DROP\s+SECURITY\s+PROFILE\s+(\w+)\s*$";

            const string grantPattern = @"^GRANT\s+(DELETE|INSERT|SELECT|UPDATE)\s+ON\s+(\w+)\s+TO\s+([a-zA-Z]+)\s*$";

            const string revokePattern = @"^REVOKE\s+(DELETE|INSERT|SELECT|UPDATE)\s+ON\s+(\w+)\s+TO\s+([a-zA-Z]+)\s*$";

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
            if (match.Success)
            {
                string columnsRaw = match.Groups[1].Value;

                List<string> columns;
                if (columnsRaw == "*")
                    columns = new List<string> { "*" };
                else
                    columns = columnsRaw.Split(',').ToList();

                string table = match.Groups[2].Value;

                Condition where = null;

                if (match.Groups[3].Success)
                {
                    string col = match.Groups[3].Value;
                    string op = match.Groups[4].Value;
                    string val = match.Groups[5].Value.Trim('\'');

                    where = new Condition(col, op, val);
                }

                return new Select(table, columns, where);
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

                if (columnsText != "")
                {
                    if (!Regex.IsMatch(columnsText,@"^[a-zA-Z]\w*\s+(TEXT|INT|DOUBLE)(,[a-zA-Z]\w*\s+(TEXT|INT|DOUBLE))*$"))
                    {
                        return null;
                    }

                    List<string> parts = CommaSeparatedNames(columnsText);
                    foreach (string part in parts)
                    {
                        Match columnMatch = Regex.Match(part,@"^([a-zA-Z]\w*)\s+(TEXT|INT|DOUBLE)$");
                        if (!columnMatch.Success)
                        {
                            return null;
                        } 

                        string columnName = columnMatch.Groups[1].Value;
                        string columnType = columnMatch.Groups[2].Value;

                        ColumnDefinition.DataType type;
                        if (columnType == textType)
                        {
                            type = ColumnDefinition.DataType.String;
                        }                         
                        else if (columnType == intType) 
                        { 
                            type = ColumnDefinition.DataType.Int;
                        }    
                        else if (columnType == doubleType)
                        {
                            type = ColumnDefinition.DataType.Double;
                        }
                        else
                        {
                            return null;
                        }

                        columns.Add(new ColumnDefinition(type, columnName));
                    }
                }

                return new CreateTable(table, columns);

            }

            //UPDATE
            match = Regex.Match(miniSQLQuery, updateTablePattern);
            if (match.Success)
            {
                string table = match.Groups[1].Value;
                string setText = match.Groups[2].Value;

                List<SetValue> values = new List<SetValue>();

                string[] assignments = setText.Split(',');

                foreach (string assignment in assignments)
                {
                    string[] parts = assignment.Split('=');

                    if (parts.Length != 2)
                        return null;

                    string column = parts[0];
                    string rawValue = parts[1];

                    if (rawValue.StartsWith("'"))
                    {
                        if (!rawValue.EndsWith("'"))
                            return null;

                        values.Add(new SetValue(column, rawValue.Substring(1, rawValue.Length - 2)));
                    }
                    else
                    {
                        if (!Regex.IsMatch(rawValue, @"^\d+$"))
                            return null;

                        values.Add(new SetValue(column, rawValue));
                    }
                }

                Condition where = null;

                if (match.Groups[3].Success)
                {
                    string col = match.Groups[3].Value;
                    string op = match.Groups[4].Value;
                    string rawVal = match.Groups[5].Value;

                    if (rawVal.StartsWith("'"))
                    {
                        if (!rawVal.EndsWith("'"))
                            return null;

                        where = new Condition(col, op, rawVal.Substring(1, rawVal.Length - 2));
                    }
                    else
                    {
                        if (!Regex.IsMatch(rawVal, @"^\d+$"))
                            return null;

                        where = new Condition(col, op, rawVal);
                    }
                }

                return new Update(table, values, where);
            }

            //DELETE
            match = Regex.Match(miniSQLQuery, deletePattern);
            if (match.Success)
            {
                string table = match.Groups[1].Value;
                Condition where = null;
                if (match.Groups[2].Success && !string.IsNullOrWhiteSpace(match.Groups[2].Value))
                {
                    where = new Condition(
                        match.Groups[2].Value,
                        match.Groups[3].Value,
                        match.Groups[4].Value.Trim('\'')
                    );
                }
                return new Delete(table, where);
            }

            //TODO DEADLINE 4
            //Do the same for the security queries (CREATE SECURITY PROFILE, ...)

            //ADDUSER
            match = Regex.Match(miniSQLQuery, addUserPattern);
            if (match.Success)
            {
                string user = match.Groups[1].Value;
                string pass = match.Groups[2].Value;
                string profile = match.Groups[3].Value;
                return new AddUser(user, pass, profile);
            }

            //CREATESECURITYPROFILE
            match = Regex.Match(miniSQLQuery, createSecurityProfilePattern);
            if (match.Success)
            {
                string profileName = match.Groups[1].Value;
                return new CreateSecurityProfile(profileName);
            }
            
            //DELETEUSER

            match = Regex.Match(miniSQLQuery, deleteUserPattern);
            if (match.Success)
            {
                string username = match.Groups[1].Value;
                return new DeleteUser(username);
            }

            //DROPSECURITYPROFILE
            match = Regex.Match(miniSQLQuery, dropSecurityProfilePattern);
            if (match.Success)
            {
                string profileName = match.Groups[1].Value;
                return new DropSecurityProfile(profileName);
            }   
            
            //GRANT
            match = Regex.Match(miniSQLQuery, grantPattern);
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

