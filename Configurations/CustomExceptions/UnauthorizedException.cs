using System;

namespace AirIQ.Configurations.CustomExceptions;

[System.Serializable]
public class UnauthorizedException : System.Exception
{
    public UnauthorizedException() { }
    public UnauthorizedException(string message) : base(message) { }
    public UnauthorizedException(string message, System.Exception inner) : base(message, inner) { }
    protected UnauthorizedException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
