using DbManager;
using DbManager.Parser;
using DbManager.Security;
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

        //ADDUSER

        [Fact]
        public void AddUser_Execute_Success()
        {
            Database db = Database.CreateTestDatabase();
            db.SecurityManager.AddProfile(new Profile { Name = "AdminProfile" });

            AddUser addUser = new AddUser("Juan", "1234", "AdminProfile");

            string result = addUser.Execute(db);

            Assert.Equal(Constants.AddUserSuccess, result);

            var user = db.SecurityManager.UserByName("Juan");
            Assert.NotNull(user);
        }

        [Fact]
        public void AddUser_Execute_ProfileDoesNotExist()
        {
            Database db = Database.CreateTestDatabase();

            AddUser addUser = new AddUser("Juan", "1234", "PerfilFake");

            string result = addUser.Execute(db);

            Assert.Equal(Constants.SecurityProfileDoesNotExistError, result);
        }

        [Fact]
        public void AddUser_Execute_UserAlreadyExists()
        {
            Database db = Database.CreateTestDatabase();

            db.SecurityManager.AddProfile(new Profile { Name = "AdminProfile" });
            AddUser addUser1 = new AddUser("Juan", "1234", "AdminProfile");
            addUser1.Execute(db);
            AddUser addUser2 = new AddUser("Juan", "1234", "AdminProfile");
            string result = addUser2.Execute(db);

            Assert.Equal(Constants.Error + "User already exists", result);
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

        /*[Fact]
        public void Select_Execute_SynctaticalErrorCondition()
        {
            Database db = Database.CreateTestDatabase();
            List<string> columns = new List<string> { "Name" };

            Condition wrongCond = new Condition("Age", "!!", "67");

            Select select = new Select(Table.TestTableName, columns, wrongCond);
            string result = select.Execute(db);

            Assert.Equal(Constants.SyntaxError, result);
        }*/

        [Fact]
        public void Select_Execute_NonExistentTable()
        {
            Database db = Database.CreateTestDatabase();
            List<string> columns = new List<string> { "Name", "Age" };

            Select select = new Select("Table doesn't exist", columns);
            string result = select.Execute(db);

            Assert.Equal(Constants.TableDoesNotExistError, result);
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
            Database db = Database.CreateTestDatabase();

            List<string> columns = new List<string> { "Name", "NotCOlumn" };

            Select select = new Select(Table.TestTableName, columns);
            string result = select.Execute(db);

            Assert.Equal(Constants.ColumnDoesNotExistError, result);
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
        }
        
        [Fact]
        public void Insert_Execute_MultipleColumns()
        {
        }

        [Fact]
        public void Insert_Execute_IncorrectColumns()
        {
        }

        [Fact]
        public void Insert_Execute_EmptyTableColumns()
        {
        }
        */

        //CREATETABLE

        [Fact]
        public void CreateTable_Execute_Success()
        {
            Database db = Database.CreateTestDatabase();
            List<ColumnDefinition> columns = new List<ColumnDefinition>
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "Age")
            };

            CreateTable create = new CreateTable("NewTable", columns);

            string result = create.Execute(db);

            Assert.Equal(Constants.CreateTableSuccess, result);        
        }

        [Fact]
        public void CreateTable_Execute_TableAlreadyExists()
        {
            Database db = Database.CreateTestDatabase();
            List<ColumnDefinition> columns = new List<ColumnDefinition>
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name")
            };

            CreateTable create = new CreateTable(Table.TestTableName, columns);

            string result = create.Execute(db);

            Assert.Equal(Constants.TableAlreadyExistsError, result);
        }

        [Fact]
        public void CreateTable_Execute_WithoutColumns_ReturnsError()
        {
            Database db = Database.CreateTestDatabase();
            List<ColumnDefinition> columns = new List<ColumnDefinition>();

            CreateTable create = new CreateTable("EmptyTable", columns);

            string result = create.Execute(db);

            Assert.Equal(Constants.DatabaseCreatedWithoutColumnsError, result);
        }


        //DROPTABLE

        [Fact]
        public void DropTable_Execute_Success()
        {
            Database db = Database.CreateTestDatabase();

            DropTable drop = new DropTable(Table.TestTableName);

            string result = drop.Execute(db);

            Assert.Equal(Constants.DropTableSuccess, result);
            
        }

        [Fact]
        public void DropTable_Execute_TableDoesNotExist()
        {
            Database db = Database.CreateTestDatabase();

            DropTable drop = new DropTable("TablaFake");

            string result = drop.Execute(db);

            Assert.Equal(Constants.TableDoesNotExistError, result);
        }
        
        //DELETE
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


        // REVOKE

        [Fact]
        public void Revoke_Execute_Success()
        {
            Database db = Database.CreateTestDatabase();

            Profile profile = new Profile { Name = "TestProfile" };
            db.SecurityManager.AddProfile(profile);
            db.SecurityManager.GrantPrivilege("TestProfile", Table.TestTableName, Privilege.Select);

            Revoke revoke = new Revoke("Select", Table.TestTableName, "TestProfile");

            string result = revoke.Execute(db);

            Assert.Equal(Constants.RevokePrivilegeSuccess, result);
        }

        [Fact]
        public void Revoke_Execute_ProfileDoesNotExist()
        {
            Database db = Database.CreateTestDatabase();
            Revoke revoke = new Revoke("Select", Table.TestTableName, "PerfilFake");

            string result = revoke.Execute(db);

            Assert.Equal(Constants.SecurityProfileDoesNotExistError, result);
        }

        [Fact]
        public void Revoke_Execute_TableDoesNotExist()
        {
            Database db = Database.CreateTestDatabase();

            Profile profile = new Profile { Name = "TestProfile" };
            db.SecurityManager.AddProfile(profile);

            Revoke revoke = new Revoke("Select", "TablaFake", "TestProfile");

            string result = revoke.Execute(db);

            Assert.Equal(Constants.TableDoesNotExistError, result);
        }

        [Fact]
        public void Revoke_Execute_PrivilegeDoesNotExist()
        {
            Database db = Database.CreateTestDatabase();

            Profile profile = new Profile { Name = "TestProfile" };
            db.SecurityManager.AddProfile(profile);

            Revoke revoke = new Revoke("PrivilegioFake", Table.TestTableName, "TestProfile");

            string result = revoke.Execute(db);

            Assert.Equal(Constants.PrivilegeDoesNotExistError, result);
        }

        // DELETE USER
        [Fact]
        public void DeleteUser_Execute_Success()
        {
            Database db = Database.CreateTestDatabase();

            Profile profile = new Profile { Name = "TestProfile" };
            profile.Users.Add(new User("Juan", "1234"));
            db.SecurityManager.AddProfile(profile);

            DeleteUser deleteUser = new DeleteUser("Juan");

            string result = deleteUser.Execute(db);

            Assert.Equal(Constants.DeleteUserSuccess, result);
        }

        [Fact]
        public void DeleteUser_Execute_UserDoesNotExist()
        {
            Database db = Database.CreateTestDatabase();

            DeleteUser deleteUser = new DeleteUser("UsuarioFake");

            string result = deleteUser.Execute(db);

            Assert.Equal(Constants.UserDoesNotExistError, result);
        }

    }
}
