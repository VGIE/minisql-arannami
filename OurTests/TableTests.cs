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
        public void GetRowWhenIndexNeg()
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
        public void DeleteWhere()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "Age")
            };

            Table table = new Table();
            Row r1 = new Row(columns, new List<string> { "Ane", "20" });
            Row r2 = new Row(columns, new List<string> { "Jon", "30" });
            Row r3 = new Row(columns, new List<string> { "Mikel", "40" });
            Row r4 = new Row(columns, new List<string> { "Arene", "30" });

            table.AddRow(r1);
            table.AddRow(r2);
            table.AddRow(r3);
            table.AddRow(r4);
            Condition condition = new Condition("Age", "=", "30");
            table.DeleteWhere(condition);
            Assert.Equal(2, table.NumRows());
            Assert.Equal(r1, table.GetRow(0));
            Assert.Equal(r3, table.GetRow(1));
        }
        [Fact]
        public bool Update(List<SetValue> setValues, Condition condition)
        {
            //if (condition == null)
            // return false;
            // var indices = RowIndicesWhereConditionIsTrue(condition);

            //if (indices.Count == 0)
            // return false;

            //foreach (var idx in indices)
            // {
            // Row row = GetRow(idx);
            //foreach (var sv in setValues)
            //{
            // row.SetValue(sv.ColumnName, sv.Value);
            //}
            //  }

            //return true;
            return false;
        }

        [Fact]
        public void ToString()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>
    {
        new ColumnDefinition(ColumnDefinition.DataType.String, "Name"),
        new ColumnDefinition(ColumnDefinition.DataType.Int, "Age")
    };

            Table table = new Table("People", columns);

            table.AddRow(new Row(columns, new List<string> { "Adolfo", "23" }));
            table.AddRow(new Row(columns, new List<string> { "Jacinto", "24" }));

            string expected = "['Name','Age']{'Adolfo','23'}{'Jacinto','24'}";

            Assert.Equal(expected, table.ToString());
        }

    }
}

