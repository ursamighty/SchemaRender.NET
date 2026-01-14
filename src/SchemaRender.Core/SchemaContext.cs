namespace SchemaRender;

/// <summary>
/// Default implementation of <see cref="ISchemaContext"/> that collects schemas during request execution.
/// This class is registered as scoped and should be injected into pages and the layout.
/// </summary>
public sealed class SchemaContext : ISchemaContext
{
    private readonly List<ISchema> schemas = [];

    /// <inheritdoc />
    public void Add(ISchema schema)
    {
        ArgumentNullException.ThrowIfNull(schema);
        this.schemas.Add(schema);
    }

    /// <inheritdoc />
    public IReadOnlyList<ISchema> Schemas => this.schemas;

    /// <inheritdoc />
    public bool HasSchemas => this.schemas.Count > 0;
}
