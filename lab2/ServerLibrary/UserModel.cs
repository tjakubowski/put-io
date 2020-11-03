using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary
{
    public class UserModel
    {
        public int Id;
        public string Username;
        public string Password;

        public UserModel() {}

        public UserModel(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public static string CreatePassword(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (var t in hashBytes)
                    sb.Append(t.ToString("X2"));

                return sb.ToString();
            }
        }

    }
}
