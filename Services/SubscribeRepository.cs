using Microsoft.EntityFrameworkCore;
using MyDeckAPI.Interfaces;
using MyDeckAPI.Models;
using MyDeckApi_Experimental.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Services
{
    public class SubscribeRepository : ISubscribeRepository
    {
        private readonly MDContext _context;
        private readonly DbSet<Subscribe> table;
        public SubscribeRepository(MDContext context)
        {
            _context = context;
            table = _context.Set<Subscribe>();
        }

        public Task Delete(Guid Id)
        {
            Subscribe exists = table.Find(Id);
            table.Remove(exists);
            return Task.Run(()=> _context.SaveChangesAsync()); 
        }

        public Task<List<Subscribe>> FindAll()
        {
            return table.ToListAsync();
        }

        public ValueTask<Subscribe> FindById(Guid Id)
        {
            return table.FindAsync(Id);
        }

        public async Task Insert(Subscribe obj)
        {
            await table.AddAsync(obj);
            _context.SaveChangesAsync();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public Task Update(Subscribe obj)
        {
            table.Update(obj);
            return Task.Run(() => _context.SaveChangesAsync());
        }
    }
}
