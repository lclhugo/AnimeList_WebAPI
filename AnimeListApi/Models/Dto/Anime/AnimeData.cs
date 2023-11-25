// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
using Newtonsoft.Json;

namespace AnimeListApi.Models.Dto.Anime
{
    public record AnimeData(
        [property: JsonProperty("data")] Data? Data
    );

    public record Aired(
        [property: JsonProperty("from")] DateTime? From,
        [property: JsonProperty("to")] DateTime? To,
        [property: JsonProperty("prop")] Prop? Prop,
        [property: JsonProperty("string")] string? String
    );

    public record Broadcast(
        [property: JsonProperty("day")] object? Day,
        [property: JsonProperty("time")] object? Time,
        [property: JsonProperty("timezone")] object? Timezone,
        [property: JsonProperty("string")] string? String
    );

    public record Data(
        [property: JsonProperty("mal_id")] int MalId,
        [property: JsonProperty("url")] string? Url,
        [property: JsonProperty("images")] Images? Images,
        [property: JsonProperty("trailer")] Trailer? Trailer,
        [property: JsonProperty("approved")] bool? Approved,
        [property: JsonProperty("titles")] IReadOnlyList<Titles>? Titles,
        [property: JsonProperty("title")] string? Title,
        [property: JsonProperty("title_english")]
        string? TitleEnglish,
        [property: JsonProperty("title_japanese")]
        string? TitleJapanese,
        [property: JsonProperty("title_synonyms")]
        IReadOnlyList<string>? TitleSynonyms,
        [property: JsonProperty("type")] string? Type,
        [property: JsonProperty("source")] string? Source,
        [property: JsonProperty("episodes")] int? Episodes,
        [property: JsonProperty("status")] string? Status,
        [property: JsonProperty("airing")] bool? Airing,
        [property: JsonProperty("aired")] Aired? Aired,
        [property: JsonProperty("duration")] string? Duration,
        [property: JsonProperty("rating")] string? Rating,
        [property: JsonProperty("score")] double? Score,
        [property: JsonProperty("scored_by")] int? ScoredBy,
        [property: JsonProperty("rank")] int? Rank,
        [property: JsonProperty("popularity")] int? Popularity,
        [property: JsonProperty("members")] int? Members,
        [property: JsonProperty("favorites")] int? Favorites,
        [property: JsonProperty("synopsis")] string? Synopsis,
        [property: JsonProperty("background")] object? Background,
        [property: JsonProperty("season")] string? Season,
        [property: JsonProperty("year")] int? Year,
        [property: JsonProperty("broadcast")] Broadcast? Broadcast,
        [property: JsonProperty("producers")] IReadOnlyList<Producer>? Producers,
        [property: JsonProperty("licensors")] IReadOnlyList<Licensor>? Licensors,
        [property: JsonProperty("studios")] IReadOnlyList<Studio>? Studios,
        [property: JsonProperty("genres")] IReadOnlyList<Genre>? Genres,
        [property: JsonProperty("explicit_genres")]
        IReadOnlyList<object>? ExplicitGenres,
        [property: JsonProperty("themes")] IReadOnlyList<object>? Themes,
        [property: JsonProperty("demographics")]
        IReadOnlyList<Demographic>? Demographics
    );

    public record Demographic(
        [property: JsonProperty("mal_id")] int MalId,
        [property: JsonProperty("type")] string? Type,
        [property: JsonProperty("name")] string? Name,
        [property: JsonProperty("url")] string? Url
    );

    public record From(
        [property: JsonProperty("day")] int? Day,
        [property: JsonProperty("month")] int? Month,
        [property: JsonProperty("year")] int? Year
    );

    public record Genre(
        [property: JsonProperty("mal_id")] int MalId,
        [property: JsonProperty("type")] string? Type,
        [property: JsonProperty("name")] string? Name,
        [property: JsonProperty("url")] string? Url
    );

    public record Images(
        [property: JsonProperty("jpg")] Jpg? Jpg,
        [property: JsonProperty("webp")] Webp? Webp,
        [property: JsonProperty("image_url")] string? ImageUrl,
        [property: JsonProperty("small_image_url")]
        string? SmallImageUrl,
        [property: JsonProperty("medium_image_url")]
        string? MediumImageUrl,
        [property: JsonProperty("large_image_url")]
        string? LargeImageUrl,
        [property: JsonProperty("maximum_image_url")]
        string? MaximumImageUrl
    );

    public record Jpg(
        [property: JsonProperty("image_url")] string? ImageUrl,
        [property: JsonProperty("small_image_url")]
        string? SmallImageUrl,
        [property: JsonProperty("large_image_url")]
        string? LargeImageUrl
    );

    public record Licensor(
        [property: JsonProperty("mal_id")] int MalId,
        [property: JsonProperty("type")] string? Type,
        [property: JsonProperty("name")] string? Name,
        [property: JsonProperty("url")] string? Url
    );

    public record Producer(
        [property: JsonProperty("mal_id")] int MalId,
        [property: JsonProperty("type")] string? Type,
        [property: JsonProperty("name")] string? Name,
        [property: JsonProperty("url")] string? Url
    );

    public record Prop(
        [property: JsonProperty("from")] From? From,
        [property: JsonProperty("to")] To? To
    );

    public record Studio(
        [property: JsonProperty("mal_id")] int MalId,
        [property: JsonProperty("type")] string? Type,
        [property: JsonProperty("name")] string? Name,
        [property: JsonProperty("url")] string? Url
    );

    public record Titles(
        [property: JsonProperty("type")] string? Type,
        [property: JsonProperty("title")] string? Title
    );

    public record To(
        [property: JsonProperty("day")] int? Day,
        [property: JsonProperty("month")] int? Month,
        [property: JsonProperty("year")] int? Year
    );

    public record Trailer(
        [property: JsonProperty("youtube_id")] string? YoutubeId,
        [property: JsonProperty("url")] string? Url,
        [property: JsonProperty("embed_url")] string? EmbedUrl,
        [property: JsonProperty("images")] Images? Images
    );

    public record Webp(
        [property: JsonProperty("image_url")] string? ImageUrl,
        [property: JsonProperty("small_image_url")]
        string? SmallImageUrl,
        [property: JsonProperty("large_image_url")]
        string? LargeImageUrl
    );
}
