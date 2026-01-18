using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchemaRender;
using SchemaRender.Schemas;

namespace SchemaRender.Generator.Tests;

[TestClass]
public class SchemaRendererTests
{
    [TestMethod]
    public void RenderToString_WithEmptyContext_ReturnsEmptyString()
    {
        // Arrange
        var context = new SchemaContext();

        // Act
        var result = SchemaRenderer.RenderToString(context);

        // Assert
        Assert.AreEqual(string.Empty, result);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void RenderToString_WithNullContext_ThrowsArgumentNullException()
    {
        // Act
        SchemaRenderer.RenderToString(null!);

        // Assert - ExpectedException
    }

    [TestMethod]
    public void RenderToString_WithSingleSchema_ReturnsScriptTag()
    {
        // Arrange
        var context = new SchemaContext();
        var schema = new PersonSchema { Name = "John Doe" };
        context.Add(schema);

        // Act
        var result = SchemaRenderer.RenderToString(context);

        // Assert
        Assert.IsTrue(result.StartsWith("<script type=\"application/ld+json\">"));
        Assert.IsTrue(result.EndsWith("</script>"));
        Assert.IsTrue(result.Contains("\"@type\":\"Person\""));
        Assert.IsTrue(result.Contains("\"name\":\"John Doe\""));
    }

    [TestMethod]
    public void RenderToString_WithMultipleSchemas_ReturnsMultipleScriptTags()
    {
        // Arrange
        var context = new SchemaContext();
        context.Add(new PersonSchema { Name = "John Doe" });
        context.Add(new PersonSchema { Name = "Jane Smith" });

        // Act
        var result = SchemaRenderer.RenderToString(context);

        // Assert
        var scriptTagCount = CountOccurrences(result, "<script type=\"application/ld+json\">");
        Assert.AreEqual(2, scriptTagCount);
        Assert.IsTrue(result.Contains("John Doe"));
        Assert.IsTrue(result.Contains("Jane Smith"));
    }

    [TestMethod]
    public void RenderSchemaToString_WithPersonSchema_ReturnsValidJsonLd()
    {
        // Arrange
        var schema = new PersonSchema { Name = "John Doe", Url = "https://example.com/john" };

        // Act
        var result = SchemaRenderer.RenderSchemaToString(schema);

        // Assert
        Assert.IsTrue(result.StartsWith("<script type=\"application/ld+json\">"));
        Assert.IsTrue(result.EndsWith("</script>"));
        Assert.IsTrue(result.Contains("\"@type\":\"Person\""));
        Assert.IsTrue(result.Contains("\"name\":\"John Doe\""));
        Assert.IsTrue(result.Contains("\"url\":\"https://example.com/john\""));
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void RenderSchemaToString_WithNullSchema_ThrowsArgumentNullException()
    {
        // Act
        SchemaRenderer.RenderSchemaToString(null!);

        // Assert - ExpectedException
    }

    [TestMethod]
    public void Render_WithStreamAndEmptyContext_WritesNothing()
    {
        // Arrange
        var context = new SchemaContext();
        using var stream = new MemoryStream();

        // Act
        SchemaRenderer.Render(context, stream);

        // Assert
        Assert.AreEqual(0, stream.Length);
    }

    [TestMethod]
    public void Render_WithStreamAndSchema_WritesToStream()
    {
        // Arrange
        var context = new SchemaContext();
        context.Add(new PersonSchema { Name = "John Doe" });
        using var stream = new MemoryStream();

        // Act
        SchemaRenderer.Render(context, stream);

        // Assert
        Assert.IsTrue(stream.Length > 0);
        stream.Position = 0;
        var result = Encoding.UTF8.GetString(stream.ToArray());
        Assert.IsTrue(result.Contains("John Doe"));
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Render_WithNullContext_ThrowsArgumentNullException()
    {
        // Arrange
        using var stream = new MemoryStream();

        // Act
        SchemaRenderer.Render(null!, stream);

        // Assert - ExpectedException
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Render_WithNullStream_ThrowsArgumentNullException()
    {
        // Arrange
        var context = new SchemaContext();

        // Act
        SchemaRenderer.Render(context, null!);

        // Assert - ExpectedException
    }

    [TestMethod]
    public void RenderSchema_WithStream_WritesToStream()
    {
        // Arrange
        var schema = new PersonSchema { Name = "Jane Smith" };
        using var stream = new MemoryStream();

        // Act
        SchemaRenderer.RenderSchema(schema, stream);

        // Assert
        Assert.IsTrue(stream.Length > 0);
        stream.Position = 0;
        var result = Encoding.UTF8.GetString(stream.ToArray());
        Assert.IsTrue(result.Contains("Jane Smith"));
        Assert.IsTrue(result.StartsWith("<script type=\"application/ld+json\">"));
        Assert.IsTrue(result.EndsWith("</script>"));
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void RenderSchema_WithNullSchema_ThrowsArgumentNullException()
    {
        // Arrange
        using var stream = new MemoryStream();

        // Act
        SchemaRenderer.RenderSchema(null!, stream);

        // Assert - ExpectedException
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void RenderSchema_WithNullStream_ThrowsArgumentNullException()
    {
        // Arrange
        var schema = new PersonSchema { Name = "Test" };

        // Act
        SchemaRenderer.RenderSchema(schema, null!);

        // Assert - ExpectedException
    }

    [TestMethod]
    public void RenderToString_WithRecipeSchema_IncludesAllProperties()
    {
        // Arrange
        var context = new SchemaContext();
        var recipe = new RecipeSchema
        {
            Name = "Chocolate Cake",
            Description = "A delicious chocolate cake",
            CookTime = TimeSpan.FromMinutes(45),
            PrepTime = TimeSpan.FromMinutes(15),
            RecipeIngredient = new[] { "flour", "sugar", "chocolate" },
            RecipeInstructions = new[] { "Mix ingredients", "Bake at 350F" }
        };
        context.Add(recipe);

        // Act
        var result = SchemaRenderer.RenderToString(context);

        // Assert
        Assert.IsTrue(result.Contains("\"name\":\"Chocolate Cake\""));
        Assert.IsTrue(result.Contains("\"description\":\"A delicious chocolate cake\""));
        Assert.IsTrue(result.Contains("\"cookTime\":\"PT45M\""));
        Assert.IsTrue(result.Contains("\"prepTime\":\"PT15M\""));
        Assert.IsTrue(result.Contains("flour"));
        Assert.IsTrue(result.Contains("sugar"));
        Assert.IsTrue(result.Contains("chocolate"));
    }

    private static int CountOccurrences(string text, string pattern)
    {
        int count = 0;
        int index = 0;
        while ((index = text.IndexOf(pattern, index, StringComparison.Ordinal)) != -1)
        {
            count++;
            index += pattern.Length;
        }
        return count;
    }
}
