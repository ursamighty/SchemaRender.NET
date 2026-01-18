using System.Text.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchemaRender;
using SchemaRender.Schemas;

namespace SchemaRender.Generator.Tests;

[TestClass]
public class SourceGeneratorTests
{
    [TestMethod]
    public void GeneratedSchema_ImplementsISchema()
    {
        // Arrange
        var schema = new TestRecipeSchema
        {
            Name = "Generated Recipe"
        };

        // Assert
        Assert.IsInstanceOfType(schema, typeof(ISchema));
    }

    [TestMethod]
    public void GeneratedSchema_SerializesRequiredProperties()
    {
        // Arrange
        var schema = new TestRecipeSchema
        {
            Name = "Test Recipe"
        };

        // Act
        var json = SerializeSchema(schema);

        // Assert
        Assert.IsTrue(json.Contains("\"@context\":\"https://schema.org\""));
        Assert.IsTrue(json.Contains("\"@type\":\"Recipe\""));
        Assert.IsTrue(json.Contains("\"name\":\"Test Recipe\""));
    }

    [TestMethod]
    public void GeneratedSchema_SerializesOptionalProperties()
    {
        // Arrange
        var schema = new TestRecipeSchema
        {
            Name = "Test Recipe",
            Description = "A delicious recipe",
            CookTime = TimeSpan.FromMinutes(30),
            PrepTime = TimeSpan.FromMinutes(15)
        };

        // Act
        var json = SerializeSchema(schema);

        // Assert
        Assert.IsTrue(json.Contains("\"description\":\"A delicious recipe\""));
        Assert.IsTrue(json.Contains("\"cookTime\":\"PT30M\""));
        Assert.IsTrue(json.Contains("\"prepTime\":\"PT15M\""));
    }

    [TestMethod]
    public void GeneratedSchema_SerializesArrayProperties()
    {
        // Arrange
        var schema = new TestRecipeSchema
        {
            Name = "Test Recipe",
            Ingredients = new[] { "flour", "sugar", "eggs" }
        };

        // Act
        var json = SerializeSchema(schema);

        // Assert
        Assert.IsTrue(json.Contains("\"ingredients\""));
        Assert.IsTrue(json.Contains("flour"));
        Assert.IsTrue(json.Contains("sugar"));
        Assert.IsTrue(json.Contains("eggs"));
    }

    [TestMethod]
    public void GeneratedSchema_SerializesNestedSchemas()
    {
        // Arrange
        var schema = new TestRecipeSchema
        {
            Name = "Test Recipe",
            Author = new PersonSchema { Name = "Chef John" }
        };

        // Act
        var json = SerializeSchema(schema);

        // Assert
        Assert.IsTrue(json.Contains("\"author\""));
        Assert.IsTrue(json.Contains("\"@type\":\"Person\""));
        Assert.IsTrue(json.Contains("\"name\":\"Chef John\""));
    }

    [TestMethod]
    public void GeneratedSchema_IgnoresPropertiesWithSchemaIgnore()
    {
        // Arrange
        var schema = new TestRecipeSchema
        {
            Name = "Test Recipe",
            InternalNote = "This should not appear in the JSON"
        };

        // Act
        var json = SerializeSchema(schema);

        // Assert
        Assert.IsFalse(json.Contains("internalNote"));
        Assert.IsFalse(json.Contains("This should not appear in the JSON"));
    }

    [TestMethod]
    public void GeneratedSchema_CanBeRendered()
    {
        // Arrange
        var schema = new TestRecipeSchema
        {
            Name = "Rendered Recipe"
        };

        // Act
        var html = SchemaRenderer.RenderSchemaToString(schema);

        // Assert
        Assert.IsTrue(html.StartsWith("<script type=\"application/ld+json\">"));
        Assert.IsTrue(html.EndsWith("</script>"));
        Assert.IsTrue(html.Contains("Rendered Recipe"));
    }

    [TestMethod]
    public void GeneratedSchema_CanBeAddedToContext()
    {
        // Arrange
        var context = new SchemaContext();
        var schema = new TestRecipeSchema
        {
            Name = "Context Recipe"
        };

        // Act
        context.Add(schema);

        // Assert
        Assert.AreEqual(1, context.Schemas.Count);
        Assert.IsTrue(context.HasSchemas);
        Assert.AreSame(schema, context.Schemas[0]);
    }

    [TestMethod]
    public void GeneratedSchema_OmitsNullOptionalProperties()
    {
        // Arrange
        var schema = new TestRecipeSchema
        {
            Name = "Minimal Recipe",
            Description = null,
            CookTime = null,
            PrepTime = null,
            Ingredients = null,
            Author = null
        };

        // Act
        var json = SerializeSchema(schema);

        // Assert
        Assert.IsTrue(json.Contains("\"name\":\"Minimal Recipe\""));
        Assert.IsFalse(json.Contains("\"description\""));
        Assert.IsFalse(json.Contains("\"cookTime\""));
        Assert.IsFalse(json.Contains("\"prepTime\""));
        Assert.IsFalse(json.Contains("\"ingredients\""));
        Assert.IsFalse(json.Contains("\"author\""));
    }

    private static string SerializeSchema(ISchema schema)
    {
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream);
        schema.Write(writer);
        writer.Flush();
        return System.Text.Encoding.UTF8.GetString(stream.ToArray());
    }
}
