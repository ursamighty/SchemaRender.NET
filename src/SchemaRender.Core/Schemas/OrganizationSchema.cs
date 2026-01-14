using System.Text.Json;

namespace SchemaRender.Schemas;

/// <summary>
/// Schema.org Organization structured data.
/// See: https://schema.org/Organization
/// </summary>
public sealed class OrganizationSchema : ISchema
{
    /// <summary>
    /// The name of the organization.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The URL of the organization's website.
    /// </summary>
    public string? Url { get; init; }

    /// <summary>
    /// The URL of the organization's logo.
    /// </summary>
    public string? Logo { get; init; }

    /// <summary>
    /// A description of the organization.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// The email address of the organization.
    /// </summary>
    public string? Email { get; init; }

    /// <summary>
    /// The telephone number of the organization.
    /// </summary>
    public string? Telephone { get; init; }

    /// <summary>
    /// The street address.
    /// </summary>
    public string? StreetAddress { get; init; }

    /// <summary>
    /// The locality (city).
    /// </summary>
    public string? AddressLocality { get; init; }

    /// <summary>
    /// The region (state/province).
    /// </summary>
    public string? AddressRegion { get; init; }

    /// <summary>
    /// The postal code.
    /// </summary>
    public string? PostalCode { get; init; }

    /// <summary>
    /// The country.
    /// </summary>
    public string? AddressCountry { get; init; }

    /// <summary>
    /// Social media profile URLs.
    /// </summary>
    public IReadOnlyList<string>? SameAs { get; init; }

    /// <inheritdoc />
    public void Write(Utf8JsonWriter w)
    {
        w.WriteStartObject();
        w.WriteString("@context", "https://schema.org");
        w.WriteString("@type", "Organization");
        w.WriteString("name", Name);

        if (Url is not null)
            w.WriteString("url", Url);

        if (Logo is not null)
            w.WriteString("logo", Logo);

        if (Description is not null)
            w.WriteString("description", Description);

        if (Email is not null)
            w.WriteString("email", Email);

        if (Telephone is not null)
            w.WriteString("telephone", Telephone);

        if (HasAddress())
        {
            w.WritePropertyName("address");
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

        if (SameAs is { Count: > 0 })
        {
            w.WritePropertyName("sameAs");
            w.WriteStartArray();
            foreach (var url in SameAs)
                w.WriteStringValue(url);
            w.WriteEndArray();
        }

        w.WriteEndObject();
    }

    private bool HasAddress() =>
        StreetAddress is not null ||
        AddressLocality is not null ||
        AddressRegion is not null ||
        PostalCode is not null ||
        AddressCountry is not null;
}
