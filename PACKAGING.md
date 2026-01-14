# NuGet Packaging Guide

This guide explains how to package and publish SchemaRender to NuGet.

## Prerequisites

1. **NuGet Account**: Create an account at [nuget.org](https://www.nuget.org/)
2. **API Key**: Generate an API key from your NuGet account settings
3. **.NET SDK**: Ensure you have .NET 8.0 SDK installed

## Before Packaging

### 1. Update Version Numbers

Update the `<Version>` in all three project files:
- `src/SchemaRender.Core/SchemaRender.Core.csproj`
- `src/SchemaRender.Generator/SchemaRender.Generator.csproj`
- `src/SchemaRender.AspNetCore/SchemaRender.AspNetCore.csproj`

Follow [Semantic Versioning](https://semver.org/):
- **Major** (1.0.0): Breaking changes
- **Minor** (1.1.0): New features, backwards compatible
- **Patch** (1.0.1): Bug fixes, backwards compatible

### 2. Update Package Metadata

Edit the following in each `.csproj` file:
- `<Authors>`: Your name or organization
- `<Company>`: Your company name (optional)
- `<PackageProjectUrl>`: Your GitHub repository URL
- `<RepositoryUrl>`: Your GitHub repository URL

### 3. Update CHANGELOG.md

Document all changes in `CHANGELOG.md` following the existing format.

### 4. Add a LICENSE File

Create a `LICENSE` file in the root directory (MIT is already specified in project files):

```
MIT License

Copyright (c) 2026 Your Name

Permission is hereby granted, free of charge, to any person obtaining a copy...
```

## Building Packages

### Clean and Restore

```bash
dotnet clean
dotnet restore
```

### Build in Release Mode

```bash
dotnet build -c Release
```

### Create NuGet Packages

```bash
# Core package
dotnet pack src/SchemaRender.Core/SchemaRender.Core.csproj -c Release -o ./artifacts

# Generator package
dotnet pack src/SchemaRender.Generator/SchemaRender.Generator.csproj -c Release -o ./artifacts

# AspNetCore package
dotnet pack src/SchemaRender.AspNetCore/SchemaRender.AspNetCore.csproj -c Release -o ./artifacts
```

This creates `.nupkg` files in the `./artifacts` directory.

### Inspect Packages (Optional)

Use NuGet Package Explorer or:

```bash
# List package contents
dotnet nuget verify ./artifacts/SchemaRender.Core.1.0.0.nupkg
```

## Publishing to NuGet

### Set Your API Key

```bash
dotnet nuget push ./artifacts/SchemaRender.Core.1.0.0.nupkg --api-key YOUR_API_KEY --source https://api.nuget.org/v3/index.json
dotnet nuget push ./artifacts/SchemaRender.Generator.1.0.0.nupkg --api-key YOUR_API_KEY --source https://api.nuget.org/v3/index.json
dotnet nuget push ./artifacts/SchemaRender.AspNetCore.1.0.0.nupkg --api-key YOUR_API_KEY --source https://api.nuget.org/v3/index.json
```

Or store your API key once:

```bash
dotnet nuget setApiKey YOUR_API_KEY --source https://api.nuget.org/v3/index.json
```

Then push without specifying the key:

```bash
dotnet nuget push ./artifacts/*.nupkg --source https://api.nuget.org/v3/index.json
```

### Publish Symbol Packages

If symbol packages (`.snupkg`) were created:

```bash
dotnet nuget push ./artifacts/SchemaRender.Core.1.0.0.snupkg --source https://api.nuget.org/v3/index.json
dotnet nuget push ./artifacts/SchemaRender.Generator.1.0.0.snupkg --source https://api.nuget.org/v3/index.json
dotnet nuget push ./artifacts/SchemaRender.AspNetCore.1.0.0.snupkg --source https://api.nuget.org/v3/index.json
```

## Testing Locally

Before publishing to NuGet, test packages locally:

### 1. Create a Local NuGet Feed

```bash
mkdir ~/local-nuget
dotnet nuget add source ~/local-nuget --name LocalFeed
```

### 2. Push Packages to Local Feed

```bash
cp ./artifacts/*.nupkg ~/local-nuget/
```

### 3. Test Installation

In a test project:

```bash
dotnet add package SchemaRender.AspNetCore --source ~/local-nuget
```

## Package Validation

NuGet.org performs automatic validation:
- **Icon**: Optional but recommended (add 128x128 PNG to project)
- **README**: Automatically included from `README.md`
- **License**: MIT license expression included
- **Repository URL**: Included for "View on GitHub" link
- **SourceLink**: Enabled for debugging support

## CI/CD Automation

### GitHub Actions Example

Create `.github/workflows/publish.yml`:

```yaml
name: Publish to NuGet

on:
  release:
    types: [published]

jobs:
  publish:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3

      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: 8.0.x

      - name: Build
        run: dotnet build -c Release

      - name: Pack
        run: |
          dotnet pack src/SchemaRender.Core/SchemaRender.Core.csproj -c Release -o ./artifacts
          dotnet pack src/SchemaRender.Generator/SchemaRender.Generator.csproj -c Release -o ./artifacts
          dotnet pack src/SchemaRender.AspNetCore/SchemaRender.AspNetCore.csproj -c Release -o ./artifacts

      - name: Push to NuGet
        run: dotnet nuget push ./artifacts/*.nupkg --api-key ${{secrets.NUGET_API_KEY}} --source https://api.nuget.org/v3/index.json
        env:
          NUGET_API_KEY: ${{ secrets.NUGET_API_KEY }}
```

Store your API key in GitHub Secrets as `NUGET_API_KEY`.

## Package Dependencies

The packages have the following dependency structure:

- **SchemaRender.Core**: No dependencies (except Microsoft.Extensions.DependencyInjection.Abstractions)
- **SchemaRender.Generator**: Development dependency, no runtime dependencies
- **SchemaRender.AspNetCore**: Depends on SchemaRender.Core

Users can install:
- Just `SchemaRender.AspNetCore` for full functionality
- Add `SchemaRender.Generator` for source generation features

## Post-Publication

1. **Wait for Indexing**: Packages appear on NuGet.org within 5-10 minutes
2. **Create GitHub Release**: Tag the release with version (e.g., `v1.0.0`)
3. **Update Documentation**: Update README badges if needed
4. **Announce**: Share on social media, forums, etc.

## Troubleshooting

### Package Already Exists

NuGet doesn't allow overwriting published versions. Increment the version number and republish.

### Symbol Package Rejected

Ensure SourceLink is properly configured and the repository is public.

### Missing Dependencies

Check that `<ProjectReference>` entries generate proper `<PackageReference>` dependencies in the output package.

## Versioning Strategy

- **Prereleases**: Use `-alpha`, `-beta`, `-rc` suffixes (e.g., `1.0.0-beta1`)
- **Stable**: Use three-part versions (e.g., `1.0.0`)
- **Align Versions**: Keep all three packages on the same version for simplicity

Example:
```
1.0.0-alpha1 → 1.0.0-beta1 → 1.0.0-rc1 → 1.0.0
```

## Support

- NuGet Documentation: https://docs.microsoft.com/en-us/nuget/
- Package Upload: https://www.nuget.org/packages/manage/upload
- API Keys: https://www.nuget.org/account/apikeys
