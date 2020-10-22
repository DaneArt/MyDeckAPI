using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using MyDeckAPI.Interfaces;
using MyDeckAPI.Security;
using MyDeckApi_Experimental.Interfaces;
using Newtonsoft.Json;

namespace MyDeckApi_Experimental.Services.Usecases
{
    public class RefreshTokensUseCase : IUseCase<string, RefreshTokensParams>
    {
        private readonly IUserRepository userRepository;
        private readonly SnakeCaseConverter snakeCaseConverter;
        private readonly ISessionRepository sessionRepository;
        private readonly ITokenRepository tokenRepository;

        public RefreshTokensUseCase(SnakeCaseConverter snakeCaseConverter,IUserRepository userRepository, ISessionRepository sessionRepository, ITokenRepository tokenRepository)
        {
            this.userRepository = userRepository;
            this.sessionRepository = sessionRepository;
            this.tokenRepository = tokenRepository;
            this.snakeCaseConverter = snakeCaseConverter;
        }
        public async Task<string> Invoke(RefreshTokensParams param)
        {
            var tkns = param.Tokens;
            var sessionId = param.SessionId;

            if (tokenRepository.ValidateExpiredAccessToken(tkns.Access_Token))
            {

               

                var handler = new JwtSecurityTokenHandler();
                var tkn = handler.ReadJwtToken(tkns.Access_Token);
                var access_tkns_sample = new { id = "" };
                var access_tkn_payload = JsonConvert.DeserializeAnonymousType(tkn.Payload.SerializeToJson(), access_tkns_sample);
                var userId = Guid.Parse(access_tkn_payload.id);

                 var session =  sessionRepository.FindById(sessionId, userId);
                if (await session == null) { throw new Exception("Non existent session"); }

                var usr =  userRepository.FindById(userId);
                
                if (await usr == null) { throw new Exception("Non existent user"); }

                if (tkns.Refresh_Token == (await session).RefreshToken)
                {
                    var tokens = tokenRepository.GetNewTokens(await usr);
                    var result = new
                    {
                        access_token = tokens.Access_Token,
                        refresh_token = tokens.Refresh_Token,
                        user_Id = userId
                    };
                    return snakeCaseConverter.ConvertToSnakeCase(result);
                }
            }
            throw new Exception();
        }
    }

    public class RefreshTokensParams : Params
    {
        public readonly Tokens Tokens;
        public readonly Guid SessionId;

        public RefreshTokensParams(Tokens tokens, Guid sessionId)
        {
            this.Tokens = tokens;
            this.SessionId = sessionId;
        }


        public override bool Equals(object obj)
        {

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }


            return Tokens == (obj as RefreshTokensParams).Tokens && SessionId == (obj as RefreshTokensParams).SessionId;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return Tokens.GetHashCode() + SessionId.GetHashCode();
        }
    }
}