using System.Text.Json;

namespace SchemaRender.Schemas;

/// <summary>
/// Schema.org Product structured data.
/// See: https://schema.org/Product
/// </summary>
public sealed class ProductSchema : ISchema
{
    /// <summary>
    /// The name of the product.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// A description of the product.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// The primary image of the product.
    /// </summary>
    public ImageObjectSchema? Image { get; init; }

    /// <summary>
    /// Additional images of the product.
    /// </summary>
    public IReadOnlyList<ImageObjectSchema>? Images { get; init; }

    /// <summary>
    /// The brand of the product.
    /// </summary>
    public string? Brand { get; init; }

    /// <summary>
    /// The Stock Keeping Unit (SKU) of the product.
    /// </summary>
    public string? Sku { get; init; }

    /// <summary>
    /// The Global Trade Item Number (GTIN) of the product.
    /// </summary>
    public string? Gtin { get; init; }

    /// <summary>
    /// The Manufacturer Part Number (MPN) of the product.
    /// </summary>
    public string? Mpn { get; init; }

    /// <summary>
    /// The category of the product.
    /// </summary>
    public string? Category { get; init; }

    /// <summary>
    /// The aggregate rating for the product.
    /// </summary>
    public AggregateRatingSchema? AggregateRating { get; init; }

    /// <summary>
    /// Reviews of the product.
    /// </summary>
    public IReadOnlyList<ReviewSchema>? Review { get; init; }

    /// <summary>
    /// Offers to sell the product.
    /// </summary>
    public IReadOnlyList<OfferSchema>? Offers { get; init; }

    /// <inheritdoc />
    public void Write(Utf8JsonWriter w)
    {
        w.WriteStartObject();
        w.WriteString("@context", "https://schema.org");
        w.WriteString("@type", "Product");
        w.WriteString("name", Name);

        if (Description is not null)
            w.WriteString("description", Description);

        if (Image is { HasValue: true })
        {
            w.WritePropertyName("image");
            Image.Write(w);
        }

        if (Images is { Count: > 0 })
        {
            w.WritePropertyName("image");
            w.WriteStartArray();
            foreach (var image in Images)
            {
                if (image.HasValue)
                    image.Write(w);
            }
            w.WriteEndArray();
        }

        if (Brand is not null)
            w.WriteString("brand", Brand);

        if (Sku is not null)
            w.WriteString("sku", Sku);

        if (Gtin is not null)
            w.WriteString("gtin", Gtin);

        if (Mpn is not null)
            w.WriteString("mpn", Mpn);

        if (Category is not null)
            w.WriteString("category", Category);

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

        if (Offers is { Count: > 0 })
        {
            w.WritePropertyName("offers");
            w.WriteStartArray();
            foreach (var offer in Offers)
            {
                if (offer.HasValue)
                    offer.Write(w);
            }
            w.WriteEndArray();
        }

        w.WriteEndObject();
    }
}
