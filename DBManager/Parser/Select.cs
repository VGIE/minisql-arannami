using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbManager 
{
    public class Select: MiniSqlQuery
    {
        public string Table { get; private set; }
        public List<string> Columns { get; private set; }
        public Condition Where { get; private set; }

        public Select(string table, List<string> columns, Condition condition=null)
        {
            //TODO DEADLINE 2: Initialize member variables
            this.Table = table;
            this.Columns = columns ?? new List<string>();
            this.Where = condition;
        }

        public Select()
        {
            Columns = new List<string>();
        }

        public string Execute(Database database)
        {
            //TODO DEADLINE 3: Run the query and return the table as a string (or the last error in the database)
            /*if (database != null) 
            {

            }*/
            return null;
            
        }
    }
}
