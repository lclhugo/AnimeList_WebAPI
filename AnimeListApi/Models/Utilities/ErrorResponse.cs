namespace AnimeListApi.Models.Utilities {
    public class ErrorResponse {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public Dictionary<string, string> Details { get; set; }
    }
}
