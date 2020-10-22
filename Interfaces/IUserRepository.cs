using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyDeckAPI.Models;

namespace MyDeckApi_Experimental.Services
{
    public interface IUserRepository
    {
        Task Delete(Guid Id);
        Task<List<User>> FindAll();
        ValueTask<User> FindById(Guid Id);
        Task<User> FindByUsername(string username);
        Task<User> FindByEmail(string email);
        Task Insert(User user);
        void Save();
        Task Update(User user);
        bool IsUserNameUnique(string name);
        bool IsEmailUnique(string email);
        bool ValidateUserUnique(User user);
        Task<User> getUserIfGoogleAuth(string gooleId);
        string GetUniqueUserName(string name);
        ValueTask<string> GetUserProfile(Guid userId);
        ValueTask<string> GetSubscribersOfDeck(Guid userId);
    }
}