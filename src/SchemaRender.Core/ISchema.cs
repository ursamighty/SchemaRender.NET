using System.Text.Json;

namespace SchemaRender;

/// <summary>
/// Represents a Schema.org structured data object that can write itself as JSON-LD.
/// </summary>
public interface ISchema
{
    /// <summary>
    /// Writes this schema as JSON-LD to the specified writer.
    /// The implementation must write a complete JSON object including @context and @type.
    /// </summary>
    /// <param name="writer">The UTF-8 JSON writer to write to.</param>
    void Write(Utf8JsonWriter writer);
}
