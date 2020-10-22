using System;
using System.Threading.Tasks;
using MyDeckAPI.Exceptions;
using MyDeckAPI.Interfaces;
using MyDeckAPI.Models;
using MyDeckApi_Experimental.Interfaces;
using MyDeckAPI.Security;
using MyDeckAPI.Services;

namespace MyDeckApi_Experimental.Services.Usecases
{
    public class SignUpWithEmailUseCase : IUseCase<string, SignUpWithEmailParams>
    { private readonly IUserRepository userRepository;
        private readonly SnakeCaseConverter snakeCaseConverter;
        private readonly ISessionRepository sessionRepository;
        private readonly ITokenRepository tokenRepository;
        private readonly IFileRepository fileRepository;
        private readonly MailService mailService;

        public SignUpWithEmailUseCase(IUserRepository userRepository, SnakeCaseConverter snakeCaseConverter, ISessionRepository sessionRepository, ITokenRepository tokenRepository, IFileRepository fileRepository, MailService mailService)
        {
            this.userRepository = userRepository;
            this.snakeCaseConverter = snakeCaseConverter;
            this.sessionRepository = sessionRepository;
            this.tokenRepository = tokenRepository;
            this.fileRepository = fileRepository;
            this.mailService = mailService;
        }

        public async Task<string> Invoke(SignUpWithEmailParams param)
        {
            
            var sessionId = param.SessionId;
           try
            {
              
                var convertedUsr = param.User;
                if ( userRepository.ValidateUserUnique(convertedUsr)) { 
                    var usrGuid = Guid.NewGuid();
                    var guid = Guid.NewGuid();
                     fileRepository.Insert(new File() { File_Id = guid, Md5 = "", Path = "", Size = 12, Type = "" });
                    var user = new User()
                    {
                        User_Id = usrGuid,
                        Avatar = guid,
                        Role_Name = "User",
                        UserName = convertedUsr.UserName,
                        Email = convertedUsr.Email,
                        Password = new AuthUtils().GetPasswordWithSalt(convertedUsr.Password),
                        Tag = "Confirmed"
                    };
                    await userRepository.Insert(user);

                    /* var token = await tokenRepository.GetEmailConfirmationToken(usrGuid);

                     await mailService.SendConfirmationEmail(convertedUsr.Email, token);*/

                    var tokens = tokenRepository.GetNewTokens(user);
                    var currentSession = await sessionRepository.FindById(sessionId, user.User_Id);
                    if (currentSession != null)
                    {
                        sessionRepository.RefreshSession(currentSession, tokens.Refresh_Token);
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
                            email = user.Email,
                            username = user.UserName,
                            avatar = user.Avatar
                    };
                    return snakeCaseConverter.ConvertToSnakeCase(result);
                }
                else
                {
                    throw new AlreadyUsedEmailException();
                }
            }
            catch
            {
                throw;
            }
        }
    }

    public class SignUpWithEmailParams : Params{
        public readonly User User;
        public readonly Guid SessionId;


        public SignUpWithEmailParams(User user, Guid sessionId)
        {
            this.User = user;
            this.SessionId = sessionId;
        }


        public override bool Equals(object obj)
        {

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }


            return User == (obj as SignUpWithEmailParams).User && SessionId == (obj as SignUpWithEmailParams).SessionId;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return User.GetHashCode() + SessionId.GetHashCode();
        }

    }
}