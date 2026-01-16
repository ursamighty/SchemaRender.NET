# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.0.0] - 2026-01-16

### Changed

- **BREAKING:** Replaced "magic" auto-nesting with explicit schema composition
  - Address properties (StreetAddress, AddressLocality, etc.) are no longer auto-grouped into nested PostalAddress
  - Geo coordinates (Latitude, Longitude) are no longer auto-grouped into nested GeoCoordinates
  - Use explicit `PostalAddressSchema` and `GeoCoordinatesSchema` types instead

- **BREAKING:** Removed `NestedType` property from `[SchemaProperty]` attribute
  - Use explicit schema types (e.g., `PersonSchema`) instead of `[SchemaProperty(NestedType = "Person")]`

- **BREAKING:** Updated built-in schemas to use explicit composition:
  - `OrganizationSchema.Address` is now `PostalAddressSchema?` (was 5 separate string properties)
  - `RecipeSchema.Author` is now `PersonSchema?` (was `string?`)
  - `ArticleSchema.Author` is now `PersonSchema?` (was `string?`)
  - `ArticleSchema.Publisher` is now `OrganizationSchema?` (was `string?` + `string? PublisherLogo`)

### Added

- `PostalAddressSchema` - explicit type for postal address structured data
- `GeoCoordinatesSchema` - explicit type for geographic coordinates
- `PersonSchema` - explicit type for person structured data

### Migration Guide

**Before (1.x):**
```csharp
// Magic auto-nesting based on property names
public partial class MyBusinessSchema
{
    public string? StreetAddress { get; init; }
    public string? AddressLocality { get; init; }
    public double? Latitude { get; init; }
    public double? Longitude { get; init; }

    [SchemaProperty(NestedType = "Person")]
    public string? Author { get; init; }
}
```

**After (2.x):**
```csharp
// Explicit composition with real types
public partial class MyBusinessSchema
{
    public PostalAddressSchema? Address { get; init; }
    public GeoCoordinatesSchema? Geo { get; init; }
    public PersonSchema? Author { get; init; }
}

// Usage:
var business = new MyBusinessSchema
{
    Address = new PostalAddressSchema
    {
        StreetAddress = "123 Main St",
        AddressLocality = "New York"
    },
    Geo = new GeoCoordinatesSchema
    {
        Latitude = 40.7128,
        Longitude = -74.0060
    },
    Author = new PersonSchema { Name = "John Doe" }
};
```

## [1.0.0] - 2026-01-14

### Added
- Core library (`SchemaRender.Core`)
  - `ISchema` interface for strongly-typed schemas
  - `ISchemaContext` for request-scoped schema collection
  - `SchemaRenderer` with `Utf8JsonWriter`-based serialization
  - Built-in schemas: `RecipeSchema`, `ArticleSchema`, `OrganizationSchema`

- ASP.NET Core integration (`SchemaRender.AspNetCore`)
  - `<schema-render />` Tag Helper for layouts
  - `@Html.RenderSchemas()` HtmlHelper extension
  - `SchemaPageModel` base class for Razor Pages
  - `SchemaController` base class for MVC controllers
  - `HttpContext` extension methods

- Source Generator (`SchemaRender.Generator`)
  - `[SchemaType]` attribute for marking partial classes
  - `[SchemaProperty]` attribute for customization
  - `[SchemaIgnore]` attribute for excluding properties
  - Automatic nesting of address properties as `PostalAddress`
  - Automatic nesting of geo coordinates as `GeoCoordinates`
  - Support for primitives, dates, collections, and nested schemas
  - ISO 8601 duration formatting for `TimeSpan`

### Performance
- Zero reflection at runtime
- Direct streaming with `Utf8JsonWriter`
- No intermediate string allocations
- Near-zero overhead JSON-LD generation

[1.0.0]: https://github.com/ursamighty/SchemaRender.NET/releases/tag/v1.0.0
