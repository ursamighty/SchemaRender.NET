using System.Text.Json;

namespace SchemaRender.Schemas;

/// <summary>
/// Schema.org BreadcrumbList structured data.
/// See: https://schema.org/BreadcrumbList
/// </summary>
public sealed class BreadcrumbListSchema : ISchema
{
    /// <summary>
    /// The list of breadcrumb items.
    /// </summary>
    public required IReadOnlyList<ListItemSchema> ItemListElement { get; init; }

    /// <summary>
    /// The name of the breadcrumb list.
    /// </summary>
    public string? Name { get; init; }

    /// <inheritdoc />
    public void Write(Utf8JsonWriter w)
    {
        w.WriteStartObject();
        w.WriteString("@context", "https://schema.org");
        w.WriteString("@type", "BreadcrumbList");

        if (Name is not null)
            w.WriteString("name", Name);

        w.WritePropertyName("itemListElement");
        w.WriteStartArray();
        foreach (var item in ItemListElement)
        {
            item.Write(w);
        }
        w.WriteEndArray();

        w.WriteEndObject();
    }
}
