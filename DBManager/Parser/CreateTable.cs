using System;
using System.Collections.Generic;
using System.Text;
using DbManager.Parser;

namespace DbManager
{
 
    public class CreateTable : MiniSqlQuery
    {
        public string Table { get; private set; }
        public List<ColumnDefinition> ColumnsParameters { get; private set; } = new List<ColumnDefinition>();

        public CreateTable(string table, List<ColumnDefinition> columns)
        {
            //TODO DEADLINE 2: Initialize member variables
            Table = table;
            ColumnsParameters = columns;

        }
        public string Execute(Database database)
        {
            //TODO DEADLINE 3: Run the query and return the appropriate message
            //CreateTableSuccess or the last error in the database

            database.CreateTable(Table, ColumnsParameters);
            return database.LastErrorMessage;

        }

    }
}
