using System.Text.Json.Serialization;

namespace foroLIS_backend.DTOs
{
    public class MercadoPagoTokenResponseDto
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("user_id")]
        public long? UserId { get; set; }
    }
}
