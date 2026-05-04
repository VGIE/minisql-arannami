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
