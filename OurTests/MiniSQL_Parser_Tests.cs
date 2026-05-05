using DbManager;
using DbManager.Parser;
using Xunit;


namespace OurTests
{
    public class MiniSQL_Parser_Tests
    {
        //UPDATE
        
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
            string query = "UPDATE Users SET Name='Juan Perez',Age=30 WHERE ID=1";

            var result = MiniSQLParser.Parse(query) as Update;

            Assert.NotNull(result);
            Assert.Equal("Users", result.Table);
            Assert.Equal(2, result.Columns.Count);

            Assert.Equal("Name", result.Columns[0].ColumnName);
            Assert.Equal("Juan Perez", result.Columns[0].Value);

            Assert.Equal("Age", result.Columns[1].ColumnName);
            Assert.Equal("30", result.Columns[1].Value);

            Assert.NotNull(result.Where);
            Assert.Equal("ID", result.Where.ColumnName);
            Assert.Equal("=", result.Where.Operator);
            Assert.Equal("1", result.Where.LiteralValue);
        }
        [Fact]
        public void Update_ParseWithSpacesInSetSReturnNull()
        {
            string query = "UPDATE Users SET Name='Juan Perez', Age=30 WHERE ID=1";
            var result = MiniSQLParser.Parse(query);
            Assert.Null(result);
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

        //ADDUSER

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

        //DELETE

        [Fact]
        public void DeleteParse_String()
        {
            var result = MiniSQLParser.Parse("DELETE FROM People WHERE Name='Juan'");

            Assert.NotNull(result);
            Assert.IsType<Delete>(result);

            Delete delete = (Delete)result;
            Assert.Equal("People", delete.Table);
            Assert.Equal("Name", delete.Where.ColumnName);
            Assert.Equal("=", delete.Where.Operator);
            Assert.Equal("Juan", delete.Where.LiteralValue);
        }

        [Fact]
        public void DeleteParse_Numeric()
        {
            var result = MiniSQLParser.Parse("DELETE FROM People WHERE Age>'25'");

            Assert.NotNull(result);
            Assert.IsType<Delete>(result);

            Delete delete = (Delete)result;
            Assert.Equal("People", delete.Table);
            Assert.Equal("Age", delete.Where.ColumnName);
            Assert.Equal(">", delete.Where.Operator);
            Assert.Equal("25", delete.Where.LiteralValue);
        }

        [Fact]
        public void Delete_WithWhere()
        {
            var result = MiniSQLParser.Parse("DELETE FROM users WHERE id=5");

            Assert.IsType<Delete>(result);
            var delete = (Delete)result;

            Assert.Equal("users", delete.Table);
            Assert.NotNull(delete.Where);
            Assert.Equal("id", delete.Where.ColumnName);
            Assert.Equal("=", delete.Where.Operator);
            Assert.Equal("5", delete.Where.LiteralValue);
        }
        [Fact]
        public void DeleteParse_WithAColumnMissing()
        {
            var result = MiniSQLParser.Parse("DELETE FROM  WHERE id='5'");
            var result2 = MiniSQLParser.Parse("DELETE FROM Table WHERE id=''");
            var result3 = MiniSQLParser.Parse("DELETE FROM Table WHERE>='2000'");
            var result4 = MiniSQLParser.Parse("DELETE FROM Table WHERE id '3'");

            Assert.Null(result);
            Assert.Null(result2);
            Assert.Null(result3);
            Assert.Null(result4);
            

            // Assert.IsType<Delete>(result);
            // var delete = (Delete)result;
            // Assert.Equal("users", delete.Table);
            // Assert.Null(delete.Where);
        }

        [Fact]
        public void DeleteSyntaxError()
        {
            var result1 = MiniSQLParser.Parse("delete People where Name='Juan'");
            var result2 = MiniSQLParser.Parse("DELETE People Name = 'Juan'");
            var result3 = MiniSQLParser.Parse("DELETE FROM People  Name = 'Juan'");
            var result4 = MiniSQLParser.Parse(" ");
            
            Assert.Null(result1);
            Assert.Null(result2); 
            Assert.Null(result3);
            Assert.Null(result4);

        }

        [Fact]
        public void DeleteParse_SpaceCondition()
        {
        MiniSqlQuery result1 = MiniSQLParser.Parse("DELETE People WHERE Name= 'Juan'");
        MiniSqlQuery result2 = MiniSQLParser.Parse("DELETE FROM People WHERE Name = 'Juan'");
        MiniSqlQuery result3 = MiniSQLParser.Parse(" DELETE FROM People WHERE Name='Juan'");

        Assert.Null(result1);
        Assert.Null(result2);
        Assert.Null(result3);
        }


        [Fact]
        public void DeleteParse_MarksCondition()
        {
        MiniSqlQuery query = MiniSQLParser.Parse("DELETE FROM People WHERE Name = Juan'");
        Assert.Null(query);
        }

        [Fact]
        public void DeleteParse_InvalidIdFormat()
        {
        var result = MiniSQLParser.Parse("DELETE FROM users WHERE id = 5.5.5");
        Assert.Null(result);
        }


        [Fact]
        public void DeleteParse_Capitalization()
        {
            var result1 = MiniSQLParser.Parse("DELETE FROM PeOpLe WHERE NaMe='JuAn'");
            var result2 = MiniSQLParser.Parse("DELETE FROM uSeRS WHERE iD>='4'");
            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.IsType<Delete>(result1);
            Assert.IsType<Delete>(result2);

            Delete delete1 = (Delete)result1;
            Assert.Equal("PeOpLe", delete1.Table);
            Assert.Equal("NaMe", delete1.Where.ColumnName);
            Assert.Equal("=", delete1.Where.Operator);
            Assert.Equal("JuAn", delete1.Where.LiteralValue);   

            Delete delete2 = (Delete)result2;
            Assert.Equal("uSeRS", delete2.Table);
            Assert.Equal("iD", delete2.Where.ColumnName);
            Assert.Equal(">=", delete2.Where.Operator);
            Assert.Equal("4", delete2.Where.LiteralValue);  

        }


        //DROPTABLE

        [Fact]
        public void DropTable_Parse_CorrectSyntax()
        {
            string query = "DROP TABLE MyTable";

            var result = MiniSQLParser.Parse(query) as DropTable;

            Assert.NotNull(result);
            Assert.Equal("MyTable", result.Table);
        }

        [Fact]
        public void DropTable_Parse_InvalidSyntax_ReturnsNull()
        {
            string query = "DROP MyTable";

            var result = MiniSQLParser.Parse(query);

            Assert.Null(result);
        }

        [Fact]
        public void DropTable_Parse_IncorrectCapitalization_ReturnsNull()
        {
            string query = "drop table MyTable";

            var result = MiniSQLParser.Parse(query);

            Assert.Null(result);
        }

        //CREATETABLE

        [Fact]
        public void CreateTable_Parse_CorrectSyntax()
        {
            string query = "CREATE TABLE People(Name TEXT,Age INT)";

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
        public void CreateTable_Parse_EmptyColumns()
        {
            string query = "CREATE TABLE EmptyTable()";

            var result = MiniSQLParser.Parse(query) as CreateTable;

            Assert.NotNull(result);
            Assert.Equal("EmptyTable", result.Table);
            Assert.Empty(result.ColumnsParameters);
        }

        [Fact]
        public void CreateTable_Parse_IncorrectCapitalization_ReturnsNull()
        {
            string query = "create table People(Name TEXT)";

            var result = MiniSQLParser.Parse(query);

            Assert.Null(result);
        }

        [Fact]
        public void CreateTable_Parse_WithoutTableKeyword_ReturnsNull()
        {
            string query = "CREATE People(Name TEXT)";

            var result = MiniSQLParser.Parse(query);

            Assert.Null(result);
        }

        [Fact]
        public void CreateTable_Parse_InvalidType_ReturnsNull()
        {
            string query = "CREATE TABLE People(Name BADTYPE)";

            var result = MiniSQLParser.Parse(query);

            Assert.Null(result);
        }

        [Fact]
        public void CreateTable_Parse_LowercaseType_ReturnsNull()
        {
            string query = "CREATE TABLE People(Name text)";

            var result = MiniSQLParser.Parse(query);

            Assert.Null(result);
        }

        [Fact]
        public void CreateTable_Parse_ColumnWithoutType_ReturnsNull()
        {
            string query = "CREATE TABLE People(Name)";

            var result = MiniSQLParser.Parse(query);

            Assert.Null(result);
        }

        [Fact]
        public void CreateTable_Parse_IncorrectWithMultipleColumnsAndSpaces()
        {
            string query = "CREATE TABLE People(Name  TEXT,   Age  INT)";

            var result = MiniSQLParser.Parse(query);

            Assert.Null(result);
        }

        [Fact]
        public void CreateTable_Parse_DoubleComma_ReturnsNull()
        {
            string query = "CREATE TABLE People(Name TEXT,, Age INT)";

            var result = MiniSQLParser.Parse(query);

            Assert.Null(result);
        }

        [Fact]
        public void CreateTable_Parse_TrailingComma_ReturnsNull()
        {
            string query = "CREATE TABLE People(Name TEXT, Age INT,)";

            var result = MiniSQLParser.Parse(query);

            Assert.Null(result);
        }

        [Fact]
        public void CreateTable_Parse_LeadingComma_ReturnsNull()
        {
            string query = "CREATE TABLE People(,Name TEXT, Age INT)";

            var result = MiniSQLParser.Parse(query);

            Assert.Null(result);
        }

      

        //SELECT
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
            string query = "SELECT Age FROM Users WHERE Name='Rodolfo'";
            var result = MiniSQLParser.Parse(query) as Select;

            Assert.NotNull(result);
            Assert.Equal("Users", result.Table);

            Assert.Equal(1, result.Columns.Count);
            Assert.Equal("Age", result.Columns[0]);

            Assert.NotNull(result.Where);
            Assert.Equal("Name", result.Where.ColumnName);
            Assert.Equal("=", result.Where.Operator);
            Assert.Equal("Rodolfo", result.Where.LiteralValue);
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
        public void Select_Parse_CommasBetweenColumnNames()
        {
            string query1 = "SELECT Name, Age WHERE Age='18'";
            string query2 = "SELECT Name ,Age WHERE Age='18'";
            string query3 = "SELECT Name , Age WHERE age='18'";

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

        //INSERT

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
            string query1 = "INSERT Users VALUES 'Juan'";

            var result1 = MiniSQLParser.Parse(query1);

            Assert.Null(result1);
        }

        [Fact]
        public void Insert_Parse_SpaceBetweenValues_ReturnsNull()
        {
            string query2 = "INSERT Users VALUES ('Juan', '20')";

            var result2 = MiniSQLParser.Parse(query2);

            Assert.Null(result2);
        }

        //DELETEUSER

        [Fact]
        public void DeleteUser_Parse_CorrectSyntax()
        {
            string query = "DELETE USER Mikel";
            var result = MiniSQLParser.Parse(query) as DeleteUser;

            Assert.NotNull(result);
            Assert.Equal("Mikel", result.Username);
        }

        [Fact]
        public void DeleteUser_Parse_InvalidSyntax_ReturnsNull()
        {
            string query = "DELETEUSER Mikel";
            var result = MiniSQLParser.Parse(query);

            Assert.Null(result);
        }

        //REVOKE

        [Fact]
        public void Revoke_Parse_CorrectSyntax()
        {
            string query = "REVOKE SELECT ON People TO Admin";
            var result = MiniSQLParser.Parse(query) as Revoke;

            Assert.NotNull(result);
            Assert.Equal("SELECT", result.PrivilegeName);
            Assert.Equal("People", result.TableName);
            Assert.Equal("Admin", result.ProfileName);
        }

        [Fact]
        public void Revoke_Parse_InvalidSyntax_ReturnsNull()
        {
            string query = "REVOKE SELECT People TO Admin";
            var result = MiniSQLParser.Parse(query);

            Assert.Null(result);
        }
        
        // GRANT

        [Fact]
        public void Grant_Parse_SyntaxOK()
        {
            string query = "GRANT SELECT ON People TO Admin";
            var result = MiniSQLParser.Parse(query) as Grant;
            Assert.NotNull(result);
            Assert.Equal("SELECT", result.PrivilegeName);
            Assert.Equal("People", result.TableName);
            Assert.Equal("Admin", result.ProfileName);
        }

        [Fact]
        public void Grant_Parse_InvalidSyntax_NULL()
        {
            string query = "GRANT SELECT People TO Admin"; 
           var result = MiniSQLParser.Parse(query);
            Assert.Null(result);
        }
        [Fact]
        public void Grant_Parse_InvalidPrivilege_NULL()
        {
            string query = "GRANT FAKE ON People TO Admin";
            var result = MiniSQLParser.Parse(query);
            Assert.Null(result);
        }
    }
}