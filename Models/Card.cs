using MyDeckAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Models
{
    public class Card : ModelPart, IEquatable<Card>,IValidator
    {
        [Key]
        public Guid Card_Id { get; set; }
        [Required]
        public Guid Answer { get; set; }
        [Required]
        public Guid Question { get; set; }
        public Guid Parent_Deck_Id { get; set; }
        public File _Answer { get; set; } 
        public File _Question { get; set; }
        public Deck Parent_Deck { get; set; }
        public ICollection<Statistics> Statistics { get; set; }
        public Card():base()
        {
            Statistics = new List<Statistics>();
        }


        public bool IsValid()
        {
            if (
            Card_Id != null &&
            Card_Id != Guid.Empty &&
            Answer != null &&
            Question != null &&
            
            Answer != Guid.Empty &&
            Question != Guid.Empty
               )
            {
                return true;
            }
            return false;
        }

        public bool Equals([AllowNull] Card obj)
        {
            var card = obj as Card;
            if (Card_Id == card.Card_Id &&
               Answer == card.Answer &&
               Question == card.Question &&
               Parent_Deck_Id == card.Parent_Deck_Id &&
               _Answer == card._Answer &&
               _Question == card._Question &&
               Parent_Deck == Parent_Deck
               )
            {
                return true;
            }
            return false;
        }
    }
}
