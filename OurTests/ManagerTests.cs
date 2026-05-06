using DbManager.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OurTests
{
    public class ManagerTests
    {   

        //ISUSERADMIN
        [Fact]
        public void IsUserAdmin_AdminReturnsTrue()
        {
            Manager manager = new Manager("admin");
            bool result = manager.IsUserAdmin();
            Assert.True(result);
        }

        [Fact]
        public void IsUserAdmin_NotAdminReturnsFalse()
        {
            Manager manager = new Manager("juan");
            bool result = manager.IsUserAdmin();
            Assert.False(result);
        }

        [Fact]
        public void IsUserAdmin_Insensitive()
        {
            Manager manager = new Manager("ADMIN");
            bool result = manager.IsUserAdmin();
            Assert.True(result);
        }
        [Fact]
        public void IsUserAdmin_ReturnsFalse_WhenUsernameIsEmpty()
        {
            Manager manager = new Manager("");
            bool result = manager.IsUserAdmin();
            Assert.False(result);
        }

        //ISPASSWORDCORRECT

        [Fact]
        public void IsPasswordCorrect_ReturnsTruePasswordCorrect()
        {
            Manager manager = new Manager("admin");
            bool result = manager.IsPasswordCorrect("admin", "admin");
            Assert.True(result);
        }

        [Fact]
        public void IsPasswordCorrect_ReturnsFalsePasswordIncorrect()
        {
            Manager manager = new Manager("admin");
            bool result = manager.IsPasswordCorrect("admin", "wrongpassword");
            Assert.False(result);
        }

        [Fact]
        public void IsPasswordCorrect_ReturnsFalseUserDoesNotExist()
        {
            Manager manager = new Manager("admin");
            bool result = manager.IsPasswordCorrect("nonexistent", "admin");
            Assert.False(result);
        }
        [Fact]
        public void IsPasswordCorrect_EncryptionCorrectly()
        {
            Manager manager = new Manager("admin");
            bool result = manager.IsPasswordCorrect("admin", "admin");
            Assert.True(result);
        }
        [Fact]
        public void IsPasswordCorrect_ReturnsFalseUsernameNull()
        {
            Manager manager = new Manager("admin");
            bool result = manager.IsPasswordCorrect(null, "admin");
            Assert.False(result);
        }
        [Fact]
        public void IsPasswordCorrect_ReturnsFalsePasswordEmpty()
        {
            Manager manager = new Manager("admin");
            bool result = manager.IsPasswordCorrect("admin", "");
            Assert.False(result);
        }
        [Fact]
        public void IsPasswordCorrect_ReturnsFalsePasswordExtraSpaces()
        {
            Manager manager = new Manager("admin");
            bool result = manager.IsPasswordCorrect("admin", "admin ");
            Assert.False(result);
        }
        [Fact]
        public void IsPasswordCorrect_ReturnsFalse_WhenPasswordIsNull()
        {
            Manager manager = new Manager("admin");
            bool result = manager.IsPasswordCorrect("admin", null);
            Assert.False(result);
        }
        [Fact]
        public void IsPasswordCorrect_WorksWithDefaultAdminUser()
        {
            Manager manager = new Manager("admin");
            Assert.True(manager.IsPasswordCorrect("admin", "admin"));
        }
        
        //GRANTPRIVILEGE
        [Fact]
        public void GrantPrivilege_AddsPrivilegeToProfile()
        {
            Manager manager = new Manager("admin");
            Profile profile = new Profile { Name = "test" };
            manager.AddProfile(profile);
            manager.GrantPrivilege("test", "users", Privilege.Select);
            
            Assert.True(profile.IsGrantedPrivilege("users", Privilege.Select));
        }

        [Fact]
        public void GrantPrivilege_ProfileDoesNotExist()
        {
            Manager manager = new Manager("admin");
            manager.GrantPrivilege("missing", "users", Privilege.Select);
            
            Assert.Null(manager.ProfileByName("missing"));
        }


        //ISGRANTEDPRIVILEGE
        [Fact]
        public void IsGrantedPrivilege_PrivilegeIsGranted()
        {
            Manager manager = new Manager("admin");
            Profile profile = new Profile { Name = "test" };
            manager.AddProfile(profile);
            manager.GrantPrivilege("test", "users", Privilege.Select);
            bool result = manager.IsGrantedPrivilege("test", "users", Privilege.Select);
            
            Assert.True(result);
        }

        [Fact]
        public void IsGrantedPrivilege_PrivilegeIsNotGranted()
        {
            Manager manager = new Manager("Juan");
            Profile profile = new Profile { Name = "test" };
            User user = new User("Juan", Encryption.Encrypt("1234"));
            profile.Users.Add(user);
            manager.AddProfile(profile);
            bool result = manager.IsGrantedPrivilege("Juan", "users", Privilege.Select);
            
            Assert.False(result);
        }  

        [Fact]
        public void IsGrantedPrivilege_AdminAlwaysPrivilege()
        {
            Manager manager = new Manager("admin");
            bool result = manager.IsGrantedPrivilege("admin", "CualquierTabla", Privilege.Delete);
            Assert.True(result);
        } 

        //ADDPROFILE
        [Fact]
        public void AddProfile_AddsProfile()
        {
            Manager manager = new Manager("admin");
            Profile profile = new Profile { Name = "test" };

            manager.AddProfile(profile);

            Assert.Equal(profile, manager.ProfileByName("test"));
        }

        [Fact]
        public void AddProfile_DoesNotAddNull()
        {
            Manager manager = new Manager("admin");

            manager.AddProfile(null);

            Assert.Null(manager.ProfileByName(null));
        }

        [Fact]
        public void AddProfile_DoesNotAddDuplicateProfile()
        {
            Manager manager = new Manager("admin");
            Profile profile1 = new Profile { Name = "test" };
            Profile profile2 = new Profile { Name = "test" };

            manager.AddProfile(profile1);
            manager.AddProfile(profile2);

            Assert.Equal(profile1, manager.ProfileByName("test"));
        }

        //USERBYPROFILE
        [Fact]
        public void UserByName_ReturnsUser_Exists()
        {
            Manager manager = new Manager("admin");
            Profile profile = new Profile { Name = "test" };
            User user = new User("juan", Encryption.Encrypt("1234"));

            profile.Users.Add(user);
            manager.AddProfile(profile);

            var result = manager.UserByName("juan");

            Assert.Equal(user, result);
        }

        [Fact]
        public void UserByName_ReturnsNull_UserDoesntExist()
        {
            Manager manager = new Manager("admin");

            var result = manager.UserByName("ghost");

            Assert.Null(result);
        }

        [Fact]
        public void UserByName_ReturnsNull_WhenUsernameIsNullOrEmpty()
        {
            Manager manager = new Manager("admin");

            Assert.Null(manager.UserByName(null));
            Assert.Null(manager.UserByName(""));
        }

        //PROFILEBYNAME
        [Fact]
        public void ProfileByName_ReturnsProfile_Exists()
        {
            Manager manager = new Manager("admin");
            Profile profile = new Profile { Name = "dev" };

            manager.AddProfile(profile);

            var result = manager.ProfileByName("dev");

            Assert.Equal(profile, result);
        }

        [Fact]
        public void ProfileByName_ReturnsNull_DoesntExists()
        {
            Manager manager = new Manager("admin");

            var result = manager.ProfileByName("ghost");

            Assert.Null(result);
        }

        [Fact]
        public void ProfileByName_ReturnsNull_WhenNameIsNullOrEmpty()
        {
            Manager manager = new Manager("admin");

            Assert.Null(manager.ProfileByName(null));
            Assert.Null(manager.ProfileByName(""));
        }

        //PROFILEBYUSER
        [Fact]
        public void ProfileByUser_ReturnsProfile_UserExists()
        {
            Manager manager = new Manager("admin");
            Profile profile = new Profile { Name = "dev" };
            User user = new User("juan", Encryption.Encrypt("1234"));

            profile.Users.Add(user);
            manager.AddProfile(profile);

            var result = manager.ProfileByUser("juan");

            Assert.Equal(profile, result);
        }

        [Fact]
        public void ProfileByUser_ReturnsNull_UserDoesntExist()
        {
            Manager manager = new Manager("admin");

            var result = manager.ProfileByUser("noExistente");

            Assert.Null(result);
        }

        [Fact]
        public void ProfileByUser_ReturnsNull_WhenUsernameIsNullOrEmpty()
        {
            Manager manager = new Manager("admin");

            Assert.Null(manager.ProfileByUser(null));
            Assert.Null(manager.ProfileByUser(""));
        }

        //REVOKEPRIVILEGE
        [Fact]
        public void RevokePrivilege_RemovesPrivilegeFromProfile()
        {
            Manager manager = new Manager("admin");
            Profile profile = new Profile { Name = "test" };

            manager.AddProfile(profile);
            profile.GrantPrivilege("users", Privilege.Select);

            manager.RevokePrivilege("test", "users", Privilege.Select);

            Assert.False(profile.IsGrantedPrivilege("users", Privilege.Select));
        }

        [Fact]
        public void RevokePrivilege_DoesNothing_WhenProfileDoesNotExist()
        {
            Manager manager = new Manager("admin");

            manager.RevokePrivilege("missing", "users", Privilege.Select);

            Assert.Null(manager.ProfileByName("missing"));
        }

        //LOAD
        [Fact]
        public void Load_LoadsProfilesAndUsersCorrectly()
        {
            string dbName = "testdb_load_basic";

            try
            {
                File.WriteAllText(dbName + ".path",
        @"Profile: dev
User: alice, Password: " + Encryption.Encrypt("1234") + @"
");

                Manager manager = Manager.Load(dbName, "admin");

                Assert.NotNull(manager.ProfileByName("dev"));
                Assert.NotNull(manager.UserByName("alice"));
            }
            finally
            {
                File.Delete(dbName + ".path");
            }
        }
        [Fact]
        public void Load_LoadsPrivileges()
        {
            string dbName = "testdb_load_priv";

            File.WriteAllText(dbName + ".path",
                    @"Profile: dev
            User: alice, Password: " + Encryption.Encrypt("1234") + @"
            Table: users
            Privilege: Select
            ");

            Manager manager = Manager.Load(dbName, "admin");
            bool hasPrivilege = manager.IsGrantedPrivilege("alice", "users", Privilege.Select);
            Assert.True(hasPrivilege);
        }
        [Fact]
        public void Load_FileDoesNotExistReturnsDefaultManager()
        {
            string dbName = "file_that_does_not_exist_xxx";
            Manager manager = Manager.Load(dbName, "admin");
            Assert.NotNull(manager);
            Assert.True(manager.IsUserAdmin());
        }
        [Fact]
        public void Load_LoadsFullStructureCorrectly()
        {
            string dbName = "testdb_full";

            File.WriteAllText(dbName + ".path",
@"Profile: dev
User: alice, Password: " + Encryption.Encrypt("1234") + @"
User: bob, Password: " + Encryption.Encrypt("5678") + @"
Table: users
Privilege: Select
Privilege: Insert
");

            Manager manager = Manager.Load(dbName, "admin");

            Assert.NotNull(manager.ProfileByName("dev"));
            Assert.NotNull(manager.UserByName("alice"));
            Assert.NotNull(manager.UserByName("bob"));
            Assert.True(manager.IsGrantedPrivilege("alice", "users", Privilege.Select));
            Assert.True(manager.IsGrantedPrivilege("alice", "users", Privilege.Insert));
        }
        [Fact]
        public void Load_IgnoresMalformedUserLine()
        {
            string dbName = "testdb_malformed";

            File.WriteAllText(dbName + ".path",
                        @"Profile: dev
                User: alice
                ");

            Manager manager = Manager.Load(dbName, "admin");
            Assert.NotNull(manager.ProfileByName("dev"));
            Assert.Null(manager.UserByName("alice"));
        }
        [Fact]
        public void Load_EmptyFile_ReturnsOnlyAdmin()
        {
            string dbName = "testdb_empty";
            File.WriteAllText(dbName + ".path", "");
            Manager manager = Manager.Load(dbName, "admin");
            Assert.NotNull(manager.ProfileByName("admin"));
        }

        //SAVE
        [Fact]
        public void SaveAndCheckCorrectCredentials()
        {
            Manager manager = new Manager("admin");
            string dbName = "Corrrect";
            manager.Save(dbName);
            bool esCorrecta = manager.IsPasswordCorrect("admin", "admin");
            Assert.True(esCorrecta, "La contraseña es correcta");
            Assert.True(File.Exists(dbName + ".path"));
        }

        [Fact]
        public void SaveAndCheckIncorrectCredentials()
        {
            Manager manager = new Manager("admin");
            string dbName = "Incorrect";  
            manager.Save(dbName);
            bool esCorrecta = manager.IsPasswordCorrect("admin", "1234");
            Assert.False(esCorrecta, "La contraseña es incorrecta");
            Assert.True(File.Exists(dbName + ".path"));
        }
            
    }
}


     





