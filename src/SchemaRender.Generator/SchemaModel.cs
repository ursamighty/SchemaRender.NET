using System.Collections.Immutable;

namespace SchemaRender.Generator;

/// <summary>
/// Model representing a schema class to be generated.
/// </summary>
internal sealed class SchemaModel
{
    public SchemaModel(
        string? ns,
        string className,
        string schemaTypeName,
        bool isSealed,
        ImmutableArray<SchemaPropertyModel> properties)
    {
        Namespace = ns;
        ClassName = className;
        SchemaTypeName = schemaTypeName;
        IsSealed = isSealed;
        Properties = properties;
    }

    public string? Namespace { get; }
    public string ClassName { get; }
    public string SchemaTypeName { get; }
    public bool IsSealed { get; }
    public ImmutableArray<SchemaPropertyModel> Properties { get; }

    public string FullTypeName => Namespace is null ? ClassName : $"{Namespace}.{ClassName}";
}

/// <summary>
/// Model representing a property to be serialized.
/// </summary>
internal sealed class SchemaPropertyModel
{
    public SchemaPropertyModel(
        string propertyName,
        string jsonName,
        PropertyTypeKind typeKind,
        string? elementType,
        bool isNullable,
        bool isRequired,
        string? nestedType,
        string timeFormat,
        string dateFormat,
        string typeFullName)
    {
        PropertyName = propertyName;
        JsonName = jsonName;
        TypeKind = typeKind;
        ElementType = elementType;
        IsNullable = isNullable;
        IsRequired = isRequired;
        NestedType = nestedType;
        TimeFormat = timeFormat;
        DateFormat = dateFormat;
        TypeFullName = typeFullName;
    }

    public string PropertyName { get; }
    public string JsonName { get; }
    public PropertyTypeKind TypeKind { get; }
    public string? ElementType { get; }
    public bool IsNullable { get; }
    public bool IsRequired { get; }
    public string? NestedType { get; }
    public string TimeFormat { get; }
    public string DateFormat { get; }
    public string TypeFullName { get; }
}

/// <summary>
/// The kind of property type for code generation.
/// </summary>
internal enum PropertyTypeKind
{
    String,
    Boolean,
    Integer,
    Number,
    TimeSpan,
    DateTime,
    DateOnly,
    Uri,
    Array,
    NestedSchema
}
