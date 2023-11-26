using System.Threading.Tasks;
using System.Xml.Linq;
using AnimeListApi.Models.Data;
using AnimeListApi.Models.Dto.Manga;
using AnimeListApi.Models.Dto.Requests;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AnimeListApi.Services.Manga {
    public class MangaListService {
        private readonly AnimeListContext _dbContext;
        private readonly MangaService _mangaService;

        public MangaListService(AnimeListContext dbContext, MangaService mangaService) {
            _dbContext = dbContext;
            _mangaService = mangaService;
        }

        public async Task<object?> GetMangaFromList(string username) {
            var user = await _dbContext.Profiles.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return null;

            var mangaList = await _dbContext.Mangalist
                .Where(al => al.Userid == user.Id)
                .Select(al => new {
                    al.Mangaid,
                    al.Status,
                    al.Readchapters,
                    al.Rating,
                    MangaInfo = new {
                        al.Manga.Title,
                        al.Manga.Image,
                        al.Manga.Chaptercount,
                        al.Manga.Releaseyear
                    }
                })
                .OrderBy(a => a.Status)
                .ThenBy(a => a.MangaInfo.Title)
                .ToListAsync();

            return mangaList;
        }

        public async Task<object?> GetLatestReadingEntries(string username, int number) {
            var user = await _dbContext.Profiles.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return null;

            var userId = user.Id;
            var mangaList = await _dbContext.Mangalist
                .Where(al => al.Userid == userId && al.Statusid == 1)
                .OrderByDescending(al => al.Lastupdated)
                .Take(number)
                .Select(al => new {
                    al.Mangaid,
                    al.Status,
                    al.Readchapters,
                    al.Rating,
                    al.Lastupdated,
                    MangaInfo = new {
                        al.Manga.Title,
                        al.Manga.Image,
                        al.Manga.Chaptercount,
                        al.Manga.Releaseyear
                    }
                })
                .ToListAsync();

            return mangaList;
        }

        public async Task<object?> GetMangaListInfosById(Guid guid, int mangaId) {
            var user = await _dbContext.Profiles.FirstOrDefaultAsync(u => u.Id == guid);
            if (user == null) return null;

            var userId = user.Id;
            var mangaList = await _dbContext.Mangalist
                .Where(al => al.Mangaid == mangaId && al.Userid == userId)
                .Select(al => new {
                    al.Mangaid,
                    al.Status,
                    al.Readchapters,
                    al.Rating,
                    al.Created,
                    al.Lastupdated,
                })
                .OrderBy(a => a.Status)
                .ToListAsync();

            if (mangaList.Count == 0) throw new Exception("Manga not found in list");
            return mangaList;
        }

        public async Task<MangaListDto?> AddMangaToList(Guid userId, int mangaId) {
            var isInList = await IsMangaInList(mangaId, userId);
            if (isInList) return null;

            var user = await _dbContext.Profiles.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return null;

            var isManga = await _mangaService.CheckIfMangaIsInDb(mangaId);
            if (isManga == null) await _mangaService.AddMangaToDatabase(mangaId);

            var mangaList = new Mangalist {
                Userid = userId,
                Mangaid = mangaId,
                Statusid = 1,
                Readchapters = 0,
                Rating = 0,
                Created = DateTime.Now,
                Lastupdated = DateTime.Now
            };

            _dbContext.Mangalist.Add(mangaList);
            await _dbContext.SaveChangesAsync();

            var mangaListDto = new MangaListDto {
                UserId = userId,
                MangaId = mangaId,
                Status = 1,
                ReadChapters = 0,
                Rating = 0,
                Created = DateTime.Now,
                LastUpdated = DateTime.Now
            };
            return mangaListDto;
        }

        public async Task<MangaListDto?> UpdateMangaList(Guid guid, int mangaId, Requests.MangaListRequest request) {
            var isInList = await IsMangaInList(mangaId, guid);
            if (!isInList) return null;

            var userId = guid;
            var readChapters = request.ReadChapters;
            var status = request.StatusId;
            var rating = request.Rating;

            var isManga = await _mangaService.CheckIfMangaIsInDb(mangaId);
            if (isManga == null) await _mangaService.AddMangaToDatabase(mangaId);
            var manga = await _dbContext.Manga.FirstOrDefaultAsync(a => a.Mangaid == mangaId);

            if (request.ReadChapters >= manga?.Chaptercount)
            {
                readChapters = manga.Chaptercount;
                status = await GetStatusIdByName("Completed");
            }

            if (request.StatusId == 2) readChapters = manga?.Chaptercount;

            rating = request.Rating switch {
                > 10 => 10,
                < 0 => 0,
                _ => rating
            };

            var mangaList =
                await _dbContext.Mangalist.FirstOrDefaultAsync(a => a.Mangaid == mangaId && a.Userid == userId);
            mangaList.Statusid = status;
            mangaList.Readchapters = readChapters;
            mangaList.Rating = rating;
            mangaList.Lastupdated = DateTime.Now;
            _dbContext.Mangalist.Update(mangaList);
            await _dbContext.SaveChangesAsync();

            var mangaListDto = new MangaListDto {
                UserId = userId,
                MangaId = mangaId,
                Status = status,
                ReadChapters = readChapters,
                Rating = rating,
                Created = mangaList.Created.Value,
                LastUpdated = mangaList.Lastupdated.Value
            };
            return mangaListDto;
        }

        public async Task<object?> RemoveMangaFromList(Guid requestUserId, int requestMangaId) {
            var mangaList =
                await _dbContext.Mangalist.FirstOrDefaultAsync(a =>
                    a.Mangaid == requestMangaId && a.Userid == requestUserId);
            if (mangaList == null) return null;
            _dbContext.Mangalist.Remove(mangaList);
            await _dbContext.SaveChangesAsync();
            return mangaList;
        }

        private async Task<bool> IsMangaInList(int id, Guid userId) {
            var mangaList = await _dbContext.Mangalist.FirstOrDefaultAsync(a => a.Mangaid == id && a.Userid == userId);
            return mangaList != null;
        }

        private async Task<int> GetStatusIdByName(string statusName) {
            var status = await _dbContext.Status.FirstOrDefaultAsync(s => s.Statusname == statusName);
            return status?.Statusid ?? 1;
        }
    }
}
