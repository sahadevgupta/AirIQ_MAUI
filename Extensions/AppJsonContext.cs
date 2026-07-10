using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using AirIQ.Models.Response;

namespace AirIQ.Extensions;

[JsonSerializable(typeof(LoginDto))]
[JsonSerializable(typeof(UserDto))]
[JsonSerializable(typeof(FlightSearchResultDto))]
public partial class AppJsonContext : JsonSerializerContext
{
}
