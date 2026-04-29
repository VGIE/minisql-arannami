using System;
using System.Collections.Generic;
using System.Text;
using DbManager.Parser;

namespace DbManager
{
 
    public class DropSecurityProfile : MiniSqlQuery
    {
        public string ProfileName { get; set; }

        public DropSecurityProfile(string profileName)
        {
            //TODO DEADLINE 4: Initialize member variables
            this.ProfileName = profileName;
        }
        public string Execute(Database database)
        {
            //TODO DEADLINE 5: Run the query and return the appropriate message
            //UsersProfileIsNotGrantedRequiredPrivilege, SecurityProfileDoesNotExistError, DropSecurityProfileSuccess
            database.SecurityManager.RemoveProfile(this.ProfileName);
            if (string.IsNullOrEmpty(database.LastErrorMessage))
            {
                return Constants.DropSecurityProfileSuccess;
            }
            return database.LastErrorMessage;
            
        }

    }
}
