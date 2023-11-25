// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

namespace AnimeListApi.Models.Utilities;

public record RawAppMetaData(
    string? provider,
    IReadOnlyList<string>? providers
);

public record RawUserMetaData(
    string? username
);

public record Record(
    string? id,
    string? aud,
    string? role,
    string? email,
    object? phone,
    DateTime created_at,
    object? deleted_at,
    object? invited_at,
    DateTime? updated_at,
    string? instance_id,
    bool? is_sso_user,
    object? banned_until,
    object? confirmed_at,
    string? email_change,
    string? phone_change,
    object? is_super_admin,
    string? recovery_token,
    object? last_sign_in_at,
    object? recovery_sent_at,
    RawAppMetaData? raw_app_meta_data,
    string? confirmation_token,
    object? email_confirmed_at,
    string? encrypted_password,
    string? phone_change_token,
    object? phone_confirmed_at,
    RawUserMetaData? raw_user_meta_data,
    object? confirmation_sent_at,
    object? email_change_sent_at,
    object? phone_change_sent_at,
    string? email_change_token_new,
    string? reauthentication_token,
    object? reauthentication_sent_at,
    string? email_change_token_current,
    int? email_change_confirm_status
);

public record Webhook(
    string? type,
    string? table,
    Record? record,
    string? schema,
    object? old_record
);
