using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerLibrary.Models
{
    [Serializable]
    [Table("Message")]
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("text")]
        public string Text { get; set; }

        public int ChannelId{ get; set; }
        public Channel Channel{ get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
