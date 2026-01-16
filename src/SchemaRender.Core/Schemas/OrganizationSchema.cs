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
    /// The organization's logo.
    /// </summary>
    public ImageObjectSchema? Logo { get; init; }

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
    /// The postal address of the organization.
    /// </summary>
    public PostalAddressSchema? Address { get; init; }

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

        if (Logo is { HasValue: true })
        {
            w.WritePropertyName("logo");
            Logo.Write(w);
        }

        if (Description is not null)
            w.WriteString("description", Description);

        if (Email is not null)
            w.WriteString("email", Email);

        if (Telephone is not null)
            w.WriteString("telephone", Telephone);

        if (Address is { HasValue: true })
        {
            w.WritePropertyName("address");
            Address.Write(w);
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
}
