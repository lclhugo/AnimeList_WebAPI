namespace AnimeListApi.Models.Dto.Requests
{
    public class Requests
    {
        public record MangaListRequest(int MangaId, int? StatusId, int? ReadChapters,
            int? Rating, DateTime? Created, DateTime? LastUpdated);

        public record AnimeListRequest(int? StatusId, int? WatchedEpisodes,
            int? Rating, DateTime? Created, DateTime? LastUpdated);

        public record QuickRequest(int Id, int StatusId);

    }
}
