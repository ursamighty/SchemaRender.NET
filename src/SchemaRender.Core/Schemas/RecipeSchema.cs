using System.Text.Json;

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
    public string? Image { get; init; }

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
            w.WriteString("cookTime", FormatDuration(CookTime.Value));

        if (PrepTime is not null)
            w.WriteString("prepTime", FormatDuration(PrepTime.Value));

        if (TotalTime is not null)
            w.WriteString("totalTime", FormatDuration(TotalTime.Value));

        if (RecipeYield is not null)
            w.WriteString("recipeYield", RecipeYield);

        if (RecipeCategory is not null)
            w.WriteString("recipeCategory", RecipeCategory);

        if (RecipeCuisine is not null)
            w.WriteString("recipeCuisine", RecipeCuisine);

        if (Image is not null)
            w.WriteString("image", Image);

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

        w.WriteEndObject();
    }

    private static string FormatDuration(TimeSpan duration)
    {
        // ISO 8601 duration format
        if (duration.TotalHours >= 1)
            return $"PT{(int)duration.TotalHours}H{duration.Minutes}M";
        return $"PT{(int)duration.TotalMinutes}M";
    }
}
