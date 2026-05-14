using DbManager;
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
            Manager manager = new Manager("user");
            manager.GrantPrivilege("missing", "users", Privilege.Select);

            Assert.Null(manager.ProfileByName("missing"));
        }

        [Fact]
        public void GrantPrivilege_NotBeingAdminShouldDoNothing()
        {
            Manager manager = new Manager("juan");
            Profile profile = new Profile { Name = "testProfile" };
            
            manager.AddProfile(profile);
            manager.GrantPrivilege("testProfile", "users", Privilege.Select);

            Assert.False(profile.IsGrantedPrivilege("users", Privilege.Select));
        }

        [Fact]  
        public void GrantPrivilege_TableDoesNotExist()
        {
            Manager manager = new Manager("admin");
            Profile profile = new Profile { Name = "test" };
            manager.AddProfile(profile);
            manager.GrantPrivilege("test", "noExiste", Privilege.Select);

            Assert.True(profile.IsGrantedPrivilege("noExiste", Privilege.Select));
        }

        [Fact]
        public void GrantPrivilege_GrantPrivilege_NonAdminCannotGrantPrivilege()
        {
            Manager manager = new Manager("user");
            Profile profile = new Profile { Name = "test" };

            manager.Profiles.Add(profile);

            manager.GrantPrivilege("test", "users", Privilege.Select);

            Assert.False(profile.IsGrantedPrivilege("users", Privilege.Select));
        }

        [Fact]
        public void GrantedPrivilege_UserDoesNotExist()
        {
            Manager manager = new Manager("admin");
            Profile profile = new Profile { Name = "dev" };
            profile.GrantPrivilege("users", Privilege.Select);
            manager.Profiles.Add(profile);

            bool result = manager.IsGrantedPrivilege("noExiste", "users", Privilege.Select);
            Assert.False(result);
        }

        //ISGRANTEDPRIVILEGE
        [Fact]
        public void IsGrantedPrivilege_PrivilegeIsGranted()
        {
            Manager manager = new Manager("admin");
            Profile profile = new Profile { Name = "test" };
            profile.Users.Add(new User("Juan", "1234"));
            manager.AddProfile(profile);

            manager.GrantPrivilege("test", "users", Privilege.Select);
            bool result = manager.IsGrantedPrivilege("Juan", "users", Privilege.Select);

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
            Assert.False(manager.IsGrantedPrivilege("admin", "users", Privilege.Delete));

            Profile adminProfile = new Profile { Name = "admin" };
            adminProfile.Users.Add(new User("admin", "admin"));
            manager.AddProfile(adminProfile);
            manager.GrantPrivilege("admin", "users", Privilege.Delete);
            bool result = manager.IsGrantedPrivilege("admin", "users", Privilege.Delete);
            Assert.True(result);

        }

        [Fact]
        public void IsGrantedPrivilege_UserDoesNotExist()
        {
            Manager manager = new Manager("usuario");
            Profile profile = new Profile { Name = "test" };
            profile.Users.Add(new User("user", "1234"));
            profile.GrantPrivilege("Users", Privilege.Insert);
            manager.Profiles.Add(profile);
            
            var result = manager.IsGrantedPrivilege("noExiste", "Users", Privilege.Insert);
            Assert.False(result);
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

        [Fact]
        public void AddProfile_NotBeingAdminShouldDoNothing()
        {
            Manager manager = new Manager("mikel");
            Profile profile = new Profile { Name = "test" };

            manager.AddProfile(profile);

            Assert.Null(manager.ProfileByName("test"));
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
            Profile profile1 = new Profile { Name = "yes" };
            Profile profile2 = new Profile { Name = "no" };

            manager.AddProfile(profile1);
            manager.AddProfile(profile2);

            var result1 = manager.ProfileByName("yes");
            var result2 = manager.ProfileByName("no");

            Assert.Equal(profile1, result1);
            Assert.Equal(profile2, result2);
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
            User user1 = new User("juan", "1234");
            User user2 = new User("Admin", "1023");

            Manager manager = new Manager("admin");

            Profile p1 = new Profile();
            p1.Users.Add(user1);
            p1.Name = "yes";
            Profile p2 = new Profile();
            p2.Users.Add(user2);
            p2.Name = "no";

            manager.AddProfile(p1);
            manager.AddProfile(p2);

            var result1 = manager.ProfileByUser("juan");
            var result2 = manager.ProfileByUser("Admin");

            Assert.Equal(p1, result1);
            Assert.Equal(p2, result2);
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

        [Fact]
        public void RevokePrivilege_NotBeingAdminShouldDoNothing()
        {
            Database db = new Database("test2", "mikel");

            Profile profile = new Profile { Name = "test" };
            profile.Users.Add(new User("mikel", "1234"));
            profile.GrantPrivilege("users", Privilege.Select);

            db.SecurityManager.Profiles.Add(profile);

            db.SecurityManager.RevokePrivilege("test", "users", Privilege.Select);

            Assert.True(profile.IsGrantedPrivilege("users", Privilege.Select));
        }

        [Fact]
        public void RevokePrivilege_NotBeingAdminShouldDoNothing_AfterPrivilegeExists()
        {
            Manager adminManager = new Manager("admin");
            Profile profile = new Profile { Name = "test" };

            adminManager.AddProfile(profile);
            adminManager.GrantPrivilege("test", "users", Privilege.Select);

            Manager normalManager = new Manager("mikel");
            normalManager.Profiles.Add(profile);

            normalManager.RevokePrivilege("test", "users", Privilege.Select);

            Assert.True(profile.IsGrantedPrivilege("users", Privilege.Select));
        }


        [Fact]
        public void RevokePrivilege_DoesNothing_WhenTableDoesNotExist()
        {
            Manager manager = new Manager("admin");
            Profile profile = new Profile { Name = "test" };

            manager.AddProfile(profile);
            profile.GrantPrivilege("users", Privilege.Select);

            manager.RevokePrivilege("test", "orders", Privilege.Select);

            Assert.True(profile.IsGrantedPrivilege("users", Privilege.Select));
        }


        //LOAD
        [Fact]
        public void Load_LoadsFullStructureCorrectly()
        {
            string dbName = "testdb_full";
            string path = dbName + ".path";
            
            // Setup file content with encrypted password
            string encrypted = Encryption.Encrypt("1234");
            File.WriteAllText(path, 
                $"Profile: dev\n" +
                $"User: alice, Password: {encrypted}\n" +
                $"Table: users\n" +
                $"Privilege: Select\n");

            try 
            {
                Manager manager = Manager.Load(dbName, "admin");
                Assert.NotNull(manager.ProfileByName("dev"));
                Assert.True(manager.IsGrantedPrivilege("alice", "users", Privilege.Select));
            }
            finally 
            {
                if (File.Exists(path)) File.Delete(path);
            }
        }

        [Fact]
        public void Load_LoadsProfilesAndUsersCorrectly()
        {
            string dbName = "testdb_load_basic";

            File.WriteAllText(dbName + ".path",
        @"Profile: dev
User: alice, Password: " + Encryption.Encrypt("1234") + @"
");
            Manager manager = Manager.Load(dbName, "admin");

            Assert.NotNull(manager.ProfileByName("dev"));
            Assert.NotNull(manager.UserByName("alice"));

            File.Delete(dbName + ".path");
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
        
        /*[Fact]
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
        }*/

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

            Assert.NotNull(manager);
            Assert.Empty(manager.Profiles);
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

        //REMOVE
        [Fact]
        public void RemoveProfile_RemovesExistingProfile()
        {
            Manager manager = new Manager("admin");
            Profile profile = new Profile { Name = "test" };

            manager.AddProfile(profile);

            bool result = manager.RemoveProfile("test");

            Assert.True(result);
            Assert.Null(manager.ProfileByName("test"));
        }
        [Fact]
        public void RemoveProfile_FalseProfileDoesNotExist()
        {
            Manager manager = new Manager("admin");
            bool result = manager.RemoveProfile("ghost");
            Assert.False(result);
        }
        [Fact]
        public void RemoveProfile_NotAdminCannotRemove()
        {
            Manager manager = new Manager("user");
            Profile profile = new Profile { Name = "test" };

            manager.Profiles.Add(profile);

            bool result = manager.RemoveProfile("test");

            Assert.False(result);
            Assert.NotNull(manager.ProfileByName("test"));
        }
        [Fact]
        public void RemoveProfile_ReturnsFalse_WhenNameIsNull()
        {
            Manager manager = new Manager("admin");
            bool result = manager.RemoveProfile(null);
            Assert.False(result);
        }
        [Fact]
        public void RemoveProfile_ReturnsFalse_WhenNameIsEmpty()
        {
            Manager manager = new Manager("admin");
            bool result = manager.RemoveProfile("");
            Assert.False(result);
        }
        
        
        /*[Fact]
        public void RemoveProfile_CannotRemoveAdmin()
        {
            Manager manager = new Manager("admin");
            bool result = manager.RemoveProfile("admin");
            Assert.False(result);
            Assert.NotNull(manager.ProfileByName("admin"));
        } */

        [Fact]
        public void RemoveProfile_CannotRemoveAdmin_CaseInsensitive()
        {
            Manager manager = new Manager("admin");
            bool result = manager.RemoveProfile("ADMIN");
            Assert.False(result);
        }
        [Fact]
        public void RemoveProfile_RemovesProfileWithUsers()
        {
            Manager manager = new Manager("admin");
            Profile profile = new Profile { Name = "dev" };
            profile.Users.Add(new User("juan", Encryption.Encrypt("1234")));
            manager.AddProfile(profile);
            bool result = manager.RemoveProfile("dev");
            Assert.True(result);
            Assert.Null(manager.ProfileByName("dev"));
            Assert.Null(manager.UserByName("juan"));
        }
        [Fact]
        public void RemoveProfile_RemovesProfileWithPrivileges()
        {
            Manager manager = new Manager("admin");
            Profile profile = new Profile { Name = "dev" };
            profile.GrantPrivilege("users", Privilege.Select);
            manager.AddProfile(profile);
            bool result = manager.RemoveProfile("dev");
            Assert.True(result);
            Assert.Null(manager.ProfileByName("dev"));
        }
        [Fact]
        public void RemoveProfile_RemovesOnlySpecifiedProfile()
        {
            Manager manager = new Manager("admin");
            Profile p1 = new Profile { Name = "p1" };
            Profile p2 = new Profile { Name = "p2" };
            manager.AddProfile(p1);
            manager.AddProfile(p2);
            manager.RemoveProfile("p1");
            Assert.Null(manager.ProfileByName("p1"));
            Assert.NotNull(manager.ProfileByName("p2"));
        }

    }
}





