using SchemaRender;

namespace SchemaRender.Generator.Tests;

[SchemaType("Recipe")]
public partial class TestRecipeSchema
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public TimeSpan? CookTime { get; init; }
    public TimeSpan? PrepTime { get; init; }
    public IReadOnlyList<string>? Ingredients { get; init; }

    [SchemaProperty(NestedType = "Person")]
    public string? Author { get; init; }
}
