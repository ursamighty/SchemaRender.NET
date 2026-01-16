namespace SchemaRender;

/// <summary>
/// Customizes how a property is serialized in the generated JSON-LD output.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class SchemaPropertyAttribute : Attribute
{
    /// <summary>
    /// Creates a new SchemaProperty attribute.
    /// </summary>
    public SchemaPropertyAttribute()
    {
    }

    /// <summary>
    /// Creates a new SchemaProperty attribute with a custom JSON property name.
    /// </summary>
    /// <param name="name">The JSON property name to use instead of the C# property name.</param>
    public SchemaPropertyAttribute(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Gets or sets the JSON property name. If null, uses camelCase of the C# property name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the format for TimeSpan properties.
    /// Default is ISO 8601 duration format (e.g., "PT45M").
    /// </summary>
    public SchemaTimeFormat TimeFormat { get; set; } = SchemaTimeFormat.Iso8601Duration;

    /// <summary>
    /// Gets or sets the format for DateTime/DateTimeOffset properties.
    /// Default is ISO 8601 format.
    /// </summary>
    public SchemaDateFormat DateFormat { get; set; } = SchemaDateFormat.Iso8601;
}

/// <summary>
/// Specifies the format for TimeSpan serialization.
/// </summary>
public enum SchemaTimeFormat
{
    /// <summary>
    /// ISO 8601 duration format (e.g., "PT45M", "PT1H30M").
    /// </summary>
    Iso8601Duration,

    /// <summary>
    /// Total minutes as an integer.
    /// </summary>
    TotalMinutes,

    /// <summary>
    /// Total seconds as an integer.
    /// </summary>
    TotalSeconds
}

/// <summary>
/// Specifies the format for date/time serialization.
/// </summary>
public enum SchemaDateFormat
{
    /// <summary>
    /// Full ISO 8601 date-time format (e.g., "2024-01-15T10:30:00Z").
    /// </summary>
    Iso8601,

    /// <summary>
    /// Date only format (e.g., "2024-01-15").
    /// </summary>
    DateOnly
}
