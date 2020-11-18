using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyDeckAPI.Data.MediaContent;
using MyDeckAPI.Interfaces;
using MyDeckAPI.Models;
using MyDeckAPI.Services;
using MyDeckAPI.Services.Usecases;
using MyDeckApi_Experimental.Interfaces;
using Newtonsoft.Json;

namespace MyDeckAPI.Controllers
{
    // [Authorize]
    [Route("mydeckapi/[controller]")]
    public class DeckController : Controller
    {
        private readonly IDeckRepository deckRepository;
        private readonly IUserDeckRepository userDeckRepository;
        private readonly ILogger<DeckController> logger;
        private readonly DeleteDeckUseCase deleteDeckUseCase;
        private readonly UpdateDeckUseCase updateDeckUseCase;
        private readonly GetDecksForTrainUseCase getDecksForTrainUseCase;
        private readonly SnakeCaseConverter snakeCaseConverter;

        public DeckController(IDeckRepository deckRepository, IUserDeckRepository userDeckRepository,
            ILogger<DeckController> logger, SnakeCaseConverter snakeCaseConverter, DeleteDeckUseCase deleteDeckUseCase, UpdateDeckUseCase updateDeckUseCase, GetDecksForTrainUseCase getDecksForTrainUseCase)
        {
            this.deckRepository = deckRepository;
            this.userDeckRepository = userDeckRepository;
            this.logger = logger;
            this.snakeCaseConverter = snakeCaseConverter;
            this.deleteDeckUseCase = deleteDeckUseCase;
            this.updateDeckUseCase = updateDeckUseCase;
            this.getDecksForTrainUseCase = getDecksForTrainUseCase;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> FindAllAsync()
        {
            try
            {
                var content = await deckRepository.FindAll();
                logger.LogInformation("------------> All decks have been returned <------------");
                return Ok(snakeCaseConverter.ConvertToSnakeCase(content));
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("[action]/{id}")]
        public IActionResult FindById(Guid id)
        {
            try
            {
                var content = deckRepository.FindById(id);
                if (content != null)
                {
                    logger.LogInformation("------------> Deck have been returned <------------");
                    return Ok(snakeCaseConverter.ConvertToSnakeCase(content));
                }
                else
                {
                    logger.LogWarning("------------> Deck not found <------------");
                    return NotFound("Deck not found");
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Insert([FromBody] System.Text.Json.JsonElement value)
        {
            try
            {
                Deck deck = JsonConvert.DeserializeObject<Deck>(value.GetRawText());


                await deckRepository.Insert(deck);

                await userDeckRepository.Insert(
                    new UserDeck {User_Id = Guid.Parse(deck.Author), Deck_Id = deck.Deck_Id});

                deckRepository.Save();

                logger.LogInformation("------------> Deck/s have been added <------------");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            try
            {
                var result = await deleteDeckUseCase.Invoke(new DeleteDeckParams(id));
                if (result == 0)
                {
                    logger.LogInformation("------------> Deck have been deleted <------------");
                    return Ok();
                }
                else
                {
                    logger.LogWarning("------------> An error has occurred <------------ \n" );
                    return BadRequest();
                }
               
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("[action]")]
        public async Task<IActionResult> Update([FromBody] System.Text.Json.JsonElement value)
        {
            Deck deck = JsonConvert.DeserializeObject<Deck>(value.GetRawText());
            logger.LogInformation("------------> Deck have been converted <------------");
             var content = await updateDeckUseCase.Invoke(new UpdateDeckParams(deck));
            logger.LogInformation($@"------------>UDeckUseCase returned result: {content} <------------");
            if (content == 0)
            {
                logger.LogInformation("------------> Deck/s have been updated <------------");
                return Ok();
            }
            else
            {
                logger.LogInformation("------------> Deck have not been updated <------------");
                return BadRequest();
            }
        }

        [HttpGet("[action]/{userId}")]
        public async Task<IActionResult> FindDecksForTrain(string userId)
        {
            try
            {
                var decks = await getDecksForTrainUseCase.Invoke(new GetDecksForTrainParams(Guid.Parse(userId)));
                return Ok(decks);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> AllCurrentUserDecksAsync(string id)
        {
            try
            {
                var content = await deckRepository.AllCurrentUserDecks(id);
                if (content.Count != 0)
                {
                    logger.LogInformation("------------> Deck/s have been returned <------------");
                    return Ok(snakeCaseConverter.ConvertToSnakeCase(content));
                }
                else
                {
                    logger.LogWarning("------------> Deck/s not found <------------");
                    return NotFound("Deck/s not found");
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("[action]/{category}/{page}")]
        public async Task<IActionResult> ChosenCategoryFeedAsync(string category, int page)
        {
            try
            {
                var content = await deckRepository.ChosenCategoryFeed(category, page);
                if (content.Count != 0)
                {
                    logger.LogInformation("------------> Deck/s have been returned <------------");
                    return Ok(snakeCaseConverter.ConvertToSnakeCase(content));
                }
                else
                {
                    logger.LogWarning("------------> Deck/s not found <------------");
                    return NotFound("Deck/s not found");
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> AllUserDecks(Guid id)
        {
            try
            {
                var content = await deckRepository.AllUserDecks(id);
                if (content.Count != 0)
                {
                    logger.LogInformation("------------> Deck/s have been returned <------------");
                    return Ok(snakeCaseConverter.ConvertToSnakeCase(content));
                }
                else
                {
                    logger.LogWarning("------------> Deck/s not found <------------");
                    return NotFound("Deck/s not found");
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> AllCurrentUserDecksWithCardsAsync(string id)
        {
            try
            {
                var content = await deckRepository.AllCurrentUserDecksWithCards(id);
                if (content.Count != 0)
                {
                    logger.LogInformation("------------> Deck/s have been returned <------------");
                    return Ok(content);
                }
                else
                {
                    logger.LogWarning("------------> Deck/s not found <------------");
                    return NotFound("Deck / s not found");
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning("------------> An error has occurred <------------ \n" + ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}