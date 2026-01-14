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
    public string? Image { get; init; }

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
    public string? Author { get; init; }

    /// <summary>
    /// The publisher of the article.
    /// </summary>
    public string? Publisher { get; init; }

    /// <summary>
    /// The publisher's logo URL.
    /// </summary>
    public string? PublisherLogo { get; init; }

    /// <summary>
    /// The main body of the article.
    /// </summary>
    public string? ArticleBody { get; init; }

    /// <summary>
    /// The URL of the article.
    /// </summary>
    public string? Url { get; init; }

    /// <inheritdoc />
    public void Write(Utf8JsonWriter w)
    {
        w.WriteStartObject();
        w.WriteString("@context", "https://schema.org");
        w.WriteString("@type", "Article");
        w.WriteString("headline", Headline);

        if (Description is not null)
            w.WriteString("description", Description);

        if (Image is not null)
            w.WriteString("image", Image);

        if (DatePublished is not null)
            w.WriteString("datePublished", DatePublished.Value.ToString("O"));

        if (DateModified is not null)
            w.WriteString("dateModified", DateModified.Value.ToString("O"));

        if (Author is not null)
        {
            w.WritePropertyName("author");
            w.WriteStartObject();
            w.WriteString("@type", "Person");
            w.WriteString("name", Author);
            w.WriteEndObject();
        }

        if (Publisher is not null)
        {
            w.WritePropertyName("publisher");
            w.WriteStartObject();
            w.WriteString("@type", "Organization");
            w.WriteString("name", Publisher);
            if (PublisherLogo is not null)
            {
                w.WritePropertyName("logo");
                w.WriteStartObject();
                w.WriteString("@type", "ImageObject");
                w.WriteString("url", PublisherLogo);
                w.WriteEndObject();
            }
            w.WriteEndObject();
        }

        if (ArticleBody is not null)
            w.WriteString("articleBody", ArticleBody);

        if (Url is not null)
            w.WriteString("url", Url);

        w.WriteEndObject();
    }
}
