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

        [Fact]
        public void DeleteParse_String()
        {
            MiniSqlQuery query = MiniSQLParser.Parse("DELETE FROM People WHERE Name = 'Juan'");

            Assert.NotNull(query);
            Assert.IsType<Delete>(query);

            Delete delete = (Delete)query;
            Assert.Equal("People", delete.Table);
            Assert.Equal("Name", delete.Where.ColumnName);
            Assert.Equal("=", delete.Where.Operator);
            Assert.Equal("Juan", delete.Where.LiteralValue);
        }

        [Fact]
        public void DeleteParse_Numeric()
        {
            MiniSqlQuery query = MiniSQLParser.Parse("DELETE FROM People WHERE Age > 25");

            Assert.NotNull(query);
            Assert.IsType<Delete>(query);

            Delete delete = (Delete)query;
            Assert.Equal("People", delete.Table);
            Assert.Equal("Age", delete.Where.ColumnName);
            Assert.Equal(">", delete.Where.Operator);
            Assert.Equal("25", delete.Where.LiteralValue);
        }

        [Fact]
        public void DeleteSyntaxError()
        {
            // Falta FROM, debe devolver null
            MiniSqlQuery query = MiniSQLParser.Parse("DELETE People WHERE Name = 'Juan'");
            Assert.Null(query);
        }

        [Fact]
        public void Parse_DropTable_CorrectSyntax()
        {
            string query = "DROP TABLE MyTable";

            var result = MiniSQLParser.Parse(query) as DropTable;

            Assert.NotNull(result);
            Assert.Equal("MyTable", result.Table);
        }

        [Fact]
        public void Parse_DropTable_InvalidSyntax_ReturnsNull()
        {
            string query = "DROP MyTable";

            var result = MiniSQLParser.Parse(query);

            Assert.Null(result);
        }

        [Fact]
        public void Parse_CreateTable_CorrectSyntax()
        {
            string query = "CREATE TABLE People(Name TEXT, Age INT)";

            var result = MiniSQLParser.Parse(query) as CreateTable;

            Assert.NotNull(result);
            Assert.Equal("People", result.Table);

            Assert.Equal(2, result.ColumnsParameters.Count);

            Assert.Equal("Name", result.ColumnsParameters[0].Name);
            Assert.Equal(ColumnDefinition.DataType.String, result.ColumnsParameters[0].Type);

            Assert.Equal("Age", result.ColumnsParameters[1].Name);
            Assert.Equal(ColumnDefinition.DataType.Int, result.ColumnsParameters[1].Type);
        }

        [Fact]
        public void Parse_CreateTable_EmptyColumns()
        {
            string query = "CREATE TABLE EmptyTable()";

            var result = MiniSQLParser.Parse(query) as CreateTable;

            Assert.NotNull(result);
            Assert.Equal("EmptyTable", result.Table);
            Assert.Empty(result.ColumnsParameters);
        }

        [Fact]
        public void Delete_Execute_Success()
        {
            Database db = Database.CreateTestDatabase();
            Condition where = new Condition("Age", "=", "67");
            Delete delete = new Delete(Table.TestTableName, where);

            string result = delete.Execute(db);

            Assert.Equal(Constants.DeleteSuccess, result);
            db.CheckForTesting(Table.TestTableName, new List<List<string>>
            {
                new List<string> { Table.TestColumn1Row1, Table.TestColumn2Row1, Table.TestColumn3Row1 },
                new List<string> { Table.TestColumn1Row3, Table.TestColumn2Row3, Table.TestColumn3Row3 }
            });
        }

        [Fact]
        public void Delete_Execute_TableDoesNotExist()
        {
            Database db = Database.CreateTestDatabase();
            Condition where = new Condition("Age", "=", "67");
            Delete delete = new Delete("TablaQueNoExiste", where);

            string result = delete.Execute(db);

            Assert.Equal(Constants.TableDoesNotExistError, result);
        }

        [Fact]
        public void Delete_Execute_ColumnDoesNotExist()
        {
            Database db = Database.CreateTestDatabase();
            Condition where = new Condition("ColumnaQueNoExiste", "=", "67");
            Delete delete = new Delete(Table.TestTableName, where);

            string result = delete.Execute(db);

            Assert.Equal(Constants.ColumnDoesNotExistError, result);
        }

        [Fact]
        public void Delete_Execute_NullWhere_DeletesAll()
        {
            Database db = Database.CreateTestDatabase();
            Delete delete = new Delete(Table.TestTableName, null);

            string result = delete.Execute(db);

            Assert.Equal(Constants.DeleteSuccess, result);
            Assert.Equal(0, db.TableByName(Table.TestTableName).NumRows());
        }

        [Fact]
        public void Select()
        {
            Database db = Database.CreateTestDatabase();
        }

        [Fact]
        public void Insert()
        {
            Database db = Database.CreateTestDatabase();
        }
    }
}