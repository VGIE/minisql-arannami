using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DbManager.Security
{
    public class Manager
    {
        public List<Profile> Profiles { get; private set; } = new List<Profile>();

        private string m_username;
        public Manager(string username)
        {
            m_username = username;
        }

        public bool IsUserAdmin()
        {
            //TODO DEADLINE 5: Return true if the user logged-in (m_username) is the admin, false otherwise
            return m_username.Equals("admin", StringComparison.OrdinalIgnoreCase);
        }

        public bool IsPasswordCorrect(string username, string password)
        {
            //TODO DEADLINE 5: Return true if the user's password is correct. The given password should be encrypted before comparing with the saved one
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return false;

            if (username.Equals("admin", StringComparison.OrdinalIgnoreCase))
                return password == "admin";

            User user = UserByName(username);

            if (user == null)
                return false;

            string encryptedPassword = Encryption.Encrypt(password);
            return user.EncryptedPassword.Equals(encryptedPassword);
        }
        

        public void GrantPrivilege(string profileName, string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Add this privilege on this table to the profile with this name
            
            if (!IsUserAdmin())
                return;
            if (string.IsNullOrEmpty(profileName) || string.IsNullOrEmpty(table))
                return;
            var profile = ProfileByName(profileName);
            //if (profile == null) return;

            if (profile.IsGrantedPrivilege(table, privilege))
                return;

            profile.GrantPrivilege(table, privilege);
        }

        public void RevokePrivilege(string profileName, string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Remove this privilege on this table to the profile with this name
            //If the profile or the table don't exist, do nothing

            if (!IsUserAdmin())
                return;

            if (string.IsNullOrEmpty(profileName) || string.IsNullOrEmpty(table))
                return;

            Profile profile = ProfileByName(profileName);

            if (profile == null)
                return;

            profile.RevokePrivilege(table, privilege);
        }

        public bool IsGrantedPrivilege(string username, string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Return true if the username has this privilege on this table. False otherwise (also in case of error)
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(table) || privilege == null)
                return false;
            
            var profile = ProfileByUser(username);
            if (profile == null) return false;
            return profile.IsGrantedPrivilege(table, privilege);
        }

        public void AddProfile(Profile profile)
        {
            //TODO DEADLINE 5: Add this profile
            if (!IsUserAdmin())
                return;

            if (profile == null)
                return;

            if (string.IsNullOrEmpty(profile.Name))
                return;

            if (ProfileByName(profile.Name) != null)
                return;

            Profiles.Add(profile);
        }

        public User UserByName(string username)
        {
            //TODO DEADLINE 5: Return the user by name. If it doesn't exist, return null
            if (string.IsNullOrEmpty(username)) return null;

            for (int i=0; i<Profiles.Count; i++)
            {
                var profile = Profiles[i];
                for (int j=0; j<profile.Users.Count; j++)
                {
                    var user = profile.Users[j];
                    if(user.Username.Equals(username)) return user;
                }
            }
            return null;
        }

        public Profile ProfileByName(string profileName)
        {
            //TODO DEADLINE 5: Return the profile by name. If it doesn't exist, return null
            if(string.IsNullOrEmpty(profileName)) return null;

            //if (profileName.Equals("admin", StringComparison.OrdinalIgnoreCase)) return new Profile { Name = "admin" };

            for (int i=0; i<Profiles.Count; i++)
            {
                var profile = Profiles[i];
                if(profile.Name.Equals(profileName)) return profile;
            }
            return null;
        }

        public Profile ProfileByUser(string username)
        {
            //TODO DEADLINE 5: Return the profile by user. If the user doesn't exist, return null
            if(string.IsNullOrEmpty (username)) return null;

            for(int i=0; i < Profiles.Count; i++)
            {
                var profile= Profiles[i];
                for(int j=0; j<profile.Users.Count; j++)
                {
                    var user = profile.Users[j];
                    if (user.Username.Equals(username)) return profile;
                }
            }
            return null;
        }

        public bool RemoveProfile(string profileName)
        {
            //TODO DEADLINE 5: Remove this profile
            if (!IsUserAdmin())
                return false;

            if (string.IsNullOrEmpty(profileName))
                return false;
            if (profileName.Equals("admin", StringComparison.OrdinalIgnoreCase))
                return false;

            var profile = ProfileByName(profileName);

            if (profile == null)
                return false;

            return Profiles.Remove(profile);
        }
        

        public static Manager Load(string databaseName, string username)
        {
            //TODO DEADLINE 5: Load all the profiles and users saved for this database. The Manager instance should be created with the given username
            string fileName = databaseName + ".path";
            Manager manager = new Manager(username);
            if (!File.Exists(fileName))
            {
                Profile adminProfile = new Profile()
                {
                    Name = "admin"
                };
                adminProfile.Users.Add(new User("admin", "admin"));
                manager.Profiles.Add(adminProfile);
                return manager;
            }
            string[] lines = File.ReadAllLines(fileName);
            Profile currentProfile = null;
            string currentTable = null;

            foreach (string linea in lines)
            {
                string line = linea.Trim();
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (line.StartsWith("Profile: "))
                {
                    currentTable = null;

                    string profileName = line.Substring("Profile: ".Length).Trim();

                    currentProfile = new Profile()
                    {
                        Name = profileName
                    };

                    manager.Profiles.Add(currentProfile);
                }
                else if (line.StartsWith("User: "))
                {
                    if (currentProfile == null)
                        continue;

                    string content = line.Substring("User: ".Length);
                    string[] parts = content.Split(',');

                    if (parts.Length < 2)
                        continue;

                    string usernamePart = parts[0].Trim();
                    string passwordPart = parts[1].Trim();

                    string userName = usernamePart;
                    string password = passwordPart.Replace("Password: ", "").Trim();

                    currentProfile.Users.Add(new User()
                    {
                        Username = userName,
                        EncryptedPassword = password
                    });
                }
                else if (line.StartsWith("Table: "))
                {
                    currentTable = line.Substring("Table: ".Length).Trim();
                }
                else if (line.StartsWith("Privilege: "))
                {
                    if (currentProfile == null)
                        continue;
                    if (string.IsNullOrWhiteSpace(currentTable)) //HAU KENDU LEIKE goiku ---> WIYHOUT table null testa kendute ra
                        continue;
                    string privilegeStr = line.Substring("Privilege: ".Length).Trim();

                    if (Enum.TryParse(privilegeStr, out Privilege privilege))
                    {
                        currentProfile.GrantPrivilege(currentTable, privilege);
                    }
                }
            }
            if (manager.ProfileByName("admin") == null)
            {
                Profile adminProfile = new Profile() { Name = "admin" };
                adminProfile.Users.Add(new User("admin", "admin"));
                manager.Profiles.Add(adminProfile);
            }

            return manager;
        }
        
        
        public void Save(string databaseName)
        {
            //TODO DEADLINE 5: Save all the profiles and users/passwords created for this database.
            string nombreArchivo = databaseName + ".path";
            string contenidoTotal = "";
            foreach (var profile in Profiles)
            {
                string perfil = $"Profile: {profile.Name}\n";
                contenidoTotal += perfil;
                foreach (var user in profile.Users)
                {
                    string usuario = $"User: {user.Username}, Password: {user.EncryptedPassword}\n";
                    contenidoTotal += usuario;
                }
                foreach (var entrada in profile.PrivilegesOn)
                {
                    string tabla = $"Table: {entrada.Key}\n";
                    contenidoTotal += tabla;
                    foreach (var privilege in entrada.Value)
                    {
                        string privilegio = $"Privilege: {privilege}\n";
                        contenidoTotal += privilegio;
                    }
                }
                contenidoTotal += "\n";
            }
            File.WriteAllText(nombreArchivo, contenidoTotal);


        }
    }
}
