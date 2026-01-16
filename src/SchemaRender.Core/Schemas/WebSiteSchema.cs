using System.Text.Json;

namespace SchemaRender.Schemas;

/// <summary>
/// Schema.org WebSite structured data.
/// See: https://schema.org/WebSite
/// </summary>
public sealed class WebSiteSchema : ISchema
{
    /// <summary>
    /// The name of the website.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The URL of the website.
    /// </summary>
    public required string Url { get; init; }

    /// <summary>
    /// A description of the website.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Optional SearchAction JSON for site search functionality.
    /// This should be a properly formatted SearchAction if provided.
    /// </summary>
    public string? PotentialAction { get; init; }

    /// <inheritdoc />
    public void Write(Utf8JsonWriter w)
    {
        w.WriteStartObject();
        w.WriteString("@context", "https://schema.org");
        w.WriteString("@type", "WebSite");
        w.WriteString("name", Name);
        w.WriteString("url", Url);

        if (Description is not null)
            w.WriteString("description", Description);

        if (PotentialAction is not null)
            w.WriteString("potentialAction", PotentialAction);

        w.WriteEndObject();
    }
}
