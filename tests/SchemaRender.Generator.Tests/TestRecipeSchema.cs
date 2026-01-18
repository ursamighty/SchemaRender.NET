using SchemaRender;
using SchemaRender.Schemas;

namespace SchemaRender.Generator.Tests;

// This class is used to test the source generator
// The generator should create an ISchema implementation for this class
[SchemaType("Recipe")]
public partial class TestRecipeSchema
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public TimeSpan? CookTime { get; init; }
    public TimeSpan? PrepTime { get; init; }
    public IReadOnlyList<string>? Ingredients { get; init; }

    public PersonSchema? Author { get; init; }

    [SchemaIgnore]
    public string? InternalNote { get; init; }
}
