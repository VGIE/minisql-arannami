using DbManager.Parser;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DbManager
{
    public class MiniSQLParser
    {
        public static MiniSqlQuery Parse(string miniSQLQuery)
        {
            //TODO DEADLINE 2
            const string selectPattern = @"SELECT\s\FROM\s(\s\WHERE\s)?";

            const string insertPattern = @"INSERT\s+INTO\s+VALUES\s";
            
            const string dropTablePattern = @"^DROP\s+TABLE\s+(\w+)\s*$";

            //Note: The parsing of CREATE TABLE should accept empty columns "()"
            //And then, an execution error should be given if a CreateTable without columns is executed
            const string createTablePattern = @"^CREATE\s+TABLE\s+(\w+)\s*\((.*)\)\s*$";

            const string updateTablePattern = @"^UPDATE\s+(\w+)\s+SET\s+(.+?)(?:\s+WHERE\s+(\w+)\s*(=|<>|<|>|<=|>=)\s*(.+))?\s*$";


            const string deletePattern = null;

            //TODO DEADLINE 4
            const string createSecurityProfilePattern = null;
            
            const string dropSecurityProfilePattern = null;
            
            const string grantPattern = null;
            
            const string revokePattern = null;
            
            const string addUserPattern = null;
            
            const string deleteUserPattern = null;


            //TODO DEADLINE 2
            //Parse query using the regular expressions above one by one. If there is a match, create an instance of the query with the parsed parameters
            //For example, if the query is a "SELECT ...", there should be a match with selectPattern. We would create and return an instance of Select
            //initialized with the table name, the columns, and (possibly) an instance of Condition.
            //If there is no match, it means there is a syntax error. We will return null.

            Match match;

            //DROPTABLE
            match = Regex.Match(miniSQLQuery, dropTablePattern, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                string table = match.Groups[1].Value;
                return new DropTable(table);
            }

            //CREATETABLE
            match = Regex.Match(miniSQLQuery, createTablePattern, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                
            }

            // UPDATETABLE
            match = Regex.Match(miniSQLQuery, updateTablePattern, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                string table = match.Groups[1].Value;
                string setText = match.Groups[2].Value;

                List<SetValue> values = new List<SetValue>();
                var assignmentMatches = Regex.Matches(setText, @"(\w+)\s*=\s*('[^']*'|[^,]+)");
                foreach (Match am in assignmentMatches)
                {
                    string column = am.Groups[1].Value.Trim();
                    string value = am.Groups[2].Value.Trim().Trim('\'');
                    values.Add(new SetValue(column, value));
                }

                Condition condition = null;

                if (match.Groups[3].Success)
                {
                    string column = match.Groups[3].Value;
                    string op = match.Groups[4].Value;
                    string value = match.Groups[5].Value;
                    condition = new Condition(column, op, value);
                }

                return new Update(table, values, condition);
            }

            //TODO DEADLINE 4
            //Do the same for the security queries (CREATE SECURITY PROFILE, ...)


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
