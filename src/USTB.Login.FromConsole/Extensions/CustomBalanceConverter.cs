namespace USTB.Login.FromConsole.Extensions;

public class CustomBalanceConverter : JsonConverter<double>
{
    public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetDouble() / 100;
    }

    public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}