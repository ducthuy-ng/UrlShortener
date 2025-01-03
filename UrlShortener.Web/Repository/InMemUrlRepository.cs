namespace UrlShortener.Web.Repository;

public class InMemUrlRepository : IUrlRepository
{
    readonly Dictionary<string, string> _internalStorage = [];

    void IUrlRepository.Add(string key, string value)
    {
        _internalStorage.Add(key, value);
    }

    string? IUrlRepository.GetValue(string key)
    {
        return _internalStorage.GetValueOrDefault(key);
    }

    bool IUrlRepository.ContainsKey(string key)
    {
        return _internalStorage.ContainsKey(key);
    }
}
