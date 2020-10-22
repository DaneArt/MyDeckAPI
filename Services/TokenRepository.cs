using System;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using MyDeckAPI.Models;
using MyDeckAPI.Security;
using Newtonsoft.Json;


namespace MyDeckApi_Experimental.Services
{
    public class TokenRepository : ITokenRepository
    {


        public async Task<bool> ValidateGoogleIdToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,                                   //TODO SET TRUE
                ValidAudience = AuthUtils.GOOGLEAUDIENCE,
                ValidIssuer = AuthUtils.GOOGLEISSUER,
                IssuerSigningKey = AuthUtils.GetGoogleSecurityKey(),
                ValidateIssuerSigningKey = true
            };
            SecurityToken validatedToken;
            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            }
            catch (Exception)
            {
                return false;
            }
            return validatedToken != null;
        }
        public bool ValidateExpiredAccessToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidAudience = AuthUtils.AUDIENCE,
                ValidIssuer = AuthUtils.ISSUER,
                IssuerSigningKey = AuthUtils.GetSymmetricSecurityKey(),
                ValidateIssuerSigningKey = true
            };
            SecurityToken validatedToken;
            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            }
            catch (Exception)
            {
                return false;
            }
            return validatedToken != null;
        }



        public async Task<string> GetEmailConfirmationToken(Guid userId)
        {
            var now = DateTime.UtcNow;
            List<Claim> claim = new List<Claim> { new Claim(type: "id", value: userId.ToString()) };
            ClaimsIdentity claims = new ClaimsIdentity(claim, "Bearer", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            var jwt = new JwtSecurityToken(
                    issuer: AuthUtils.ISSUER,
                    audience: AuthUtils.AUDIENCE,
                    notBefore: now,
                    claims: claims.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthUtils.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthUtils.GetEmailConfirmationSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);


            return encodedJwt;
        }


        public Tokens GetNewTokens(User user)
        {
            var now = DateTime.UtcNow;
            List<Claim> claim = new List<Claim> { new Claim(type: ClaimsIdentity.DefaultRoleClaimType, value: user.Role_Name),
                                                   new Claim(type: ClaimsIdentity.DefaultNameClaimType,value: user.UserName),
                                                    new Claim(type:"id",value:user.User_Id.ToString()),
                                                    new Claim(type:"role",value:user.Role_Name)};
            ClaimsIdentity claims = new ClaimsIdentity(claim, "Bearer", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            var jwt = new JwtSecurityToken(
                    issuer: AuthUtils.ISSUER,
                    audience: AuthUtils.AUDIENCE,
                    notBefore: now,
                    claims: claims.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthUtils.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthUtils.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var jwtrfrsh = new JwtSecurityToken(
                    issuer: AuthUtils.ISSUER,
                    notBefore: now,
                    claims: claims.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthUtils.REFRESH_LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthUtils.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            string encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            string refreshToken = new JwtSecurityTokenHandler().WriteToken(jwtrfrsh);

            return new Tokens{ Access_Token = encodedJwt,Refresh_Token = refreshToken};

        }


    }
}