using DbManager;
using DbManager;
using DbManager.Parser;

namespace OurTests
{
    public class DataBaseTests
    {
        [Fact]
        public void DeleteWhere()
        {

            Database db = Database.CreateTestDatabase();
            Condition condition = new Condition("Age", "=", "67");
            bool result = db.DeleteWhere(Table.TestTableName, condition);
            Assert.True(result);
            Assert.Equal(Constants.DeleteSuccess, db.LastErrorMessage);
            db.CheckForTesting(Table.TestTableName, new List<List<string>>
            {
                new List<string> { Table.TestColumn1Row1, Table.TestColumn2Row1, Table.TestColumn3Row1 },
                new List<string> { Table.TestColumn1Row3, Table.TestColumn2Row3, Table.TestColumn3Row3 }
            });

        }
        /*
        [Fact]
        public void Update()
        {
            Database db = Database.CreateTestDatabase();
            Condition condition = new Condition("Age", "=", "67");
            List<SetValue> updates = new List<SetValue>
            {
                new SetValue("Name", "NuevaMaider")
            };
            bool result = db.Update(Table.TestTableName, updates, condition);

            Assert.True(result);
            Assert.Equal(Constants.UpdateSuccess, db.LastErrorMessage);
            db.CheckForTesting(Table.TestTableName, new List<List<string>>
            {
                new List<string> { Table.TestColumn1Row1, Table.TestColumn2Row1, Table.TestColumn3Row1 },
                new List<string> { "NuevaMaider", Table.TestColumn2Row2, Table.TestColumn3Row2 },
                new List<string> { Table.TestColumn1Row3, Table.TestColumn2Row3, Table.TestColumn3Row3 }
            });
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