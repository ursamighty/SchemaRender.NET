# SchemaRender.NET

An easy way to add [Schema.org](https://schema.org) structured data (JSON-LD) to your ASP.NET Core application.

[![NuGet](https://img.shields.io/nuget/v/SchemaRender.AspNetCore.svg)](https://www.nuget.org/packages/SchemaRender.AspNetCore/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/SchemaRender.AspNetCore.svg)](https://www.nuget.org/packages/SchemaRender.AspNetCore/)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE.txt)
[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)](https://dotnet.microsoft.com/)

## Why SchemaRender?

Adding structured data to your site improves SEO and how search engines display your content. SchemaRender makes this trivial:

- **Type-safe and intuitive** - Work with C# objects instead of manually writing JSON-LD
- **Seamless integration** - Add schemas directly in your page models and controllers
- **Zero boilerplate** - Just add schemas to the page, SchemaRender handles the rendering
- **Production-ready** - Includes 20+ common schemas, or generate custom ones with attributes
- **Optimized** - Source-generated code with no reflection and direct JSON streaming

## Quick Start

**1. Install the package:**

```bash
dotnet add package SchemaRender.AspNetCore
```

**2. Register in `Program.cs`:**

```csharp
builder.Services.AddSchemaRender();
```

**3. Add the tag helper to `_ViewImports.cshtml`:**

```razor
@addTagHelper *, SchemaRender.AspNetCore
```

**4. Render schemas in your layout's `<head>`:**

```razor
<schema-render />
```

**5. Add schemas in your pages:**

```csharp
public class RecipePage : PageModel
{
    public void OnGet([FromServices] ISchemaContext schema)
    {
        schema.Add(new RecipeSchema
        {
            Name = "Mom's Lasagna",
            CookTime = TimeSpan.FromMinutes(45),
            RecipeIngredient = ["pasta", "ricotta", "mozzarella"]
        });
    }
}
```

That's it! SchemaRender will automatically output the JSON-LD script tags.

## Usage Patterns

### Razor Pages

Inject `ISchemaContext` using `[FromServices]`:

```csharp
public class ArticlePage : PageModel
{
    public void OnGet([FromServices] ISchemaContext schema)
    {
        schema.Add(new ArticleSchema
        {
            Headline = "My Article",
            DatePublished = DateTimeOffset.Now,
            Author = new PersonSchema { Name = "Jane Doe" }
        });
    }
}
```

### MVC Controllers

Inject via controller constructor or action parameter:

```csharp
public class ProductController : Controller
{
    private readonly ISchemaContext _schema;

    public ProductController(ISchemaContext schema)
    {
        _schema = schema;
    }

    public IActionResult Details(int id)
    {
        var product = GetProduct(id);
        _schema.Add(new ProductSchema
        {
            Name = product.Name,
            Description = product.Description,
            Image = new ImageObjectSchema { Url = product.ImageUrl }
        });
        return View(product);
    }
}
```

### Razor Views

Use `@inject` to add schemas directly in views:

```razor
@inject ISchemaContext Schema
@model BlogPost

@{
    Schema.Add(new BlogPostingSchema
    {
        Headline = Model.Title,
        DatePublished = Model.PublishedDate,
        Author = new PersonSchema { Name = Model.AuthorName }
    });
}

<article>
    <h1>@Model.Title</h1>
    <!-- your content -->
</article>
```

### Generate Custom Schemas

Add the generator package to create schemas for types not included in the library:

```bash
dotnet add package SchemaRender.Generator
```

```csharp
[SchemaType("JobPosting")]
public partial class JobPostingSchema
{
    public required string Title { get; init; }
    public string? Description { get; init; }
    public DateTimeOffset? DatePosted { get; init; }
    public DateTimeOffset? ValidThrough { get; init; }

    [SchemaProperty(NestedType = "Organization")]
    public string? HiringOrganization { get; init; }

    public string? EmploymentType { get; init; }
    public string? JobLocation { get; init; }
}
```

The generator creates the `ISchema` implementation automatically, handling JSON serialization and Schema.org formatting.

## Source Generator

Install the optional generator package to create custom schemas:

```bash
dotnet add package SchemaRender.Generator
```

### Attributes

**`[SchemaType("TypeName")]`** - Marks a partial class for generation:

```csharp
[SchemaType("LocalBusiness")]
public partial class LocalBusinessSchema
{
    public required string Name { get; init; }
}
```

**`[SchemaProperty(Name = "...")]`** - Customizes the JSON property name:

```csharp
[SchemaProperty(Name = "dateCreated")]
public DateTimeOffset? CreatedAt { get; init; }
```

**`[SchemaIgnore]`** - Excludes a property from output:

```csharp
[SchemaIgnore]
public int InternalId { get; init; }
```

### Supported Types

The generator supports primitives (`string`, `bool`, `int`, `double`), dates (`DateTime`, `DateTimeOffset`, `DateOnly`, `TimeSpan`), collections (`IReadOnlyList<T>`, arrays), and nested `ISchema` implementations.

## Built-in Schemas

The library includes 20+ ready-to-use schemas:

`Article`, `BlogPosting`, `Recipe`, `Product`, `LocalBusiness`, `Event`, `FAQPage`, `HowTo`, `BreadcrumbList`, `Review`, `AggregateRating`, `Offer`, `Organization`, `Person`, `ImageObject`, `VideoObject`, `WebSite`, `Question`, `Answer`, `ListItem`, `PostalAddress`, `GeoCoordinates`, `HowToStep`

See the [Schema.org documentation](https://schema.org) for details on each type.


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
