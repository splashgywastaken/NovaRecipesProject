namespace NovaRecipesProject.Services.Cache;

using Common.Extensions;
using StackExchange.Redis;

/// <inheritdoc />
public class CacheService : ICacheService
{
    private readonly TimeSpan _defaultLifetime;
    private readonly IDatabase _cacheDb;
    private static string _redisUri = null!;
    private static readonly Lazy<ConnectionMultiplexer> LazyConnection = 
        new(() => ConnectionMultiplexer.Connect(_redisUri));
    private static ConnectionMultiplexer Connection => LazyConnection.Value;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="settings"></param>
    public CacheService(CacheSettings settings)
    {
        var localCacheSettings = settings;

        _redisUri = localCacheSettings.Uri;
        _defaultLifetime = TimeSpan.FromMinutes(localCacheSettings.CacheLifeTime);

        _cacheDb = Connection.GetDatabase();
    }

    /// <inheritdoc />
    public string KeyGenerate()
    {
        return Guid.NewGuid().Shrink();
    }

    /// <inheritdoc />
    public async Task<bool> Delete(string key)
    {
        return await _cacheDb.KeyDeleteAsync(key);
    }

    /// <inheritdoc />
    public async Task<T> Get<T>(string key, bool resetLifeTime = false)
    {
        try
        {
            string cachedData = (await _cacheDb.StringGetAsync(key))!;
            if (cachedData.IsNullOrEmpty())
                return default!;

            var data = cachedData.FromJsonString<T>();

            if (resetLifeTime)
                await SetStoreTime(key);

            return data!;
        }
        catch (Exception ex)
        {
            throw new Exception($"Can`t get data from cache for {key}", ex);
        }
    }

    /// <inheritdoc />
    public async Task<bool> Put<T>(string key, T data, TimeSpan? storeTime = null)
    {
        return await _cacheDb.StringSetAsync(key, data!.ToJsonString(), storeTime ?? _defaultLifetime);
    }

    /// <inheritdoc />
    public async Task SetStoreTime(string key, TimeSpan? storeTime = null)
    {
        await _cacheDb.KeyExpireAsync(key, storeTime ?? _defaultLifetime);
    }
}
