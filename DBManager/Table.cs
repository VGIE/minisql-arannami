using DbManager.Parser;
using System;
using System.Collections.Generic;

namespace DbManager
{
    public class Table
    {
        private List<ColumnDefinition> ColumnDefinitions = new List<ColumnDefinition>();
        private List<Row> Rows = new List<Row>();
        
        public string Name { get; private set; } = null;

        // public Table(string name, List<ColumnDefinition> columns)
        // {
        //     //TODO DEADLINE 1.A: Initialize member variables
        //     this.Name = name;
        //     this.ColumnDefinitions = columns;
        //     Rows = new List<Row>();
        // }

        public Table(string name, List<ColumnDefinition> columns)
        {
            this.Name = name;
            this.ColumnDefinitions = columns ?? new List<ColumnDefinition>();  // <- asegurar que no sea null
            this.Rows = new List<Row>();
        }

        public Table()
        {
            ColumnDefinitions = new List<ColumnDefinition>();
            Rows = new List<Row>();
        }


        public Row GetRow(int i)
        {
            //TODO DEADLINE 1.A: Return the i-th row
            if (i < 0 || i >= Rows.Count) return null;
                return Rows[i];
        }

        public void AddRow(Row row)
        {
            //TODO DEADLINE 1.A: Add a new row
            Rows.Add(row);
        }

        public int NumRows()
        {
            //TODO DEADLINE 1.A: Return the number of rows
            return Rows.Count;
            
        }

        public ColumnDefinition GetColumn(int i)
        {
            //TODO DEADLINE 1.A: Return the i-th column
            return ColumnDefinitions[i];
            
        }

        public int NumColumns()
        {
            //TODO DEADLINE 1.A: Return the number of columns
            return ColumnDefinitions.Count;
            
        }
        
        // public ColumnDefinition ColumnByName(string column)
        // {
        //     if (column == null) return null;

        //     // foreach (var col in ColumnDefinitions)
        //     // {
        //     //     if (string.Equals(col.Name, column, StringComparison.OrdinalIgnoreCase))
        //     //         return col;
        //     // }
        //     foreach (var col in ColumnDefinitions)
        //     {
        //         Console.WriteLine("DEBUG COLUMN: " + col.Name);
        //     }

        //     return null;
        // }
        public ColumnDefinition ColumnByName(string name)
        {
            int index = ColumnIndexByName(name);
            if (index == -1) return null;
            return ColumnDefinitions[index];
        }

        // public int ColumnIndexByName(string columnName)
        // {
        //     //TODO DEADLINE 1.A: Return the zero-based index of the column named columnName
        //     if (columnName == null) return -1;

        //     for (int i = 0; i < ColumnDefinitions.Count; i++)
        //     {
        //         if (string.Equals(ColumnDefinitions[i].Name, columnName, StringComparison.OrdinalIgnoreCase))
        //             return i;
        //     }

        //     return -1;
        // }
        public int ColumnIndexByName(string name)
        {
            if (string.IsNullOrEmpty(name)) return -1;

            for (int i = 0; i < ColumnDefinitions.Count; i++)
            {
                var colName = ColumnDefinitions[i].Name;
                if (!string.IsNullOrEmpty(colName) && colName.Equals(name, StringComparison.OrdinalIgnoreCase))
                    return i;
            }

            return -1;
        }





        public override string ToString()
        {
            //TODO DEADLINE 1.A: Return the table as a string. The format is specified in the documentation
            //Valid examples:
            //"['Name']{'Adolfo'}{'Jacinto'}" <- one column, two rows
            //"['Name','Age']{'Adolfo','23'}{'Jacinto','24'}" <- two columns, two rows
            //"" <- no columns, no rows
            //"['Name']" <- one column, no rows
            
            return null;
            
        }

        // public void DeleteIthRow(int row)
        // {
        //     //TODO DEADLINE 1.A: Delete the i-th row. If there is no i-th row, do nothing
        //     for (int i=0; i<Rows.Count; i++)
        //     {
        //         if (row == i)
        //         {
        //             Rows[i] = null;
        //         }
        //     }
        // }

        public void DeleteIthRow(int index)
        {
            if (index >= 0 && index < Rows.Count)
            {
                Rows.RemoveAt(index);
            }
        }

        private List<int> RowIndicesWhereConditionIsTrue(Condition condition)
        {
            //TODO DEADLINE 1.A: Returns the indices of all the rows where the condition is true. Check Row.IsTrue()
            var listaIndices = new List<int>();
        
            for (int i=0; i<Rows.Count; i++)
            {
                if (Rows[i] == null) continue;
                if (condition == null || Rows[i].IsTrue(condition))
                {
                    listaIndices.Add(i);
                }
            }
            return listaIndices;
            
        }

        public void DeleteWhere(Condition condition)
        {
            //TODO DEADLINE 1.A: Delete all rows where the condition is true. Check RowIndicesWhereConditionIsTrue()
            
        }

        public Table Select(List<string> columnNames, Condition condition)
        {
            //TODO DEADLINE 1.A: Return a new table (with name 'Result') that contains the result of the select. The condition
            //may be null (if no condition, all rows should be returned). This is the most difficult method in this class
            List<ColumnDefinition> selectedColumns = new List<ColumnDefinition>();
            List<int> selected= new List<int>();

            if (columnNames == null || columnNames.Count == 0)
            {
                for (int i = 0; i < ColumnDefinitions.Count; i++)
                {
                    selected.Add(i);
                    selectedColumns.Add(ColumnDefinitions[i]);
                }
            }
            else
            {
                for (int j=0; j<columnNames.Count; j++)
                {
                    string name = columnNames[j];
                    int index = ColumnIndexByName(name);

                    if(index >= 0)
                    {
                        selected.Add(index);
                        selectedColumns.Add(ColumnDefinitions[index]);
                    }
                }
            }

            Table table = new Table("Result", selectedColumns);

            for(int z=0; z<Rows.Count; z++)
            {
                Row row = Rows[z];
                if (condition==null || row.IsTrue(condition))
                {
                    List<string> newValues = new List<string>();
                    for (int k = 0; k<selected.Count; k++)
                    {
                        newValues.Add(row.Values[selected[k]]);
                    }
                    table.AddRow(new Row(selectedColumns, newValues));
                }
            }

            return table;
            
        }

        // public bool Insert(List<string> values)
        // {
        //     //TODO DEADLINE 1.A: Insert a new row with the values given. If the number of values is not correct, return false. True otherwise
            
        //     return false;
            
        // }
        public bool Insert(List<string> values)
        {
            //TODO DEADLINE 1.A: Insert a new row with the values given. If the number of values is not correct, return false. True otherwise
            /* comprobar 3 cosas
             * 1. si tiene las filas que dice tener pero no las columnas
             * 2. que tenga las columnas que dice tener pero no las filas
             * 3. si no coincide en ninguno
             * 4. los valores no son correctos -> false
            */
            return false;
            
        }


        public bool Update(List<SetValue> setValues, Condition condition)
        {
            //TODO DEADLINE 1.A: Update all the rows where the condition is true using all the SetValues (ColumnName-Value). If condition is null,
            //return false, otherwise return true
            
            return false;
            
        }



        //Only for testing purposes
        public const string TestTableName = "TestTable";
        public const string TestColumn1Name = "Name";
        public const string TestColumn2Name = "Height";
        public const string TestColumn3Name = "Age";
        public const string TestColumn1Row1 = "Rodolfo";
        public const string TestColumn1Row2 = "Maider";
        public const string TestColumn1Row3 = "Pepe";
        public const string TestColumn2Row1 = "1.62";
        public const string TestColumn2Row2 = "1.67";
        public const string TestColumn2Row3 = "1.55";
        public const string TestColumn3Row1 = "25";
        public const string TestColumn3Row2 = "67";
        public const string TestColumn3Row3 = "51";
        public const ColumnDefinition.DataType TestColumn1Type = ColumnDefinition.DataType.String;
        public const ColumnDefinition.DataType TestColumn2Type = ColumnDefinition.DataType.Double;
        public const ColumnDefinition.DataType TestColumn3Type = ColumnDefinition.DataType.Int;
        public static Table CreateTestTable(string tableName = TestTableName)
        {
            Table table = new Table(tableName, new List<ColumnDefinition>()
            {
                new ColumnDefinition(TestColumn1Type, TestColumn1Name),
                new ColumnDefinition(TestColumn2Type, TestColumn2Name),
                new ColumnDefinition(TestColumn3Type, TestColumn3Name)
            });
            table.Insert(new List<string>() { TestColumn1Row1, TestColumn2Row1, TestColumn3Row1 });
            table.Insert(new List<string>() { TestColumn1Row2, TestColumn2Row2, TestColumn3Row2 });
            table.Insert(new List<string>() { TestColumn1Row3, TestColumn2Row3, TestColumn3Row3 });
            return table;
        }

        public void CheckForTesting(List<List<string>> rows)
        {
            if (rows.Count != NumRows())
                throw new Exception($"The table has {NumRows()} rows and {rows.Count} were expected");
            int rowIndex = 0;
            foreach (List<string> row in rows)
            {
                if (GetRow(rowIndex).Values.Count != row.Count)
                    if (rows.Count != NumRows())
                        throw new Exception($"The {rowIndex}-th row has {GetRow(rowIndex).Values.Count} values and {row.Count} were expected");

                for (int columnIndex = 0; columnIndex < row.Count; columnIndex++)
                {
                    if (GetRow(rowIndex).Values[columnIndex] != row[columnIndex])
                        if (rows.Count != NumRows())
                            throw new Exception($"The [{rowIndex},{columnIndex}] element is {GetRow(rowIndex).Values[columnIndex]} instead of {row[columnIndex]}");
                }

                rowIndex++;
            }
        }
    }
}