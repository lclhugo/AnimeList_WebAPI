using AnimeListApi.Handlers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace AnimeListApi.Controllers.Manga;

[ApiController]
[Route("api/manga")]
public class MangaController : ControllerBase {
    private readonly JikanHandler _jikanHandler = new(new HttpClient(), new MemoryCache(new MemoryCacheOptions()));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetMangaById(int id) {
        if (id <= 0) return ErrorHandler.CreateErrorResponse(400, "BadRequest", "Invalid manga id");

        try
        {
            var result = await _jikanHandler.GetMangaDetails(id);
            return Ok(result);
        }
        catch (Exception e)
        {
            return ErrorHandler.CreateErrorResponse(500, "InternalServerError", e.Message);
        }
    }
}