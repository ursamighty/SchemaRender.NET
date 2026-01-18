using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchemaRender;
using SchemaRender.Schemas;

namespace SchemaRender.Generator.Tests;

[TestClass]
public class SchemaContextTests
{
    [TestMethod]
    public void Add_AddsSchemaToContext()
    {
        // Arrange
        var context = new SchemaContext();
        var schema = new PersonSchema { Name = "John Doe" };

        // Act
        context.Add(schema);

        // Assert
        Assert.AreEqual(1, context.Schemas.Count);
        Assert.AreSame(schema, context.Schemas[0]);
    }

    [TestMethod]
    public void Add_WithMultipleSchemas_AddsAllSchemas()
    {
        // Arrange
        var context = new SchemaContext();
        var schema1 = new PersonSchema { Name = "John Doe" };
        var schema2 = new PersonSchema { Name = "Jane Smith" };
        var schema3 = new ArticleSchema
        {
            Headline = "Test Article",
            DatePublished = DateTimeOffset.UtcNow
        };

        // Act
        context.Add(schema1);
        context.Add(schema2);
        context.Add(schema3);

        // Assert
        Assert.AreEqual(3, context.Schemas.Count);
        Assert.AreSame(schema1, context.Schemas[0]);
        Assert.AreSame(schema2, context.Schemas[1]);
        Assert.AreSame(schema3, context.Schemas[2]);
    }

    [TestMethod]
    public void Add_WithNullSchema_ThrowsArgumentNullException()
    {
        // Arrange
        var context = new SchemaContext();

        // Act & Assert
        Assert.ThrowsExactly<ArgumentNullException>(() => context.Add(null!));
    }

    [TestMethod]
    public void HasSchemas_WhenEmpty_ReturnsFalse()
    {
        // Arrange
        var context = new SchemaContext();

        // Assert
        Assert.IsFalse(context.HasSchemas);
    }

    [TestMethod]
    public void HasSchemas_WhenNotEmpty_ReturnsTrue()
    {
        // Arrange
        var context = new SchemaContext();
        var schema = new PersonSchema { Name = "John Doe" };

        // Act
        context.Add(schema);

        // Assert
        Assert.IsTrue(context.HasSchemas);
    }

    [TestMethod]
    public void Schemas_WhenEmpty_ReturnsEmptyList()
    {
        // Arrange
        var context = new SchemaContext();

        // Assert
        Assert.AreEqual(0, context.Schemas.Count);
        Assert.IsNotNull(context.Schemas);
    }

    [TestMethod]
    public void Schemas_PreservesInsertionOrder()
    {
        // Arrange
        var context = new SchemaContext();
        var schemas = new[]
        {
            new PersonSchema { Name = "First" },
            new PersonSchema { Name = "Second" },
            new PersonSchema { Name = "Third" }
        };

        // Act
        foreach (var schema in schemas)
        {
            context.Add(schema);
        }

        // Assert
        Assert.AreEqual(3, context.Schemas.Count);
        for (int i = 0; i < schemas.Length; i++)
        {
            Assert.AreSame(schemas[i], context.Schemas[i]);
        }
    }
}
