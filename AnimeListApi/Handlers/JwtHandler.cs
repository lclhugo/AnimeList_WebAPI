using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AnimeListApi.Handlers {
    public static class JwtHandler {
        public static Guid GetGuidFromJwt(string jwtToken) {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(jwtToken);

            var subClaim = token.Claims.FirstOrDefault(claim => claim.Type == "sub");
            var sub = subClaim?.Value;
            var guid = Guid.Parse(sub ?? string.Empty);

            return guid;
        }
    }
}