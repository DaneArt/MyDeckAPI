using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyDeckAPI.Models;

namespace MyDeckApi_Experimental.Interfaces
{
    public interface ISubscribeRepository
    {
        Task Delete(Guid Id);
        Task<List<Subscribe>> FindAll();
        ValueTask<Subscribe> FindById(Guid Id);
        Task Insert(Subscribe obj);
        void Save();
        Task Update(Subscribe obj);

    }

}