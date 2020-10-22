using Microsoft.EntityFrameworkCore;
using MyDeckAPI.Interfaces;
using MyDeckAPI.Models;
using MyDeckApi_Experimental.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Services
{
    public class UserDeckRepository : IUserDeckRepository
    {
        private readonly MDContext _context;
        private readonly DbSet<UserDeck> table;

        public UserDeckRepository(MDContext context)
        {
            _context = context;
            table = _context.Set<UserDeck>();
        }

        public Task Delete(object Id)
        {
            UserDeck exists = table.Find(Id);
            table.Remove(exists);
            return Task.Run(() => _context.SaveChangesAsync());
        }

        public Task<List<UserDeck>> FindAll()
        {
            return table.ToListAsync();
        }

        public ValueTask<UserDeck> FindById(object Id)
        {
            return table.FindAsync(Id);
        }

        public async Task Insert(UserDeck obj)
        {
            await table.AddAsync(obj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public Task Update(UserDeck obj)
        {
            table.Update(obj);
            return Task.Run(() => _context.SaveChangesAsync());
        }
        
    }
}
