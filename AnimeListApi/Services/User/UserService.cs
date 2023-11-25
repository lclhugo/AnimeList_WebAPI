using AnimeListApi.Models.Data;
using AnimeListApi.Models.Dto.User;
using Microsoft.EntityFrameworkCore;

namespace AnimeListApi.Services.User
{
    public class UserService
    {
        private readonly AnimeListContext _dbContext;

        public UserService(AnimeListContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserPageDto?> GetUserByUsername(string username)
        {
            var user = await _dbContext.Profiles
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null) return null;

            var userDto = new UserPageDto(user.Id, user.Username, user.AvatarUrl, user.Bio);
            return userDto;
        }

        public async Task<(List<UserDto> Users, int TotalPages)> SearchUserByUsername(string username, int pageNumber)
        {
            const int pageSize = 20;

            var totalCount = await _dbContext.Profiles
                .Where(u => u.Username.Contains(username))
                .CountAsync();

            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var users = await _dbContext.Profiles
                .Where(u => u.Username.Contains(username))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var userDtos = users.Select(u => new UserDto(u.Id, u.Username, u.AvatarUrl)).ToList();

            return (userDtos, totalPages);
        }


        public async Task<bool> CheckIfUsernameIsAvailable(string username)
        {
            var user = await _dbContext.Profiles
                .FirstOrDefaultAsync(u => u.Username == username);

            return user == null;
        }

        public async Task<bool> ChangeBio(Guid userId, string bio)
        {
            var user = await _dbContext.Profiles
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return false;

            user.Bio = bio;
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
