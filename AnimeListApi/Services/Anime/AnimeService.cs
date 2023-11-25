using AnimeListApi.Handlers;
using AnimeListApi.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace AnimeListApi.Services.Anime
{
    public class AnimeService
    {
        private readonly AnimeListContext _dbContext;
        private readonly JikanHandler _jikanHandler;

        public AnimeService(AnimeListContext dbContext, JikanHandler jikanHandler)
        {
            _dbContext = dbContext;
            _jikanHandler = jikanHandler;
        }

        public async Task<Models.Data.Anime?> CheckIfAnimeIsInDb(int animeId)
        {
            var anime = await _dbContext.Anime.FirstOrDefaultAsync(a => a.Animeid == animeId);
            return anime ?? null;
        }


        public async Task<object?> AddAnimeToDatabase(int animeId)
        {
            try
            {
                var animeData = await _jikanHandler.GetAnimeDetails(animeId);

                var animeToAdd = new Models.Data.Anime
                {
                    Animeid = animeData.Data.MalId,
                    Title = animeData.Data.Title,
                    Image = animeData.Data.Images.Jpg.ImageUrl,
                    Type = animeData.Data.Type,
                    Episodecount = animeData.Data.Episodes,
                    Season = animeData.Data.Season,
                    Releaseyear = animeData.Data.Year
                };

                _dbContext.Anime.Add(animeToAdd);
                await _dbContext.SaveChangesAsync();
                return "Anime added to the database";
            }
            catch (Exception)
            {
                return "An error occurred while adding the anime to the database";
            }
        }

        public async Task<int> GetEpisodeCount(int id)
        {
            var anime = await _dbContext.Anime.FirstOrDefaultAsync(a => a.Animeid == id);
            return anime?.Episodecount ?? 0;
        }
    }
}