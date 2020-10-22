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
    public class SubscribeController : Controller
    {
        private readonly ISubscribeRepository db;
        private readonly ILogger<SubscribeController> logger;
        private readonly SnakeCaseConverter snakeCaseConverter;

        public SubscribeController(ILogger<SubscribeController> _logger, ISubscribeRepository context, SnakeCaseConverter snakeCaseConverter)
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
                logger.LogInformation("------------> All subscribes have been returned <------------");
                return Ok(snakeCaseConverter.ConvertToSnakeCase(content));
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n"+ex.Message);
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
                    logger.LogInformation("------------> Subscribe have been returned <------------");
                    return Ok(snakeCaseConverter.ConvertToSnakeCase(content));
                }
                else
                {
                    logger.LogWarning("------------> Subscribe not found <------------");
                    return NotFound("Subscribe not found");
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n"+ ex.Message);
                return BadRequest(ex.Message);
            }
        }

   
        [HttpPost("[action]")]
        public async Task<IActionResult> InsertAsync([FromBody] IEnumerable<Subscribe> value)
        {
            try
            {
                var content = value;
                foreach (Subscribe sbs in content)
                {
                   await db.Insert(sbs);
                }

                logger.LogInformation("------------> Subscribe/s have been added <------------");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n"+ ex.Message);
                return BadRequest(ex.Message);
            }
        }

  
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateAsync([FromBody]IEnumerable<Subscribe> value)
        {
            try
            {
                var content = value;

                foreach (Subscribe sbs in content)
                {
                   await db.Update(sbs);
                }

                logger.LogInformation("------------> Subscribe/s have been updated <------------");
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
                    logger.LogInformation("------------> Subscribe have been deleted <------------");
                    return Ok();
                }
                else
                {
                    logger.LogWarning("------------> Subscribe not found <------------");
                    return NotFound("Subscribe not found");
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
