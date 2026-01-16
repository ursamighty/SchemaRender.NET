using System.Text.Json;

namespace SchemaRender.Schemas;

/// <summary>
/// Schema.org Review structured data.
/// See: https://schema.org/Review
/// </summary>
/// <remarks>
/// This schema can be used both as a top-level schema (with @context) or as a nested
/// object within other schemas like Product. Use the includeContext parameter in Write()
/// to control this behavior.
/// </remarks>
public sealed class ReviewSchema : ISchema
{
    /// <summary>
    /// The actual body of the review.
    /// </summary>
    public required string ReviewBody { get; init; }

    /// <summary>
    /// The rating given in this review (typically 1-5).
    /// </summary>
    public int? ReviewRating { get; init; }

    /// <summary>
    /// The author of the review (name or Person schema).
    /// </summary>
    public string? Author { get; init; }

    /// <summary>
    /// The date the review was published.
    /// </summary>
    public DateTimeOffset? DatePublished { get; init; }

    /// <summary>
    /// The name of the item being reviewed.
    /// </summary>
    public string? ItemReviewed { get; init; }

    /// <inheritdoc />
    public void Write(Utf8JsonWriter w)
    {
        Write(w, includeContext: true);
    }

    /// <summary>
    /// Writes the review schema to the JSON writer.
    /// </summary>
    /// <param name="w">The JSON writer.</param>
    /// <param name="includeContext">Whether to include the @context property (true for top-level, false for nested).</param>
    public void Write(Utf8JsonWriter w, bool includeContext)
    {
        w.WriteStartObject();

        if (includeContext)
            w.WriteString("@context", "https://schema.org");

        w.WriteString("@type", "Review");
        w.WriteString("reviewBody", ReviewBody);

        if (ReviewRating is not null)
        {
            w.WritePropertyName("reviewRating");
            w.WriteStartObject();
            w.WriteString("@type", "Rating");
            w.WriteNumber("ratingValue", ReviewRating.Value);
            w.WriteEndObject();
        }

        if (Author is not null)
            w.WriteString("author", Author);

        if (DatePublished is not null)
            w.WriteString("datePublished", DatePublished.Value.ToString("O"));

        if (ItemReviewed is not null)
            w.WriteString("itemReviewed", ItemReviewed);

        w.WriteEndObject();
    }
}
