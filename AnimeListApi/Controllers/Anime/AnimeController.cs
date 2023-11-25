using AnimeListApi.Handlers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using AnimeListApi.Services.Anime;

namespace AnimeListApi.Controllers.Anime;

[ApiController]
[Route("api/anime")]
public class AnimeController : ControllerBase
{
    private readonly JikanHandler _jikanHandler = new(new HttpClient(), new MemoryCache(new MemoryCacheOptions()));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAnimeById(int id)
    {
        if (id <= 0) return ErrorHandler.CreateErrorResponse(400, "BadRequest", "Invalid anime id");

        try
        {
            var result = await _jikanHandler.GetAnimeDetails(id);
            return Ok(result);
        }
        catch (Exception e)
        {
            return ErrorHandler.CreateErrorResponse(500, "InternalServerError", e.Message);
        }
    }
}
