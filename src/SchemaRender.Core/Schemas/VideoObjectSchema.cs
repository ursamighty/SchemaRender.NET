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

        w.WriteEndObject();
    }
}
