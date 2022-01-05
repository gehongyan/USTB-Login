namespace USTB.Login.FromConsole.Extensions;

public class CustomIPAddressConverter : JsonConverter<IPAddress>
{
    public override IPAddress? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return IPAddress.TryParse(reader.GetString(), out IPAddress? address)
            ? address
            : null;
    }

    public override void Write(Utf8JsonWriter writer, IPAddress value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}