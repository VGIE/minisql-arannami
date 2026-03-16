using DbManager;
using DbManager.Parser;
using Xunit;

namespace OurTests
{
    public class MiniSQL_Parser_Tests
    {
        [Fact]
        public void Update_Parse_DropTable_CorrectSyntax()
        {
            string query = "DROP TABLE MyTable";

            var result = MiniSQLParser.Parse(query) as DropTable;

            Assert.NotNull(result);
            Assert.Equal("MyTable", result.Table);
        }

        [Fact]
        public void Update_Parse_WithWhere()
        {
            string query = "UPDATE Users SET Name='Juan Perez', Age=30 WHERE ID=1";

            var result = MiniSQLParser.Parse(query) as Update;

            Assert.NotNull(result);
            Assert.Equal("Users", result.Table);
            Assert.Equal(2, result.Columns.Count);

            
            Assert.Equal("Name", result.Columns[0].ColumnName);
            Assert.Equal("Juan Perez", result.Columns[0].Value);

            Assert.NotNull(result.Where);
            Assert.Equal("ID", result.Where.ColumnName);    
            Assert.Equal("=", result.Where.Operator);     
            Assert.Equal("1", result.Where.LiteralValue);
        }

        [Fact]
        public void Update_Parse_WithoutWhere()
        {
            string query = "UPDATE Inventory SET Stock=10";

            var result = MiniSQLParser.Parse(query) as Update;

            Assert.NotNull(result);
            Assert.Equal("Inventory", result.Table);

            Assert.Single(result.Columns);
            Assert.Equal("Stock", result.Columns[0].ColumnName);
            Assert.Equal("10", result.Columns[0].Value);

            Assert.Null(result.Where);
        }

        [Fact]
        public void Update_Parse_InvalidQuery_ReturnsNull()
        {
            string query = "INVALID COMMAND";

            var result = MiniSQLParser.Parse(query);

            Assert.Null(result);
        }

        [Fact]
        public void AddUser_Parse_CorrectSyntaxis()
        {
            string query = "ADD USER (AdminUser,Pass123,AdminProfile)";
            var result = MiniSQLParser.Parse(query) as AddUser;
            Assert.NotNull(result);
            Assert.Equal("AdminUser", result.Username);
            Assert.Equal("Pass123", result.Password);
            Assert.Equal("AdminProfile", result.ProfileName);
        }
    }
}