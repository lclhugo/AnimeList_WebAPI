namespace AnimeListApi.Models.Dto.User
{
    public record UserPageDto (Guid id, string username, string avatarUrl, string bio);

    public record UserDto (Guid id, string username, string avatarUrl);
}
