using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbManager.Security
{
    public class Profile
    {
        public const string AdminProfileName = "Admin";
        public string Name { get; set; }
        public List<User> Users { get; set; } = new List<User>();

        public Dictionary<string, List<Privilege>> PrivilegesOn { get; private set; } = new Dictionary<string, List<Privilege>>();

        public bool GrantPrivilege(string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Grant this privilege on this table. Return false if there is an error, true otherwise
            string tableName = table.ToUpper();

            if (!PrivilegesOn.ContainsKey(tableName))
            {
                PrivilegesOn[tableName] = new List<Privilege>();
            }

            if (!PrivilegesOn[tableName].Contains(privilege))
            {
                PrivilegesOn[tableName].Add(privilege);
                return true;
            }
            return false;
        }

        public bool IsGrantedPrivilege(string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Return whether this profile is granted this privilege on this table
            string tableName = table.ToUpper();
            if (PrivilegesOn.ContainsKey(tableName))
            {
                return PrivilegesOn[tableName].Contains(privilege);
            }

            return false;

        }

        public bool RevokePrivilege(string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Revoke this privilege on this table. Return false if there is an error, true otherwise
            string tableName = table.ToUpper();

            if (!PrivilegesOn.ContainsKey(tableName))
                return false;

            if (string.IsNullOrEmpty(table))
                return false;

            return PrivilegesOn[tableName].Remove(privilege);
        }
        
    }
}
