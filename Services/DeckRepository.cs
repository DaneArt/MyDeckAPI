using Microsoft.EntityFrameworkCore;
using MyDeckAPI.Interfaces;
using MyDeckAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MyDeckAPI.Data.MediaContent;
using MyDeckApi_Experimental.Interfaces;

namespace MyDeckAPI.Services
{
    public class DeckRepository : IDeckRepository
    {
        private readonly MDContext _context;
        private readonly DbSet<Deck> table;
        

        public DeckRepository(MDContext context)
        {
            _context = context;
            table = _context.Set<Deck>();
        }

        public  void Delete(Guid deckId)
        {
            var deck = table.Find(deckId);
            table.Remove(deck);
           
        }

        public void Update(Deck deck)
        {
            table.Update(deck);
        }

        public Task<List<Deck>> FindAll() => table.ToListAsync();


        public UserModelDeck FindById(Guid Id)
        {
            Deck deck = _context.Decks.Where((d)=>d.Deck_Id == Id).Include((d)=>d.Cards).ToList()[0];
            User author = _context.Users.Find(Guid.Parse(deck.Author));
            return new UserModelDeck { Author = new UserModel { User_Id = author.User_Id, Email = author.Email, Avatar = author.Avatar, Username = author.UserName}, Deck = FullDeckModel.FromDeck(deck,_context.Files) };

        }
        
        public void Save() => _context.SaveChanges();
        
        public Task<List<Deck>> AllCurrentUserDecks(string login) => _context.Decks.Where(d => d.Author == login && d.IsPrivate == false).ToListAsync();
        public Task<List<Deck>> AllCurrentUserDecksWithCards(string login) => _context.Decks.Where(d => d.Author == login && d.IsPrivate == false).Include(c => c.Cards).ToListAsync();

        public Task<List<DeckView>> ChosenCategoryFeed(string categoryname, int pagenumber = 0) => _context.Decks.Where(d => d.Category_Name == categoryname && d.IsPrivate == false)
                                    .Skip(pagenumber * 15)
                                    .Take(15)
                                    .Select(d => new
                                   DeckView
                                    {
                                        Deck_Id = d.Deck_Id,
                                        Title = d.Title,
                                        Description = d.Description,
                                        IsPrivate = d.IsPrivate,
                                        Icon = d.Icon,
                                        Category_Name = d.Category_Name,
                                        Author = d.Author,
                                        Cards_Count = d.Cards.Count,
                                        Subscribers_Count = _context.UserDecks.Where(ud => ud.Deck_Id == d.Deck_Id).Count()
                                    }).ToListAsync();
        public Task<List<DeckView>> AllUserDecks(Guid id) => (from userdeck in _context.Set<UserDeck>().Where(u=>u.User_Id==id)
                        join deck in _context.Set<Models.Deck>().Include(d=>d.Cards)
                            on userdeck.Deck_Id equals deck.Deck_Id
                                                          select new
                                   DeckView
                                                          {
                                                              Deck_Id = deck.Deck_Id,
                                                              Title = deck.Title,
                                                              Description = deck.Description,
                                                              IsPrivate = deck.IsPrivate,
                                                              Icon = deck.Icon,
                                                              Category_Name = deck.Category_Name,
                                                              Author = deck.Author,
                                                              Cards_Count = deck.Cards.Count,
                                                              AvailableQuickTrain = deck.AvailableQuickTrain,
                                                              Subscribers_Count = _context.UserDecks.Where(ud => ud.Deck_Id == deck.Deck_Id).Count()
                                                          }).ToListAsync();

        public  Task Insert(Deck deck)
        {
            _context.Add(deck);
           return _context.SaveChangesAsync();
        }

        



        /*public string WatchDeck(Guid id)
        {
            var deck = _context.Decks.Where(d => d.Deck_Id == id).Include(d => d.Cards).FirstOrDefault();
            //TODO REFACTORING
            var repo = new UserRepository(_context,snakeCaseConverter);
            var usr = JsonConvert.DeserializeObject(repo.UserProfile(Guid.Parse(deck.Author)));
            return snakeCaseConverter.ConvertToSnakeCase(new { Deck = deck, Author = usr });
        }*/
    }
}
