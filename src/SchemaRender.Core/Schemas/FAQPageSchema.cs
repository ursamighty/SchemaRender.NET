using System.Text.Json;

namespace SchemaRender.Schemas;

/// <summary>
/// Schema.org FAQPage structured data.
/// See: https://schema.org/FAQPage
/// </summary>
public sealed class FAQPageSchema : ISchema
{
    /// <summary>
    /// The list of questions and answers on the FAQ page.
    /// </summary>
    public required IReadOnlyList<QuestionSchema> MainEntity { get; init; }

    /// <summary>
    /// The name of the FAQ page.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// A description of the FAQ page.
    /// </summary>
    public string? Description { get; init; }

    /// <inheritdoc />
    public void Write(Utf8JsonWriter w)
    {
        w.WriteStartObject();
        w.WriteString("@context", "https://schema.org");
        w.WriteString("@type", "FAQPage");

        if (Name is not null)
            w.WriteString("name", Name);

        if (Description is not null)
            w.WriteString("description", Description);

        w.WritePropertyName("mainEntity");
        w.WriteStartArray();
        foreach (var question in MainEntity)
        {
            question.Write(w);
        }
        w.WriteEndArray();

        w.WriteEndObject();
    }
}
