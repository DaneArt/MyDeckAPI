using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyDeckAPI.Models;
using MyDeckAPI.Services.Usecases;
using MyDeckApi_Experimental.Services.Usecases;

namespace MyDeckApi_Experimental.Services
{
    public class AuthFacade : IAuthFacade
    {


        private readonly SignUpWithGoogleUseCase signUpWithGoogleUseCase;
        private readonly SignUpWithEmailUseCase signUpWithEmailUseCase;
        private readonly SignInWithUsernameUseCase signInWithUsernameUseCase;
        private readonly SignInWithEmailUseCase signInWithEmailUseCase;

        public AuthFacade(SignUpWithGoogleUseCase signUpWithGoogleUseCase, SignUpWithEmailUseCase signUpWithEmailUseCase, SignInWithUsernameUseCase signInWithUsernameUseCase, SignInWithEmailUseCase signInWithEmailUseCase)
        {
            this.signUpWithGoogleUseCase = signUpWithGoogleUseCase;
            this.signUpWithEmailUseCase = signUpWithEmailUseCase;
            this.signInWithUsernameUseCase = signInWithUsernameUseCase;
            this.signInWithEmailUseCase = signInWithEmailUseCase;
        }

        public Task<string> SignInWithEmail(string email, byte[] password, Guid sessionId)
        {
            return signInWithEmailUseCase.Invoke(new SignInWithEmailParams(
                email,password,
                 sessionId
            ));
        }

        public Task<string> SignInWithUsername(string username, byte[] password, Guid sessionId)
        {
            return signInWithUsernameUseCase.Invoke(new SignInWithUsernameParams(
                 username,password,
                 sessionId
            ));
        }

        public Task<string> SignUpWithEmail(User possibleUser, Guid sessionId)
        {
            return signUpWithEmailUseCase.Invoke(new SignUpWithEmailParams(
                 possibleUser,
                 sessionId
            ));
        }

        public Task<string> SignUpWithGoogle(string token, Guid sessionId)
        {
            Debug.WriteLine("------------> Invoke signup with google <------------ \n");
            
            return signUpWithGoogleUseCase.Invoke(new SignUpWithGoogleParams(
                             token,
                             sessionId
                        ));
        }
    }
}