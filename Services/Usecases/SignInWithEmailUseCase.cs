using MyDeckAPI.Data.MediaContent;
using MyDeckAPI.Exceptions;
using MyDeckAPI.Interfaces;
using MyDeckAPI.Models;
using MyDeckAPI.Security;
using MyDeckApi_Experimental.Interfaces;
using MyDeckApi_Experimental.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyDeckAPI.Services.Usecases
{
    public class SignInWithEmailUseCase : IUseCase<string, SignInWithEmailParams>
    {
        private readonly IUserRepository userRepository;
        private readonly SnakeCaseConverter snakeCaseConverter;
        private readonly ISessionRepository sessionRepository;
        private readonly ITokenRepository tokenRepository;
        private readonly AuthUtils authOptions;

        public SignInWithEmailUseCase(IUserRepository userRepository, SnakeCaseConverter snakeCaseConverter, ISessionRepository sessionRepository, ITokenRepository tokenRepository, AuthUtils authOptions)
        {
            this.userRepository = userRepository;
            this.snakeCaseConverter = snakeCaseConverter;
            this.sessionRepository = sessionRepository;
            this.tokenRepository = tokenRepository;
            this.authOptions = authOptions;
        }

        public async Task<string> Invoke(SignInWithEmailParams param)
        {
            try
            {
                if (userRepository.IsEmailUnique(param.email))
                {
                    var user = await userRepository.FindByEmail(param.email);

                    if (authOptions.validatePassword(param.password, user.Password))
                    {
                        var tokens = tokenRepository.GetNewTokens(user);
                        var currentSession = await sessionRepository.FindById(param.SessionId, user.User_Id);
                        if (currentSession != null)
                        {
                            sessionRepository.RefreshSession(currentSession, tokens.Refresh_Token);
                        }
                        else
                        {
                            await sessionRepository.Insert(new Session
                            {
                                Session_Id = param.SessionId,
                                User_Id = user.User_Id,
                                RefreshToken = tokens.Refresh_Token

                            });
                        }
                        var result = new
                        {
                            access_token = tokens.Access_Token,
                            refresh_token = tokens.Refresh_Token,
                            user_id = user.User_Id,
                            username = user.UserName,
                            email = user.Email,
                            avatar = user.Avatar
                        };
                        return snakeCaseConverter.ConvertToSnakeCase(result);
                    }
                    else
                    {
                        throw new WrongPasswordException();
                    }

                }
                else
                {
                    throw new UserNotFoundException();
                }
            }

            catch { throw; }


        }
    }

    public class SignInWithEmailParams
    {
        public readonly String email;
        public readonly byte[] password;
        public readonly Guid SessionId;

        public SignInWithEmailParams(string email, byte[] password, Guid sessionId)
        {
            this.email = email;
            this.password = password;
            SessionId = sessionId;
        }

        public override bool Equals(object obj)
        {

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }


            return email == (obj as SignInWithEmailParams).email && password == (obj as SignInWithEmailParams).password && SessionId == (obj as SignInWithEmailParams).SessionId;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return email.GetHashCode()+ password.GetHashCode() + SessionId.GetHashCode();
        }

    }
}
