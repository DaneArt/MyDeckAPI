using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Models
{
    public class FullDeckModel
    {
        [Key]
        public Guid Deck_Id { get; set; }
        [Required(AllowEmptyStrings = true)]
        public string Title { get; set; }
        [Required(AllowEmptyStrings = true)]
        public string Description { get; set; }
        [Required]
        public bool IsPrivate { get; set; }
        [Required]
        public bool AvailableQuickTrain { get; set; }
        [Required]
        public Guid Icon { get; set; }
        [Required]
        public string Category_Name { get; set; }
        [Required]
        public string Author { get; set; }
        public ICollection<FullCardModel> Cards { get; set; }
        public FullDeckModel() : base()
        {

            Cards = new List<FullCardModel>();
        }
        public Deck ToDeck()
        {
            return new Deck
            {
                Deck_Id = this.Deck_Id,
                Title = this.Title,
                Description = this.Description,
                Is_Private = this.IsPrivate,
                Icon = this.Icon,
                Available_Quick_Train = this.AvailableQuickTrain,
                Category_Name = this.Category_Name,
                Author = this.Author,
                Cards = Cards.Select((c) => new Card
                {
                    Card_Id = c.Card_Id,
                    Answer = c.Answer.Id,
                    Question = c.Question.Id
                }).ToList()
            };
        }
        public static FullDeckModel FromDeck(Deck deck, DbSet<File> files){

            return new FullDeckModel { 
            Deck_Id = deck.Deck_Id,
            Title = deck.Title,
            AvailableQuickTrain = deck.Available_Quick_Train,
            Description = deck.Description,
            IsPrivate = deck.Is_Private,
            Icon = deck.Icon,
            Category_Name = deck.Category_Name,
            Author = deck.Author,
            Cards = deck.Cards.Select((c)=> new FullCardModel { 
            Card_Id = c.Card_Id,
            Answer = new FullCardPart{ Id = c.Answer,
                    Type = files.Find(c.Answer).Type,
            },
                Question = new FullCardPart
                {
                    Id = c.Question,
                    Type = files.Find(c.Question).Type,
                },
            }).ToList()};

            }
    }
}
