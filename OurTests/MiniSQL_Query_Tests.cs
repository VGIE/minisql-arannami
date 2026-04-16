using DbManager;
using DbManager.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurTests
{
    public class MiniSQL_Query_Tests
    {
        
        //UPDATE
        [Fact]
        public void Update_ExecuteBien()
        {
            Database db = Database.CreateTestDatabase();

            List<SetValue> values = new List<SetValue>
            {
                new SetValue("Age", "99")
            };

            Condition where = new Condition("Age", "=", "67");

            Update update = new Update(Table.TestTableName, values, where);

            string result = update.Execute(db);

            Assert.Equal(Constants.UpdateSuccess, result);
        }

        [Fact]
        public void Update_Execute_TableDoesNotExist()
        {
            Database db = Database.CreateTestDatabase();

            List<SetValue> values = new List<SetValue>
            {
                new SetValue("Age", "99")
            };

            Update update = new Update("TablaFake", values, null);

            string result = update.Execute(db);

            Assert.Equal(Constants.TableDoesNotExistError, result);
        }

        [Fact]
        public void Update_Execute_ColumnDoesNotExist()
        {
            Database db = Database.CreateTestDatabase();

            List<SetValue> values = new List<SetValue>
            {
                new SetValue("FakeColumn", "99")
            };

            Update update = new Update(Table.TestTableName, values, null);

            string result = update.Execute(db);

            Assert.Equal(Constants.ColumnDoesNotExistError, result);
        }

        //SELECT
        [Fact]
        public void Select_ExecuteOkey()
        {
            Database db = Database.CreateTestDatabase();
            List<string> columns = new List<string> { "Name", "Age" };
            Condition where = new Condition("Age", "=", "67");

            Select select = new Select(Table.TestTableName, columns, where);
            string result = select.Execute(db);

            Assert.NotEqual(Constants.ColumnDoesNotExistError, result);
        }

        [Fact]
        public void Select_Execute_SynctaticalError()
        {

        }

        [Fact]
        public void Select_Execute_NonExistentTable()
        {
            
        }

        [Fact]
        public void Select_Execute_EmptyWhere()
        {
            Database db = Database.CreateTestDatabase();
            List<string> columns = new List<string> { "Name", "Age" };

            Select select = new Select(Table.TestTableName, columns);
            string result = select.Execute(db);

            Assert.NotEqual(Constants.ColumnDoesNotExistError, result);
        }

        [Fact]
        public void Select_Execute_NonExistentColumn()
        {

        }

        //INSERT
        /*[Fact]
        public void Insert_ExecuteOkey()
        {
            Database db = Database.CreateTestDatabase();
            List<string> values = new List<string> { "Juan", "30" }; 

            Insert insert = new Insert(Table.TestTableName, values);
            string result = insert.Execute(db);

            Assert.Equal(Constants.InsertSuccess, result);
        }*/


    }
}
