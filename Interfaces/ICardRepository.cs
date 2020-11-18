using System.Collections.Generic;
using System.Threading.Tasks;
using MyDeckAPI.Models;

namespace MyDeckApi_Experimental.Interfaces
{
    public interface ICardRepository
    {

        void Delete(IEnumerable<Card> cards);
        Task<List<Card>> FindAll();
        ValueTask<Card> FindById(object Id);
        void Save();
        Task InsertAsync(IEnumerable<Card> obj);
        Task UpdateAsync(IEnumerable<Card> obj);


    }
}