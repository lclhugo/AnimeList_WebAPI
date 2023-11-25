using AnimeListApi.Handlers;
using AnimeListApi.Services.Character;
using AnimeListApi.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace AnimeListApi.Controllers.User
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }


        [HttpGet("{username}")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            try
            {
                var result = await _userService.GetUserByUsername(username);
                return Ok(result);
            }
            catch (Exception e)
            {
                return ErrorHandler.CreateErrorResponse(500, "InternalServerError", e.Message);
            }
        }


        [HttpGet("check-username/{username}")]
        public async Task<IActionResult> CheckIfUsernameIsAvailable(string username)
        {
            try
            {
                var result = await _userService.CheckIfUsernameIsAvailable(username);
                return Ok(result);
            }
            catch (Exception e)
            {
                return ErrorHandler.CreateErrorResponse(500, "InternalServerError", e.Message);
            }
        }

        //search user by username
        [HttpGet("search/{username}")]
        public async Task<IActionResult> SearchUserByUsername(string username, int pageNumber)
        {
            try
            {
                var (users, totalPages) = await _userService.SearchUserByUsername(username, pageNumber);
                var result = new { Users = users, TotalPages = totalPages };

                return Ok(result);
            }
            catch (Exception e)
            {
                return ErrorHandler.CreateErrorResponse(500, "InternalServerError", e.Message);
            }
        }

        [Authorize]
        [HttpPut("change-bio")]
        public async Task<IActionResult> ChangeBio(string bio)
        {
            var jwt = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var guid = JwtHandler.GetGuidFromJwt(jwt);
            if (guid == Guid.Empty) return ErrorHandler.CreateErrorResponse(401, "Unauthorized", "You are not authorized to perform this action.");

            if (bio.Length > 2000) return ErrorHandler.CreateErrorResponse(400, "BadRequest", "Bio is too long.");
            var sanitizedBio = InputHandler.SanitizeInput(bio);
            try
            {
                var result = await _userService.ChangeBio(guid, sanitizedBio);
                return Ok(result);
            }
            catch (Exception e)
            {
                return ErrorHandler.CreateErrorResponse(500, "InternalServerError", e.Message);
            }
        }
    }
}
