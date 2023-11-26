using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AnimeListApi.Models.Utilities;

namespace AnimeListApi.Handlers {
    public abstract class ErrorHandler {
        public static IActionResult CreateErrorResponse(int statusCode, string errorCode, string errorMessage, IDictionary<string, string>? details = null) {
            var response = new ErrorResponse {
                ErrorCode = errorCode,
                ErrorMessage = errorMessage,
                Details = details != null ? new Dictionary<string, string>(details) : new Dictionary<string, string>()
            };

            return new ObjectResult(response) {
                StatusCode = statusCode,
            };
        }
    }
}