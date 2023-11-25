namespace AnimeListApi.Models.Dto.Manga;

public class MangaListDto
{
    public Guid? UserId { get; set; }

    public int? MangaId { get; set; }

    public int? Status { get; set; }

    public int? ReadChapters { get; set; }

    public int? Rating { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? LastUpdated { get; set; }

}