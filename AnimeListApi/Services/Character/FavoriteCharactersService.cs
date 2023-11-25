using System.Threading.Tasks;
using System.Xml.Linq;
using AnimeListApi.Models.Data;
using AnimeListApi.Models.Dto.Anime;
using AnimeListApi.Models.Dto.Character;
using AnimeListApi.Models.Dto.Requests;
using AnimeListApi.Services.Anime;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;

namespace AnimeListApi.Services.Character
{
    public class FavoriteCharactersService
    {
        private readonly AnimeListContext _dbContext;
        private readonly CharacterService _characterService;

        public FavoriteCharactersService(AnimeListContext dbContext, CharacterService characterService)
        {
            _dbContext = dbContext;
            _characterService = characterService;
        }

        private async Task<bool> IsCharaInList(int charaId, Guid userId)
        {
            var favChara = await _dbContext.Favoritecharacters.FirstOrDefaultAsync(c => c.Characterid == charaId && c.Userid == userId);
            return favChara != null;
        }

        public async Task<object?> GetFavoriteCharacters(string username)
        {
            var user = await _dbContext.Profiles.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) throw new Exception("User not found");

            var favCharas = await _dbContext.Favoritecharacters
                .Where(c => c.Userid == user.Id)
                .Select(c => new
                {
                    c.Characterid,

                    Character = new
                    {
                        c.Character.Characterid,
                        c.Character.Name,
                        c.Character.Image
                    }
                })
                .OrderBy(c => c.Character.Name)
                .ToListAsync();

            return favCharas;
        }

        public async Task<object?> AddCharacterToFavorites(Guid userId, int CharacterId)
        {
            var isCharaInDb = await _characterService.CheckIfCharacterIsInDb(CharacterId);
            if (isCharaInDb == null) await _characterService.AddCharaToDatabase(CharacterId);
            var isCharaInFav = await IsCharaInList(CharacterId, userId);
            if (isCharaInFav) throw new Exception("Character already in your favorites");

            var favChara = new Favoritecharacters
            {
                Characterid = CharacterId,
                Userid = userId
            };

            _dbContext.Favoritecharacters.Add(favChara);
            await _dbContext.SaveChangesAsync();

            return "Character successfully added to your favorites";
        }

        public async Task<object?> RemoveCharacterFromFavorites(Guid userId, int characterId)
        {
            var isCharaInFav = await IsCharaInList(characterId, userId);
            if (!isCharaInFav) throw new Exception("Character not in your favorites");
            var favChara = await _dbContext.Favoritecharacters
                .FirstOrDefaultAsync(c => c.Characterid == characterId && c.Userid == userId);
            if (favChara == null) throw new Exception("Character not in your favorites");
            _dbContext.Favoritecharacters.Remove(favChara);
            await _dbContext.SaveChangesAsync();
            return "Character successfully removed from your favorites";

        }

        public async Task<bool> IsCharaInFav(Guid userId, int requestId)
        {
            var favChara = await _dbContext.Favoritecharacters
                .FirstOrDefaultAsync(c => c.Characterid == requestId && c.Userid == userId);
            return favChara != null;
        }
    }
}