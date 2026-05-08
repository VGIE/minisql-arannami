using DbManager.Parser;
using DbManager.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbManager
{
 
    public class Revoke : MiniSqlQuery
    {
        public string PrivilegeName { get; set; }
        public string TableName { get; set; }
        public string ProfileName { get; set; }

        public Revoke(string privilegeName, string tableName, string profileName)
        {
            //TODO DEADLINE 4: Initialize member variables
            this.PrivilegeName = privilegeName;
            this.TableName = tableName;
            this.ProfileName = profileName;

        }
        public string Execute(Database database)
        {
            //TODO DEADLINE 5: Run the query and return the appropriate message
            //UsersProfileIsNotGrantedRequiredPrivilege, SecurityProfileDoesNotExistError, RevokePrivilegeSuccess, 

            if (database == null || database.SecurityManager == null)
                return Constants.UsersProfileIsNotGrantedRequiredPrivilege;

            if (!database.SecurityManager.IsUserAdmin())
                return Constants.UsersProfileIsNotGrantedRequiredPrivilege;

            var profile = database.SecurityManager.ProfileByName(ProfileName);
            if (profile == null)
                return Constants.SecurityProfileDoesNotExistError;

            if (!Enum.TryParse(PrivilegeName, true, out Privilege privilege))
                return Constants.UsersProfileIsNotGrantedRequiredPrivilege;

            database.SecurityManager.RevokePrivilege(ProfileName, TableName, privilege);

            return Constants.RevokePrivilegeSuccess;
        }

    }
}
