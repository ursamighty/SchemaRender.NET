namespace SchemaRender;

/// <summary>
/// Excludes a property from Schema.org JSON-LD serialization.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class SchemaIgnoreAttribute : Attribute
{
}
