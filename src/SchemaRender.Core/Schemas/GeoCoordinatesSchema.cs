using System.Text.Json;

namespace SchemaRender.Schemas;

/// <summary>
/// Schema.org GeoCoordinates structured data.
/// See: https://schema.org/GeoCoordinates
/// </summary>
/// <remarks>
/// This schema is intended to be used as a nested object within other schemas
/// (e.g., LocalBusiness, Place). It does not include @context when serialized.
/// </remarks>
public sealed class GeoCoordinatesSchema : ISchema
{
    /// <summary>
    /// The latitude of the location.
    /// </summary>
    public required double Latitude { get; init; }

    /// <summary>
    /// The longitude of the location.
    /// </summary>
    public required double Longitude { get; init; }

    /// <summary>
    /// The elevation of the location in meters.
    /// </summary>
    public double? Elevation { get; init; }

    /// <summary>
    /// Physical address of the location.
    /// </summary>
    public PostalAddressSchema? Address { get; init; }

    /// <inheritdoc />
    public void Write(Utf8JsonWriter w)
    {
        w.WriteStartObject();
        w.WriteString("@type", "GeoCoordinates");
        w.WriteNumber("latitude", Latitude);
        w.WriteNumber("longitude", Longitude);

        if (Elevation is not null)
            w.WriteNumber("elevation", Elevation.Value);

        if (Address is { HasValue: true })
        {
            w.WritePropertyName("address");
            Address.Write(w);
        }

        w.WriteEndObject();
    }
}
