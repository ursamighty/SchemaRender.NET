using System.Text.Json;
using SchemaRender.Helpers;

namespace SchemaRender.Schemas;

/// <summary>
/// Schema.org VideoObject structured data.
/// See: https://schema.org/VideoObject
/// </summary>
public sealed class VideoObjectSchema : ISchema
{
    /// <summary>
    /// The title of the video.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The direct URL to the video content file.
    /// </summary>
    public required string ContentUrl { get; init; }

    /// <summary>
    /// A description of the video.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// The URL of the video thumbnail image.
    /// </summary>
    public string? ThumbnailUrl { get; init; }

    /// <summary>
    /// The date the video was uploaded.
    /// </summary>
    public DateTimeOffset? UploadDate { get; init; }

    /// <summary>
    /// The duration of the video.
    /// </summary>
    public TimeSpan? Duration { get; init; }

    /// <summary>
    /// The URL of the video embed player.
    /// </summary>
    public string? EmbedUrl { get; init; }

    /// <summary>
    /// The transcript of the video.
    /// </summary>
    public string? Transcript { get; init; }

    /// <summary>
    /// The encoding format of the video (e.g., "video/mp4", "video/webm").
    /// </summary>
    public string? EncodingFormat { get; init; }

    /// <summary>
    /// The author or creator of the video.
    /// </summary>
    public string? Author { get; init; }

    /// <summary>
    /// Indicates whether access to the video requires a subscription.
    /// </summary>
    public bool? RequiresSubscription { get; init; }

    /// <summary>
    /// The official rating of the video (e.g., "PG-13", "TV-MA").
    /// </summary>
    public string? ContentRating { get; init; }

    /// <summary>
    /// The width of the video in pixels.
    /// </summary>
    public int? Width { get; init; }

    /// <summary>
    /// The height of the video in pixels.
    /// </summary>
    public int? Height { get; init; }

    /// <summary>
    /// An image object representing the video thumbnail.
    /// </summary>
    public ImageObjectSchema? Thumbnail { get; init; }

    /// <inheritdoc />
    public void Write(Utf8JsonWriter w)
    {
        w.WriteStartObject();
        w.WriteString("@context", "https://schema.org");
        w.WriteString("@type", "VideoObject");
        w.WriteString("name", Name);
        w.WriteString("contentUrl", ContentUrl);

        if (Description is not null)
            w.WriteString("description", Description);

        if (ThumbnailUrl is not null)
            w.WriteString("thumbnailUrl", ThumbnailUrl);

        if (UploadDate is not null)
            w.WriteString("uploadDate", UploadDate.Value.ToString("O"));

        if (Duration is not null)
            w.WriteString("duration", SchemaHelpers.FormatDuration(Duration.Value));

        if (EmbedUrl is not null)
            w.WriteString("embedUrl", EmbedUrl);

        if (Transcript is not null)
            w.WriteString("transcript", Transcript);

        if (EncodingFormat is not null)
            w.WriteString("encodingFormat", EncodingFormat);

        if (Author is not null)
            w.WriteString("author", Author);

        if (RequiresSubscription is not null)
            w.WriteBoolean("requiresSubscription", RequiresSubscription.Value);

        if (ContentRating is not null)
            w.WriteString("contentRating", ContentRating);

        if (Width is not null)
            w.WriteNumber("width", Width.Value);

        if (Height is not null)
            w.WriteNumber("height", Height.Value);

        if (Thumbnail is { HasValue: true })
        {
            w.WritePropertyName("thumbnail");
            Thumbnail.Write(w);
        }

        w.WriteEndObject();
    }
}
