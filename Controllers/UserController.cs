using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using MyDeckAPI.Interfaces;
using MyDeckAPI.Models;
using MyDeckAPI.Security;
using MyDeckAPI.Services;
using MyDeckApi_Experimental.Services;
using MyDeckApi_Experimental.Services.Usecases;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MyDeckAPI.Controllers
{
    [Authorize]
    [Route("mydeckapi/[controller]")]
    public class UserController : Controller
    {


        private readonly ILogger<UserController> logger;
        private readonly SnakeCaseConverter snakeCaseConverter;
        private readonly IAuthFacade authFacade;
        private readonly GetNewTokensUseCase getNewTokesUseCase;
        private readonly RefreshTokensUseCase refreshTokensUseCase;

        private readonly IUserRepository db;

        public UserController(ILogger<UserController> logger, SnakeCaseConverter snakeCaseConverter, IAuthFacade authFacade, GetNewTokensUseCase getNewTokesUseCase, RefreshTokensUseCase refreshTokensUseCase, IUserRepository db)
        {
            this.logger = logger;
            this.snakeCaseConverter = snakeCaseConverter;
            this.authFacade = authFacade;
            this.getNewTokesUseCase = getNewTokesUseCase;
            this.refreshTokensUseCase = refreshTokensUseCase;
            this.db = db;
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<IActionResult> FindAllAsync()
        {
            try
            {
                var content = await db.FindAll();
                logger.LogInformation("------------> All users have been returned <------------");
                return Ok(snakeCaseConverter.ConvertToSnakeCase(content));
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> FindByIdAsync(Guid id)
        {
            try
            {
                var content = await db.FindById(id);
                if (content != null)
                {
                    logger.LogInformation("------------> User have been returned <------------");
                    return Ok(snakeCaseConverter.ConvertToSnakeCase(content));
                }
                else
                {
                    logger.LogWarning("------------> User not found <------------");
                    return NotFound("User not found");
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> UserProfileAsync(Guid id)
        {
            try
            {
                var content = await db.GetUserProfile(id);
                if (content != null)
                {
                    logger.LogInformation("------------> Profile have been returned <------------");
                    return Ok(content);
                }
                else
                {
                    logger.LogWarning("------------> User not found <------------");
                    return NotFound("User not found");
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> SignInByGoogle([FromHeader(Name = "idtoken")] string idtoken)
        {
            try
            {
                string sessionId;
                Request.Cookies.TryGetValue("sessionId", out sessionId);
                Debug.WriteLine("Send to debug output.");

                if (sessionId == null) { throw new Exception("Empty SessionId"); }
                var tmp = await authFacade.SignUpWithGoogle(idtoken, Guid.Parse(sessionId));
                if (tmp != null)
                {
                    logger.LogWarning("------------> U are signed in <------------ \n");
                    return Ok(tmp);
                }
                logger.LogWarning("------------> U are not signed in <------------ \n");
                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshTokens([FromBody] Tokens value)
        {
            try
            {
                string sessionId;
                Request.Cookies.TryGetValue("sessionId", out sessionId);
                if (sessionId == null) { throw new Exception("Empty SessionId"); }

                var tmp = await refreshTokensUseCase.Invoke(new RefreshTokensParams(value, Guid.Parse(sessionId)));
                if (tmp != null)
                {
                    logger.LogWarning("------------> Token has been refreshed <------------ \n");
                    return Ok(tmp);
                }
                logger.LogWarning("------------> Token has not been refreshed <------------ \n");
                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateAsync([FromBody] IEnumerable<User> value)
        {
            try
            {
                var content = value;

                foreach (User usr in content)
                {
                    await db.Update(usr);
                }

                logger.LogInformation("------------> User/s have been updated <------------");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Owner, User")]
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteByIdAsync(Guid id)
        {
            try
            {
                var content = await db.FindById(id);
                if (content != null)
                {
                    await db.Delete(id);
                    logger.LogInformation("------------> User have been deleted <------------");
                    return Ok();
                }
                else
                {
                    logger.LogWarning("------------> User not found <------------");
                    return NotFound("User not found");
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("[action]/{username}")]
        public IActionResult IsUserNameUnique(string username)
        {
            try
            {
                var content = db.IsUserNameUnique(username);
                if (content)
                {
                    logger.LogInformation("------------> User is unique <------------");
                    return Ok(snakeCaseConverter.ConvertToSnakeCase(content));
                }
                else
                {
                    logger.LogWarning("------------> User is not unique <------------");
                    return BadRequest(snakeCaseConverter.ConvertToSnakeCase(content));
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> SignUpWithEmail([FromBody] System.Text.Json.JsonElement usr) 
        {
            try
            { 
                string sessionId;
                Request.Cookies.TryGetValue("sessionId", out sessionId);
                if (sessionId == null) { throw new Exception("Empty SessionId"); }
                var user = JsonConvert.DeserializeObject<User>(usr.GetRawText());
                var response = await authFacade.SignUpWithEmail(user, Guid.Parse(sessionId));
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> SignInWithEmail([FromBody] System.Text.Json.JsonElement usr)
        {
            try
            {
                string sessionId;
                Request.Cookies.TryGetValue("sessionId", out sessionId);
                if (sessionId == null) { throw new Exception("Empty SessionId"); }
                var user = JsonConvert.DeserializeObject<AuthUserModel>(usr.GetRawText());
                var response = await authFacade.SignInWithEmail(user.Email, user.Password, Guid.Parse(sessionId));
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> SignInWithUsername([FromBody] System.Text.Json.JsonElement usr)
        {
            try
            {
                string sessionId;
                Request.Cookies.TryGetValue("sessionId", out sessionId);
                if (sessionId == null) { throw new Exception("Empty SessionId"); }
                var user = JsonConvert.DeserializeObject<AuthUserModel>(usr.GetRawText());
                var response = await authFacade.SignInWithUsername(user.UserName, user.Password, Guid.Parse(sessionId));
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("[action]/{token}")]
        public async Task<IActionResult> ConfirmEmail(string token)
        {
            try
            {
                var obj = new { Username = "asdasd", Email = "asdasda", Password = Encoding.ASCII.GetBytes("sadsadsadsad") };
                return Ok(Json(obj));
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
