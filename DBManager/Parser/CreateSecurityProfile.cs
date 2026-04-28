using System;
using System.Collections.Generic;
using System.Text;
using DbManager.Parser;
using DbManager.Security;

namespace DbManager
{
 
    public class CreateSecurityProfile : MiniSqlQuery
    {
        public string ProfileName { get; set; }

        public CreateSecurityProfile(string profileName)
        {
            //TODO DEADLINE 4: Initialize member variables
            ProfileName = profileName;
        }
        public string Execute(Database database)
        {
            //TODO DEADLINE 5: Run the query and return the appropriate message
            //UsersProfileIsNotGrantedRequiredPrivilege, CreateSecurityProfileSuccess
            bool esAdmin = database.SecurityManager.IsUserAdmin();
            if (esAdmin == false)
            {
                return Constants.UsersProfileIsNotGrantedRequiredPrivilege;
            }
            Profile profile = new Profile
            {
                Name = ProfileName
            };
            database.SecurityManager.AddProfile(profile);
            return Constants.CreateSecurityProfileSuccess;
                    
        }
    }
}
