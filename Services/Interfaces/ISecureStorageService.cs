using System;

namespace AirIQ.Services.Interfaces;

public interface ISecureStorageService
{
    Task<T> GetAsync<T>(string key);
    Task<string> GetAsync(string key);
    Task SetAsync(string key, string value);
    bool Remove(string key);
    void RemoveAll();
}
