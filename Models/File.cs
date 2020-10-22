using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Models
{
    public class File : ModelPart
    {
        [Key]
        public Guid File_Id { get; set; }
        [Required(AllowEmptyStrings = true)]
        public string Type { get; set; }
        [Required(AllowEmptyStrings = true)]
        public string Md5 { get; set; }
        [Required(AllowEmptyStrings = true)]
        public string Path { get; set; }
        [Required]
        public long Size { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<Deck> Decks { get; set; }
        public ICollection<Card> Answers { get; set; }
        public ICollection<Card> Questions { get; set; }
        public File() : base()
        {
            Users = new List<User>();
            Answers = new List<Card>();
            Questions = new List<Card>();
            Decks = new List<Deck>();
        }
    }
}
