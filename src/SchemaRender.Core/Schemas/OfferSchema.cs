using System.Text.Json;

namespace SchemaRender.Schemas;

/// <summary>
/// Schema.org Offer structured data.
/// See: https://schema.org/Offer
/// </summary>
/// <remarks>
/// This schema is intended to be used as a nested object within other schemas
/// (e.g., Product, Event). It does not include @context when serialized.
/// </remarks>
public sealed class OfferSchema : ISchema
{
    /// <summary>
    /// The offer price.
    /// </summary>
    public decimal? Price { get; init; }

    /// <summary>
    /// The currency of the price (ISO 4217 currency code, e.g., "USD", "EUR").
    /// </summary>
    public string? PriceCurrency { get; init; }

    /// <summary>
    /// The availability of the item (e.g., "https://schema.org/InStock", "InStock").
    /// </summary>
    public string? Availability { get; init; }

    /// <summary>
    /// The URL where the offer can be purchased.
    /// </summary>
    public string? Url { get; init; }

    /// <summary>
    /// The date and time when the offer becomes valid.
    /// </summary>
    public DateTimeOffset? ValidFrom { get; init; }

    /// <summary>
    /// The date and time after which the offer is no longer valid.
    /// </summary>
    public DateTimeOffset? ValidThrough { get; init; }

    /// <summary>
    /// The organization offering the item.
    /// </summary>
    public OrganizationSchema? Seller { get; init; }

    /// <summary>
    /// The condition of the item (e.g., "https://schema.org/NewCondition", "NewCondition").
    /// </summary>
    public string? ItemCondition { get; init; }

    /// <summary>
    /// The date after which the price is no longer available.
    /// </summary>
    public DateTimeOffset? PriceValidUntil { get; init; }

    /// <summary>
    /// The item being offered (e.g., product name).
    /// </summary>
    public string? ItemOffered { get; init; }

    /// <summary>
    /// The Stock Keeping Unit (SKU) of the item being offered.
    /// </summary>
    public string? Sku { get; init; }

    /// <summary>
    /// The Global Trade Item Number (GTIN) of the item being offered.
    /// </summary>
    public string? Gtin { get; init; }

    /// <summary>
    /// The geographic area where the offer is eligible (e.g., "US", "California").
    /// </summary>
    public string? EligibleRegion { get; init; }

    /// <inheritdoc />
    public void Write(Utf8JsonWriter w)
    {
        w.WriteStartObject();
        w.WriteString("@type", "Offer");

        if (Price is not null)
            w.WriteNumber("price", Price.Value);

        if (PriceCurrency is not null)
            w.WriteString("priceCurrency", PriceCurrency);

        if (Availability is not null)
            w.WriteString("availability", Availability);

        if (Url is not null)
            w.WriteString("url", Url);

        if (ValidFrom is not null)
            w.WriteString("validFrom", ValidFrom.Value.ToString("O"));

        if (ValidThrough is not null)
            w.WriteString("validThrough", ValidThrough.Value.ToString("O"));

        if (Seller is not null)
        {
            w.WritePropertyName("seller");
            Seller.Write(w);
        }

        if (ItemCondition is not null)
            w.WriteString("itemCondition", ItemCondition);

        if (PriceValidUntil is not null)
            w.WriteString("priceValidUntil", PriceValidUntil.Value.ToString("O"));

        if (ItemOffered is not null)
            w.WriteString("itemOffered", ItemOffered);

        if (Sku is not null)
            w.WriteString("sku", Sku);

        if (Gtin is not null)
            w.WriteString("gtin", Gtin);

        if (EligibleRegion is not null)
            w.WriteString("eligibleRegion", EligibleRegion);

        w.WriteEndObject();
    }

    /// <summary>
    /// Returns true if any offer property has a value.
    /// </summary>
    public bool HasValue =>
        Price is not null ||
        PriceCurrency is not null ||
        Availability is not null ||
        Url is not null ||
        ValidFrom is not null ||
        ValidThrough is not null ||
        Seller is not null ||
        ItemCondition is not null ||
        PriceValidUntil is not null ||
        ItemOffered is not null ||
        Sku is not null ||
        Gtin is not null ||
        EligibleRegion is not null;
}
