using System.Threading.Tasks;
using System.Xml.Linq;
using AnimeListApi.Models.Data;
using AnimeListApi.Models.Dto;
using AnimeListApi.Models.Dto.Anime;
using AnimeListApi.Models.Dto.Requests;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AnimeListApi.Services.Anime
{
    public class AnimeListService
    {
        private readonly AnimeListContext _dbContext;
        private readonly AnimeService _animeService;

        public AnimeListService(AnimeListContext dbContext, AnimeService animeService)
        {
            _dbContext = dbContext;
            _animeService = animeService;
        }

        public async Task<object?> GetAnimeFromList(string username)
        {
            var user = await _dbContext.Profiles.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return null;

            var animeList = await _dbContext.Animelist
                .Where(al => al.Userid == user.Id)
                .Select(al => new
                {
                    al.Animeid,
                    al.Status,
                    al.Watchedepisodes,
                    al.Rating,
                    AnimeInfo = new
                    {
                        al.Anime.Title,
                        al.Anime.Image,
                        al.Anime.Type,
                        al.Anime.Episodecount,
                        al.Anime.Season,
                        al.Anime.Releaseyear
                    }
                })
                .OrderBy(a => a.Status)
                .ThenBy(a => a.AnimeInfo.Title)
                .ToListAsync();

            return animeList;
        }

        public async Task<object?> GetLatestWatchingEntries(string username, int number)
        {
            var user = await _dbContext.Profiles.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return null;

            var userId = user.Id;
            var animeList = await _dbContext.Animelist
                .Where(al => al.Userid == userId && al.Statusid == 1)
                .OrderByDescending(al => al.Lastupdated)
                .Take(number)
                .Select(al => new
                {
                    al.Animeid,
                    al.Status,
                    al.Watchedepisodes,
                    al.Rating,
                    al.Lastupdated,
                    AnimeInfo = new
                    {
                        al.Anime.Title,
                        al.Anime.Image,
                        al.Anime.Type,
                        al.Anime.Episodecount,
                        al.Anime.Season,
                        al.Anime.Releaseyear
                    }
                })
                .ToListAsync();

            return animeList;
        }

        public async Task<object?> GetAnimeListInfosById(Guid guid, int animeId)
        {
            var user = await _dbContext.Profiles.FirstOrDefaultAsync(u => u.Id == guid);
            if (user == null) return null;

            var userId = user.Id;
            var animeList = await _dbContext.Animelist
                .Where(al => al.Animeid == animeId && al.Userid == userId)
                .Select(al => new
                {
                    al.Animeid,
                    al.Status,
                    al.Watchedepisodes,
                    al.Rating,
                    al.Created,
                    al.Lastupdated,
                })
                .OrderBy(a => a.Status)
                .ToListAsync();

            if (animeList.Count == 0) throw new Exception("Anime not found in list");
            return animeList;
        }

        public async Task<AnimeListDto?> AddAnimeToList(Guid userId, int animeId)
        {
            var isInList = await IsAnimeInList(animeId, userId);
            if (isInList) return null;

            var user = await _dbContext.Profiles.FirstOrDefaultAsync(u => u.Id== userId);
            if (user == null) return null;

            var isAnime = await _animeService.CheckIfAnimeIsInDb(animeId);
            if (isAnime == null) await _animeService.AddAnimeToDatabase(animeId);

            var animeList = new Animelist
            {
                Userid = userId,
                Animeid = animeId,
                Statusid = 1,
                Watchedepisodes = 0,
                Rating = 0,
                Created = DateTime.Now,
                Lastupdated = DateTime.Now
            };

            _dbContext.Animelist.Add(animeList);
            await _dbContext.SaveChangesAsync();

            var animeListDto = new AnimeListDto
            {
                UserId = userId,
                AnimeId = animeId,
                Status = 1,
                WatchedEpisodes = 0,
                Rating = 0,
                Created = DateTime.Now,
                LastUpdated = DateTime.Now
            };
            return animeListDto;
        }

        public async Task<AnimeListDto?> UpdateAnimeList(Guid guid, int animeId, Requests.AnimeListRequest request)
        {
            var isInList = await IsAnimeInList(animeId, guid);
            if (!isInList) return null;

            var userId = guid;
            var watchedEpisodes = request.WatchedEpisodes;
            var status = request.StatusId;
            var rating = request.Rating;

            var isAnime = await _animeService.CheckIfAnimeIsInDb(animeId);
            if (isAnime == null) await _animeService.AddAnimeToDatabase(animeId);
            var anime = await _dbContext.Anime.FirstOrDefaultAsync(a => a.Animeid == animeId);

            if (request.WatchedEpisodes >= anime?.Episodecount)
            {
                watchedEpisodes = anime.Episodecount;
                status = await GetStatusIdByName("Completed");
            }

            if (request.StatusId == 2) watchedEpisodes = anime?.Episodecount;

            rating = request.Rating switch
            {
                > 10 => 10,
                < 0 => 0,
                _ => rating
            };

            var animeList =
                await _dbContext.Animelist.FirstOrDefaultAsync(a => a.Animeid == animeId && a.Userid == userId);
            animeList.Statusid = status;
            animeList.Watchedepisodes = watchedEpisodes;
            animeList.Rating = rating;
            animeList.Lastupdated = DateTime.Now;
            _dbContext.Animelist.Update(animeList);
            await _dbContext.SaveChangesAsync();

            var animeListDto = new AnimeListDto
            {
                UserId = userId,
                AnimeId = animeId,
                Status = status,
                WatchedEpisodes = watchedEpisodes,
                Rating = rating,
                Created = animeList.Created.Value,
                LastUpdated = animeList.Lastupdated.Value
            };
            return animeListDto;
        }

        public async Task<object?> RemoveAnimeFromList(Guid requestUserId, int requestAnimeId)
        {
            var animeList =
                await _dbContext.Animelist.FirstOrDefaultAsync(a =>
                    a.Animeid == requestAnimeId && a.Userid == requestUserId);
            if (animeList == null) return null;
            _dbContext.Animelist.Remove(animeList);
            await _dbContext.SaveChangesAsync();
            return animeList;
        }

        private async Task<bool> IsAnimeInList(int id, Guid userId)
        {
            var animeList = await _dbContext.Animelist.FirstOrDefaultAsync(a => a.Animeid == id && a.Userid == userId);
            return animeList != null;
        }

        private async Task<int> GetStatusIdByName(string statusName)
        {
            var status = await _dbContext.Status.FirstOrDefaultAsync(s => s.Statusname == statusName);
            return status?.Statusid ?? 1;
        }
    }
}
