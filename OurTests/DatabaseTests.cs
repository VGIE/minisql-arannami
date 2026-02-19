using DbManager;
namespace OurTests
{
    public class DatabaseTests
    {
        //TODO DEADLINE 1B : Create your own tests for Database
        /*
        [Fact]
        public void Test1()
        {
        }
        */
        [Fact]
        public void AddTable()
        {
            Database db = new Database("admin","123");
            Table t = Table.CreateTestTable("People");
            bool result = db.AddTable(t);
            Assert.True(result);
        }

        [Fact]
        public void AddTable_Duplicado()
        {
            Database db = new Database("admin","123");
            Table t = Table.CreateTestTable("People");
            db.AddTable(t);
            bool result = db.AddTable(t);
            Assert.False(result);
        }

        [Fact]
        public void TableByName()
        {
            Database db = Database.CreateTestDatabase();
            var table = db.TableByName(Table.TestTableName);
            Assert.NotNull(table);
        }

        [Fact]
        public void TableByName_NoExiste()
        {
            Database db = Database.CreateTestDatabase();
            var table = db.TableByName("Unknown");
            Assert.Null(table);
        }
    }

}