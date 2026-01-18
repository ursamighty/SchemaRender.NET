using System.Text.Json;

namespace SchemaRender.Schemas;

/// <summary>
/// Schema.org NutritionInformation structured data.
/// See: https://schema.org/NutritionInformation
/// </summary>
/// <remarks>
/// This schema is intended to be used as a nested object within Recipe schemas.
/// It does not include @context when serialized.
/// </remarks>
public sealed class NutritionInformationSchema : ISchema
{
    /// <summary>
    /// The number of calories (e.g., "250 calories" or "250").
    /// </summary>
    public string? Calories { get; init; }

    /// <summary>
    /// The number of grams of carbohydrates (e.g., "30 g" or "30").
    /// </summary>
    public string? CarbohydrateContent { get; init; }

    /// <summary>
    /// The number of milligrams of cholesterol (e.g., "50 mg" or "50").
    /// </summary>
    public string? CholesterolContent { get; init; }

    /// <summary>
    /// The number of grams of fat (e.g., "10 g" or "10").
    /// </summary>
    public string? FatContent { get; init; }

    /// <summary>
    /// The number of grams of fiber (e.g., "5 g" or "5").
    /// </summary>
    public string? FiberContent { get; init; }

    /// <summary>
    /// The number of grams of protein (e.g., "20 g" or "20").
    /// </summary>
    public string? ProteinContent { get; init; }

    /// <summary>
    /// The number of grams of saturated fat (e.g., "3 g" or "3").
    /// </summary>
    public string? SaturatedFatContent { get; init; }

    /// <summary>
    /// The serving size (e.g., "1 cup", "200 g").
    /// </summary>
    public string? ServingSize { get; init; }

    /// <summary>
    /// The number of milligrams of sodium (e.g., "500 mg" or "500").
    /// </summary>
    public string? SodiumContent { get; init; }

    /// <summary>
    /// The number of grams of sugar (e.g., "15 g" or "15").
    /// </summary>
    public string? SugarContent { get; init; }

    /// <summary>
    /// The number of grams of trans fat (e.g., "0 g" or "0").
    /// </summary>
    public string? TransFatContent { get; init; }

    /// <summary>
    /// The number of grams of unsaturated fat (e.g., "7 g" or "7").
    /// </summary>
    public string? UnsaturatedFatContent { get; init; }

    /// <inheritdoc />
    public void Write(Utf8JsonWriter w)
    {
        w.WriteStartObject();
        w.WriteString("@type", "NutritionInformation");

        if (Calories is not null)
            w.WriteString("calories", Calories);

        if (CarbohydrateContent is not null)
            w.WriteString("carbohydrateContent", CarbohydrateContent);

        if (CholesterolContent is not null)
            w.WriteString("cholesterolContent", CholesterolContent);

        if (FatContent is not null)
            w.WriteString("fatContent", FatContent);

        if (FiberContent is not null)
            w.WriteString("fiberContent", FiberContent);

        if (ProteinContent is not null)
            w.WriteString("proteinContent", ProteinContent);

        if (SaturatedFatContent is not null)
            w.WriteString("saturatedFatContent", SaturatedFatContent);

        if (ServingSize is not null)
            w.WriteString("servingSize", ServingSize);

        if (SodiumContent is not null)
            w.WriteString("sodiumContent", SodiumContent);

        if (SugarContent is not null)
            w.WriteString("sugarContent", SugarContent);

        if (TransFatContent is not null)
            w.WriteString("transFatContent", TransFatContent);

        if (UnsaturatedFatContent is not null)
            w.WriteString("unsaturatedFatContent", UnsaturatedFatContent);

        w.WriteEndObject();
    }

    /// <summary>
    /// Returns true if any nutrition property has a value.
    /// </summary>
    public bool HasValue =>
        Calories is not null ||
        CarbohydrateContent is not null ||
        CholesterolContent is not null ||
        FatContent is not null ||
        FiberContent is not null ||
        ProteinContent is not null ||
        SaturatedFatContent is not null ||
        ServingSize is not null ||
        SodiumContent is not null ||
        SugarContent is not null ||
        TransFatContent is not null ||
        UnsaturatedFatContent is not null;
}
