using System.Text.Json;

namespace SchemaRender.Schemas;

/// <summary>
/// Schema.org Article structured data.
/// See: https://schema.org/Article
/// </summary>
public sealed class ArticleSchema : ISchema
{
    /// <summary>
    /// The headline of the article.
    /// </summary>
    public required string Headline { get; init; }

    /// <summary>
    /// A description of the article.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// The main image for the article.
    /// </summary>
    public ImageObjectSchema? Image { get; init; }

    /// <summary>
    /// The date and time the article was published.
    /// </summary>
    public DateTimeOffset? DatePublished { get; init; }

    /// <summary>
    /// The date and time the article was last modified.
    /// </summary>
    public DateTimeOffset? DateModified { get; init; }

    /// <summary>
    /// The author of the article.
    /// </summary>
    public PersonSchema? Author { get; init; }

    /// <summary>
    /// The publisher of the article.
    /// </summary>
    public OrganizationSchema? Publisher { get; init; }

    /// <summary>
    /// The main body of the article.
    /// </summary>
    public string? ArticleBody { get; init; }

    /// <summary>
    /// The URL of the article.
    /// </summary>
    public string? Url { get; init; }

    /// <summary>
    /// The section or category of the article (e.g., "Technology", "Sports", "Lifestyle").
    /// </summary>
    public string? ArticleSection { get; init; }

    /// <summary>
    /// An alternative or secondary headline for the article.
    /// </summary>
    public string? AlternativeHeadline { get; init; }

    /// <summary>
    /// The language of the article content (e.g., "en-US", "es", "fr-FR").
    /// Uses IETF BCP 47 language codes.
    /// </summary>
    public string? InLanguage { get; init; }

    /// <summary>
    /// Keywords or tags associated with the article.
    /// </summary>
    public string? Keywords { get; init; }

    /// <summary>
    /// Indicates whether the article is accessible for free.
    /// </summary>
    public bool? IsAccessibleForFree { get; init; }

    /// <summary>
    /// The number of comments on the article.
    /// </summary>
    public int? CommentCount { get; init; }

    /// <summary>
    /// The number of words in the article.
    /// </summary>
    public int? WordCount { get; init; }

    /// <summary>
    /// The aggregate rating for the article.
    /// </summary>
    public AggregateRatingSchema? AggregateRating { get; init; }

    /// <summary>
    /// A video associated with the article.
    /// </summary>
    public VideoObjectSchema? Video { get; init; }

    /// <summary>
    /// The canonical URL of the page. Indicates the main page for this article.
    /// </summary>
    public string? MainEntityOfPage { get; init; }

    /// <summary>
    /// The URL of a thumbnail image for the article.
    /// </summary>
    public string? ThumbnailUrl { get; init; }

    /// <summary>
    /// URLs to reference web pages that confirm the item's identity (e.g., Wikipedia page, social media profiles).
    /// </summary>
    public IReadOnlyList<string>? SameAs { get; init; }

    /// <summary>
    /// The year the article copyright was first asserted.
    /// </summary>
    public int? CopyrightYear { get; init; }

    /// <summary>
    /// A license document that applies to this article, typically indicated by URL.
    /// </summary>
    public string? License { get; init; }

    /// <inheritdoc />
    public void Write(Utf8JsonWriter w)
    {
        w.WriteStartObject();
        w.WriteString("@context", "https://schema.org");
        w.WriteString("@type", "Article");
        w.WriteString("headline", Headline);

        if (Description is not null)
            w.WriteString("description", Description);

        if (Image is { HasValue: true })
        {
            w.WritePropertyName("image");
            Image.Write(w);
        }

        if (DatePublished is not null)
            w.WriteString("datePublished", DatePublished.Value.ToString("O"));

        if (DateModified is not null)
            w.WriteString("dateModified", DateModified.Value.ToString("O"));

        if (Author is not null)
        {
            w.WritePropertyName("author");
            Author.Write(w);
        }

        if (Publisher is not null)
        {
            w.WritePropertyName("publisher");
            WritePublisher(w, Publisher);
        }

        if (ArticleBody is not null)
            w.WriteString("articleBody", ArticleBody);

        if (Url is not null)
            w.WriteString("url", Url);

        if (ArticleSection is not null)
            w.WriteString("articleSection", ArticleSection);

        if (AlternativeHeadline is not null)
            w.WriteString("alternativeHeadline", AlternativeHeadline);

        if (InLanguage is not null)
            w.WriteString("inLanguage", InLanguage);

        if (Keywords is not null)
            w.WriteString("keywords", Keywords);

        if (IsAccessibleForFree is not null)
            w.WriteBoolean("isAccessibleForFree", IsAccessibleForFree.Value);

        if (CommentCount is not null)
            w.WriteNumber("commentCount", CommentCount.Value);

        if (WordCount is not null)
            w.WriteNumber("wordCount", WordCount.Value);

        if (AggregateRating is { HasValue: true })
        {
            w.WritePropertyName("aggregateRating");
            AggregateRating.Write(w);
        }

        if (Video is not null)
        {
            w.WritePropertyName("video");
            Video.Write(w);
        }

        if (MainEntityOfPage is not null)
            w.WriteString("mainEntityOfPage", MainEntityOfPage);

        if (ThumbnailUrl is not null)
            w.WriteString("thumbnailUrl", ThumbnailUrl);

        if (SameAs is { Count: > 0 })
        {
            w.WritePropertyName("sameAs");
            w.WriteStartArray();
            foreach (var url in SameAs)
                w.WriteStringValue(url);
            w.WriteEndArray();
        }

        if (CopyrightYear is not null)
            w.WriteNumber("copyrightYear", CopyrightYear.Value);

        if (License is not null)
            w.WriteString("license", License);

        w.WriteEndObject();
    }

    private static void WritePublisher(Utf8JsonWriter w, OrganizationSchema publisher)
    {
        // Write publisher as nested Organization (without @context)
        w.WriteStartObject();
        w.WriteString("@type", "Organization");
        w.WriteString("name", publisher.Name);

        if (publisher.Url is not null)
            w.WriteString("url", publisher.Url);

        if (publisher.Logo is { HasValue: true })
        {
            w.WritePropertyName("logo");
            publisher.Logo.Write(w);
        }

        w.WriteEndObject();
    }
}
