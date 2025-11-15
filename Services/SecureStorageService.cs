using System;
using AirIQ.Services.Interfaces;
using Newtonsoft.Json;

namespace AirIQ.Services;

public class SecureStorageService : ISecureStorageService
{
    public async Task<T> GetAsync<T>(string key)
    {
        var content = await SecureStorage.Default.GetAsync(key);
        if (!string.IsNullOrWhiteSpace(content))
        {
            return JsonConvert.DeserializeObject<T>(content)!;
        }

        return default(T)!;
    }

    public async Task<string> GetAsync(string key)
    {
        return await SecureStorage.Default.GetAsync(key) ?? string.Empty;
    }

    public async Task SetAsync(string key, string value)
    {
        await SecureStorage.Default.SetAsync(key, value);
    }

    public bool Remove(string key)
    {
        return SecureStorage.Default.Remove(key);
    }
    
    public void RemoveAll()
    {
       SecureStorage.Default.RemoveAll();
    }
}
