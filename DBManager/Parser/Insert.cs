using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DbManager
{
    public class Insert: MiniSqlQuery
    {
        public string Table { get; private set; }
        public List<string> Values { get; private set; }
        public Insert(string table, List<string> values)
        {
            //TODO DEADLINE 2: Initialize member variables
            this.Table = table;
            this.Values = values ?? new List<string>();
        }

        public Insert()
        {

        }

        public string Execute(Database database)
        {
            //TODO DEADLINE 3: Run the query and return the appropriate message
            //InsertSuccess or the last error in the database
            
            return null;
            
        }
    }
}
