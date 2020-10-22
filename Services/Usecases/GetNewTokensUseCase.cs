using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using MyDeckAPI.Interfaces;
using MyDeckAPI.Models;
using MyDeckAPI.Security;
using MyDeckAPI.Services;
using MyDeckApi_Experimental.Interfaces;

namespace MyDeckApi_Experimental.Services.Usecases
{
    public class GetNewTokensUseCase : IUseCase<string, GetTokensParams>
    {
        private readonly IUserRepository userRepository;
        private readonly SnakeCaseConverter snakeCaseConverter;
        private readonly ISessionRepository sessionRepository;
        private readonly ITokenRepository tokenRepository;
        public GetNewTokensUseCase(SnakeCaseConverter snakeCaseConverter,IUserRepository userRepository, ISessionRepository sessionRepository, ITokenRepository tokenRepository)
        {
            this.userRepository = userRepository;
            this.sessionRepository = sessionRepository;
            this.tokenRepository = tokenRepository;
            this.snakeCaseConverter = snakeCaseConverter;
        }


        public async Task<string> Invoke(GetTokensParams getParams)
        {


            var user =  userRepository.FindById(getParams.UserId);
            var session =  sessionRepository.FindById(getParams.SessionId, getParams.UserId);
            var now = DateTime.UtcNow;

            var tokensPair = tokenRepository.GetNewTokens(await user);


            if (session == null)
            {
                sessionRepository.Insert(new Session
                {
                    Session_Id = (await session).Session_Id,
                    User_Id = (await user).User_Id,
                    RefreshToken = tokensPair.Refresh_Token,
                });
            }
            else
            {
                sessionRepository.RefreshSession(await session, tokensPair.Refresh_Token);

            }

            var result = new
            {
                access_token = tokensPair.Access_Token,
                refresh_token = tokensPair.Refresh_Token,
                user_Id = (await user).User_Id
            };
            return snakeCaseConverter.ConvertToSnakeCase(result);
        }


    }
    public class GetTokensParams : Params
    {

        public readonly Guid UserId;
        public readonly Guid SessionId;
        
        public GetTokensParams(Guid userId, Guid sessionId)
        {
            this.UserId = userId;
            this.SessionId = sessionId;
        }


        public override bool Equals(object obj)
        {

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }


            return UserId == (obj as GetTokensParams).UserId && SessionId == (obj as GetTokensParams).SessionId;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return UserId.GetHashCode() + SessionId.GetHashCode();
        }

    }
}