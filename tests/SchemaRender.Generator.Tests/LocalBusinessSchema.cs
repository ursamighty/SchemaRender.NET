using SchemaRender;
using SchemaRender.Schemas;

namespace SchemaRender.Generator.Tests;

/// <summary>
/// Schema.org LocalBusiness structured data.
/// See: https://schema.org/LocalBusiness
/// </summary>
[SchemaType("LocalBusiness")]
public partial class LocalBusinessSchema
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
    /// An image or logo representing the business.
    /// </summary>
    public string? Image { get; init; }

    /// <summary>
    /// The telephone number.
    /// </summary>
    public string? Telephone { get; init; }

    /// <summary>
    /// The email address.
    /// </summary>
    public string? Email { get; init; }

    /// <summary>
    /// The price range of the business (e.g., "$", "$$", "$$$", "$$$$").
    /// </summary>
    public string? PriceRange { get; init; }

    /// <summary>
    /// The postal address of the business.
    /// </summary>
    public PostalAddressSchema? Address { get; init; }

    /// <summary>
    /// The geographic coordinates of the business.
    /// </summary>
    public GeoCoordinatesSchema? Geo { get; init; }

    /// <summary>
    /// The opening hours (e.g., "Mo-Sa 11:00-14:30", "Mo-Th 09:00-17:00").
    /// </summary>
    public IReadOnlyList<string>? OpeningHoursSpecification { get; init; }

    /// <summary>
    /// The currencies accepted in ISO 4217 format (e.g., "USD", "EUR").
    /// </summary>
    public string? CurrenciesAccepted { get; init; }

    /// <summary>
    /// The payment methods accepted (e.g., "Cash, Credit Card").
    /// </summary>
    public string? PaymentAccepted { get; init; }

    /// <summary>
    /// Whether the business serves cuisine (for restaurants).
    /// </summary>
    public string? ServesCuisine { get; init; }

    /// <summary>
    /// Social media profile URLs and other identifiers.
    /// </summary>
    public IReadOnlyList<string>? SameAs { get; init; }
}
