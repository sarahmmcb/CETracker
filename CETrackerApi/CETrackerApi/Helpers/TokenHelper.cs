using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace CETrackerApi.Helpers;

public static class TokenHelper
{
    public static ClaimsPrincipal ValidateToken(string jwtToken)
    {
        var jwtConfig = ConfigurationHelper.Section("JwtConfig");
        var secret = jwtConfig.GetSection("Secret").Value;
        var issuer = jwtConfig.GetSection("ValidIssuer").Value;
        var audience = jwtConfig.GetSection("ValidAudiences").Get<List<string>>();

        IdentityModelEventSource.ShowPII = true;

        SecurityToken validatedToken;
        TokenValidationParameters validationParameters = new TokenValidationParameters();

        validationParameters.ValidateLifetime = true;

        validationParameters.ValidAudiences = audience;
        validationParameters.ValidIssuer = issuer;
        validationParameters.IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);


        return principal;
    }
}
