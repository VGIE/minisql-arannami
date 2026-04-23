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
        public void IsUserAdmin_ReturnsTrue()
        {
            var manager = new Manager("admin");
            Assert.True(manager.IsUserAdmin());
        }

        [Fact]
        public void IsUserAdmin_ReturnsFalse()
        {
            var manager = new Manager("user1");
            Assert.False(manager.IsUserAdmin());
        }

    }
}
