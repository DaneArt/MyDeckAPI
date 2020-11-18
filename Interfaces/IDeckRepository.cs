using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyDeckAPI.Models;

namespace MyDeckApi_Experimental.Interfaces
{
    public interface IDeckRepository
    {
        void Delete(Guid deckId);
        Task Insert(Deck deck);
        void Update(Deck deck);
        Task<List<Deck>> FindAll();
        UserModelDeck FindById(Guid Id);
        Deck FindStaticById(Guid Id);
        List<Deck> FindDecksForQuickTrain(Guid userId);
        void Save();
        Task<List<Deck>> AllCurrentUserDecks(string login);
        Task<List<Deck>> AllCurrentUserDecksWithCards(string login);
        Task<List<DeckView>> ChosenCategoryFeed(string categoryname, int pagenumber = 0);
        Task<List<DeckView>> AllUserDecks(Guid id);
    }
}