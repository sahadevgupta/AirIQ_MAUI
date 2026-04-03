using System;

namespace AirIQ.Helpers;

public static class ServiceHelper
{
    public static T? GetService<T>() where T : class
        => Current?.GetService<T>();

    public static object? GetService(Type type)
        => Current?.GetService(type);

    private static IServiceProvider? Current
        => IPlatformApplication.Current?.Services;
}
