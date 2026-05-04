using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbManager.Security;
using DbManager;
using Xunit;

namespace OurTests
{
    public class ProfileTests
    {

       //GRANTPRIVILEGE
        [Fact]
        public void GrantPrivilege_AddsPrivilege()
        {
            Profile profile = new Profile();

            profile.GrantPrivilege("users", Privilege.Select);

            Assert.True(profile.IsGrantedPrivilege("users", Privilege.Select));
        }

        [Fact]
        public void GrantPrivilege_NotDuplicatePrivileges()
        {
            Profile profile = new Profile();

            profile.GrantPrivilege("users", Privilege.Select);
            Assert.False(profile.GrantPrivilege("users", Privilege.Select));
        }

        [Fact]
        public void GrantPrivilege_TableIsEmpty()
        {
            Profile profile = new Profile();
            bool resultado = profile.IsGrantedPrivilege("", Privilege.Select);
            Assert.False(resultado);
        }


        //ISGRANTEDPRIVILEGE
        [Fact]
        public void IsGrantedPrivilege_PrivilegeIsGranted()
        {
            Profile profile = new Profile();
            profile.GrantPrivilege("users", Privilege.Select);                  

            bool result = profile.IsGrantedPrivilege("users", Privilege.Select);
            Assert.True(result);
        }

        [Fact]
        public void IsGrantedPrivilege_PrivilegeIsNotGranted()
        {
            Profile profile = new Profile();
            profile.GrantPrivilege("users", Privilege.Insert);  

            bool result = profile.IsGrantedPrivilege("users", Privilege.Select);
            Assert.False(result);
        }

        [Fact]
        public void IsGrantedPrivilege_TableDoesNotExist()
        {
            Profile profile = new Profile();                                            
            bool result = profile.IsGrantedPrivilege("NoExist", Privilege.Select);
            Assert.False(result);
        }


       //REVOKEPRIVILEGE
        [Fact]
        public void RevokePrivilege_RemovesExistingPrivilege()
        {
            Profile profile = new Profile();

            profile.GrantPrivilege("users", Privilege.Select);

            bool result = profile.RevokePrivilege("users", Privilege.Select);

            Assert.True(result);
            Assert.False(profile.IsGrantedPrivilege("users", Privilege.Select));
        }

        [Fact]
        public void RevokePrivilege_ReturnsFalse_WhenTableDoesNotExist()
        {
            Profile profile = new Profile();

            bool result = profile.RevokePrivilege("users", Privilege.Select);

            Assert.False(result);
        }

        [Fact]
        public void RevokePrivilege_ReturnsFalse_WhenPrivilegeDoesNotExist()
        {
            Profile profile = new Profile();

            profile.GrantPrivilege("users", Privilege.Insert);

            bool result = profile.RevokePrivilege("users", Privilege.Select);

            Assert.False(result);
            Assert.True(profile.IsGrantedPrivilege("users", Privilege.Insert));
        }
    }
}
