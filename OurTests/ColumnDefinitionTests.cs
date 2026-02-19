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


        [Fact]
        public void Decode()
        {
            string encoded = "Name[ARROW]String";

            var decoded = (string)typeof(ColumnDefinition)
                .GetMethod("Decode", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .Invoke(null, new object[] { encoded });
            Assert.Equal("Name->String", decoded);
        }
    }
}

