using Newtonsoft.Json;

namespace AnimeListApi.Models.Dto.Character
{
    public record Data(
        [property: JsonProperty("mal_id")] int mal_id,
        [property: JsonProperty("url")] string url,
        [property: JsonProperty("images")] Images images,
        [property: JsonProperty("name")] string name,
        [property: JsonProperty("name_kanji")] string name_kanji,
        [property: JsonProperty("nicknames")] IReadOnlyList<object> nicknames,
        [property: JsonProperty("favorites")] int? favorites,
        [property: JsonProperty("about")] string about
    );

    public record Images(
        [property: JsonProperty("jpg")] Jpg jpg,
        [property: JsonProperty("webp")] Webp webp
    );

    public record Jpg(
        [property: JsonProperty("image_url")] string image_url
    );

    public record CharacterData(
        [property: JsonProperty("data")] Data data
    );

    public record Webp(
        [property: JsonProperty("image_url")] string image_url,
        [property: JsonProperty("small_image_url")] string small_image_url
    );
}
