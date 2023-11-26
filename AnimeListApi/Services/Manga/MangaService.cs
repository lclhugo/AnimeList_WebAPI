using AnimeListApi.Handlers;
using AnimeListApi.Models.Data;
using AnimeListApi.Models.Dto.Manga;
using Microsoft.EntityFrameworkCore;

namespace AnimeListApi.Services.Manga {
    public class MangaService {
        private readonly AnimeListContext _dbContext;
        private readonly JikanHandler _jikanHandler;

        public MangaService(AnimeListContext dbContext, JikanHandler jikanHandler) {
            _dbContext = dbContext;
            _jikanHandler = jikanHandler;
        }

        public async Task<Models.Data.Manga?> CheckIfMangaIsInDb(int mangaId) {
            var manga = await _dbContext.Manga.FirstOrDefaultAsync(a => a.Mangaid == mangaId);
            return manga ?? null;
        }


        public async Task<string> AddMangaToDatabase(int mangaId) {
            try
            {
                var mangaData = await _jikanHandler.GetMangaDetails(mangaId);

                var mangaToAdd = new Models.Data.Manga {
                    Mangaid = mangaData.data.mal_id,
                    Title = mangaData.data.title,
                    Image = mangaData.data.images.jpg.large_image_url,
                    Chaptercount = mangaData.data.chapters,
                    Releaseyear = mangaData.data.published.from.Value.Year
                };

                _dbContext.Manga.Add(mangaToAdd);
                await _dbContext.SaveChangesAsync();
                return "Manga added to the database";
            }
            catch (Exception)
            {
                return "An error occurred while adding the manga to the database";
            }
        }

        public async Task<int> GetChaptersCount(int id) {
            var manga = await _dbContext.Manga.FirstOrDefaultAsync(a => a.Mangaid == id);
            return manga?.Chaptercount ?? 0;
        }
    }
}