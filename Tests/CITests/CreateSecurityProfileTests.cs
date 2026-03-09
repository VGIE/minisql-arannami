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
    public class CreateSecurityProfileTests
    {
        /*[Fact]
        public void Correct()
        {
            CreateSecurityProfile query = MiniSQLParser.Parse("CREATE SECURITY PROFILE profile") as CreateSecurityProfile;
            Assert.Equal("profile", query.ProfileName);

            query = MiniSQLParser.Parse("CREATE SECURITY PROFILE OtherProfile") as CreateSecurityProfile;
            Assert.Equal("OtherProfile", query.ProfileName);
        }

        [Fact]
        public void CorrectWithSpaces()
        {
            CreateSecurityProfile query = MiniSQLParser.Parse("CREATE     SECURITY PROFILE      profile") as CreateSecurityProfile;
            Assert.Equal("profile", query.ProfileName);

            query = MiniSQLParser.Parse("CREATE SECURITY     PROFILE OtherProfile") as CreateSecurityProfile;
            Assert.Equal("OtherProfile", query.ProfileName);
        }

        [Fact]
        public void IncorrectCapitalization()
        {
            CreateSecurityProfile query = MiniSQLParser.Parse("Create SECURITY PROFILE profile") as CreateSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("create security profile OtherProfile") as CreateSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("CREATE SECURITY PROFILE profile") as CreateSecurityProfile;
            Assert.NotNull(query);
        }

        [Fact]
        public void IncorrectProfileWithForbiddenChars()
        {
            CreateSecurityProfile query = MiniSQLParser.Parse("CREATE SECURITY PROFILE pro-file") as CreateSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("CREATE SECURITY PROFILE Pro file") as CreateSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("CREATE SECURITY PROFILE profile") as CreateSecurityProfile;
            Assert.NotNull(query);
        }

        [Fact]
        public void IncorrectWithoutProfile()
        {
            CreateSecurityProfile query = MiniSQLParser.Parse("CREATE SECURITY PROFILE ") as CreateSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("CREATE SECURITY PROFILE") as CreateSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("CREATE SECURITY PROFILE profile") as CreateSecurityProfile;
            Assert.NotNull(query);
        }
    }
}*/
