using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace ServerLibrary.Models
{
    [Serializable]
    [Table("User")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public bool Admin { get; set; }

        public virtual ICollection<Channel> Channels { get; set; }

        public virtual ICollection<Message> Messages { get; set; }

        public User()
        {
            Channels = new HashSet<Channel>();
            Messages = new HashSet<Message>();
        }

        public static string CreatePassword(string input)
        {
            using (MD5 md5 = MD5.Create())
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
