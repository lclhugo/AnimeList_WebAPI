using AnimeListApi.Handlers;
using AnimeListApi.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace AnimeListApi.Services.Character {
    public class CharacterService {
        private readonly AnimeListContext _dbContext;
        private readonly JikanHandler _jikanHandler;

        public CharacterService(AnimeListContext dbContext, JikanHandler jikanHandler) {
            _dbContext = dbContext;
            _jikanHandler = jikanHandler;
        }

        public async Task<Characters?> CheckIfCharacterIsInDb(int charaId) {
            var chara = await _dbContext.Characters
                .FirstOrDefaultAsync(c => c.Characterid == charaId);
            return chara ?? null;
        }

        public async Task<string> AddCharaToDatabase(int charaId) {
            try
            {
                var charaData = await _jikanHandler.GetCharacterDetails(charaId);

                var charaToAdd = new Characters {
                    Characterid = charaData.data.mal_id,
                    Name = charaData.data.name,
                    Image = charaData.data.images.jpg.image_url
                };

                _dbContext.Characters.Add(charaToAdd);
                await _dbContext.SaveChangesAsync();
                return "Character added to the database";
            }
            catch (Exception)
            {
                return "An error occurred while adding the anime to the database";
            }
        }
    }
}
