using System.Text.Json;

namespace SchemaRender.Schemas;

/// <summary>
/// Schema.org Answer structured data.
/// See: https://schema.org/Answer
/// </summary>
/// <remarks>
/// This schema is intended to be used as a nested object within Question schemas.
/// It does not include @context when serialized.
/// </remarks>
public sealed class AnswerSchema : ISchema
{
    /// <summary>
    /// The text of the answer.
    /// </summary>
    public required string Text { get; init; }

    /// <summary>
    /// The author of the answer.
    /// </summary>
    public string? Author { get; init; }

    /// <summary>
    /// The date the answer was created.
    /// </summary>
    public DateTimeOffset? DateCreated { get; init; }

    /// <inheritdoc />
    public void Write(Utf8JsonWriter w)
    {
        w.WriteStartObject();
        w.WriteString("@type", "Answer");
        w.WriteString("text", Text);

        if (Author is not null)
            w.WriteString("author", Author);

        if (DateCreated is not null)
            w.WriteString("dateCreated", DateCreated.Value.ToString("O"));

        w.WriteEndObject();
    }
}
