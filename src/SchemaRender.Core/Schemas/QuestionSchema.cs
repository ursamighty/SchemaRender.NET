using System.Text.Json;

namespace SchemaRender.Schemas;

/// <summary>
/// Schema.org Question structured data.
/// See: https://schema.org/Question
/// </summary>
/// <remarks>
/// This schema is intended to be used as a nested object within FAQPage schemas.
/// It does not include @context when serialized.
/// </remarks>
public sealed class QuestionSchema : ISchema
{
    /// <summary>
    /// The text of the question.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The accepted answer(s) to the question.
    /// </summary>
    public IReadOnlyList<AnswerSchema>? AcceptedAnswer { get; init; }

    /// <summary>
    /// Suggested answer(s) to the question.
    /// </summary>
    public IReadOnlyList<AnswerSchema>? SuggestedAnswer { get; init; }

    /// <inheritdoc />
    public void Write(Utf8JsonWriter w)
    {
        w.WriteStartObject();
        w.WriteString("@type", "Question");
        w.WriteString("name", Name);

        if (AcceptedAnswer is { Count: > 0 })
        {
            w.WritePropertyName("acceptedAnswer");
            w.WriteStartArray();
            foreach (var answer in AcceptedAnswer)
            {
                answer.Write(w);
            }
            w.WriteEndArray();
        }

        if (SuggestedAnswer is { Count: > 0 })
        {
            w.WritePropertyName("suggestedAnswer");
            w.WriteStartArray();
            foreach (var answer in SuggestedAnswer)
            {
                answer.Write(w);
            }
            w.WriteEndArray();
        }

        w.WriteEndObject();
    }
}
