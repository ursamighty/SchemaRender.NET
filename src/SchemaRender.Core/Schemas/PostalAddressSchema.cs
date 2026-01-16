using System.Text.Json;

namespace SchemaRender.Schemas;

/// <summary>
/// Schema.org PostalAddress structured data.
/// See: https://schema.org/PostalAddress
/// </summary>
/// <remarks>
/// This schema is intended to be used as a nested object within other schemas
/// (e.g., Organization, LocalBusiness). It does not include @context when serialized.
/// </remarks>
public sealed class PostalAddressSchema : ISchema
{
    /// <summary>
    /// The street address (e.g., "123 Main St").
    /// </summary>
    public string? StreetAddress { get; init; }

    /// <summary>
    /// The locality/city (e.g., "New York").
    /// </summary>
    public string? AddressLocality { get; init; }

    /// <summary>
    /// The region/state/province (e.g., "NY").
    /// </summary>
    public string? AddressRegion { get; init; }

    /// <summary>
    /// The postal code (e.g., "10001").
    /// </summary>
    public string? PostalCode { get; init; }

    /// <summary>
    /// The country (e.g., "US" or "United States").
    /// </summary>
    public string? AddressCountry { get; init; }

    /// <inheritdoc />
    public void Write(Utf8JsonWriter w)
    {
        w.WriteStartObject();
        w.WriteString("@type", "PostalAddress");

        if (StreetAddress is not null)
            w.WriteString("streetAddress", StreetAddress);

        if (AddressLocality is not null)
            w.WriteString("addressLocality", AddressLocality);

        if (AddressRegion is not null)
            w.WriteString("addressRegion", AddressRegion);

        if (PostalCode is not null)
            w.WriteString("postalCode", PostalCode);

        if (AddressCountry is not null)
            w.WriteString("addressCountry", AddressCountry);

        w.WriteEndObject();
    }

    /// <summary>
    /// Returns true if any address property has a value.
    /// </summary>
    public bool HasValue =>
        StreetAddress is not null ||
        AddressLocality is not null ||
        AddressRegion is not null ||
        PostalCode is not null ||
        AddressCountry is not null;
}
