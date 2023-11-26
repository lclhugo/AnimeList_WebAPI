using Microsoft.AspNetCore.SignalR;

namespace AnimeListApi.Models.Dto.Anime {
    public class AnimeListDto {
        public Guid UserId { get; set; }

        public int AnimeId { get; set; }

        public int? Status { get; set; }

        public int? WatchedEpisodes { get; set; }

        public int? Rating { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}