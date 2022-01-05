namespace USTB.Login.FromConsole.Extensions;

public class CustomPhysicalAddressConverter : JsonConverter<PhysicalAddress>
{
    public override PhysicalAddress? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return PhysicalAddress.TryParse(reader.GetString(), out PhysicalAddress? address)
            ? address
            : null;
    }

    public override void Write(Utf8JsonWriter writer, PhysicalAddress value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}