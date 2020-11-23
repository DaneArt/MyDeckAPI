using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyDeckAPI.Interfaces;
using MyDeckAPI.Models;
using MyDeckAPI.Services;
using MyDeckApi_Experimental.Interfaces;

namespace MyDeckAPI.Controllers
{   
    [Authorize]
    [Route("mydeckapi/[controller]")]
    public class UserDeckController : Controller
    {
        private readonly IUserDeckRepository db;
        private readonly ILogger<UserDeckController> logger;
        private readonly SnakeCaseConverter snakeCaseConverter;

        public UserDeckController(ILogger<UserDeckController> _logger, IUserDeckRepository context, SnakeCaseConverter snakeCaseConverter)
        {
            db = context;
            logger = _logger;
            this.snakeCaseConverter = snakeCaseConverter;
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> FindAllAsync()
        {
            try
            {
                var content = await db.FindAll();
                logger.LogInformation("------------> All userdecks have been returned <------------");
                return Ok(snakeCaseConverter.ConvertToSnakeCase(content));
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n"+ ex.Message);
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
                    logger.LogInformation("------------> Userdeck have been returned <------------");
                    return Ok(snakeCaseConverter.ConvertToSnakeCase(content));
                }
                else
                {
                    logger.LogWarning("------------> Userdeck not found <------------");
                    return NotFound("Userdeck not found");
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n"+ ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("[action]/{deckId}/{userId}")]
        public async Task<IActionResult> Subscribe(string deckId, string userId)
        {
            try
            {
                var content = new UserDeck
                {
                    Deck_Id = Guid.Parse(deckId),
                    User_Id = Guid.Parse(userId),
                };
                
                   await db.Insert(content);
                
                   db.Save();

                logger.LogInformation("------------> Userdeck/s have been added <------------");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n"+ ex.Message);
                return BadRequest(ex.Message);
            }
        }        

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateAsync([FromBody]IEnumerable<UserDeck> value)
        {
            try
            {
                var content = value;

                foreach (UserDeck usrdck in content)
                {
                   await db.Update(usrdck);
                }

             
                logger.LogInformation("------------> Userdeck/s have been updated <------------");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n"+ ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteByIdAsync(Guid id)
        {
            try
            {
                var content = await db.FindById(id);
                if (content != null)
                {
                   await db.Delete(id);
                    logger.LogInformation("------------> Userdeck have been deleted <------------");
                    return Ok();
                }
                else
                {
                    logger.LogWarning("------------> Userdeck not found <------------");
                    return NotFound("Userdeck not found");
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n"+ ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}

