using System;
using System.Security.Cryptography;
using System.Text;


namespace DbManager.Security
{
    public class User
    {
        public string Username { get; set; }
        public string EncryptedPassword { get; set; }

        public User(string username, string password)
        {
            //TODO DEADLINE 5: Initialize the member variables. We must encrypt the password
            this.Username = username;
            this.EncryptedPassword = password;

        }

        public User() {
           
            }
        }
  } 

