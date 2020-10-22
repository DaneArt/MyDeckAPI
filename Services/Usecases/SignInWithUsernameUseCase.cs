using MyDeckAPI.Data.MediaContent;
using MyDeckAPI.Exceptions;
using MyDeckAPI.Interfaces;
using MyDeckAPI.Models;
using MyDeckAPI.Security;
using MyDeckApi_Experimental.Interfaces;
using MyDeckApi_Experimental.Services;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace MyDeckAPI.Services.Usecases
{
    public class SignInWithUsernameUseCase : IUseCase<string, SignInWithUsernameParams>
    {
        private readonly IUserRepository userRepository;
        private readonly SnakeCaseConverter snakeCaseConverter;
        private readonly ISessionRepository sessionRepository;
        private readonly ITokenRepository tokenRepository;
        private readonly AuthUtils authOptions;

        public SignInWithUsernameUseCase(IUserRepository userRepository, SnakeCaseConverter snakeCaseConverter, ISessionRepository sessionRepository, ITokenRepository tokenRepository, AuthUtils authOptions)
        {
            this.userRepository = userRepository;
            this.snakeCaseConverter = snakeCaseConverter;
            this.sessionRepository = sessionRepository;
            this.tokenRepository = tokenRepository;
            this.authOptions = authOptions;
        }

        public async Task<string>  Invoke(SignInWithUsernameParams param)
        {
            try
            {
                if (!userRepository.IsUserNameUnique(param.username))
                {
                    var user = await userRepository.FindByUsername(param.username);
                   
                    if(authOptions.validatePassword(param.password, user.Password))
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

    public class SignInWithUsernameParams
    {
        public readonly String username;
        public readonly byte[] password;
        public readonly Guid SessionId;

        public SignInWithUsernameParams(string username, byte[] password, Guid sessionId)
        {
            this.username = username;
            this.password = password;
            SessionId = sessionId;
        }

        public override bool Equals(object obj)
        {

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }


            return username == (obj as SignInWithUsernameParams).username && password == (obj as SignInWithUsernameParams).password && SessionId == (obj as SignInWithUsernameParams).SessionId;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return username.GetHashCode()+ password.GetHashCode() + SessionId.GetHashCode();
        }

    }
}
