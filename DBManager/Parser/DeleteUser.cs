using System;
using System.Collections.Generic;
using System.Text;
using DbManager.Parser;

namespace DbManager
{
 
    public class DeleteUser : MiniSqlQuery
    {
        public string Username { get; private set; }

        public DeleteUser(string username)
        {
            //TODO DEADLINE 4: Initialize member variables
            this.Username = username;

            
        }
        public string Execute(Database database)
        {
            //TODO DEADLINE 5: Run the query and return the appropriate message
            //UsersProfileIsNotGrantedRequiredPrivilege, UserDoesNotExistError, DeleteUserSuccess

            if (database == null || database.SecurityManager == null)
                return Constants.UsersProfileIsNotGrantedRequiredPrivilege;

            if (!database.SecurityManager.IsUserAdmin())
                return Constants.UsersProfileIsNotGrantedRequiredPrivilege;

            var user = database.SecurityManager.UserByName(Username);
            if (user == null)
                return Constants.UserDoesNotExistError;

            return Constants.DeleteUserSuccess;

        }

    }
}
