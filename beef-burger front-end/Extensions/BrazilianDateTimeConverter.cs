using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BeefBurger.FrontEnd.Extensions;

public class BrazilianDateTimeConverter : JsonConverter<DateTime>
{
    private static readonly string[] Formats = new[]
    {
        "dd/MM/yyyy HH:mm:ss",
        "yyyy-MM-ddTHH:mm:ss",
        "yyyy-MM-ddTHH:mm:ssZ",
        "yyyy-MM-dd'T'HH:mm:ss.FFFFFFFK"
    };

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dateString = reader.GetString();
        if (string.IsNullOrEmpty(dateString))
            return DateTime.MinValue;

        if (DateTime.TryParseExact(dateString, Formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
            return result;

        if (DateTime.TryParse(dateString, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            return result;

        return DateTime.MinValue;
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("dd/MM/yyyy HH:mm:ss"));
    }
}
