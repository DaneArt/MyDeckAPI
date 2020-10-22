using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using MyDeckAPI.Data.MediaContent;
using MyDeckAPI.Exceptions;
using MyDeckAPI.Interfaces;
using MyDeckAPI.Models;
using MyDeckAPI.Security;
using MyDeckApi_Experimental.Interfaces;
using Newtonsoft.Json;

namespace MyDeckApi_Experimental.Services.Usecases
{
    public class SignUpWithGoogleUseCase : IUseCase<string, SignUpWithGoogleParams>
    {
        private readonly IUserRepository userRepository;
        private readonly SnakeCaseConverter snakeCaseConverter;
        private readonly ISessionRepository sessionRepository;
        private readonly ITokenRepository tokenRepository;
        private readonly ContentSaver contentSaver;
        private readonly IFileRepository fileRepository;
        private readonly AuthUtils authOptions;

        public SignUpWithGoogleUseCase(IUserRepository userRepository, SnakeCaseConverter snakeCaseConverter, ISessionRepository sessionRepository, ITokenRepository tokenRepository, ContentSaver contentSaver, IFileRepository fileRepository, AuthUtils authOptions)
        {
            this.userRepository = userRepository;
            this.snakeCaseConverter = snakeCaseConverter;
            this.sessionRepository = sessionRepository;
            this.tokenRepository = tokenRepository;
            this.contentSaver = contentSaver;
            this.fileRepository = fileRepository;
            this.authOptions = authOptions;
        }

        public async Task<string> Invoke(SignUpWithGoogleParams param)
        {
            try
            {
                if (await tokenRepository.ValidateGoogleIdToken(param.Token))
                {
                    var googleIdTokenSample = new
                    {
                        sub = "",
                        email = "",
                        email_verified = false,
                        picture = "",
                        locale = ""
                    };
                    var handler = new JwtSecurityTokenHandler();
                    var tkn = handler.ReadJwtToken(param.Token);
                    var idToken = JsonConvert.DeserializeAnonymousType(tkn.Payload.SerializeToJson(), googleIdTokenSample);
                    
                    string name = idToken.email.Substring(0, idToken.email.IndexOf('@'));
                    
                    var googleusr = await userRepository.getUserIfGoogleAuth(idToken.sub);


                    if (googleusr != null)
                    {
                        return await RefreshSessionAsync(googleusr, param.SessionId);
                    }
                    else if (idToken.email_verified)
                    {
                        var avatarId = Guid.NewGuid();
                        await SaveAvatar(idToken.picture,avatarId);
                        var userId = Guid.NewGuid();

                        var possibleUser = new User
                        {
                            User_Id = userId,
                            UserName = name,
                            Email = idToken.email,
                            Password = authOptions.GeneratePaswordWithSaltForUser(userId),
                            Locale = idToken.locale,
                            GoogleId = idToken.sub,
                            Role_Name = "User",
                            Avatar = avatarId 
                        };

                        return await RegisterUser(possibleUser);
                    }
                    else throw new NonConfirmedEmailException();
                }
                else throw new NonValidatedGoogleIdTokenException();
            }
            catch { throw; }
        }

        private async Task<string> RefreshSessionAsync(User user, Guid sessionId)
        {
            var tokens = tokenRepository.GetNewTokens(user);
            var currentSession = await sessionRepository.FindById(sessionId, user.User_Id);
            if(currentSession != null)
            {
                sessionRepository.RefreshSession( currentSession, tokens.Refresh_Token);
            }
            else
            {
                await sessionRepository.Insert(new Session
                {
                    Session_Id = sessionId,
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
        private async Task<string> RegisterUser(User user)
        {
            if ( userRepository.IsEmailUnique(user.Email))
            {

                if (!userRepository.IsUserNameUnique(user.UserName))
                {
                    user.UserName = userRepository.GetUniqueUserName(user.UserName);
                }
                
                await userRepository.Insert(user);

                
                var tokens = tokenRepository.GetNewTokens(user);
                await sessionRepository.Insert(new Session
                {
                    Session_Id = Guid.NewGuid(),
                    User_Id = user.User_Id,
                    RefreshToken = tokens.Refresh_Token
                });
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
            else throw new AlreadyUsedEmailException();


        }


        private async Task SaveAvatar(string netPath, Guid fileId)
        {
            var fileModel = await contentSaver.DownloadPicture(netPath, fileId);
             fileRepository.Insert(fileModel);
        }
    }

    public class SignUpWithGoogleParams : Params
    {
        public readonly string Token;
        public readonly Guid SessionId;

        public SignUpWithGoogleParams(string token, Guid sessionId)
        {
            this.Token = token;
            this.SessionId = sessionId;
        }
        public override bool Equals(object obj)
        {

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }


            return Token == (obj as SignUpWithGoogleParams).Token && SessionId == (obj as SignUpWithGoogleParams).SessionId;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return Token.GetHashCode() + SessionId.GetHashCode();
        }


    }
}
