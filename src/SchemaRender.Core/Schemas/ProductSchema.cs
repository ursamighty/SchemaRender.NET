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

    /// <summary>
    /// The URL of the product page.
    /// </summary>
    public string? Url { get; init; }

    /// <summary>
    /// A global identifier for the product (e.g., ISBN).
    /// </summary>
    public string? ProductID { get; init; }

    /// <summary>
    /// The color of the product (e.g., "Red", "Blue").
    /// </summary>
    public string? Color { get; init; }

    /// <summary>
    /// The size of the product (e.g., "M", "Large", "10", "32x34").
    /// </summary>
    public string? Size { get; init; }

    /// <summary>
    /// The weight of the product (e.g., "1.5 kg", "3 lbs").
    /// </summary>
    public string? Weight { get; init; }

    /// <summary>
    /// The height of the product (e.g., "30 cm", "12 in").
    /// </summary>
    public string? Height { get; init; }

    /// <summary>
    /// The width of the product (e.g., "20 cm", "8 in").
    /// </summary>
    public string? Width { get; init; }

    /// <summary>
    /// The depth of the product (e.g., "10 cm", "4 in").
    /// </summary>
    public string? Depth { get; init; }

    /// <summary>
    /// The material(s) the product is made from (e.g., "Cotton", "Stainless Steel").
    /// </summary>
    public string? Material { get; init; }

    /// <summary>
    /// A pattern featured on the product (e.g., "Striped", "Polka Dot").
    /// </summary>
    public string? Pattern { get; init; }

    /// <summary>
    /// The manufacturer of the product.
    /// </summary>
    public string? Manufacturer { get; init; }

    /// <summary>
    /// The condition of the product (e.g., "NewCondition", "UsedCondition", "RefurbishedCondition").
    /// </summary>
    public string? ItemCondition { get; init; }

    /// <summary>
    /// Keywords or tags associated with the product.
    /// </summary>
    public string? Keywords { get; init; }

    /// <summary>
    /// The model of the product.
    /// </summary>
    public string? Model { get; init; }

    /// <summary>
    /// The release date of the product.
    /// </summary>
    public DateTimeOffset? ReleaseDate { get; init; }

    /// <summary>
    /// The logo of the product.
    /// </summary>
    public ImageObjectSchema? Logo { get; init; }

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

        if (Url is not null)
            w.WriteString("url", Url);

        if (ProductID is not null)
            w.WriteString("productID", ProductID);

        if (Color is not null)
            w.WriteString("color", Color);

        if (Size is not null)
            w.WriteString("size", Size);

        if (Weight is not null)
            w.WriteString("weight", Weight);

        if (Height is not null)
            w.WriteString("height", Height);

        if (Width is not null)
            w.WriteString("width", Width);

        if (Depth is not null)
            w.WriteString("depth", Depth);

        if (Material is not null)
            w.WriteString("material", Material);

        if (Pattern is not null)
            w.WriteString("pattern", Pattern);

        if (Manufacturer is not null)
            w.WriteString("manufacturer", Manufacturer);

        if (ItemCondition is not null)
            w.WriteString("itemCondition", ItemCondition);

        if (Keywords is not null)
            w.WriteString("keywords", Keywords);

        if (Model is not null)
            w.WriteString("model", Model);

        if (ReleaseDate is not null)
            w.WriteString("releaseDate", ReleaseDate.Value.ToString("O"));

        if (Logo is { HasValue: true })
        {
            w.WritePropertyName("logo");
            Logo.Write(w);
        }

        w.WriteEndObject();
    }
}
