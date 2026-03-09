using DbManager.Parser;
using System.Collections.Generic;

namespace DbManager
{
    public class Update: MiniSqlQuery
    {
        public string Table { get; private set; }
        public List<SetValue> Columns { get; private set; }
        public Condition Where { get; private set; }

        public Update(string table, List<SetValue> columnNames, Condition where)
        {
            //TODO DEADLINE 2: Initialize member variables
            this.Table = table;
            this.Columns = columnNames;
            this.Where = where;
            
        }

        public string Execute(Database database)
        {
            //TODO DEADLINE 3: Run the query and return the appropriate message
            //UpdateSuccess or the last error in the database
            bool result = database.Update(Table, Columns, Where);

            if (result)
            {
                return Constants.UpdateSuccess;
            }
            else
            {
                return database.LastErrorMessage;
            }

        }

       
    }
}