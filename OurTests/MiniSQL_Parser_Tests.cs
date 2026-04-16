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
            // Falta FROM debe devolver null
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
        public void Parse_DropTable_IncorrectCapitalization_ReturnsNull()
        {
            string query = "drop table MyTable";

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
        public void Parse_CreateTable_IncorrectCapitalization_ReturnsNull()
        {
            string query = "create table People(Name TEXT)";

            var result = MiniSQLParser.Parse(query);

            Assert.Null(result);
        }

        [Fact]
        public void Parse_CreateTable_WithoutTableKeyword_ReturnsNull()
        {
            string query = "CREATE People(Name TEXT)";

            var result = MiniSQLParser.Parse(query);

            Assert.Null(result);
        }

        [Fact]
        public void Parse_CreateTable_InvalidType_ReturnsNull()
        {
            string query = "CREATE TABLE People(Name BADTYPE)";

            var result = MiniSQLParser.Parse(query);

            Assert.Null(result);
        }

        [Fact]
        public void Parse_CreateTable_LowercaseType_ReturnsNull()
        {
            string query = "CREATE TABLE People(Name text)";

            var result = MiniSQLParser.Parse(query);

            Assert.Null(result);
        }

        [Fact]
        public void Parse_CreateTable_ColumnWithoutType_ReturnsNull()
        {
            string query = "CREATE TABLE People(Name)";

            var result = MiniSQLParser.Parse(query);

            Assert.Null(result);
        }

        [Fact]
        public void Parse_CreateTable_IncorrectWithMultipleColumnsAndSpaces()
        {
            string query = "CREATE TABLE People(Name  TEXT,   Age  INT)";

            var result = MiniSQLParser.Parse(query) as CreateTable;

            Assert.NotNull(result);
            Assert.Equal("People", result.Table);
            Assert.Equal(2, result.ColumnsParameters.Count);
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
        public void Select_Parse_WithoutWhere()
        {
            string query = "SELECT Name FROM Users";
            var result = MiniSQLParser.Parse(query) as Select;

            Assert.NotNull(result);
            Assert.Equal("Users", result.Table);
            Assert.Equal(1, result.Columns.Count);
            Assert.Equal("Name", result.Columns[0]);
            Assert.Null(result.Where);
        }

        [Fact]
        public void Select_Parse_WithoutWhere_MultipleColumns()
        {
            string query = "SELECT Name,Age FROM Users";
            var result = MiniSQLParser.Parse(query) as Select;

            Assert.NotNull(result);
            Assert.Equal("Users", result.Table);
            Assert.Equal(2, result.Columns.Count);
            Assert.Equal("Name", result.Columns[0]);
            Assert.Equal("Age", result.Columns[1]);
            Assert.Null(result.Where);
        }

        [Fact]
        public void Select_Parse_WithWhere_NumericCondition()
        {
            string query = "SELECT Name,Age FROM Users WHERE Age>='18'";
            var result = MiniSQLParser.Parse(query) as Select;

            Assert.NotNull(result);
            Assert.Equal("Users", result.Table);

            Assert.Equal(2, result.Columns.Count);
            Assert.Equal("Name", result.Columns[0]);
            Assert.Equal("Age", result.Columns[1]);

            Assert.NotNull(result.Where);
            Assert.Equal("Age", result.Where.ColumnName);
            Assert.Equal(">=", result.Where.Operator);
            Assert.Equal("18", result.Where.LiteralValue);
        }

        [Fact]
        public void Select_Parse_WithWhere_StringCondition()
        {
            string query = "SELECT * FROM Users WHERE City='Madrid'";
            var result = MiniSQLParser.Parse(query) as Select;

            Assert.NotNull(result);
            Assert.Equal("Users", result.Table);
            Assert.NotNull(result.Where);
            Assert.Equal("City", result.Where.ColumnName);
            Assert.Equal("=", result.Where.Operator);
            Assert.Equal("Madrid", result.Where.LiteralValue);
        }

        [Fact]
        public void Select_Parse_InvalidQuery()
        {
            string query1 = "SELECT Name,Age WHERE 'Age'>'18'";
            string query2 = "SELECT  Name WHERE 'Age'>'18'";
            string query3 = "SELECT Name  WHERE 'Age'>'18'";

            var result1 = MiniSQLParser.Parse(query1);
            var result2 = MiniSQLParser.Parse(query2);
            var result3 = MiniSQLParser.Parse(query3);

            Assert.Null(result1);
            Assert.Null(result2);
            Assert.Null(result3);
        }

        [Fact]
        public void Select_Parse_IncorrectSelectWithMultipleColumnsAndSpacesBetweenColumns()
        {
            string query1 = "SELECT  Name,Age";
            string query2 = "SELECT Name Age WHERE 'Age'>'18'";
            string query3 = "SELECT Name,Age  WHERE 'Age'>'18'";
            string query4 = "SELECT Name,Age WHERE  'Age'>'18'";

            var result1 = MiniSQLParser.Parse(query1);
            var result2 = MiniSQLParser.Parse(query2);
            var result3 = MiniSQLParser.Parse(query3);
            var result4 = MiniSQLParser.Parse(query4);

            Assert.Null(result1);
            Assert.Null(result2);
            Assert.Null(result3);
            Assert.Null(result4);
        }

        /*[Fact]
        public void Select_Parse_ColumnNotFound()
        {
            var db = new Database();
            var table = new Table("Users", this.olumns);
            table.ColumnByName("Name");
            
        }*/

        [Fact]
        public void Insert_Parse_CorrectQuery()
        {
            string query = "INSERT INTO Users VALUES ('Juan','25')";
            var result = MiniSQLParser.Parse(query) as Insert;

            Assert.NotNull(result);
            Assert.Equal("Users", result.Table);

            Assert.Equal(2, result.Values.Count);
            Assert.Equal("Juan", result.Values[0]);
            Assert.Equal("25", result.Values[1]);
        }

        [Fact]
        public void Insert_Parse_SingleColumn()
        {
            string query = "INSERT INTO Users VALUES ('Maria')";
            var result = MiniSQLParser.Parse(query) as Insert;

            Assert.NotNull(result);
            Assert.Equal("Users", result.Table);

            Assert.Single(result.Values);
            Assert.Equal("Maria", result.Values[0]);
        }

        [Fact]
        public void Insert_Parse_NumericValues()
        {
            string query = "INSERT INTO Numbers VALUES ('10','20')";
            var result = MiniSQLParser.Parse(query) as Insert;

            Assert.NotNull(result);
            Assert.Equal("Numbers", result.Table);

            Assert.Equal("10", result.Values[0]);
            Assert.Equal("20", result.Values[1]);
        }

        [Fact]
        public void Insert_Parse_MissingValues_ReturnsNull()
        {
            string query = "INSERT INTO Users ('Juan','25')";
            var result = MiniSQLParser.Parse(query);

            Assert.Null(result);
        }

        [Fact]
        public void Insert_Parse_InvalidQuery_ReturnsNull()
        {
            string query = "INSERT Users VALUES 'Juan'";
            var result = MiniSQLParser.Parse(query);

            Assert.Null(result);
        }
    }
}