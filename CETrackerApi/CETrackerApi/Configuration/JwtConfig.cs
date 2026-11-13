namespace CETrackerApi.Configuration;

public class JwtConfig
{
    public IEnumerable<string> ValidAudiences { get; set; }
    public string ValidIssuer { get; set; }
    public string Secret { get; set; }
}
