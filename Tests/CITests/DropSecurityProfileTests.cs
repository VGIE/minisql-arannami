using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using DbManager.Security;
using DbManager;

/*namespace SecurityParsingTests
{
    public class DropSecurityProfileTests
    {
        /*[Fact]
        public void Correct()
        {
            DropSecurityProfile query = MiniSQLParser.Parse("DROP SECURITY PROFILE profile") as DropSecurityProfile;
            Assert.Equal("profile", query.ProfileName);

            query = MiniSQLParser.Parse("DROP SECURITY PROFILE OtherProfile") as DropSecurityProfile;
            Assert.Equal("OtherProfile", query.ProfileName);
        }

        [Fact]
        public void CorrectWithSpaces()
        {
            DropSecurityProfile query = MiniSQLParser.Parse("DROP     SECURITY PROFILE      profile") as DropSecurityProfile;
            Assert.Equal("profile", query.ProfileName);

            query = MiniSQLParser.Parse("DROP SECURITY     PROFILE OtherProfile") as DropSecurityProfile;
            Assert.Equal("OtherProfile", query.ProfileName);
        }

        [Fact]
        public void IncorrectCapitalization()
        {
            DropSecurityProfile query = MiniSQLParser.Parse("Drop SECURITY PROFILE profile") as DropSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("drop security profile OtherProfile") as DropSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("DROP SECURITY PROFILE profile") as DropSecurityProfile;
            Assert.NotNull(query);
        }

        [Fact]
        public void IncorrectProfileWithForbiddenChars()
        {
            DropSecurityProfile query = MiniSQLParser.Parse("DROP SECURITY PROFILE pro-file") as DropSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("DROP SECURITY PROFILE Pro file") as DropSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("DROP SECURITY PROFILE profile") as DropSecurityProfile;
            Assert.NotNull(query);
        }

        [Fact]
        public void IncorrectWithoutProfile()
        {
            DropSecurityProfile query = MiniSQLParser.Parse("DROP SECURITY PROFILE ") as DropSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("DROP SECURITY PROFILE") as DropSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("DROP SECURITY PROFILE profile") as DropSecurityProfile;
            Assert.NotNull(query);
        }
    }
}*/

