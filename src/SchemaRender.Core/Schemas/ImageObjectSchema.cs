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

    /// <summary>
    /// The name or title of the image.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// A description of the image content.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// The author or creator of the image.
    /// </summary>
    public string? Author { get; init; }

    /// <summary>
    /// The date the image was published.
    /// </summary>
    public DateTimeOffset? DatePublished { get; init; }

    /// <summary>
    /// The encoding format of the image (e.g., "image/jpeg", "image/png").
    /// </summary>
    public string? EncodingFormat { get; init; }

    /// <summary>
    /// Indicates whether this image is representative of the entire page.
    /// </summary>
    public bool? RepresentativeOfPage { get; init; }

    /// <summary>
    /// The date the image was uploaded.
    /// </summary>
    public DateTimeOffset? UploadDate { get; init; }

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

        if (Name is not null)
            w.WriteString("name", Name);

        if (Description is not null)
            w.WriteString("description", Description);

        if (Author is not null)
            w.WriteString("author", Author);

        if (DatePublished is not null)
            w.WriteString("datePublished", DatePublished.Value.ToString("O"));

        if (EncodingFormat is not null)
            w.WriteString("encodingFormat", EncodingFormat);

        if (RepresentativeOfPage is not null)
            w.WriteBoolean("representativeOfPage", RepresentativeOfPage.Value);

        if (UploadDate is not null)
            w.WriteString("uploadDate", UploadDate.Value.ToString("O"));

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
        Caption is not null ||
        Name is not null ||
        Description is not null ||
        Author is not null ||
        DatePublished is not null ||
        EncodingFormat is not null ||
        RepresentativeOfPage is not null ||
        UploadDate is not null;
}
