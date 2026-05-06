using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbManager;

namespace OurTests
{
    public class DatabaseSecurityTests
    {
        /*[Fact]
        public void Load_WrongPassword_ReturnsNull()
        {
            string dbName = "SecurityTest_WrongPass";
            Database db = Database.CreateTestDatabase();
            db.Save(dbName);

            Database loadedDb = Database.Load(dbName, "admin", "admin123");

            Assert.Null(loadedDb);
        }

        [Fact]
        public void Load_WrongUser_ReturnsNull()
        {
            string dbName = "SecurityTest_WrongUser";
            Database db = Database.CreateTestDatabase();
            db.Save(dbName);

            Database loadedDb = Database.Load(dbName, "unknownUser", "adminPassword");

            Assert.Null(loadedDb);
        }

        [Fact]
        public void Load_EmptyCredentials_ReturnsNull()
        {
            string dbName = "SecurityTest_Empty";
            Database db = Database.CreateTestDatabase();
            db.Save(dbName);

            Assert.Null(Database.Load(dbName, "", "")); 
            Assert.Null(Database.Load(dbName, "admin", null)); 
        }*/

        [Fact]
        public void Save_CreatesSecurityFile()
        {
            string dbName = "SecurityFileTest";
            Database db = Database.CreateTestDatabase();

            db.Save(dbName);

            string securityPath = dbName + ".path";
            Assert.True(File.Exists(securityPath));
        }
    }
}
