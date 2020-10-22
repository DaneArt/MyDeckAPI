using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyDeckAPI.Models;

namespace MyDeckApi_Experimental.Services
{
    public class SessionRepository : ISessionRepository
    {
        private  readonly MDContext _context;
        private readonly DbSet<Session> table;

        public SessionRepository(MDContext context)
        {
            _context = context;
            table = _context.Set<Session>();
        }

        public async ValueTask<Session> FindById(Guid id, Guid userId)
        {
            var session = await table.FindAsync(id, userId);
            return session;
        }

        public async Task Insert(Session session)
        {
            await table.AddAsync(session);
           _context.SaveChangesAsync();

        }

        public void RefreshSession(Session session, string refreshToken)
        {
            session.RefreshToken = refreshToken;
            _context.Update(session);
            _context.SaveChanges();
        }
    }
}