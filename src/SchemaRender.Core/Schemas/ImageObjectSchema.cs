using System.Text.Json;

namespace SchemaRender.Schemas;

/// <summary>
/// Schema.org ImageObject structured data.
/// See: https://schema.org/ImageObject
/// </summary>
/// <remarks>
/// This schema is intended to be used as a nested object within other schemas
/// (e.g., Article, Recipe, Product). It does not include @context when serialized.
/// </remarks>
public sealed class ImageObjectSchema : ISchema
{
    /// <summary>
    /// The URL of the image.
    /// </summary>
    public string? Url { get; init; }

    /// <summary>
    /// The direct link to the image content (actual image bytes).
    /// </summary>
    public string? ContentUrl { get; init; }

    /// <summary>
    /// The width of the image in pixels.
    /// </summary>
    public int? Width { get; init; }

    /// <summary>
    /// The height of the image in pixels.
    /// </summary>
    public int? Height { get; init; }

    /// <summary>
    /// A caption or description of the image (alternative text).
    /// </summary>
    public string? Caption { get; init; }

    /// <inheritdoc />
    public void Write(Utf8JsonWriter w)
    {
        w.WriteStartObject();
        w.WriteString("@type", "ImageObject");

        if (Url is not null)
            w.WriteString("url", Url);

        if (ContentUrl is not null)
            w.WriteString("contentUrl", ContentUrl);

        if (Width is not null)
            w.WriteNumber("width", Width.Value);

        if (Height is not null)
            w.WriteNumber("height", Height.Value);

        if (Caption is not null)
            w.WriteString("caption", Caption);

        w.WriteEndObject();
    }

    /// <summary>
    /// Returns true if any image property has a value.
    /// </summary>
    public bool HasValue =>
        Url is not null ||
        ContentUrl is not null ||
        Width is not null ||
        Height is not null ||
        Caption is not null;
}
