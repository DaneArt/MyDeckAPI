using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Models
{
    public class User : ModelPart
    {
        [Key]
        public Guid User_Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        public string GoogleId { get; set; }
        [Required]
        [MaxLength(45)]
        public byte[] Password { get; set; }
        public Guid Avatar { get; set; }
        public string Locale { get; set; }
        public string Role_Name { get; set; }
        public Role Role { get; set; }
        public File _Avatar { get; set; }
        public ICollection<Subscribe> Followers { get; set; }
        public ICollection<Subscribe> Publishers { get; set; }
        public ICollection<UserDeck> UserDecks { get; set; }
        public ICollection<Session> Sessions { get; set; }
        public ICollection<Statistics> Statistics { get; set; }
        public User() : base()
        {
            Statistics = new List<Statistics>();
            UserDecks = new List<UserDeck>();
            Sessions = new List<Session>();
            Publishers = new List<Subscribe>();
            Followers = new List<Subscribe>();
            
        }


    }
}
