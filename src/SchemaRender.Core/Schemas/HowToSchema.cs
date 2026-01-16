using System.Text.Json;
using SchemaRender.Helpers;

namespace SchemaRender.Schemas;

/// <summary>
/// Schema.org HowTo structured data.
/// See: https://schema.org/HowTo
/// </summary>
public sealed class HowToSchema : ISchema
{
    /// <summary>
    /// The title of the how-to guide.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// A description of the how-to guide.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// An image representing the how-to guide.
    /// </summary>
    public ImageObjectSchema? Image { get; init; }

    /// <summary>
    /// The list of steps to complete the how-to.
    /// </summary>
    public required IReadOnlyList<HowToStepSchema> Step { get; init; }

    /// <summary>
    /// The total time required to complete all steps.
    /// </summary>
    public TimeSpan? TotalTime { get; init; }

    /// <summary>
    /// The time required to prepare before starting the steps.
    /// </summary>
    public TimeSpan? PrepTime { get; init; }

    /// <summary>
    /// A list of supplies needed for the how-to.
    /// </summary>
    public IReadOnlyList<string>? Supply { get; init; }

    /// <summary>
    /// A list of tools needed for the how-to.
    /// </summary>
    public IReadOnlyList<string>? Tool { get; init; }

    /// <inheritdoc />
    public void Write(Utf8JsonWriter w)
    {
        w.WriteStartObject();
        w.WriteString("@context", "https://schema.org");
        w.WriteString("@type", "HowTo");
        w.WriteString("name", Name);

        if (Description is not null)
            w.WriteString("description", Description);

        if (Image is { HasValue: true })
        {
            w.WritePropertyName("image");
            Image.Write(w);
        }

        w.WritePropertyName("step");
        w.WriteStartArray();
        foreach (var step in Step)
        {
            step.Write(w);
        }
        w.WriteEndArray();

        if (TotalTime is not null)
            w.WriteString("totalTime", SchemaHelpers.FormatDuration(TotalTime.Value));

        if (PrepTime is not null)
            w.WriteString("prepTime", SchemaHelpers.FormatDuration(PrepTime.Value));

        if (Supply is { Count: > 0 })
        {
            w.WritePropertyName("supply");
            w.WriteStartArray();
            foreach (var supply in Supply)
                w.WriteStringValue(supply);
            w.WriteEndArray();
        }

        if (Tool is { Count: > 0 })
        {
            w.WritePropertyName("tool");
            w.WriteStartArray();
            foreach (var tool in Tool)
                w.WriteStringValue(tool);
            w.WriteEndArray();
        }

        w.WriteEndObject();
    }
}
