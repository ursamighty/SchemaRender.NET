using System.Text.Json;

namespace SchemaRender.Schemas;

/// <summary>
/// Schema.org LocalBusiness structured data.
/// See: https://schema.org/LocalBusiness
/// </summary>
public sealed class LocalBusinessSchema : ISchema
{
    /// <summary>
    /// The name of the business.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// A description of the business.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// The URL of the business website.
    /// </summary>
    public string? Url { get; init; }

    /// <summary>
    /// An image representing the business (logo or photo).
    /// </summary>
    public ImageObjectSchema? Image { get; init; }

    /// <summary>
    /// The telephone number of the business.
    /// </summary>
    public string? Telephone { get; init; }

    /// <summary>
    /// The email address of the business.
    /// </summary>
    public string? Email { get; init; }

    /// <summary>
    /// The price range of the business (e.g., "$", "$$", "$$$").
    /// </summary>
    public string? PriceRange { get; init; }

    /// <summary>
    /// The physical address of the business.
    /// </summary>
    public PostalAddressSchema? Address { get; init; }

    /// <summary>
    /// The geographic coordinates of the business.
    /// </summary>
    public GeoCoordinatesSchema? Geo { get; init; }

    /// <summary>
    /// The opening hours of the business (e.g., "Mo-Fr 09:00-17:00").
    /// </summary>
    public IReadOnlyList<string>? OpeningHours { get; init; }

    /// <summary>
    /// The currencies accepted by the business (e.g., "USD, EUR").
    /// </summary>
    public string? CurrenciesAccepted { get; init; }

    /// <summary>
    /// The payment methods accepted (e.g., "Cash, Credit Card").
    /// </summary>
    public string? PaymentAccepted { get; init; }

    /// <summary>
    /// The type of cuisine served (for restaurants).
    /// </summary>
    public string? ServesCuisine { get; init; }

    /// <summary>
    /// URLs of social media profiles or other related pages.
    /// </summary>
    public IReadOnlyList<string>? SameAs { get; init; }

    /// <summary>
    /// The aggregate rating for the business.
    /// </summary>
    public AggregateRatingSchema? AggregateRating { get; init; }

    /// <summary>
    /// Reviews of the business.
    /// </summary>
    public IReadOnlyList<ReviewSchema>? Review { get; init; }

    /// <summary>
    /// The geographic area where the business provides service.
    /// </summary>
    public string? AreaServed { get; init; }

    /// <summary>
    /// The logo of the business.
    /// </summary>
    public ImageObjectSchema? Logo { get; init; }

    /// <summary>
    /// A short textual code that uniquely identifies a place of business (e.g., branch code).
    /// </summary>
    public string? BranchCode { get; init; }

    /// <inheritdoc />
    public void Write(Utf8JsonWriter w)
    {
        w.WriteStartObject();
        w.WriteString("@context", "https://schema.org");
        w.WriteString("@type", "LocalBusiness");
        w.WriteString("name", Name);

        if (Description is not null)
            w.WriteString("description", Description);

        if (Url is not null)
            w.WriteString("url", Url);

        if (Image is { HasValue: true })
        {
            w.WritePropertyName("image");
            Image.Write(w);
        }

        if (Telephone is not null)
            w.WriteString("telephone", Telephone);

        if (Email is not null)
            w.WriteString("email", Email);

        if (PriceRange is not null)
            w.WriteString("priceRange", PriceRange);

        if (Address is { HasValue: true })
        {
            w.WritePropertyName("address");
            Address.Write(w);
        }

        if (Geo is not null)
        {
            w.WritePropertyName("geo");
            Geo.Write(w);
        }

        if (OpeningHours is { Count: > 0 })
        {
            w.WritePropertyName("openingHours");
            w.WriteStartArray();
            foreach (var hours in OpeningHours)
                w.WriteStringValue(hours);
            w.WriteEndArray();
        }

        if (CurrenciesAccepted is not null)
            w.WriteString("currenciesAccepted", CurrenciesAccepted);

        if (PaymentAccepted is not null)
            w.WriteString("paymentAccepted", PaymentAccepted);

        if (ServesCuisine is not null)
            w.WriteString("servesCuisine", ServesCuisine);

        if (SameAs is { Count: > 0 })
        {
            w.WritePropertyName("sameAs");
            w.WriteStartArray();
            foreach (var url in SameAs)
                w.WriteStringValue(url);
            w.WriteEndArray();
        }

        if (AggregateRating is { HasValue: true })
        {
            w.WritePropertyName("aggregateRating");
            AggregateRating.Write(w);
        }

        if (Review is { Count: > 0 })
        {
            w.WritePropertyName("review");
            w.WriteStartArray();
            foreach (var review in Review)
            {
                review.Write(w, includeContext: false);
            }
            w.WriteEndArray();
        }

        if (AreaServed is not null)
            w.WriteString("areaServed", AreaServed);

        if (Logo is { HasValue: true })
        {
            w.WritePropertyName("logo");
            Logo.Write(w);
        }

        if (BranchCode is not null)
            w.WriteString("branchCode", BranchCode);

        w.WriteEndObject();
    }
}
