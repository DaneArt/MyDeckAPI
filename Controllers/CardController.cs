using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyDeckAPI.Interfaces;
using MyDeckAPI.Models;
using MyDeckAPI.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MyDeckAPI.Controllers
{
    [Authorize]
    [Route("mydeckapi/[controller]")]
    public class CardController : Controller
    {
        private readonly CardRepository db;
        private readonly ILogger<CardController> logger;
        private readonly SnakeCaseConverter snakeCaseConverter;

        public CardController(ILogger<CardController> _logger, CardRepository context,
            SnakeCaseConverter snakeCaseConverter)
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
                logger.LogInformation("------------> All cards have been returned <------------");
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
                    logger.LogInformation("------------> Card have been returned <------------");
                    return Ok(snakeCaseConverter.ConvertToSnakeCase(content));
                }
                else
                {
                    logger.LogWarning("------------> Card not found <------------");
                    return NotFound("Card not found");
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }


        /*[HttpPost("[action]")]
        public async Task<IActionResult> Insert([FromForm] IEnumerable<FilledCard> value)
        {
            try
            {
                var content = await db.Insert(value);
                db.Save();
                logger.LogInformation("------------> Card/s have been added <------------");
                return Ok(content);
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n"+ ex.Message);
                return BadRequest(ex.Message);
            }
        }*/


        /* [HttpPut("[action]")]
         public IActionResult Update([FromBody]IEnumerable<FilledCard> value)
         {
             try
             {
                 //var content = db.Update(value);
                 var content = 1;
                 db.Save();
                 logger.LogInformation("------------> Card/s have been updated <------------");
                 return Ok(content);
             }
             catch (Exception ex)
             {
                 logger.LogWarning("------------> An error has occurred <------------ \n"+ ex.Message);
                 return BadRequest(ex.Message);
             }
         }*/

        [AllowAnonymous]
        [HttpDelete("[action]")]
        public IActionResult Delete([FromBody] IEnumerable<Card> value)
        {
            try
            {
                db.Delete(value);
                logger.LogInformation("------------> Card have been deleted <------------");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}