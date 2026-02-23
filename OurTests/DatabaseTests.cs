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
        
        [Fact]
        public void AddTable()
        {
            Database db = new Database("admin", "123");
            Table t = Table.CreateTestTable("People");
            bool result = db.AddTable(t);
            Assert.True(result);
        }

        [Fact]
        public void AddTable_Duplicado()
        {
            Database db = new Database("admin", "123");
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

        [Fact]
        public void Save()
        {
            string dbName = "TestDB_Save";
            string filePath = dbName + ".db";
            Database db = Database.CreateTestDatabase();
            bool result = db.Save(dbName);
            Assert.True(result);
            Assert.True(File.Exists(filePath));

        }
        [Fact]
        public void Load()
        {
            string dbName = "TestDB_Load";
            Database db = Database.CreateTestDatabase();
            db.Save(dbName);
            Database loadedDb = Database.Load(dbName, "user", "userPassword");

            Assert.NotNull(loadedDb);
            var table = loadedDb.TableByName(Table.TestTableName);
            Assert.NotNull(table);
            loadedDb.CheckForTesting(Table.TestTableName, new List<List<string>>
            {
                new List<string> { Table.TestColumn1Row1, Table.TestColumn2Row1, Table.TestColumn3Row1 },
                new List<string> { Table.TestColumn1Row2, Table.TestColumn2Row2, Table.TestColumn3Row2 },
                new List<string> { Table.TestColumn1Row3, Table.TestColumn2Row3, Table.TestColumn3Row3 }
            });
        }

        [Fact]
        public void Load_DistintosCredenciales()
        {
            string dbName = "TestDB_Data";
            Database db = Database.CreateTestDatabase();
            db.Save(dbName);
            Database loaded = Database.Load(dbName, "user", "pass");

            Assert.NotNull(loaded);
            var table = loaded.TableByName(Table.TestTableName);
            Assert.NotNull(table);
            loaded.CheckForTesting(Table.TestTableName, new List<List<string>>
            {
                new List<string> { Table.TestColumn1Row1, Table.TestColumn2Row1, Table.TestColumn3Row1 },
                new List<string> { Table.TestColumn1Row2, Table.TestColumn2Row2, Table.TestColumn3Row2 },
                new List<string> { Table.TestColumn1Row3, Table.TestColumn2Row3, Table.TestColumn3Row3 }
            });
        }

        [Fact]
        public void Load_NoExiste()
        {
            Database loaded = Database.Load("NonExistingFile", "user", "pass");
            Assert.Null(loaded);
        }

        [Fact]
        public void CreateTable()
        {
            Database db = new Database("admin", "123");

            List<ColumnDefinition> columns = new List<ColumnDefinition>
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "Age")
            };

            bool result = db.CreateTable("People", columns);

            Assert.True(result);
            Assert.NotNull(db.TableByName("People"));
        }


        [Fact]
        public void CreateTable_SinColumnas()
        {
            Database db = new Database("admin", "123");

            List<ColumnDefinition> columns = new List<ColumnDefinition>();

            bool result = db.CreateTable("People", columns);

            Assert.False(result);
        }


        [Fact]
        public void CreateTable_Duplicado()
        {
            Database db = new Database("admin", "123");

            List<ColumnDefinition> columns = new List<ColumnDefinition>
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name")
            };

            db.CreateTable("People", columns);

            bool result = db.CreateTable("People", columns);

            Assert.False(result);
        }


        [Fact]
        public void DropTable()
        {
            Database db = new Database("admin", "123");

            List<ColumnDefinition> columns = new List<ColumnDefinition>
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name")
            };

            db.CreateTable("People", columns);

            bool result = db.DropTable("People");

            Assert.True(result);
            Assert.Null(db.TableByName("People"));
        }

        [Fact]
        public void DropTable_NoExiste()
        {
            Database db = new Database("admin", "123");

            bool result = db.DropTable("Unknown");

            Assert.False(result);
        }

        [Fact]
        public void InsertValidData_ReturnsTrueAndMessage()
        {
            Database database = Database.CreateTestDatabase();
            string tableName = Table.TestTableName;
            var newValues = new List<string> { "Ane", "1.65", "22" };
            int initialRow = database.TableByName(tableName).NumRows();

            bool done = database.Insert(tableName, newValues);

            Assert.True(done);
            Assert.Equal(Constants.InsertSuccess, database.LastErrorMessage);
            Assert.Equal(initialRow + 1, database.TableByName(tableName).NumRows());
        }

        [Fact]
        public void InsertNullTable_ReturnsFalseAndError()
        {
            Database database = Database.CreateTestDatabase();
            string NullTable = "NotRealTable";
            var newValues = new List<string> { "Data" };

            bool notDone = database.Insert(NullTable, newValues); 
            
            Assert.False(notDone);
            Assert.Equal(Constants.TableDoesNotExistError, database.LastErrorMessage);
        }

        [Fact]
        public void Insert_WrongColumnCountReturnsFalse()
        {
            Database database = Database.CreateTestDatabase();
            string tableName = Table.TestTableName;
            var newValues = new List<string> { "Ane", "1.65" };
            
            bool notDone= database.Insert(tableName, newValues);
            Assert.False(notDone);
            Assert.Equal(Constants.ColumnCountsDontMatch, database.LastErrorMessage);
        }


    }
}