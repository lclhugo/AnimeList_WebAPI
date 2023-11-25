using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;


namespace AnimeListApi.Test
{
    public class AnimeTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public AnimeTest(WebApplicationFactory<Program> factory) => _factory = factory;

        private const string AnimeApiUrl = "/api/anime/";
        private const int ValidAnimeId = 25;
        private const string InvalidAnimeId = "aaa";

        [Fact]
        public async Task GetAnimeById_ReturnsOk()
        {
            var client = _factory.CreateClient();

            // QUAND on fait GET sur /api/anime/1
            var response = await client.GetAsync($"{AnimeApiUrl}{ValidAnimeId}");
            // ALORS on reçoit un code 200
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetAnimeById_ReturnsNotFound()
        {
            var client = _factory.CreateClient();

            // QUAND on fait GET sur /api/anime/aaa
            var response = await client.GetAsync($"{AnimeApiUrl}{InvalidAnimeId}");
            // ALORS on reçoit un code 404
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetMangaById_ReturnsOk()
        {
            var client = _factory.CreateClient();

            // QUAND on fait GET sur /api/anime/1
            var response = await client.GetAsync("/api/manga/25");
            // ALORS on reçoit un code 200
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetMangaById_ReturnsNotFound()
        {
            var client = _factory.CreateClient();

            // QUAND on fait GET sur /api/manga/aaa
            var response = await client.GetAsync("/api/manga/aaa");
            // ALORS on reçoit un code 404
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetCharacterById_ReturnsOk()
        {
            var client = _factory.CreateClient();

            // QUAND on fait GET sur /api/character/1
            var response = await client.GetAsync("/api/character/25");
            // ALORS on reçoit un code 200
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetCharacterById_ReturnsNotFound()
        {
            var client = _factory.CreateClient();

            // QUAND on fait GET sur /api/character/aaa
            var response = await client.GetAsync("/api/character/aaa");
            // ALORS on reçoit un code 404
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}