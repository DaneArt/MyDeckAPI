using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Models
{
    public class Statistics : ModelPart
    {
        public Guid User_Id { get; set; }
        public Guid Card_Id { get; set; }

        [Required]
        public int Wins { get; set; }
        [Required]
        public int Trains { get; set; }
        [Required]
        public int Lvl { get; set; }
        [Required]
        public DateTime Last_Train { get; set; }
        public User User { get; set; }
        public Card Card { get; set; }
        public Statistics() : base()
        {

        }
    }
}
