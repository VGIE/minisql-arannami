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
    public class RevokeTests
    {
        [Fact]
        public void Correct()
        {
            Revoke query = MiniSQLParser.Parse("REVOKE DELETE ON Table TO User") as Revoke;
            Assert.Equal("DELETE", query.PrivilegeName);
            Assert.Equal("Table", query.TableName);
            Assert.Equal("User", query.ProfileName);

            query = MiniSQLParser.Parse("REVOKE INSERT ON Table TO User") as Revoke;
            Assert.Equal("INSERT", query.PrivilegeName);
            Assert.Equal("Table", query.TableName);
            Assert.Equal("User", query.ProfileName);

            query = MiniSQLParser.Parse("REVOKE SELECT ON Table TO User") as Revoke;
            Assert.Equal("SELECT", query.PrivilegeName);
            Assert.Equal("Table", query.TableName);
            Assert.Equal("User", query.ProfileName);

            query = MiniSQLParser.Parse("REVOKE UPDATE ON Table TO User") as Revoke;
            Assert.Equal("UPDATE", query.PrivilegeName);
            Assert.Equal("Table", query.TableName);
            Assert.Equal("User", query.ProfileName);
        }

        [Fact]
        public void CorrectWithSpaces()
        {
            Revoke query = MiniSQLParser.Parse("REVOKE DELETE    ON Table TO User") as Revoke;
            Assert.Equal("DELETE", query.PrivilegeName);
            Assert.Equal("Table", query.TableName);
            Assert.Equal("User", query.ProfileName);

            query = MiniSQLParser.Parse("REVOKE INSERT ON Table    TO User") as Revoke;
            Assert.Equal("INSERT", query.PrivilegeName);
            Assert.Equal("Table", query.TableName);
            Assert.Equal("User", query.ProfileName);

            query = MiniSQLParser.Parse("REVOKE SELECT ON Table TO     User") as Revoke;
            Assert.Equal("SELECT", query.PrivilegeName);
            Assert.Equal("Table", query.TableName);
            Assert.Equal("User", query.ProfileName);

            query = MiniSQLParser.Parse("REVOKE    UPDATE     ON    Table    TO     User") as Revoke;
            Assert.Equal("UPDATE", query.PrivilegeName);
            Assert.Equal("Table", query.TableName);
            Assert.Equal("User", query.ProfileName);
        }

        [Fact]
        public void IncorrectProfileWithForbiddenChars()
        {
            Revoke query = MiniSQLParser.Parse("REVOKE DELETE ON Table TO User 1") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE INSERT ON Table TO Us er") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE SELECT ON Table TO User-1") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE UPDATE ON Table To User_2") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE UPDATE ON Table TO User") as Revoke;
            Assert.NotNull(query);
        }

        [Fact]
        public void IncorrectCapitalization()
        {
            Revoke query = MiniSQLParser.Parse("Revoke DELETE ON Table TO User") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE Insert ON Table TO User") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE SELECT on Table TO User") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE UPDATE ON Table To User") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE UPDATE ON Table TO User") as Revoke;
            Assert.NotNull(query);
        }

        [Fact]
        public void IncorrectPrivileges()
        {
            Revoke query = MiniSQLParser.Parse("REVOKE Remove ON Table TO User") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE REMOVE ON Table TO User") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE UPGRADE ON Table TO User") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE SET ON Table TO User") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE UPDATE ON Table TO User") as Revoke;
            Assert.NotNull(query);
        }

        [Fact]
        public void IncorrectWithoutOnePart()
        {
            Revoke query = MiniSQLParser.Parse("REVOKE ON Table TO User") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE SELECT ON TO User") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE SELECT TO User") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE SELECT ON Table TO") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE UPDATE ON Table TO User") as Revoke;
            Assert.NotNull(query);
        }
    }
}
