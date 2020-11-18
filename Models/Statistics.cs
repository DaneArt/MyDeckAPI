using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Models
{
    public class Statistics : ModelPart
    {
        [Required]
        public Guid User_Id { get; set; }
        [Required]
        public Guid Card_Id { get; set; }
        
        [Required]
        public int XP { get; set; }
        
        public User User { get; set; }
        public Card Card { get; set; }
        public Statistics() : base()
        {

        }
    }
}
