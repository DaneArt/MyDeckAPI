using MyDeckAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Models
{
    public class Deck:ModelPart,IEquatable<Deck>,IValidator
    {
        [Key]
        public Guid Deck_Id { get; set; }
        [Required(AllowEmptyStrings =true)]
        public string Title { get; set; }
        [Required(AllowEmptyStrings = true)]
        public string Description { get; set; }
        [Required]
        public bool Is_Private { get; set; }
        [Required]
        public bool Available_Quick_Train { get; set; }
        [Required]
        public Guid Icon { get; set; }
        [Required]
        public string Category_Name { get; set; }
        public Category Category { get; set; }
        [Required]
        public string Author { get; set; }
        public File _Icon { get; set; }
        public int XP { get; set; }
        public ICollection<UserDeck> UserDecks { get; set; }
        public ICollection<Card> Cards { get; set; }
        public Deck() : base()
        {
            UserDecks = new List<UserDeck>();
            Cards = new List<Card>();
        }
         
        public bool Equals([AllowNull] Deck other)
        {
            throw new NotImplementedException();
        }

        public bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
