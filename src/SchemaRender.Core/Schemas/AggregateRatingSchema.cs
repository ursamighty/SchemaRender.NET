using System.Text.Json;

namespace SchemaRender.Schemas;

/// <summary>
/// Schema.org AggregateRating structured data.
/// See: https://schema.org/AggregateRating
/// </summary>
/// <remarks>
/// This schema is intended to be used as a nested object within other schemas
/// (e.g., Product, LocalBusiness). It does not include @context when serialized.
/// </remarks>
public sealed class AggregateRatingSchema : ISchema
{
    /// <summary>
    /// The average rating value (e.g., 4.5).
    /// </summary>
    public double? RatingValue { get; init; }

    /// <summary>
    /// The total number of ratings.
    /// </summary>
    public int? RatingCount { get; init; }

    /// <summary>
    /// The total number of reviews.
    /// </summary>
    public int? ReviewCount { get; init; }

    /// <summary>
    /// The highest rating value allowed (e.g., 5).
    /// </summary>
    public int? BestRating { get; init; }

    /// <summary>
    /// The lowest rating value allowed (e.g., 1).
    /// </summary>
    public int? WorstRating { get; init; }

    /// <inheritdoc />
    public void Write(Utf8JsonWriter w)
    {
        w.WriteStartObject();
        w.WriteString("@type", "AggregateRating");

        if (RatingValue is not null)
            w.WriteNumber("ratingValue", RatingValue.Value);

        if (RatingCount is not null)
            w.WriteNumber("ratingCount", RatingCount.Value);

        if (ReviewCount is not null)
            w.WriteNumber("reviewCount", ReviewCount.Value);

        if (BestRating is not null)
            w.WriteNumber("bestRating", BestRating.Value);

        if (WorstRating is not null)
            w.WriteNumber("worstRating", WorstRating.Value);

        w.WriteEndObject();
    }

    /// <summary>
    /// Returns true if any rating property has a value.
    /// </summary>
    public bool HasValue =>
        RatingValue is not null ||
        RatingCount is not null ||
        ReviewCount is not null ||
        BestRating is not null ||
        WorstRating is not null;
}
