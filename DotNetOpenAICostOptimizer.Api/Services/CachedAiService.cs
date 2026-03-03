using System.Security.Cryptography;
using System.Text;
using DotNetOpenAICostOptimizer.Services;
using Microsoft.Extensions.Caching.Distributed;

namespace DotNetOpenAICostOptimizer.Services;

public class CachedAiService : IAiProvider
{
    private readonly IAiProvider _innerService;
    private readonly IDistributedCache _cache;


    public CachedAiService(IAiProvider innerService, IDistributedCache cache)
    {
        _innerService = innerService;
        _cache = cache;
    }

    public async Task<string> AskAsync(string prompt, CancellationToken token = default)
    {
        string cacheKey = $"ai_hash:{ComputeSha256Hash(prompt)}";
        
        var cachedResponse = await _cache.GetStringAsync(cacheKey, token);
        if (!string.IsNullOrEmpty(cachedResponse))
            return cachedResponse;
        
        var response = await _innerService.AskAsync(prompt, token);

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
        };
        await _cache.SetStringAsync(cacheKey, response, options, token: token);
        
        return response;
    }

    private string ComputeSha256Hash(string data)
    {
        using var sha256Hash = SHA256.Create();
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(data));
        return Convert.ToHexString(bytes);
    }
}