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

        //ISPASSWORDCORRECT
       

        //GRANTPRIVILEGE

        //ISGRANTEDPRIVILEGE


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

        //PROFILEBYNAME

        //PROFILEBYUSER

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


     





