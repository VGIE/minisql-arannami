using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using DbManager.Security;
using DbManager;

namespace SecurityParsingTests
{
    public class DeleteUserTests
    {
        [Fact]
        public void Correct()
        {
            DeleteUser query = MiniSQLParser.Parse("DELETE USER user") as DeleteUser;
            Assert.Equal("user", query.Username);

            query = MiniSQLParser.Parse("DELETE USER OtherUser") as DeleteUser;
            Assert.Equal("OtherUser", query.Username);
        }

        [Fact]
        public void CorrectWithSpaces()
        {
            DeleteUser query = MiniSQLParser.Parse("DELETE     USER      USER") as DeleteUser;
            Assert.Equal("USER", query.Username);

            query = MiniSQLParser.Parse("DELETE USER    OtherUser") as DeleteUser;
            Assert.Equal("OtherUser", query.Username);
        }

        [Fact]
        public void IncorrectCapitalization()
        {
            DeleteUser query = MiniSQLParser.Parse("Delete User User") as DeleteUser;
            Assert.Null(query);

            query = MiniSQLParser.Parse("delete user User") as DeleteUser;
            Assert.Null(query);

            query = MiniSQLParser.Parse("DELETE USER User") as DeleteUser;
            Assert.NotNull(query);
        }

        [Fact]
        public void IncorrectUserWithForbiddenChars()
        {
            DeleteUser query = MiniSQLParser.Parse("DELETE USER User_1") as DeleteUser;
            Assert.Null(query);

            query = MiniSQLParser.Parse("DELETE USER User 1") as DeleteUser;
            Assert.Null(query);

            query = MiniSQLParser.Parse("DELETE USER User") as DeleteUser;
            Assert.NotNull(query);
        }

        [Fact]
        public void IncorrectWithoutProfile()
        {
            DeleteUser query = MiniSQLParser.Parse("DELETE USER") as DeleteUser;
            Assert.Null(query);

            query = MiniSQLParser.Parse("DELETE USER ") as DeleteUser;
            Assert.Null(query);

            query = MiniSQLParser.Parse("DELETE USER User") as DeleteUser;
            Assert.NotNull(query);
        }
    }
}
