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


        [Fact]
        public void GetValue()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "Age")
            };

            Row row = new Row(columns, new List<string> { "Jacinto", "37" });

            var result1 = row.GetValue("Name");
            Assert.Equal("Jacinto", result1);

            var result2 = row.GetValue("Age");
            Assert.Equal("37", result2);

            Assert.NotEqual(result1, result2);
        }


        [Fact]
        public void SetValue()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "Age")
            };

            Row row = new Row(columns, new List<string> { "Jacinto", "37" });

            row.SetValue("Age", "25");

            var result = row.GetValue("Age");
            Assert.Equal("25", result);
        }


        [Fact]
        public void IsTrue()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "Age")
            };

            Row row = new Row(columns, new List<string> { "Jacinto", "37" });

            Condition condition = new Condition("Name", "=", "Jacinto");

            bool result = row.IsTrue(condition);

            Assert.True(result);
        }


        [Fact]
        public void IsTrue_()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>
            {
            new ColumnDefinition(ColumnDefinition.DataType.String, "Name"),
            new ColumnDefinition(ColumnDefinition.DataType.Int, "Age")
                };

            Row row = new Row(columns, new List<string> { "Jacinto", "37" });

            Condition condition = new Condition("Name", "=", "Pedro");

            bool result = row.IsTrue(condition);

            Assert.False(result);
        }


        [Fact]
        public void IsTrue_ColumnDoesNotExist()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name")
            };

            Row row = new Row(columns, new List<string> { "Jacinto" });

            Condition condition = new Condition("Age", "=", "37");

            bool result = row.IsTrue(condition);

            Assert.False(result);
        }

        //[Fact]
        //public void Decode()
        //{
        //string encoded = "Value1[SEPARATOR]Value2";
        //var decoded = (string)typeof(Row)
        //.GetMethod("Decode", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
        //.Invoke(null, new object[] { encoded });
        //Assert.Equal("Value1:Value2", decoded);
        // }
        [Fact]
        public void Decode_Works()
        {
            string encoded = "Value1[SEPARATOR]Value2";
            var method = typeof(Row).GetMethod(
                "Decode",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static
            );

            string decoded = (string)method.Invoke(null, new object[] { encoded });
            Assert.Equal("Value1:Value2", decoded);
        }


    }
}

