using DbManager;

namespace OurTests
{
    public class ColumnDefinitionsTests
    {
        [Fact]
        public void ColumnDefinitionConstructor()
        {
            ColumnDefinition column = new ColumnDefinition(ColumnDefinition.DataType.String, "Name");

            Assert.Equal("Name", column.Name);
            Assert.Equal(ColumnDefinition.DataType.String, column.Type);
        }
    }
}