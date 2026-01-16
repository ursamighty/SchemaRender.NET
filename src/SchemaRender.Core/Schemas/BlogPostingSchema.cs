using System.Text.Json;

namespace SchemaRender.Schemas;

/// <summary>
/// Schema.org BlogPosting structured data.
/// See: https://schema.org/BlogPosting
/// </summary>
public sealed class BlogPostingSchema : ISchema
{
    /// <summary>
    /// The headline of the blog post.
    /// </summary>
    public required string Headline { get; init; }

    /// <summary>
    /// A brief description or summary of the blog post.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// The featured image of the blog post.
    /// </summary>
    public ImageObjectSchema? Image { get; init; }

    /// <summary>
    /// The date the blog post was published.
    /// </summary>
    public DateTimeOffset? DatePublished { get; init; }

    /// <summary>
    /// The date the blog post was last modified.
    /// </summary>
    public DateTimeOffset? DateModified { get; init; }

    /// <summary>
    /// The author of the blog post.
    /// </summary>
    public PersonSchema? Author { get; init; }

    /// <summary>
    /// The organization that published the blog post.
    /// </summary>
    public OrganizationSchema? Publisher { get; init; }

    /// <summary>
    /// The main content of the blog post.
    /// </summary>
    public string? ArticleBody { get; init; }

    /// <summary>
    /// The URL of the blog post.
    /// </summary>
    public string? Url { get; init; }

    /// <summary>
    /// The section or category of the blog post.
    /// </summary>
    public string? ArticleSection { get; init; }

    /// <summary>
    /// The word count of the blog post.
    /// </summary>
    public int? WordCount { get; init; }

    /// <inheritdoc />
    public void Write(Utf8JsonWriter w)
    {
        w.WriteStartObject();
        w.WriteString("@context", "https://schema.org");
        w.WriteString("@type", "BlogPosting");
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
            Publisher.Write(w);
        }

        if (ArticleBody is not null)
            w.WriteString("articleBody", ArticleBody);

        if (Url is not null)
            w.WriteString("url", Url);

        if (ArticleSection is not null)
            w.WriteString("articleSection", ArticleSection);

        if (WordCount is not null)
            w.WriteNumber("wordCount", WordCount.Value);

        w.WriteEndObject();
    }
}
