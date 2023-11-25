using AnimeListApi.Handlers;
using AnimeListApi.Models;
using AnimeListApi.Services.Anime;
using AnimeListApi.Services.Character;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace AnimeListApi.Controllers.Character
{
    [ApiController]
    [Route("api/character")]
    public class CharacterController : ControllerBase
    {
        private readonly CharacterService _characterService;
        private readonly JikanHandler _jikanHandler;


        public CharacterController(CharacterService characterService)
        {
            _characterService = characterService;
            _jikanHandler = new JikanHandler(new HttpClient(), new MemoryCache(new MemoryCacheOptions()));
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCharacterById(int id)
        {
            if (id <= 0) return ErrorHandler.CreateErrorResponse(400, "BadRequest", "Invalid character id");

            try
            {
                var result = await _jikanHandler.GetCharacterDetails(id);
                return Ok(result);
            }
            catch
            {
                return ErrorHandler.CreateErrorResponse(500, "InternalServerError", "Internal server error");
            }
        }

        [HttpPost("add/{id:int}")]
        public async Task<IActionResult> AddCharacterToDb(int id)
        {

            if (id <= 0) return ErrorHandler.CreateErrorResponse(400, "BadRequest", "Invalid character id");
            try
            {
                var result = await _characterService.AddCharaToDatabase(id);
                return Created("", result);
            }
            catch
            {
                return ErrorHandler.CreateErrorResponse(500, "InternalServerError", "Internal server error");
            }
        }
    }
}
