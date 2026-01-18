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

    /// <summary>
    /// An alternative or secondary headline for the blog post.
    /// </summary>
    public string? AlternativeHeadline { get; init; }

    /// <summary>
    /// The language of the blog post content (e.g., "en-US", "es", "fr-FR").
    /// Uses IETF BCP 47 language codes.
    /// </summary>
    public string? InLanguage { get; init; }

    /// <summary>
    /// Keywords or tags associated with the blog post.
    /// </summary>
    public string? Keywords { get; init; }

    /// <summary>
    /// Indicates whether the blog post is accessible for free.
    /// </summary>
    public bool? IsAccessibleForFree { get; init; }

    /// <summary>
    /// The number of comments on the blog post.
    /// </summary>
    public int? CommentCount { get; init; }

    /// <summary>
    /// The aggregate rating for the blog post.
    /// </summary>
    public AggregateRatingSchema? AggregateRating { get; init; }

    /// <summary>
    /// A video associated with the blog post.
    /// </summary>
    public VideoObjectSchema? Video { get; init; }

    /// <summary>
    /// The canonical URL of the page. Indicates the main page for this blog post.
    /// </summary>
    public string? MainEntityOfPage { get; init; }

    /// <summary>
    /// The URL of a thumbnail image for the blog post.
    /// </summary>
    public string? ThumbnailUrl { get; init; }

    /// <summary>
    /// URLs to reference web pages that confirm the item's identity (e.g., social media profiles).
    /// </summary>
    public IReadOnlyList<string>? SameAs { get; init; }

    /// <summary>
    /// The year the blog post copyright was first asserted.
    /// </summary>
    public int? CopyrightYear { get; init; }

    /// <summary>
    /// A license document that applies to this blog post, typically indicated by URL.
    /// </summary>
    public string? License { get; init; }

    /// <summary>
    /// A CreativeWork such as an image, video, or audio clip shared as part of this blog posting.
    /// Specific to social media postings.
    /// </summary>
    public string? SharedContent { get; init; }

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

        if (SharedContent is not null)
            w.WriteString("sharedContent", SharedContent);

        w.WriteEndObject();
    }
}
