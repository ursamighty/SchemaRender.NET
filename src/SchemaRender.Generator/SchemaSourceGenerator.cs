using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SchemaRender.Generator;

/// <summary>
/// Incremental source generator that generates ISchema implementations for classes marked with [SchemaType].
/// </summary>
[Generator(LanguageNames.CSharp)]
public sealed class SchemaSourceGenerator : IIncrementalGenerator
{
    private const string SchemaTypeAttributeName = "SchemaRender.SchemaTypeAttribute";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Find all class declarations with [SchemaType] attribute
        var schemaClasses = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                SchemaTypeAttributeName,
                predicate: static (node, _) => node is ClassDeclarationSyntax,
                transform: static (context, ct) => GetSchemaModel(context, ct))
            .Where(static model => model is not null)
            .Select(static (model, _) => model!);

        // Generate source for each schema class
        context.RegisterSourceOutput(schemaClasses, static (context, model) =>
        {
            var source = SchemaEmitter.Emit(model);
            context.AddSource($"{model.FullTypeName}.g.cs", source);
        });
    }

    private static SchemaModel? GetSchemaModel(GeneratorAttributeSyntaxContext context, System.Threading.CancellationToken ct)
    {
        if (context.TargetSymbol is not INamedTypeSymbol classSymbol)
            return null;

        var classDeclaration = (ClassDeclarationSyntax)context.TargetNode;

        // Check if class is partial
        if (!classDeclaration.Modifiers.Any(m => m.Text == "partial"))
            return null;

        // Get the SchemaType attribute data
        var attributeData = context.Attributes.FirstOrDefault(a =>
            a.AttributeClass?.ToDisplayString() == SchemaTypeAttributeName);

        if (attributeData is null)
            return null;

        // Get type name from attribute constructor argument
        var typeName = attributeData.ConstructorArguments.FirstOrDefault().Value as string ?? classSymbol.Name;

        // Get sealed property from attribute
        var isSealed = true;
        foreach (var namedArg in attributeData.NamedArguments)
        {
            if (namedArg.Key == "Sealed" && namedArg.Value.Value is bool sealedValue)
            {
                isSealed = sealedValue;
            }
        }

        // Collect properties
        var properties = new List<SchemaPropertyModel>();
        foreach (var member in classSymbol.GetMembers())
        {
            if (member is not IPropertySymbol property)
                continue;

            // Skip properties without getters
            if (property.GetMethod is null)
                continue;

            // Skip properties with [SchemaIgnore]
            if (property.GetAttributes().Any(a => a.AttributeClass?.Name == "SchemaIgnoreAttribute"))
                continue;

            var propertyModel = GetPropertyModel(property);
            if (propertyModel is not null)
                properties.Add(propertyModel);
        }

        var ns = classSymbol.ContainingNamespace.IsGlobalNamespace
            ? null
            : classSymbol.ContainingNamespace.ToDisplayString();

        return new SchemaModel(
            ns,
            classSymbol.Name,
            typeName,
            isSealed,
            properties.ToImmutableArray());
    }

    private static SchemaPropertyModel? GetPropertyModel(IPropertySymbol property)
    {
        var type = property.Type;
        var propertyName = property.Name;
        var jsonName = ToCamelCase(propertyName);
        string? nestedType = null;
        var timeFormat = "Iso8601Duration";
        var dateFormat = "Iso8601";

        // Check for [SchemaProperty] attribute
        var schemaPropertyAttr = property.GetAttributes()
            .FirstOrDefault(a => a.AttributeClass?.Name == "SchemaPropertyAttribute");

        if (schemaPropertyAttr is not null)
        {
            // Get name from constructor or named argument
            if (schemaPropertyAttr.ConstructorArguments.Length > 0 &&
                schemaPropertyAttr.ConstructorArguments[0].Value is string name)
            {
                jsonName = name;
            }

            foreach (var namedArg in schemaPropertyAttr.NamedArguments)
            {
                switch (namedArg.Key)
                {
                    case "Name" when namedArg.Value.Value is string n:
                        jsonName = n;
                        break;
                    case "NestedType" when namedArg.Value.Value is string nt:
                        nestedType = nt;
                        break;
                    case "TimeFormat" when namedArg.Value.Value is int tf:
                        timeFormat = tf switch
                        {
                            0 => "Iso8601Duration",
                            1 => "TotalMinutes",
                            2 => "TotalSeconds",
                            _ => "Iso8601Duration"
                        };
                        break;
                    case "DateFormat" when namedArg.Value.Value is int df:
                        dateFormat = df switch
                        {
                            0 => "Iso8601",
                            1 => "DateOnly",
                            _ => "Iso8601"
                        };
                        break;
                }
            }
        }

        var (typeKind, elementType, isNullable) = AnalyzeType(type);

        // Check if property is required (C# 11 required modifier)
        var isRequired = property.IsRequired;

        return new SchemaPropertyModel(
            propertyName,
            jsonName,
            typeKind,
            elementType,
            isNullable,
            isRequired,
            nestedType,
            timeFormat,
            dateFormat,
            type.ToDisplayString());
    }

    private static (PropertyTypeKind Kind, string? ElementType, bool IsNullable) AnalyzeType(ITypeSymbol type)
    {
        var isNullable = false;

        // Handle nullable types
        if (type is INamedTypeSymbol namedType && namedType.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T)
        {
            isNullable = true;
            type = namedType.TypeArguments[0];
        }
        else if (type.NullableAnnotation == NullableAnnotation.Annotated)
        {
            isNullable = true;
            if (type is INamedTypeSymbol nt && nt.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T)
            {
                type = nt.TypeArguments[0];
            }
        }

        // Check for special types
        var typeString = type.ToDisplayString();

        if (type.SpecialType == SpecialType.System_String)
            return (PropertyTypeKind.String, null, isNullable);

        if (type.SpecialType == SpecialType.System_Boolean)
            return (PropertyTypeKind.Boolean, null, isNullable);

        if (type.SpecialType == SpecialType.System_Int32 ||
            type.SpecialType == SpecialType.System_Int64 ||
            type.SpecialType == SpecialType.System_Int16)
            return (PropertyTypeKind.Integer, null, isNullable);

        if (type.SpecialType == SpecialType.System_Double ||
            type.SpecialType == SpecialType.System_Single ||
            type.SpecialType == SpecialType.System_Decimal)
            return (PropertyTypeKind.Number, null, isNullable);

        if (typeString == "System.TimeSpan")
            return (PropertyTypeKind.TimeSpan, null, isNullable);

        if (typeString == "System.DateTime" || typeString == "System.DateTimeOffset")
            return (PropertyTypeKind.DateTime, null, isNullable);

        if (typeString == "System.DateOnly")
            return (PropertyTypeKind.DateOnly, null, isNullable);

        if (typeString == "System.Uri")
            return (PropertyTypeKind.Uri, null, isNullable);

        // Check for collections
        if (type is INamedTypeSymbol collectionType)
        {
            // Check for IReadOnlyList<T>, IList<T>, List<T>, IEnumerable<T>
            if (collectionType.IsGenericType)
            {
                var typeDef = collectionType.OriginalDefinition.ToDisplayString();
                if (typeDef.StartsWith("System.Collections.Generic.IReadOnlyList<") ||
                    typeDef.StartsWith("System.Collections.Generic.IList<") ||
                    typeDef.StartsWith("System.Collections.Generic.List<") ||
                    typeDef.StartsWith("System.Collections.Generic.IEnumerable<") ||
                    typeDef.StartsWith("System.Collections.Generic.ICollection<"))
                {
                    var elementType = collectionType.TypeArguments[0];
                    var (elementKind, _, _) = AnalyzeType(elementType);
                    return (PropertyTypeKind.Array, elementType.ToDisplayString(), isNullable);
                }
            }
        }

        // Check for array
        if (type is IArrayTypeSymbol arrayType)
        {
            return (PropertyTypeKind.Array, arrayType.ElementType.ToDisplayString(), isNullable);
        }

        // Check if it implements ISchema
        if (type.AllInterfaces.Any(i => i.ToDisplayString() == "SchemaRender.ISchema"))
            return (PropertyTypeKind.NestedSchema, type.ToDisplayString(), isNullable);

        // Default to object/unknown - serialize as string
        return (PropertyTypeKind.String, null, isNullable);
    }

    private static string ToCamelCase(string name)
    {
        if (string.IsNullOrEmpty(name))
            return name;

        if (char.IsLower(name[0]))
            return name;

        return char.ToLowerInvariant(name[0]) + name.Substring(1);
    }
}
