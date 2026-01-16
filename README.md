# SchemaRender.NET

A developer-friendly ASP.NET Core library for adding [Schema.org](https://schema.org) structured data (JSON-LD) to server-rendered pages.

[![NuGet](https://img.shields.io/nuget/v/SchemaRender.AspNetCore.svg)](https://www.nuget.org/packages/SchemaRender.AspNetCore/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/SchemaRender.AspNetCore.svg)](https://www.nuget.org/packages/SchemaRender.AspNetCore/)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE.txt)
[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)](https://dotnet.microsoft.com/)

## Installation

Install the ASP.NET Core integration package (includes the core library):

```bash
dotnet add package SchemaRender.AspNetCore
```

Optionally, add the source generator for custom schema generation:

```bash
dotnet add package SchemaRender.Generator
```

## Quick Start

### 1. Register Services

In your `Program.cs`:

```csharp
builder.Services.AddSchemaRender();
```

### 2. Add Tag Helper

In `_ViewImports.cshtml`:

```razor
@addTagHelper *, SchemaRender.AspNetCore
```

### 3. Render in Layout

In `_Layout.cshtml`:

```razor
<head>
    <!-- other head elements -->
    <schema-render />
</head>
```

### 4. Add Schemas to Pages

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

### Using the Source Generator

Define schemas with attributes and let the generator create the implementation:

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

The generator creates an optimized `ISchema` implementation automatically.

### Hand-Written Schemas

Implement `ISchema` directly for full control:

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

### Dependency Injection

Inject `ISchemaContext` directly in pages or controllers:

```csharp
public class ProductPage : PageModel
{
    public void OnGet([FromServices] ISchemaContext schema)
    {
        schema.Add(new ProductSchema { Name = "Widget" });
    }
}
```

### HtmlHelper Extensions

Use Razor syntax with the HtmlHelper extension:

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

The library includes ready-to-use implementations of the following schemas:

| Schema                  | Description (see Schema.org)                |
|-------------------------|---------------------------------------------|
| `AggregateRatingSchema` | Aggregate ratings (e.g., for products)      |
| `AnswerSchema`          | Answers (for FAQ, Q&A)                      |
| `ArticleSchema`         | Articles                                    |
| `BlogPostingSchema`     | Blog posts                                  |
| `BreadcrumbListSchema`  | Breadcrumb navigation lists                 |
| `EventSchema`           | Events                                      |
| `FAQPageSchema`         | FAQ pages                                   |
| `GeoCoordinatesSchema`  | Geographic coordinates                      |
| `HowToSchema`           | How-to guides                               |
| `HowToStepSchema`       | Steps in a how-to guide                     |
| `ImageObjectSchema`     | Images                                      |
| `ListItemSchema`        | Items in a list (e.g., breadcrumbs)         |
| `LocalBusinessSchema`   | Local businesses                            |
| `OfferSchema`           | Offers (e.g., for products, events)         |
| `OrganizationSchema`    | Organizations                               |
| `PersonSchema`          | People                                      |
| `PostalAddressSchema`   | Postal addresses                            |
| `ProductSchema`         | Products                                    |
| `QuestionSchema`        | Questions (for FAQ, Q&A)                    |
| `RecipeSchema`          | Recipes                                     |
| `ReviewSchema`          | Reviews                                     |
| `VideoObjectSchema`     | Videos                                      |
| `WebSiteSchema`         | Websites                                    |

## Source Generator Attributes

### `[SchemaType("TypeName")]`

Marks a partial class for schema generation:

```csharp
[SchemaType("LocalBusiness")]
public partial class LocalBusinessSchema { }
```

### `[SchemaProperty]`

Customizes property serialization:

```csharp
[SchemaProperty(Name = "dateCreated", NestedType = "Person")]
public string? Author { get; init; }
```

### `[SchemaIgnore]`

Excludes a property from the generated output:

```csharp
[SchemaIgnore]
public string? InternalId { get; init; }
```

## Supported Types

| Category | Types |
|----------|-------|
| Primitives | `string`, `bool`, `int`, `double`, `decimal` |
| Dates | `DateTime`, `DateTimeOffset`, `DateOnly`, `TimeSpan` |
| Collections | `IReadOnlyList<T>`, `List<T>`, arrays |
| References | `Uri`, `ISchema` implementations |

## Special Property Handling

### Address Properties

Properties like `StreetAddress`, `AddressLocality`, `PostalCode`, etc. are automatically nested as `PostalAddress`:

```csharp
public string? StreetAddress { get; init; }
public string? AddressLocality { get; init; }
public string? PostalCode { get; init; }
```

Generates:

```json
{
  "address": {
    "@type": "PostalAddress",
    "streetAddress": "123 Main St",
    "addressLocality": "New York",
    "postalCode": "10001"
  }
}
```

### Geo Coordinates

`Latitude` and `Longitude` properties are automatically nested as `GeoCoordinates`:

```csharp
public double? Latitude { get; init; }
public double? Longitude { get; init; }
```

Generates:

```json
{
  "geo": {
    "@type": "GeoCoordinates",
    "latitude": 40.7128,
    "longitude": -74.0060
  }
}
```

## Performance

SchemaRender is optimized for production use:

- **No reflection** at runtime (source-generated code)
- **No intermediate strings** or DOM manipulation
- **No JSON parsing** or serialization round-trips
- **Direct streaming** to response with `Utf8JsonWriter`
- **Zero allocations** in hot paths

## Architecture

```
Your Razor Page
  │
  │  Schema.Add(new RecipeSchema {...})
  ▼
ISchemaContext (Scoped per request)
  │
  │  Collects all schemas for the request
  ▼
<schema-render /> Tag Helper
  │
  │  SchemaRenderer.Render(context)
  ▼
Utf8JsonWriter
  │
  │  Direct byte[] → Response Stream
  ▼
<script type="application/ld+json">...</script>
```

## Requirements

- .NET 8.0 or later
- ASP.NET Core 8.0 or later

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE.txt](LICENSE.txt) file for details.

## Links

- [NuGet Package](https://www.nuget.org/packages/SchemaRender.AspNetCore/)
- [GitHub Repository](https://github.com/ursamighty/SchemaRender.NET)
- [Report Issues](https://github.com/ursamighty/SchemaRender.NET/issues)
- [Schema.org Documentation](https://schema.org)
