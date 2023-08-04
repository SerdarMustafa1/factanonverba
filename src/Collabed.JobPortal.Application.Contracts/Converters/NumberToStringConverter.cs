using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Collabed.JobPortal.Converters;

public class NumberToStringConverter : JsonConverter<string>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeof(string) == typeToConvert;
    }

    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            if (reader.TryGetDouble(out var doubleNumber))
            {
                return doubleNumber.ToString(CultureInfo.InvariantCulture);
            }
            if (reader.TryGetInt64(out var number))
            {
                return number.ToString(CultureInfo.InvariantCulture);
            }
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            return reader.GetString();
        }

        using var document = JsonDocument.ParseValue(ref reader);
        return document.RootElement.Clone().ToString();
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}