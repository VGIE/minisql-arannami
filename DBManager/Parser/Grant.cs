using DbManager.Parser;
using DbManager.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbManager
{
 
    public class Grant : MiniSqlQuery
    {
        public string PrivilegeName { get; set; }
        public string TableName { get; set; }
        public string ProfileName { get; set; }

        public Grant(string privilegeName, string tableName, string profileName)
        {
            //TODO DEADLINE 4: Initialize member variables
            this.PrivilegeName = privilegeName;
            this.TableName = tableName;
            this.ProfileName= profileName;
            
        }
        public string Execute(Database database)
        {
            //TODO DEADLINE 5: Run the query and return the appropriate message
            //UsersProfileIsNotGrantedRequiredPrivilege, SecurityProfileDoesNotExistError, PrivilegeDoesNotExistError, GrantPrivilegeSuccess, ProfileAlreadyHasPrivilege
            var profile = database.SecurityManager.ProfileByName(ProfileName);
            if (profile == null)
            {
                return Constants.SecurityProfileDoesNotExistError;
            }
            var table = database.TableByName(TableName);
            if (table == null)
            {
                return Constants.TableDoesNotExistError;
            }
            if (!Enum.TryParse<Privilege>(PrivilegeName, out var privilege))
            {
                return Constants.PrivilegeDoesNotExistError;
            }
            if (database.SecurityManager.IsGrantedPrivilege(ProfileName, TableName, privilege))
            {
                return Constants.ProfileAlreadyHasPrivilege;
            }
            database.SecurityManager.GrantPrivilege(ProfileName, TableName, privilege);

            return Constants.GrantPrivilegeSuccess;
        }

    }
}
