using System;
using System.Threading.Tasks;
using MyDeckAPI.Models;

namespace MyDeckApi_Experimental.Services
{
    public interface IAuthFacade
    {
         Task<string> SignUpWithGoogle(string token,Guid sessionId);
         Task<string> SignUpWithEmail(User possibleUser, Guid sessionId);
         Task<string> SignInWithEmail(string email, byte[] password, Guid sessionId);
         Task<string> SignInWithUsername(string username, byte[] password, Guid sessionId);
    }
}