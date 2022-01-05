namespace USTB.Login.FromConsole.Extensions;

public class CustomBooleanConverter : JsonConverter<bool>
{
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetInt32() switch
        {
            0 => false,
            1 => true,
            _ => throw new JsonException()
        };
    }

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}