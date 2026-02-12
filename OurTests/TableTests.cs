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
    }
}