using System.Text.Json;

namespace SchemaRender.Schemas;

/// <summary>
/// Schema.org ListItem structured data.
/// See: https://schema.org/ListItem
/// </summary>
/// <remarks>
/// This schema is intended to be used as a nested object within BreadcrumbList schemas.
/// It does not include @context when serialized.
/// </remarks>
public sealed class ListItemSchema : ISchema
{
    /// <summary>
    /// The position of the item in the list (1-based).
    /// </summary>
    public required int Position { get; init; }

    /// <summary>
    /// The name or display text of the breadcrumb item.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The URL of the item.
    /// </summary>
    public required string Item { get; init; }

    /// <inheritdoc />
    public void Write(Utf8JsonWriter w)
    {
        w.WriteStartObject();
        w.WriteString("@type", "ListItem");
        w.WriteNumber("position", Position);
        w.WriteString("name", Name);
        w.WriteString("item", Item);
        w.WriteEndObject();
    }
}
