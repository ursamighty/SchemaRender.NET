using System.Text.Json;

namespace SchemaRender.Schemas;

/// <summary>
/// Schema.org Person structured data.
/// See: https://schema.org/Person
/// </summary>
/// <remarks>
/// This schema is intended to be used as a nested object within other schemas
/// (e.g., Recipe, Article). It does not include @context when serialized.
/// </remarks>
public sealed class PersonSchema : ISchema
{
    /// <summary>
    /// The name of the person.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The URL of a page about the person (e.g., their website or social profile).
    /// </summary>
    public string? Url { get; init; }

    /// <summary>
    /// An image of the person.
    /// </summary>
    public ImageObjectSchema? Image { get; init; }

    /// <summary>
    /// The person's job title.
    /// </summary>
    public string? JobTitle { get; init; }

    /// <summary>
    /// URLs of social media profiles or other pages that identify the same person.
    /// </summary>
    public IReadOnlyList<string>? SameAs { get; init; }

    /// <summary>
    /// Email address of the person.
    /// </summary>
    public string? Email { get; init; }

    /// <summary>
    /// Telephone number of the person.
    /// </summary>
    public string? Telephone { get; init; }

    /// <summary>
    /// Physical address of the person.
    /// </summary>
    public PostalAddressSchema? Address { get; init; }

    /// <summary>
    /// Date of birth of the person.
    /// </summary>
    public DateTimeOffset? BirthDate { get; init; }

    /// <summary>
    /// A description of the person.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// The organization that the person works for.
    /// </summary>
    public string? WorksFor { get; init; }

    /// <summary>
    /// An organization that the person is an alumni of.
    /// </summary>
    public string? AlumniOf { get; init; }

    /// <summary>
    /// An award won by or for the person.
    /// </summary>
    public string? Award { get; init; }

    /// <summary>
    /// Nationality of the person.
    /// </summary>
    public string? Nationality { get; init; }

    /// <inheritdoc />
    public void Write(Utf8JsonWriter w)
    {
        w.WriteStartObject();
        w.WriteString("@type", "Person");
        w.WriteString("name", Name);

        if (Url is not null)
            w.WriteString("url", Url);

        if (Image is { HasValue: true })
        {
            w.WritePropertyName("image");
            Image.Write(w);
        }

        if (JobTitle is not null)
            w.WriteString("jobTitle", JobTitle);

        if (SameAs is { Count: > 0 })
        {
            w.WritePropertyName("sameAs");
            w.WriteStartArray();
            foreach (var url in SameAs)
                w.WriteStringValue(url);
            w.WriteEndArray();
        }

        if (Email is not null)
            w.WriteString("email", Email);

        if (Telephone is not null)
            w.WriteString("telephone", Telephone);

        if (Address is { HasValue: true })
        {
            w.WritePropertyName("address");
            Address.Write(w);
        }

        if (BirthDate is not null)
            w.WriteString("birthDate", BirthDate.Value.ToString("O"));

        if (Description is not null)
            w.WriteString("description", Description);

        if (WorksFor is not null)
            w.WriteString("worksFor", WorksFor);

        if (AlumniOf is not null)
            w.WriteString("alumniOf", AlumniOf);

        if (Award is not null)
            w.WriteString("award", Award);

        if (Nationality is not null)
            w.WriteString("nationality", Nationality);

        w.WriteEndObject();
    }
}
