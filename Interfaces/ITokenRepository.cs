using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyDeckAPI.Models;
using MyDeckAPI.Security;

namespace MyDeckApi_Experimental.Services
{
    public interface ITokenRepository
    {
        Task<bool> ValidateGoogleIdToken(string token);
        bool ValidateExpiredAccessToken(string token);
        Task<string> GetEmailConfirmationToken(Guid userId);
        Tokens GetNewTokens(User user);
    }
}