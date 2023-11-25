using AnimeListApi.Handlers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AnimeListApi.Models.Dto.Requests;
using AnimeListApi.Services.Anime;
using Azure.Core;

namespace AnimeListApi.Controllers.Anime;

[ApiController]
[Route("api/anime/list/")]
public class AnimeListController : ControllerBase
{

    private readonly AnimeListService _animeListService;

    public AnimeListController(AnimeListService animeListService) => _animeListService = animeListService;

    /// <summary>
    /// Get anime list by username
    /// </summary>
    /// <param name="username"></param>
    /// <returns>A JSON object containing the anime list</returns>
    [HttpGet("get/{username}")]
    public async Task<IActionResult> GetAnimeList(string username)
    {
        try
        {
            var result = await _animeListService.GetAnimeFromList(username);
            return Ok(result);
        }
        catch (Exception e)
        {
            return ErrorHandler.CreateErrorResponse(500, "InternalServerError", e.Message);
        }
    }

    /// <summary>
    /// Get the most recent anime list entries by username, sorted by last updated date
    /// The number of entries returned is specified by the number parameter
    /// </summary>
    /// <param name="username"></param>
    /// <param name="number"></param>
    /// <returns>A JSON object containing the anime list</returns>
    [HttpGet("get/{username}/watching/{number:int}")]
    public async Task<IActionResult> GetAnimeListWatching(string username, int number)
    {
        try
        {
            var result = await _animeListService.GetLatestWatchingEntries(username, number);
            return Ok(result);
        }
        catch (Exception e)
        {
            return ErrorHandler.CreateErrorResponse(500, "InternalServerError", e.Message);
        }
    }

    /// <summary>
    /// Get the list infos by anime id for a user, if the anime is in the list
    /// Fetch the user id from the jwt 
    /// </summary>
    /// <param name="animeId"></param>
    /// <returns>A JSON object containing the anime list infos for the user and the anime</returns>
    [Authorize]
    [HttpGet("get/user/{animeId:int}")]
    public async Task<IActionResult> GetAnimeListInfosById(int animeId)
    {
        var jwt = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var guid = JwtHandler.GetGuidFromJwt(jwt);
        if (guid == Guid.Empty) return ErrorHandler.CreateErrorResponse(401, "Unauthorized", "You are not authorized to perform this action.");
        try
        {
            var result = await _animeListService.GetAnimeListInfosById(guid, animeId);
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
    /// Add the anime to the user's list
    /// Fetch the user id from the jwt 
    /// </summary>
    /// <param name="animeId"></param>
    /// <returns>A message and a JSON object containing the anime list infos for the user and the anime</returns>
    [Authorize]
    [HttpPost("add/{animeId:int}")]
    public async Task<IActionResult> AddAnimeToList(int animeId)
    {
        var jwt = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var guid = JwtHandler.GetGuidFromJwt(jwt);
        if (guid == Guid.Empty) return ErrorHandler.CreateErrorResponse(401, "Unauthorized", "You are not authorized to perform this action.");

        try
        {
            var result = await _animeListService.AddAnimeToList(guid, animeId);
            return Ok(new { message = "Anime added successfully to your list.", result });
        }
        catch (Exception e)
        {
            return ErrorHandler.CreateErrorResponse(500, "InternalServerError", e.Message);
        }
    }

    /// <summary>
    /// Update the anime list entry for the user
    /// Fetch the user id from the jwt
    /// Get all the infos from the request body
    /// </summary>
    /// <param name="request"></param>
    /// <param name="animeId"></param>
    /// <returns>A message and a JSON object containing the anime list infos for the user and the anime</returns>
    /// <exception cref="ArgumentNullException"></exception>
    [Authorize]
    [HttpPut("update/{animeId:int}")]
    public async Task<IActionResult> UpdateAnimeList([FromBody] Requests.AnimeListRequest request, int animeId)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));
        var jwt = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var guid = JwtHandler.GetGuidFromJwt(jwt);
        if (guid == Guid.Empty) return ErrorHandler.CreateErrorResponse(401, "Unauthorized", "You are not authorized to perform this action.");

        try
        {
            var result = await _animeListService.UpdateAnimeList(guid, animeId, request);
            return Ok(new { message = "Anime updated successfully.", result });
        }
        catch (Exception e)
        {
            return ErrorHandler.CreateErrorResponse(500, "InternalServerError", e.Message);
        }

    }

    /// <summary>
    /// Delete the anime list entry for the user
    /// </summary>
    /// <param name="animeId"></param>
    /// <returns>A message and a JSON object containing the anime list infos for the user and the anime</returns>
    [Authorize]
    [HttpDelete("remove")]
    public async Task<IActionResult> RemoveAnimeFromList(int animeId)
    {
        var jwt = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var guid = JwtHandler.GetGuidFromJwt(jwt);
        if (guid == Guid.Empty) return ErrorHandler.CreateErrorResponse(401, "Unauthorized", "You are not authorized to perform this action.");

        try
        {
            var result = await _animeListService.RemoveAnimeFromList(guid, animeId);
            return Ok(new { message = "Anime removed successfully from your list.", result });
        }
        catch (Exception e)
        {
            return ErrorHandler.CreateErrorResponse(500, "InternalServerError", e.Message);
        }
    }
}