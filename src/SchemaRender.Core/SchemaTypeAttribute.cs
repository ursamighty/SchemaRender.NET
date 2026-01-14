namespace SchemaRender;

/// <summary>
/// Marks a partial class as a Schema.org type for source generation.
/// The generator will implement <see cref="ISchema"/> and generate the Write method.
/// </summary>
/// <example>
/// <code>
/// [SchemaType("Recipe")]
/// public partial class RecipeSchema
/// {
///     public required string Name { get; init; }
///     public TimeSpan? CookTime { get; init; }
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class SchemaTypeAttribute : Attribute
{
    /// <summary>
    /// Creates a new SchemaType attribute.
    /// </summary>
    /// <param name="typeName">The Schema.org type name (e.g., "Recipe", "Article", "Organization").</param>
    public SchemaTypeAttribute(string typeName)
    {
        TypeName = typeName;
    }

    /// <summary>
    /// Gets the Schema.org type name.
    /// </summary>
    public string TypeName { get; }

    /// <summary>
    /// Gets or sets whether to generate the class as sealed. Default is true.
    /// </summary>
    public bool Sealed { get; set; } = true;
}
