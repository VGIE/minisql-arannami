using DbManager;

namespace OurTests
{
    public class TableTests
    {
        [Fact]
        public void TableConstructor()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "Age")
            };

            Table table = new Table("Person", columns);

            Assert.Equal("Person", table.Name);
        }

        /*[Fact]
        public void TableConstructorWhenNameNull()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition> 
            { 
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name")
            }; 
            
            Table table = new Table(null, columns); 
            Assert.Null(table.Name); 
            Assert.Equal(1, table.NumColumns());
        }*/

        [Fact]
        public void TableConstructorWhenEmpty() 
        {
            Table table = new Table();
            Assert.Null(table.Name);
            Assert.Equal(0, table.NumColumns());
            Assert.Equal(0, table.NumRows());
        }

        [Fact]
        public void GetRow_Works()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "Age")
            };

            Table table = new Table();

            Row r1 = new Row(columns, new List<string> { "Jacinto", "37" });
            Row r2 = new Row(columns, new List<string> { "Paco", "20" });
            Row r3 = new Row(columns, new List<string> { "Alma", "27" });

            table.AddRow(r1);
            table.AddRow(r2);
            table.AddRow(r3);

            var result1 = table.GetRow(0);
            Assert.Equal(r1, result1);
            var result2 = table.GetRow(1);
            Assert.Equal(r2, result2);

            Assert.NotEqual(result1, result2);
        }

        /*[Fact]
        public void GetRowWhenTableEmpty()
        {
            Table table = new Table();

            var result1 = table.GetRow(0);
            Assert.Null(result1);
        }*/

        /*[Fact]
        public void GetRowWhenIndexNegative()
        {
            Table table = new Table();

            var result1 = table.GetRow(-1);
            Assert.Null(result1);
        }*/

        [Fact]
        public void AddRow_Works() 
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "Age")
            };

            Table table = new Table();

            Row row = new Row(columns, new List<string> { "Jacinto", "37"});
            table.AddRow(row);
            Assert.Equal(1, table.NumRows());
            Assert.Equal(row, table.GetRow(0));
        }

        /*[Fact]
        public void AddRowWhenRowNull()
        {
            Table table = new Table();
            table.AddRow(null);
            Assert.Equal(0, table.NumRows());
        }*/

        [Fact]
        public void AddRowWhenAreNotColumns()
        {
            Table table = new Table();

            Row row = new Row(new List<ColumnDefinition>(), new List<string>());
            table.AddRow(row);
            Assert.Equal(1, table.NumRows());
        }

        [Fact]
        public void NumRows_Works()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "Age")
            };

            Table table = new Table("Person", columns);

            Assert.Equal(0, table.NumRows());

            Row r1 = new Row(columns, new List<string> { "Jacinto", "37" });
            Row r2 = new Row(columns, new List<string> { "Paco", "20" });
            Row r3 = new Row(columns, new List<string> { "Alma", "27" });

            table.AddRow(r1);
            Assert.Equal(1, table.NumRows());
            table.AddRow(r2);
            Assert.Equal(2, table.NumRows());
            table.AddRow(r3);
            Assert.Equal(3, table.NumRows());
        }
    
        [Fact]
        public void ColumnByName()
        {
            var table = Table.CreateTestTable();
            var column = table.ColumnByName("Name"); 
            Assert.NotNull(column);
            Assert.Equal("Name", column.Name);
        }
//         [Fact]
//         public void ColumnByName()
//         {
//             var table = Table.CreateTestTable();
//             var column = table.ColumnByName(Table.TestColumn1Name);
//             Assert.NotNull(column);
//             Assert.Equal(Table.TestColumn1Name, column.Name);
//         }

        [Fact]
        public void ColumnByName_NoHayColumnas()
        {
            var table = Table.CreateTestTable();
            var column = table.ColumnByName("DoesNotExist"); 
            Assert.Null(column);
        }
//         [Fact]
//         public void ColumnByName_NoHayColumnas()
//         {
//             var table = Table.CreateTestTable();
//             var column = table.ColumnByName("DoesNotExist");
//             Assert.Null(column);
//         }


        [Fact]
        public void ColumnIndexByName()
        {
            var table = Table.CreateTestTable();
            int index = table.ColumnIndexByName("Height"); 
            Assert.Equal(1, index); // segunda columna -> índice 1
        }
//         [Fact]
//         public void ColumnIndexByName()
//         {
//             var table = Table.CreateTestTable();
//             int index = table.ColumnIndexByName(Table.TestColumn2Name);
//             Assert.Equal(1, index);
//         }


        [Fact]
        public void ColumnIndexByName_NoHayColumnas()
        {
            var table = Table.CreateTestTable();
            int index = table.ColumnIndexByName("Unknown");
            Assert.Equal(-1, index);
        }

//         [Fact]
//         public void ColumnIndexByName_NoHayColumnas()
//         {
//             var table = Table.CreateTestTable();
//             int index = table.ColumnIndexByName("Unknown");
//             Assert.Equal(-1, index);
//         }


        [Fact]
        public void Select_AllColumns_NoCondition_CheckAll()
        {
            //Checks if when we don't have any condition all columns that are
            //selected are right added into the new table = "Result"

            Table table = Table.CreateTestTable();
            Table result = table.Select(null, null);

            Assert.Equal("Result", result.Name);
            Assert.Equal(table.NumColumns(), result.NumColumns());
            Assert.Equal(table.NumRows(), result.NumRows());

            //two checks, one for the first row and the other for the last
            //to see if their values are all good

            //first row is equal
            Assert.Equal(table.GetRow(0).Values, result.GetRow(0).Values);

            //last row
            int lastIndex = table.NumRows() - 1;
            Assert.Equal(table.GetRow(lastIndex).Values, result.GetRow(lastIndex).Values);
        }

        [Fact]
        public void Select_CheckAllTable()
        {
            Table table = Table.CreateTestTable();
            Table result = table.Select(null,null);

            Assert.Equal("Result", result.Name);
            Assert.Equal(table.NumColumns(), result.NumColumns());
            Assert.Equal(table.NumRows(), result.NumRows());

            //as well as the previous test, with no condition, checks all the
            //positions are correctly valuated
            for(int i=0; i<table.NumRows(); i++)
            {
                Assert.Equal(table.GetRow(i).Values, result.GetRow(i).Values);
                //Assert.Equal(table.GetRow(i).ColumnDefinitions.Count, result.GetRow(i).ColumnDefinitions.Count);
            }
        }

        [Fact]
        public void Select_SpecificColumns_NoCondition()
        {
            Table table = Table.CreateTestTable();
            Table result = table.Select(new List<string> { "Name", "Age" }, null);

            Assert.Equal(2, result.NumColumns());
            Assert.Equal(3, result.NumRows());

            Assert.Equal("Name", result.GetColumn(0).Name);
            Assert.Equal("Age", result.GetColumn(1).Name);

            Assert.Equal("Rodolfo", result.GetRow(0).Values[0]);
            Assert.Equal("25", result.GetRow(0).Values[1]);
        }

        /*[Fact]
        public void Select_WithCondition()
        {
            Table table = Table.CreateTestTable();

            // Example: Age > 50
            Condition condition = new Condition(
                "Age",
                Condition.Operator.Greater,
                "50"
            );

            Table result = table.Select(new List<string> { "Name" }, condition);

            Assert.Equal(1, result.NumColumns());
            Assert.Equal(2, result.NumRows()); // Maider (67), Pepe (51)

            Assert.Equal("Maider", result.GetRow(0).Values[0]);
            Assert.Equal("Pepe", result.GetRow(1).Values[0]);
        }*/

        /*[Fact]
        public void Select_NoRowsMatchCondition()
        {
            Table table = Table.CreateTestTable();

            Condition condition = new Condition(
                "Age",
                Condition.Operator.Less,
                "10"
            );

            Table result = table.Select(new List<string> { "Name" }, condition);

            Assert.Equal(1, result.NumColumns());
            Assert.Equal(0, result.NumRows());
        }*/

    }
}