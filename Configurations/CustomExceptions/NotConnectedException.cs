using System;

namespace AirIQ.Configurations.CustomExceptions;

[System.Serializable]
public class NotConnectedException : System.Exception
{
    public NotConnectedException() { }
    public NotConnectedException(string message) : base(message) { }
    public NotConnectedException(string message, System.Exception inner) : base(message, inner) { }
    protected NotConnectedException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
