using DbManager;

namespace OurTests
{
    public class RowTests
    {
        [Fact]
        public void RowConstructor()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>();
            List<string> values = new List<string> { "Name" };

            Row row = new Row(columns, values);

            Assert.Equal("Name", row.Values[0]);
        }


    }
}