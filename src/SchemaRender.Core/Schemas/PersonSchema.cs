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

        w.WriteEndObject();
    }
}
