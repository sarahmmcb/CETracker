using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using CETrackerApi.Helpers;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace CETrackerApi.Security;

public class TokenAccessor
{
    private PropertyInfo[] _tokenProperties { get; set; }
    
    public TokenAccessor()
    {
        var type = typeof(Token);
        _tokenProperties = type.GetProperties();
    }
    
    private Token _token { get; set; }
    private bool _tokenDecoded = false;

    public void DecodeToken(string bearerToken)
    {
        if (_tokenDecoded)
        {
            return;  // TODO: Log and/or throw error or something
        }

        var principal = ValidateToken(bearerToken);
        _token = new Token(principal);
        _tokenDecoded = true;
    }

    public string GetProperty(string name)
    {
        var prop = _tokenProperties.FirstOrDefault(p => p.Name.ToLower() == name.ToLower());
        if (prop is null)
        {
            return string.Empty;
        }

        var propValue = prop.GetValue(_token);
        if (propValue is null)
        {
            return string.Empty;
        }

        return propValue.ToString();
    }

    private ClaimsPrincipal ValidateToken(string jwtToken)
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

    public class Token(ClaimsPrincipal principal) // TODO: extract this to a shareable package between Auth and this API
    {
        public string? UserId { get; private set; } = principal.FindFirst("user_id")?.Value;
        public string? Username { get; private set; } = principal.FindFirst(ClaimTypes.Name)?.Value;
        public List<string>? Roles { get; private set; } = [.. principal.Claims.Where(c => string.Equals(c, ClaimTypes.Role)).Select(c => c.Value)];
        public string? Nbf { get; private set; } = principal.FindFirst("nbf")?.Value;
        public string? Exp { get; private set; } = principal.FindFirst("exp")?.Value;
        public string? Iat { get; private set; } = principal.FindFirst("iat")?.Value;
        public string? Iss { get; private set; } = principal.FindFirst("iss")?.Value;
        public List<string>? Aud { get; private set; } = [.. principal.Claims.Where(c => string.Equals(c.Type.ToLower(), "aud")).Select(c => c.Value)];
    }
}
