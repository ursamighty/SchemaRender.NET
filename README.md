# SchemaRender

High-performance, developer-friendly ASP.NET Core library for adding Schema.org structured data (JSON-LD) to server-rendered pages.

[![NuGet](https://img.shields.io/nuget/v/SchemaRender.AspNetCore.svg)](https://www.nuget.org/packages/SchemaRender.AspNetCore/)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

## Features

- **Strongly Typed**: Write schemas with full IntelliSense and compile-time validation
- **High Performance**: Direct `Utf8JsonWriter` serialization with near-zero overhead
- **Source Generated**: Optional source generator eliminates boilerplate and reflection
- **Server-Side**: SEO-correct JSON-LD rendered in document `<head>`
- **ASP.NET Core Native**: Tag Helpers, HtmlHelper extensions, DI integration
- **Zero JavaScript**: Pure server-side rendering

## Quick Start

### Installation

```bash
# Core library + ASP.NET Core integration
dotnet add package SchemaRender.AspNetCore

# Optional: Source generator for automatic schema generation
dotnet add package SchemaRender.Generator
```

### Basic Setup

**1. Register services** in `Program.cs`:

```csharp
builder.Services.AddSchemaRender();
```

**2. Add Tag Helper** to `_ViewImports.cshtml`:

```razor
@addTagHelper *, SchemaRender.AspNetCore
```

**3. Render in layout** `_Layout.cshtml`:

```razor
<head>
    <schema-render />
</head>
```

**4. Add schemas to pages**:

```csharp
public class RecipePage : SchemaPageModel
{
    public void OnGet()
    {
        Schema.Add(new RecipeSchema
        {
            Name = "Best Lasagna Ever",
            CookTime = TimeSpan.FromMinutes(45),
            PrepTime = TimeSpan.FromMinutes(30),
            RecipeIngredient = ["pasta", "ricotta", "mozzarella", "sauce"]
        });
    }
}
```

## Usage Patterns

### Using Source Generator

Define schemas with attributes:

```csharp
[SchemaType("Recipe")]
public partial class RecipeSchema
{
    public required string Name { get; init; }
    public TimeSpan? CookTime { get; init; }
    public TimeSpan? PrepTime { get; init; }

    [SchemaProperty(NestedType = "Person")]
    public string? Author { get; init; }

    public IReadOnlyList<string>? RecipeIngredient { get; init; }
}
```

The generator creates optimized `ISchema` implementation automatically.

### Using Hand-Written Schemas

Implement `ISchema` directly:

```csharp
public sealed class ArticleSchema : ISchema
{
    public required string Headline { get; init; }
    public DateTimeOffset? DatePublished { get; init; }

    public void Write(Utf8JsonWriter w)
    {
        w.WriteStartObject();
        w.WriteString("@context", "https://schema.org");
        w.WriteString("@type", "Article");
        w.WriteString("headline", Headline);

        if (DatePublished is not null)
            w.WriteString("datePublished", DatePublished.Value.ToString("O"));

        w.WriteEndObject();
    }
}
```

### Inject in Pages or Controllers

```csharp
public class ProductPage : PageModel
{
    public void OnGet([FromServices] ISchemaContext schema)
    {
        schema.Add(new ProductSchema { Name = "Widget" });
    }
}
```

### Use HtmlHelper Extensions

```razor
@inject ISchemaContext Schema

@{
    Schema.Add(new ArticleSchema
    {
        Headline = "My Post",
        DatePublished = DateTimeOffset.Now
    });
}

<head>
    @Html.RenderSchemas()
</head>
```

## Built-in Schemas

The library includes hand-written implementations of common schemas:

- `RecipeSchema`
- `ArticleSchema`
- `OrganizationSchema`

Use these as examples or extend them for your needs.

## Source Generator Features

### Attributes

- `[SchemaType("TypeName")]` - Marks a partial class for generation
- `[SchemaProperty(Name = "jsonName", NestedType = "Person")]` - Customizes serialization
- `[SchemaIgnore]` - Excludes properties from output

### Supported Types

- Primitives: `string`, `bool`, `int`, `double`, `decimal`
- Dates: `DateTime`, `DateTimeOffset`, `DateOnly`, `TimeSpan`
- Collections: `IReadOnlyList<T>`, `List<T>`, arrays
- Nested: `ISchema` implementations
- `Uri`

### Special Handling

**Address Properties**: Automatically nested as `PostalAddress`
```csharp
public string? StreetAddress { get; init; }
public string? AddressLocality { get; init; }
// Generates: { "address": { "@type": "PostalAddress", ... } }
```

**Geo Coordinates**: Automatically nested as `GeoCoordinates`
```csharp
public double? Latitude { get; init; }
public double? Longitude { get; init; }
// Generates: { "geo": { "@type": "GeoCoordinates", ... } }
```

## Output Example

```json
{
  "@context": "https://schema.org",
  "@type": "LocalBusiness",
  "name": "Joe's Pizza",
  "telephone": "+1-555-123-4567",
  "priceRange": "$$",
  "address": {
    "@type": "PostalAddress",
    "streetAddress": "123 Main St",
    "addressLocality": "New York",
    "postalCode": "10001"
  },
  "geo": {
    "@type": "GeoCoordinates",
    "latitude": 40.7128,
    "longitude": -74.006
  }
}
```

## Performance

SchemaRender is designed for hot-path rendering:

- **No reflection** at runtime
- **No intermediate strings** or DOM manipulation
- **No JSON parsing** or serialization round-trips
- **No client-side JavaScript** required
- **Direct streaming** to response with `Utf8JsonWriter`

## Architecture

```
┌─────────────────────────────────────────┐
│           Your Razor Page               │
│  Schema.Add(new RecipeSchema {...})    │
└─────────────────┬───────────────────────┘
                  │
                  ▼
┌─────────────────────────────────────────┐
│      ISchemaContext (Scoped)            │
│    Collects schemas per request         │
└─────────────────┬───────────────────────┘
                  │
                  ▼
┌─────────────────────────────────────────┐
│         Layout <schema-render />        │
│   SchemaRenderer.Render(context)        │
└─────────────────┬───────────────────────┘
                  │
                  ▼
┌─────────────────────────────────────────┐
│         Utf8JsonWriter                  │
│    Direct byte[] → Response Stream      │
└─────────────────────────────────────────┘
```

## License

MIT

## Contributing

Contributions welcome! Please open an issue or PR.

## Support

- Report bugs: [GitHub Issues](https://github.com/yourusername/SchemaRender/issues)
- Documentation: [GitHub Wiki](https://github.com/yourusername/SchemaRender/wiki)
