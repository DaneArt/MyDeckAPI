using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using MyDeckAPI.Interfaces;
using MyDeckAPI.Models;
using MyDeckAPI.Security;
using MyDeckApi_Experimental.Interfaces;

namespace MyDeckAPI.Services.Usecases
{
    public class UpdateDeckUseCase : IUseCase<int, UpdateDeckParams>
    {
        public UpdateDeckUseCase(IDeckRepository deckRepository,
            ICardRepository cardRepository, IFileRepository fileRepository, AuthUtils authUtils)
        {
            this.deckRepository = deckRepository;
            this.cardRepository = cardRepository;
            _fileRepository = fileRepository;
            _authUtils = authUtils;
        }

        private readonly IDeckRepository deckRepository;
        private readonly ICardRepository cardRepository;
        private readonly IFileRepository _fileRepository;
        private readonly AuthUtils _authUtils;

        public async Task<int> Invoke(UpdateDeckParams param)
        {
            var oldDeck = deckRepository.FindStaticById(param.NewDeck.Deck_Id);

            try
            {
                var mappedOldCards = oldDeck.Cards;
                var newDeck = param.NewDeck;

                if (oldDeck.Icon != newDeck.Icon)
                {
                    var fileMD5 = _authUtils.GetMd5HashString(newDeck.Icon.ToString());
                    var filePath = _authUtils.GetFilePathFromMD5(fileMD5);
                    _fileRepository.Insert(new File{File_Id = newDeck.Icon, Md5 = fileMD5, Path = filePath,Size = 0,Type = "",});
                }
                
                //update entries
                oldDeck.Category_Name = newDeck.Category_Name;
                oldDeck.Icon = newDeck.Icon;
                oldDeck.Title = newDeck.Title;
                oldDeck.Description = newDeck.Description;
                oldDeck.Is_Private = newDeck.Is_Private;
                oldDeck.Available_Quick_Train = newDeck.Available_Quick_Train;
                oldDeck.LastUpdate = DateTime.Now;
                
                 var cardsToAdd = SortCardsToAdd(mappedOldCards, param.NewDeck.Cards);
                 var cardsToDelete = SortCardsToDelete(mappedOldCards, param.NewDeck.Cards);
                 var cardsToUpdate = SortCardsToUpdate(mappedOldCards, param.NewDeck.Cards);
                 if (cardsToAdd.Count != 0)
                 {
                    
                     foreach (Card card in cardsToAdd)
                     {
                         card.Parent_Deck_Id = param.NewDeck.Deck_Id;
                         
                     }
                     await cardRepository.InsertAsync(cardsToAdd);
                 }

                 if (cardsToDelete.Count != 0)
                 {
                     cardRepository.Delete(cardsToDelete);
                     foreach (Card card in cardsToDelete)
                     {
                         oldDeck.Cards.Remove(card);
                     }
                 }

                 if (cardsToUpdate.Count != 0)
                 {
                     foreach (Card card in cardsToUpdate)
                     {
                         card.Parent_Deck_Id = newDeck.Deck_Id;
                     }
                     await cardRepository.UpdateAsync(cardsToUpdate);

                 }
                
                deckRepository.Save();
                return 0;
            }
            catch (Exception e)
            {
                 deckRepository.Delete(param.NewDeck.Deck_Id);
                 await deckRepository.Insert(deckRepository.FindStaticById(param.NewDeck.Deck_Id));
                deckRepository.Save();
                return 1;
            }
        }

        private List<Card> SortCardsToAdd(IEnumerable<Card> oldCards, IEnumerable<Card> newCards)
        {
            return (from cards in newCards
                where !oldCards.Select(c => c.Card_Id).Contains(cards.Card_Id)
                select cards).ToList();
        }

        private List<Card> SortCardsToDelete(IEnumerable<Card> oldCards, IEnumerable<Card> newCards)
        {
            return (from cards in oldCards
                where !newCards.Select(c => c.Card_Id).Contains(cards.Card_Id)
                select cards).ToList();
        }

        private List<Card> SortCardsToUpdate(IEnumerable<Card> oldCards, IEnumerable<Card> newCards)
        {
            return (from cards in newCards
                where oldCards.Select(c => c.Card_Id).Contains(cards.Card_Id)
                select cards).ToList();
        }
    }

    public class UpdateDeckParams
    {
        public Deck NewDeck { get; set; }

        public UpdateDeckParams(Deck newDeck)
        {
            this.NewDeck = newDeck;
        }
    }
}