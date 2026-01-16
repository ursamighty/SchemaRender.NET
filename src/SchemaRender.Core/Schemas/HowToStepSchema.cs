using System.Text.Json;

namespace SchemaRender.Schemas;

/// <summary>
/// Schema.org HowToStep structured data.
/// See: https://schema.org/HowToStep
/// </summary>
/// <remarks>
/// This schema is intended to be used as a nested object within HowTo schemas.
/// It does not include @context when serialized.
/// </remarks>
public sealed class HowToStepSchema : ISchema
{
    /// <summary>
    /// The instruction text for this step.
    /// </summary>
    public required string Text { get; init; }

    /// <summary>
    /// The name or title of the step.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// An image illustrating the step.
    /// </summary>
    public ImageObjectSchema? Image { get; init; }

    /// <summary>
    /// The URL of the step (for detailed instructions).
    /// </summary>
    public string? Url { get; init; }

    /// <inheritdoc />
    public void Write(Utf8JsonWriter w)
    {
        w.WriteStartObject();
        w.WriteString("@type", "HowToStep");
        w.WriteString("text", Text);

        if (Name is not null)
            w.WriteString("name", Name);

        if (Image is { HasValue: true })
        {
            w.WritePropertyName("image");
            Image.Write(w);
        }

        if (Url is not null)
            w.WriteString("url", Url);

        w.WriteEndObject();
    }
}
