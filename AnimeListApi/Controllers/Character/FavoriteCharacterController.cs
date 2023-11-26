using AnimeListApi.Handlers;
using AnimeListApi.Models;
using AnimeListApi.Models.Data;
using AnimeListApi.Models.Dto.Character;
using AnimeListApi.Models.Dto.Requests;
using AnimeListApi.Services.Character;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnimeListApi.Controllers.Character;

[ApiController]
[Route("api/favorite-characters/")]
public class FavoriteCharacterController : ControllerBase {


    private readonly FavoriteCharactersService _favCharaService;

    public FavoriteCharacterController(FavoriteCharactersService favCharaService) => _favCharaService = favCharaService;

    [HttpGet("get/{username}")]
    public async Task<IActionResult> GetFavoriteCharacter(string username) {
        try
        {
            var result = await _favCharaService.GetFavoriteCharacters(username);
            return Ok(result);
        }
        catch (Exception e)
        {
            return ErrorHandler.CreateErrorResponse(500, "InternalServerError", e.Message);
        }
    }

    [Authorize]
    [HttpPost("add")]
    public async Task<IActionResult> AddCharacterToFavorites(int characterId) {
        var jwt = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var guid = JwtHandler.GetGuidFromJwt(jwt);
        if (guid == Guid.Empty) return ErrorHandler.CreateErrorResponse(401, "Unauthorized", "You are not authorized to perform this action.");

        try
        {
            var result = await _favCharaService.AddCharacterToFavorites(guid, characterId);
            return Created("", result);
        }
        catch (Exception e)
        {
            return ErrorHandler.CreateErrorResponse(500, "InternalServerError", e.Message);
        }
    }

    [Authorize]
    [HttpDelete("remove")]
    public async Task<IActionResult> RemoveCharacterFromFavorites(int characterId) {
        var jwt = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var guid = JwtHandler.GetGuidFromJwt(jwt);
        if (guid == Guid.Empty) return ErrorHandler.CreateErrorResponse(401, "Unauthorized", "You are not authorized to perform this action.");

        try
        {
            var result = await _favCharaService.RemoveCharacterFromFavorites(guid, characterId);
            return Ok(result);
        }
        catch (Exception e)
        {
            return ErrorHandler.CreateErrorResponse(500, "InternalServerError", e.Message);
        }
    }

    [Authorize]
    [HttpGet("is-in-fav")]
    public async Task<IActionResult> IsCharaInFav(int characterId) {
        var jwt = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var guid = JwtHandler.GetGuidFromJwt(jwt);
        if (guid == Guid.Empty) return ErrorHandler.CreateErrorResponse(401, "Unauthorized", "You are not authorized to perform this action.");
        try
        {
            var result = await _favCharaService.IsCharaInFav(guid, characterId);
            return Ok(result);
        }
        catch (Exception e)
        {
            return ErrorHandler.CreateErrorResponse(500, "InternalServerError", e.Message);
        }
    }
}