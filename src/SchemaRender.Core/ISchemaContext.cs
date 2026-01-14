namespace SchemaRender;

/// <summary>
/// A request-scoped context for collecting Schema.org structured data.
/// Schemas added during request processing are rendered together in the layout.
/// </summary>
public interface ISchemaContext
{
    /// <summary>
    /// Adds a schema to be rendered in the current request.
    /// </summary>
    /// <param name="schema">The schema to add.</param>
    void Add(ISchema schema);

    /// <summary>
    /// Gets all schemas that have been added to this context.
    /// </summary>
    IReadOnlyList<ISchema> Schemas { get; }

    /// <summary>
    /// Gets whether any schemas have been added to this context.
    /// </summary>
    bool HasSchemas { get; }
}
