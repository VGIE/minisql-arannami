using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DbManager.Security
{
    public class Manager
    {
        public List<Profile> Profiles { get; private set; } = new List<Profile>();

        private string m_username;
        public Manager(string username)
        {
            m_username = username;
            if (ProfileByName("admin") == null)
            { 
                var adminProfile = new Profile()
                {
                    Name = "admin"
                };
                adminProfile.Users.Add(new User("admin", Encryption.Encrypt("admin")));
                Profiles.Add(adminProfile);
            }
        }

        public bool IsUserAdmin()
        {
            //TODO DEADLINE 5: Return true if the user logged-in (m_username) is the admin, false otherwise

            return m_username.Equals("admin", StringComparison.OrdinalIgnoreCase);

        }
        

        public bool IsPasswordCorrect(string username, string password)
        {
            //TODO DEADLINE 5: Return true if the user's password is correct. The given password should be encrypted before comparing with the saved one
            User user = UserByName(username);

            if (user == null)
                return false;

            string encryptedPassword = Encryption.Encrypt(password);
            return user.EncryptedPassword.Equals(encryptedPassword);
            
        }

        public void GrantPrivilege(string profileName, string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Add this privilege on this table to the profile with this name
            //If the profile or the table don't exist, do nothing
            var profile = ProfileByName(profileName);
            if (profile != null)
            {
                profile.GrantPrivilege(table, privilege);
            }
        }

        public void RevokePrivilege(string profileName, string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Remove this privilege on this table to the profile with this name
            //If the profile or the table don't exist, do nothing
            
        }

        public bool IsGrantedPrivilege(string username, string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Return true if the username has this privilege on this table. False otherwise (also in case of error)
            if (IsUserAdmin()) return true;
            var profile = ProfileByUser(username);
            if (profile == null) return false;
            return profile.IsGrantedPrivilege(table, privilege);

        }

        public void AddProfile(Profile profile)
        {
            //TODO DEADLINE 5: Add this profile
            Profiles.Add(profile);

        }

        public User UserByName(string username)
        {
            //TODO DEADLINE 5: Return the user by name. If it doesn't exist, return null

            foreach (var profile in Profiles)
            {
                var user = profile.Users.FirstOrDefault(u => u.Username == username);
                if (user != null)
                    return user;
            }

            return null;

        }

        public Profile ProfileByName(string profileName)
        {
            //TODO DEADLINE 5: Return the profile by name. If it doesn't exist, return null

            return Profiles.FirstOrDefault(p => p.Name == profileName);

        }

        public Profile ProfileByUser(string username)
        {
            //TODO DEADLINE 5: Return the profile by user. If the user doesn't exist, return null

            return Profiles.FirstOrDefault(p => p.Users.Any(u => u.Username == username));
        }
            

        public bool RemoveProfile(string profileName)
        {
            //TODO DEADLINE 5: Remove this profile
            
            return false;
        }

        public static Manager Load(string databaseName, string username)
        {
            //TODO DEADLINE 5: Load all the profiles and users saved for this database. The Manager instance should be created with the given username
            
            return null;
            
        }

        public void Save(string databaseName)
        {
            //TODO DEADLINE 5: Save all the profiles and users/passwords created for this database.
            
        }
    }
}
