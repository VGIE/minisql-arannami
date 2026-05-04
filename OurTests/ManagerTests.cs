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
    }


}
