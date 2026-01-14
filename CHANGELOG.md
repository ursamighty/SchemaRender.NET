# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

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

[1.0.0]: https://github.com/yourusername/SchemaRender/releases/tag/v1.0.0
