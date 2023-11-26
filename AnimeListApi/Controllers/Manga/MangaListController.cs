using AnimeListApi.Handlers;
using AnimeListApi.Models.Dto.Requests;
using AnimeListApi.Services.Manga;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AnimeListApi.Controllers.Manga;

[ApiController]
[Route("api/manga/list/")]
public class MangaListController : ControllerBase {

    private readonly MangaListService _mangaListService;

    public MangaListController(MangaListService mangaListService) => _mangaListService = mangaListService;

    /// <summary>
    /// Get manga list by username
    /// </summary>
    /// <param name="username"></param>
    /// <returns>A JSON object containing the manga list</returns>
    [HttpGet("get/{username}")]
    public async Task<IActionResult> GetMangaList(string username) {
        try
        {
            var result = await _mangaListService.GetMangaFromList(username);
            return Ok(result);
        }
        catch (Exception e)
        {
            return ErrorHandler.CreateErrorResponse(500, "InternalServerError", e.Message);
        }
    }

    /// <summary>
    /// Get the most recent manga list entries by username, sorted by last updated date
    /// The number of entries returned is specified by the number parameter
    /// </summary>
    /// <param name="username"></param>
    /// <param name="number"></param>
    /// <returns>A JSON object containing the manga list</returns>
    [HttpGet("get/{username}/watching/{number:int}")]
    public async Task<IActionResult> GetMangaListWatching(string username, int number) {
        try
        {
            var result = await _mangaListService.GetLatestWatchingEntries(username, number);
            return Ok(result);
        }
        catch (Exception e)
        {
            return ErrorHandler.CreateErrorResponse(500, "InternalServerError", e.Message);
        }
    }

    /// <summary>
    /// Get the list infos by manga id for a user, if the manga is in the list
    /// Fetch the user id from the jwt 
    /// </summary>
    /// <param name="mangaId"></param>
    /// <returns>A JSON object containing the manga list infos for the user and the manga</returns>
    [Authorize]
    [HttpGet("get/user/{mangaId:int}")]
    public async Task<IActionResult> GetMangaListInfosById(int mangaId) {
        var jwt = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var guid = JwtHandler.GetGuidFromJwt(jwt);
        if (guid == Guid.Empty) return ErrorHandler.CreateErrorResponse(401, "Unauthorized", "You are not authorized to perform this action.");
        try
        {
            var result = await _mangaListService.GetMangaListInfosById(guid, mangaId);
            return Ok(result);
        }
        catch (Exception e)
        {
            return ErrorHandler.CreateErrorResponse(404, "Not found", e.Message);
        }
        finally
        {
            ErrorHandler.CreateErrorResponse(500, "InternalServerError", "Internal server error");
        }
    }

    /// <summary>
    /// Add the manga to the user's list
    /// Fetch the user id from the jwt 
    /// </summary>
    /// <param name="mangaId"></param>
    /// <returns>A message and a JSON object containing the manga list infos for the user and the manga</returns>
    [Authorize]
    [HttpPost("add/{mangaId:int}")]
    public async Task<IActionResult> AddMangaToList(int mangaId) {
        var jwt = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var guid = JwtHandler.GetGuidFromJwt(jwt);
        if (guid == Guid.Empty) return ErrorHandler.CreateErrorResponse(401, "Unauthorized", "You are not authorized to perform this action.");

        try
        {
            var result = await _mangaListService.AddMangaToList(guid, mangaId);
            return Ok(new { message = "Manga added successfully to your list.", result });
        }
        catch (Exception e)
        {
            return ErrorHandler.CreateErrorResponse(500, "InternalServerError", e.Message);
        }
    }

    /// <summary>
    /// Update the manga list entry for the user
    /// Fetch the user id from the jwt
    /// Get all the infos from the request body
    /// </summary>
    /// <param name="request"></param>
    /// <param name="mangaId"></param>
    /// <returns>A message and a JSON object containing the manga list infos for the user and the manga</returns>
    /// <exception cref="ArgumentNullException"></exception>
    [Authorize]
    [HttpPut("update/{mangaId:int}")]
    public async Task<IActionResult> UpdateMangaList([FromBody] Requests.MangaListRequest request, int mangaId) {
        if (request == null) throw new ArgumentNullException(nameof(request));
        var jwt = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var guid = JwtHandler.GetGuidFromJwt(jwt);
        if (guid == Guid.Empty) return ErrorHandler.CreateErrorResponse(401, "Unauthorized", "You are not authorized to perform this action.");

        try
        {
            var result = await _mangaListService.UpdateMangaList(guid, mangaId, request);
            return Ok(new { message = "Manga updated successfully.", result });
        }
        catch (Exception e)
        {
            return ErrorHandler.CreateErrorResponse(500, "InternalServerError", e.Message);
        }

    }

    /// <summary>
    /// Delete the manga list entry for the user
    /// </summary>
    /// <param name="mangaId"></param>
    /// <returns>A message and a JSON object containing the manga list infos for the user and the manga</returns>
    [Authorize]
    [HttpDelete("remove")]
    public async Task<IActionResult> RemoveMangaFromList(int mangaId) {
        var jwt = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var guid = JwtHandler.GetGuidFromJwt(jwt);
        if (guid == Guid.Empty) return ErrorHandler.CreateErrorResponse(401, "Unauthorized", "You are not authorized to perform this action.");

        try
        {
            var result = await _mangaListService.RemoveMangaFromList(guid, mangaId);
            return Ok(new { message = "Manga removed successfully from your list.", result });
        }
        catch (Exception e)
        {
            return ErrorHandler.CreateErrorResponse(500, "InternalServerError", e.Message);
        }
    }
}