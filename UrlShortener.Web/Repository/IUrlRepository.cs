namespace UrlShortener.Web.Repository;

public interface IUrlRepository
{
    bool ContainsKey(string key);

    void Add(string key, string value);

    string? GetValue(string key);
}
