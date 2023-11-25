using AnimeListApi.Models.Dto.Anime;
using AnimeListApi.Models.Dto.Character;
using AnimeListApi.Models.Dto.Manga;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace AnimeListApi.Handlers;

public class JikanHandler
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;
    private const string JikanApiBaseUrl = "https://api.jikan.moe/v4";

    public JikanHandler(HttpClient httpClient, IMemoryCache cache)
    {
        _httpClient = httpClient;
        _cache = cache;
    }

    public async Task<AnimeData?> GetAnimeDetails(int animeId)
    {
        try
        {
            var cacheKey = $"Anime_{animeId}";

            if (_cache.TryGetValue(cacheKey, out AnimeData? cachedData)) return cachedData;

            var requestUri = $"{JikanApiBaseUrl}/anime/{animeId}";
            var response = await _httpClient.GetStringAsync(requestUri);
            var animeData = JsonConvert.DeserializeObject<AnimeData>(response);

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            };

            _cache.Set(cacheKey, animeData, cacheEntryOptions);

            return animeData;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<MangaData?> GetMangaDetails(int mangaId)
    {
        try
        {
            var cacheKey = $"Manga_{mangaId}";

            if (_cache.TryGetValue(cacheKey, out MangaData? cachedData)) return cachedData;

            var requestUri = $"{JikanApiBaseUrl}/manga/{mangaId}";
            var response = await _httpClient.GetStringAsync(requestUri);
            var mangaData = JsonConvert.DeserializeObject<MangaData>(response);

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            };

            _cache.Set(cacheKey, mangaData, cacheEntryOptions);

            return mangaData;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<CharacterData?> GetCharacterDetails(int characterId)
    {
        try
        {
            var cacheKey = $"Character_{characterId}";

            if (_cache.TryGetValue(cacheKey, out CharacterData? cachedData)) return cachedData;

            var requestUri = $"{JikanApiBaseUrl}/characters/{characterId}";
            var response = await _httpClient.GetStringAsync(requestUri);
            var characterData = JsonConvert.DeserializeObject<CharacterData>(response);

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            };

            _cache.Set(cacheKey, characterData, cacheEntryOptions);

            return characterData;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}