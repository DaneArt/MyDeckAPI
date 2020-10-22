using System.Collections.Generic;
using System.Threading.Tasks;
using MyDeckAPI.Models;

namespace MyDeckApi_Experimental.Interfaces
{
    public interface IUserDeckRepository
    {
         
         Task Delete(object Id);

         Task<List<UserDeck>> FindAll();

         ValueTask<UserDeck> FindById(object Id);

        Task Insert(UserDeck obj);

         void Save();

        Task Update(UserDeck obj);
    }
}