using System.Text.Json;
using SchemaRender.Helpers;

namespace SchemaRender.Schemas;

/// <summary>
/// Schema.org Recipe structured data.
/// See: https://schema.org/Recipe
/// </summary>
public sealed class RecipeSchema : ISchema
{
    /// <summary>
    /// The name of the recipe.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// A description of the recipe.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// The time it takes to actually cook the dish.
    /// </summary>
    public TimeSpan? CookTime { get; init; }

    /// <summary>
    /// The length of time it takes to prepare the items to be used.
    /// </summary>
    public TimeSpan? PrepTime { get; init; }

    /// <summary>
    /// The total time required to perform the recipe.
    /// </summary>
    public TimeSpan? TotalTime { get; init; }

    /// <summary>
    /// The quantity that results from the recipe.
    /// </summary>
    public string? RecipeYield { get; init; }

    /// <summary>
    /// The category of the recipe (e.g., "dessert", "dinner").
    /// </summary>
    public string? RecipeCategory { get; init; }

    /// <summary>
    /// The cuisine of the recipe (e.g., "Italian", "Mexican").
    /// </summary>
    public string? RecipeCuisine { get; init; }

    /// <summary>
    /// An image of the completed recipe.
    /// </summary>
    public ImageObjectSchema? Image { get; init; }

    /// <summary>
    /// The author of the recipe.
    /// </summary>
    public PersonSchema? Author { get; init; }

    /// <summary>
    /// A list of ingredients used in the recipe.
    /// </summary>
    public IReadOnlyList<string>? RecipeIngredient { get; init; }

    /// <summary>
    /// A list of instructions for preparing the recipe.
    /// </summary>
    public IReadOnlyList<string>? RecipeInstructions { get; init; }

    /// <summary>
    /// The cooking method used (e.g., "frying", "steaming", "baking").
    /// </summary>
    public string? CookingMethod { get; init; }

    /// <summary>
    /// Nutrition information for the recipe.
    /// </summary>
    public NutritionInformationSchema? Nutrition { get; init; }

    /// <summary>
    /// Indicates a dietary restriction or guideline for which this recipe is suitable
    /// (e.g., "VeganDiet", "GlutenFreeDiet", "LowFatDiet").
    /// </summary>
    public string? SuitableForDiet { get; init; }

    /// <summary>
    /// The time it takes to actually perform the instructions.
    /// </summary>
    public TimeSpan? PerformTime { get; init; }

    /// <summary>
    /// A sub-property of instrument. A supply consumed when performing instructions.
    /// </summary>
    public IReadOnlyList<string>? Supply { get; init; }

    /// <summary>
    /// A sub-property of instrument. An object used (but not consumed) when performing instructions.
    /// </summary>
    public IReadOnlyList<string>? Tool { get; init; }

    /// <summary>
    /// The estimated cost of the supplies for the recipe.
    /// </summary>
    public string? EstimatedCost { get; init; }

    /// <summary>
    /// Keywords or tags associated with the recipe.
    /// </summary>
    public string? Keywords { get; init; }

    /// <summary>
    /// The aggregate rating for the recipe.
    /// </summary>
    public AggregateRatingSchema? AggregateRating { get; init; }

    /// <summary>
    /// A video demonstrating the recipe.
    /// </summary>
    public VideoObjectSchema? Video { get; init; }

    /// <summary>
    /// The date the recipe was published.
    /// </summary>
    public DateTimeOffset? DatePublished { get; init; }

    /// <summary>
    /// The date the recipe was last modified.
    /// </summary>
    public DateTimeOffset? DateModified { get; init; }

    /// <inheritdoc />
    public void Write(Utf8JsonWriter w)
    {
        w.WriteStartObject();
        w.WriteString("@context", "https://schema.org");
        w.WriteString("@type", "Recipe");
        w.WriteString("name", Name);

        if (Description is not null)
            w.WriteString("description", Description);

        if (CookTime is not null)
            w.WriteString("cookTime", SchemaHelpers.FormatDuration(CookTime.Value));

        if (PrepTime is not null)
            w.WriteString("prepTime", SchemaHelpers.FormatDuration(PrepTime.Value));

        if (TotalTime is not null)
            w.WriteString("totalTime", SchemaHelpers.FormatDuration(TotalTime.Value));

        if (RecipeYield is not null)
            w.WriteString("recipeYield", RecipeYield);

        if (RecipeCategory is not null)
            w.WriteString("recipeCategory", RecipeCategory);

        if (RecipeCuisine is not null)
            w.WriteString("recipeCuisine", RecipeCuisine);

        if (Image is { HasValue: true })
        {
            w.WritePropertyName("image");
            Image.Write(w);
        }

        if (Author is not null)
        {
            w.WritePropertyName("author");
            Author.Write(w);
        }

        if (RecipeIngredient is { Count: > 0 })
        {
            w.WritePropertyName("recipeIngredient");
            w.WriteStartArray();
            foreach (var ingredient in RecipeIngredient)
                w.WriteStringValue(ingredient);
            w.WriteEndArray();
        }

        if (RecipeInstructions is { Count: > 0 })
        {
            w.WritePropertyName("recipeInstructions");
            w.WriteStartArray();
            foreach (var instruction in RecipeInstructions)
            {
                w.WriteStartObject();
                w.WriteString("@type", "HowToStep");
                w.WriteString("text", instruction);
                w.WriteEndObject();
            }
            w.WriteEndArray();
        }

        if (CookingMethod is not null)
            w.WriteString("cookingMethod", CookingMethod);

        if (Nutrition is { HasValue: true })
        {
            w.WritePropertyName("nutrition");
            Nutrition.Write(w);
        }

        if (SuitableForDiet is not null)
            w.WriteString("suitableForDiet", SuitableForDiet);

        if (PerformTime is not null)
            w.WriteString("performTime", SchemaHelpers.FormatDuration(PerformTime.Value));

        if (Supply is { Count: > 0 })
        {
            w.WritePropertyName("supply");
            w.WriteStartArray();
            foreach (var supply in Supply)
                w.WriteStringValue(supply);
            w.WriteEndArray();
        }

        if (Tool is { Count: > 0 })
        {
            w.WritePropertyName("tool");
            w.WriteStartArray();
            foreach (var tool in Tool)
                w.WriteStringValue(tool);
            w.WriteEndArray();
        }

        if (EstimatedCost is not null)
            w.WriteString("estimatedCost", EstimatedCost);

        if (Keywords is not null)
            w.WriteString("keywords", Keywords);

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

        if (DatePublished is not null)
            w.WriteString("datePublished", DatePublished.Value.ToString("O"));

        if (DateModified is not null)
            w.WriteString("dateModified", DateModified.Value.ToString("O"));

        w.WriteEndObject();
    }

}
