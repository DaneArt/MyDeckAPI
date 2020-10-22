using Microsoft.EntityFrameworkCore;
using MyDeckAPI.Data.MediaContent;
using MyDeckAPI.Exceptions;
using MyDeckAPI.Interfaces;
using MyDeckAPI.Models;
using MyDeckApi_Experimental.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Services
{
    public class CardRepository : ICardRepository
    {
        private readonly MDContext _context;
        private readonly DbSet<Card> table;
        private readonly SnakeCaseConverter snakeCaseConverter;

        public CardRepository(MDContext context, SnakeCaseConverter snakeCaseConverter)
        {
            _context = context;
            table = _context.Set<Card>();
            this.snakeCaseConverter = snakeCaseConverter;
        }
        public Task Delete(IEnumerable<Card> cards)
        {
            table.RemoveRange(cards);
            return null;
        }

        public Task<List<Card>> FindAll()
        {
            return table.ToListAsync();
        }

        public ValueTask<Card> FindById(object Id)
        {
            return table.FindAsync(Id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task InsertAsync(IEnumerable<Card> obj)
        {
            try
            {
                var list = new List<Card>();
                var exceptionList = new List<Card>();
                foreach (Card card in obj)
                {
                    if (card.IsValid())
                    {
                        if (await _context.Files.FindAsync(card.Answer) == null)
                        {
                            await _context.Files.AddAsync(new File() { File_Id = card.Answer });
                        }
                        if (await _context.Files.FindAsync(card.Question) == null)
                        {
                            await _context.Files.AddAsync(new File() { File_Id = card.Question });
                        }
                        list.Add(card);
                        await _context.Cards.AddAsync(card);
                    }
                    else
                    {
                        exceptionList.Add(card);
                    }
                }
                
                if (exceptionList.Count > 0) { throw new NonValidatedModelException<Card>(exceptionList); }
                
            }
            catch { throw; }
        }

        public async Task UpdateAsync(IEnumerable<Card> obj)
        {
            try
            {
                var list = new List<Card>();
                var exceptionList = new List<Card>();
                foreach (Card card in obj)
                {
                    if (card.IsValid())
                    {
                        if (await _context.Files.FindAsync(card.Answer) == null)
                        {
                            await _context.Files.AddAsync(new File() { File_Id = card.Answer });
                        }
                        if (await _context.Files.FindAsync(card.Question) == null)
                        {
                            await _context.Files.AddAsync(new File() { File_Id = card.Question });
                        }
                        list.Add(card);
                        _context.Cards.Update(card);
                    }
                    else
                    {
                        exceptionList.Add(card);
                    }
                }
                if (exceptionList.Count > 0) { throw new NonValidatedModelException<Card>(exceptionList); }
            }
            catch { throw; }
        }

    }
}
