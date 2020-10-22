using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Models
{
    public class UserDeck : ModelPart
    {
        public Guid User_Id { get; set; }
        public User User { get; set; }

        
        public Guid Deck_Id { get; set; } 
        public Deck Deck { get; set; }
        public UserDeck() : base()
        {

        }
    }
}
