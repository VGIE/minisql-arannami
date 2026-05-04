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
            var profile = ProfileByName(profileName);
            if (profile == null) return;
            if (profile.IsGrantedPrivilege(table, privilege))
                return;

            profile.GrantPrivilege(table, privilege);
        }
       


        public void RevokePrivilege(string profileName, string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Remove this privilege on this table to the profile with this name
            //If the profile or the table don't exist, do nothing
            
        }

        public bool IsGrantedPrivilege(string username, string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Return true if the username has this privilege on this table. False otherwise (also in case of error)
            return false;

        }

        public void AddProfile(Profile profile)
        {
            //TODO DEADLINE 5: Add this profile
            Profiles.Add(profile);

        }

        public User UserByName(string username)
        {
            //TODO DEADLINE 5: Return the user by name. If it doesn't exist, return null

            return null;


        }

        public Profile ProfileByName(string profileName)
        {
            //TODO DEADLINE 5: Return the profile by name. If it doesn't exist, return null
            return null;

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

            Manager manager = new Manager(username);
            string fileName = databaseName + "_security.txt";
            if (!File.Exists(fileName))
                return manager;
            var lines = File.ReadAllLines(fileName);
            Profile currentProfile = null;
            foreach (var line in lines)
            {
                if (line.StartsWith("PROFILE:"))
                {
                    string profileName = line.Substring("PROFILE:".Length);
                    currentProfile = new Profile { Name = profileName };
                    manager.Profiles.Add(currentProfile);
                }
                else if (line.StartsWith("USER:") && currentProfile != null)
                {
                    var parts = line.Substring("USER:".Length).Split(',');
                    string usernameFile = parts[0];
                    string password = parts[1];

                    currentProfile.Users.Add(new User(usernameFile, password));
                }
            }

            return manager;
        }

        public void Save(string databaseName)
        {
            //TODO DEADLINE 5: Save all the profiles and users/passwords created for this database.
            
        }
    }
}
