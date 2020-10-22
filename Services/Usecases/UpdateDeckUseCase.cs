using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyDeckAPI.Interfaces;
using MyDeckAPI.Models;
using MyDeckApi_Experimental.Interfaces;

namespace MyDeckAPI.Services.Usecases
{
    public class UpdateDeckUseCase : IUseCase<int, UpdateDeckParams>
    {
        public UpdateDeckUseCase(IDeckRepository deckRepository,
            ICardRepository cardRepository)
        {
            this.deckRepository = deckRepository;
            this.cardRepository = cardRepository;
        }

        private readonly IDeckRepository deckRepository;
        private readonly ICardRepository cardRepository;

        public async Task<int> Invoke(UpdateDeckParams param)
        {                var oldDeck = deckRepository.FindById(param.NewDeck.Deck_Id);

            try
            {
                var mappedOldCards = oldDeck.Deck.Cards.Select(c => new Card
                {
                    Card_Id = c.Card_Id,
                    Question = c.Question.Id,
                    Answer = c.Answer.Id,
                }).ToList();

                var cardsToAdd = SortCardsToAdd(mappedOldCards, param.NewDeck.Cards);
                var cardsToDelete = SortCardsToDelete(mappedOldCards, param.NewDeck.Cards);
                var cardsToUpdate = SortCardsToUpdate(mappedOldCards, param.NewDeck.Cards);
                await cardRepository.InsertAsync(cardsToAdd);
                await cardRepository.Delete(cardsToDelete);
                await cardRepository.UpdateAsync(cardsToUpdate);

                deckRepository.Update(param.NewDeck);
                deckRepository.Save();
                return 0;
            }
            catch (Exception e)
            {
                deckRepository.Delete(param.NewDeck.Deck_Id);
                await deckRepository.Insert(oldDeck.Deck.ToDeck());
                deckRepository.Save();
                return 1;
            }
            
        }

        private List<Card> SortCardsToAdd(IEnumerable<Card> oldCards, IEnumerable<Card> newCards)
        {
            return (List<Card>) from cards in newCards
                where !oldCards.Select(c => c.Card_Id).Contains(cards.Card_Id)
                select cards;
        }

        private List<Card> SortCardsToDelete(IEnumerable<Card> oldCards, IEnumerable<Card> newCards)
        {
            return (List<Card>) from cards in oldCards
                where !newCards.Select(c => c.Card_Id).Contains(cards.Card_Id)
                select cards;
        }

        private List<Card> SortCardsToUpdate(IEnumerable<Card> oldCards, IEnumerable<Card> newCards)
        {
            return (List<Card>) from cards in newCards
                where oldCards.Select(c => c.Card_Id).Contains(cards.Card_Id)
                select cards;
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