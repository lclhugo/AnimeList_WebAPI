// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

using Newtonsoft.Json;

namespace AnimeListApi.Models.Dto.Manga;

public record Author(
    [property: JsonProperty("mal_id")] int mal_id,
    [property: JsonProperty("type")] string type,
    [property: JsonProperty("name")] string name,
    [property: JsonProperty("url")] string url
);

public record Data(
    [property: JsonProperty("mal_id")] int mal_id,
    [property: JsonProperty("url")] string url,
    [property: JsonProperty("images")] Images images,
    [property: JsonProperty("approved")] bool? approved,
    [property: JsonProperty("titles")] IReadOnlyList<Title> titles,
    [property: JsonProperty("title")] string title,
    [property: JsonProperty("title_english")] string title_english,
    [property: JsonProperty("title_japanese")] string title_japanese,
    [property: JsonProperty("title_synonyms")] IReadOnlyList<object> title_synonyms,
    [property: JsonProperty("type")] string type,
    [property: JsonProperty("chapters")] int? chapters,
    [property: JsonProperty("volumes")] int? volumes,
    [property: JsonProperty("status")] string status,
    [property: JsonProperty("publishing")] bool? publishing,
    [property: JsonProperty("published")] Published published,
    [property: JsonProperty("score")] double? score,
    [property: JsonProperty("scored")] double? scored,
    [property: JsonProperty("scored_by")] int? scored_by,
    [property: JsonProperty("rank")] int? rank,
    [property: JsonProperty("popularity")] int? popularity,
    [property: JsonProperty("members")] int? members,
    [property: JsonProperty("favorites")] int? favorites,
    [property: JsonProperty("synopsis")] string synopsis,
    [property: JsonProperty("background")] string background,
    [property: JsonProperty("authors")] IReadOnlyList<Author> authors,
    [property: JsonProperty("serializations")] IReadOnlyList<Serialization> serializations,
    [property: JsonProperty("genres")] IReadOnlyList<Genre> genres,
    [property: JsonProperty("explicit_genres")] IReadOnlyList<object> explicit_genres,
    [property: JsonProperty("themes")] IReadOnlyList<Theme> themes,
    [property: JsonProperty("demographics")] IReadOnlyList<Demographic> demographics
);

public record Demographic(
    [property: JsonProperty("mal_id")] int? mal_id,
    [property: JsonProperty("type")] string type,
    [property: JsonProperty("name")] string name,
    [property: JsonProperty("url")] string url
);

public record From(
    [property: JsonProperty("day")] int? day,
    [property: JsonProperty("month")] int? month,
    [property: JsonProperty("year")] int? year
);

public record Genre(
    [property: JsonProperty("mal_id")] int? mal_id,
    [property: JsonProperty("type")] string type,
    [property: JsonProperty("name")] string name,
    [property: JsonProperty("url")] string url
);

public record Images(
    [property: JsonProperty("jpg")] Jpg jpg,
    [property: JsonProperty("webp")] Webp webp
);

public record Jpg(
    [property: JsonProperty("image_url")] string image_url,
    [property: JsonProperty("small_image_url")] string small_image_url,
    [property: JsonProperty("large_image_url")] string large_image_url
);

public record Prop(
    [property: JsonProperty("from")] From from,
    [property: JsonProperty("to")] To to
);

public record Published(
    [property: JsonProperty("from")] DateTime? from,
    [property: JsonProperty("to")] DateTime? to,
    [property: JsonProperty("prop")] Prop prop,
    [property: JsonProperty("string")] string @string
);

public record MangaData(
    [property: JsonProperty("data")] Data data
);

public record Serialization(
    [property: JsonProperty("mal_id")] int? mal_id,
    [property: JsonProperty("type")] string type,
    [property: JsonProperty("name")] string name,
    [property: JsonProperty("url")] string url
);

public record Theme(
    [property: JsonProperty("mal_id")] int? mal_id,
    [property: JsonProperty("type")] string type,
    [property: JsonProperty("name")] string name,
    [property: JsonProperty("url")] string url
);

public record Title(
    [property: JsonProperty("type")] string type,
    [property: JsonProperty("title")] string title
);

public record To(
    [property: JsonProperty("day")] int? day,
    [property: JsonProperty("month")] int? month,
    [property: JsonProperty("year")] int? year
);

public record Webp(
    [property: JsonProperty("image_url")] string image_url,
    [property: JsonProperty("small_image_url")] string small_image_url,
    [property: JsonProperty("large_image_url")] string large_image_url
);