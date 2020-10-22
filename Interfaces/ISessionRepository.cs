using System;
using System.Threading.Tasks;
using MyDeckAPI.Models;

namespace MyDeckApi_Experimental.Services
{
    public interface ISessionRepository
    {
        ValueTask<Session> FindById(Guid id, Guid userId);
        Task Insert(Session session);

        void RefreshSession(Session session, string refreshToken);
    }
}