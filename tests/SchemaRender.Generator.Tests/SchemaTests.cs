using System.Text.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchemaRender;
using SchemaRender.Schemas;

namespace SchemaRender.Generator.Tests;

[TestClass]
public class SchemaTests
{
    [TestMethod]
    public void PersonSchema_WithRequiredProperties_SerializesCorrectly()
    {
        // Arrange
        var schema = new PersonSchema { Name = "John Doe" };

        // Act
        var json = SerializeSchema(schema);

        // Assert
        // PersonSchema is a nested schema and does not include @context
        Assert.IsTrue(json.Contains("\"@type\":\"Person\""));
        Assert.IsTrue(json.Contains("\"name\":\"John Doe\""));
    }

    [TestMethod]
    public void PersonSchema_WithAllProperties_SerializesCorrectly()
    {
        // Arrange
        var schema = new PersonSchema
        {
            Name = "John Doe",
            JobTitle = "Software Engineer",
            Url = "https://example.com/john",
            SameAs = new[] { "https://twitter.com/johndoe", "https://linkedin.com/in/johndoe" }
        };

        // Act
        var json = SerializeSchema(schema);

        // Assert
        Assert.IsTrue(json.Contains("\"name\":\"John Doe\""));
        Assert.IsTrue(json.Contains("\"jobTitle\":\"Software Engineer\""));
        Assert.IsTrue(json.Contains("\"url\":\"https://example.com/john\""));
        Assert.IsTrue(json.Contains("\"sameAs\""));
        Assert.IsTrue(json.Contains("https://twitter.com/johndoe"));
        Assert.IsTrue(json.Contains("https://linkedin.com/in/johndoe"));
    }

    [TestMethod]
    public void ArticleSchema_WithRequiredProperties_SerializesCorrectly()
    {
        // Arrange
        var schema = new ArticleSchema
        {
            Headline = "Test Article",
            DatePublished = new DateTimeOffset(2024, 1, 15, 10, 30, 0, TimeSpan.Zero)
        };

        // Act
        var json = SerializeSchema(schema);

        // Assert
        Assert.IsTrue(json.Contains("\"@type\":\"Article\""));
        Assert.IsTrue(json.Contains("\"headline\":\"Test Article\""));
        Assert.IsTrue(json.Contains("\"datePublished\":\"2024-01-15T10:30:00"));
    }

    [TestMethod]
    public void ArticleSchema_WithAuthor_IncludesNestedAuthor()
    {
        // Arrange
        var schema = new ArticleSchema
        {
            Headline = "Test Article",
            DatePublished = DateTimeOffset.UtcNow,
            Author = new PersonSchema { Name = "Jane Smith" }
        };

        // Act
        var json = SerializeSchema(schema);

        // Assert
        Assert.IsTrue(json.Contains("\"author\""));
        Assert.IsTrue(json.Contains("\"name\":\"Jane Smith\""));
    }

    [TestMethod]
    public void RecipeSchema_WithRequiredProperties_SerializesCorrectly()
    {
        // Arrange
        var schema = new RecipeSchema
        {
            Name = "Chocolate Chip Cookies"
        };

        // Act
        var json = SerializeSchema(schema);

        // Assert
        Assert.IsTrue(json.Contains("\"@type\":\"Recipe\""));
        Assert.IsTrue(json.Contains("\"name\":\"Chocolate Chip Cookies\""));
    }

    [TestMethod]
    public void RecipeSchema_WithIngredients_SerializesAsArray()
    {
        // Arrange
        var schema = new RecipeSchema
        {
            Name = "Pasta",
            RecipeIngredient = new[] { "pasta", "tomato sauce", "cheese" }
        };

        // Act
        var json = SerializeSchema(schema);

        // Assert
        Assert.IsTrue(json.Contains("\"recipeIngredient\""));
        Assert.IsTrue(json.Contains("pasta"));
        Assert.IsTrue(json.Contains("tomato sauce"));
        Assert.IsTrue(json.Contains("cheese"));
    }

    [TestMethod]
    public void RecipeSchema_WithInstructions_SerializesAsHowToSteps()
    {
        // Arrange
        var schema = new RecipeSchema
        {
            Name = "Simple Recipe",
            RecipeInstructions = new[] { "Step 1: Prepare", "Step 2: Cook" }
        };

        // Act
        var json = SerializeSchema(schema);

        // Assert
        Assert.IsTrue(json.Contains("\"recipeInstructions\""));
        Assert.IsTrue(json.Contains("\"@type\":\"HowToStep\""));
        Assert.IsTrue(json.Contains("Step 1: Prepare"));
        Assert.IsTrue(json.Contains("Step 2: Cook"));
    }

    [TestMethod]
    public void RecipeSchema_WithTimeSpans_FormatsAsIsoDuration()
    {
        // Arrange
        var schema = new RecipeSchema
        {
            Name = "Quick Meal",
            CookTime = TimeSpan.FromMinutes(30),
            PrepTime = TimeSpan.FromMinutes(15),
            TotalTime = TimeSpan.FromMinutes(45)
        };

        // Act
        var json = SerializeSchema(schema);

        // Assert
        Assert.IsTrue(json.Contains("\"cookTime\":\"PT30M\""));
        Assert.IsTrue(json.Contains("\"prepTime\":\"PT15M\""));
        Assert.IsTrue(json.Contains("\"totalTime\":\"PT45M\""));
    }

    [TestMethod]
    public void BlogPostingSchema_InheritsFromArticle_SerializesCorrectly()
    {
        // Arrange
        var schema = new BlogPostingSchema
        {
            Headline = "My Blog Post",
            DatePublished = DateTimeOffset.UtcNow
        };

        // Act
        var json = SerializeSchema(schema);

        // Assert
        Assert.IsTrue(json.Contains("\"@type\":\"BlogPosting\""));
        Assert.IsTrue(json.Contains("\"headline\":\"My Blog Post\""));
    }

    [TestMethod]
    public void ProductSchema_WithOffer_IncludesNestedOffer()
    {
        // Arrange
        var schema = new ProductSchema
        {
            Name = "Amazing Product",
            Offers = new[]
            {
                new OfferSchema
                {
                    Price = 29.99m,
                    PriceCurrency = "USD"
                }
            }
        };

        // Act
        var json = SerializeSchema(schema);

        // Assert
        Assert.IsTrue(json.Contains("\"@type\":\"Product\""));
        Assert.IsTrue(json.Contains("\"name\":\"Amazing Product\""));
        Assert.IsTrue(json.Contains("\"offers\""));
        Assert.IsTrue(json.Contains("\"price\":29.99"));
        Assert.IsTrue(json.Contains("\"priceCurrency\":\"USD\""));
    }

    [TestMethod]
    public void OrganizationSchema_WithAddress_IncludesNestedAddress()
    {
        // Arrange
        var schema = new OrganizationSchema
        {
            Name = "Acme Corp",
            Address = new PostalAddressSchema
            {
                StreetAddress = "123 Main St",
                AddressLocality = "Springfield",
                PostalCode = "12345"
            }
        };

        // Act
        var json = SerializeSchema(schema);

        // Assert
        Assert.IsTrue(json.Contains("\"@type\":\"Organization\""));
        Assert.IsTrue(json.Contains("\"name\":\"Acme Corp\""));
        Assert.IsTrue(json.Contains("\"address\""));
        Assert.IsTrue(json.Contains("\"streetAddress\":\"123 Main St\""));
        Assert.IsTrue(json.Contains("\"addressLocality\":\"Springfield\""));
    }

    [TestMethod]
    public void EventSchema_WithDateRange_SerializesCorrectly()
    {
        // Arrange
        var startDate = new DateTimeOffset(2024, 6, 1, 18, 0, 0, TimeSpan.Zero);
        var endDate = new DateTimeOffset(2024, 6, 1, 21, 0, 0, TimeSpan.Zero);
        var schema = new EventSchema
        {
            Name = "Tech Conference",
            StartDate = startDate,
            EndDate = endDate
        };

        // Act
        var json = SerializeSchema(schema);

        // Assert
        Assert.IsTrue(json.Contains("\"@type\":\"Event\""));
        Assert.IsTrue(json.Contains("\"name\":\"Tech Conference\""));
        Assert.IsTrue(json.Contains("\"startDate\""));
        Assert.IsTrue(json.Contains("\"endDate\""));
    }

    [TestMethod]
    public void FAQPageSchema_WithQuestions_SerializesCorrectly()
    {
        // Arrange
        var schema = new FAQPageSchema
        {
            MainEntity = new[]
            {
                new QuestionSchema
                {
                    Name = "What is this?",
                    AcceptedAnswer = new[]
                    {
                        new AnswerSchema
                        {
                            Text = "This is the answer."
                        }
                    }
                }
            }
        };

        // Act
        var json = SerializeSchema(schema);

        // Assert
        Assert.IsTrue(json.Contains("\"@type\":\"FAQPage\""));
        Assert.IsTrue(json.Contains("\"@type\":\"Question\""));
        Assert.IsTrue(json.Contains("\"name\":\"What is this?\""));
        Assert.IsTrue(json.Contains("\"@type\":\"Answer\""));
        Assert.IsTrue(json.Contains("\"text\":\"This is the answer.\""));
    }

    [TestMethod]
    public void BreadcrumbListSchema_WithItems_SerializesCorrectly()
    {
        // Arrange
        var schema = new BreadcrumbListSchema
        {
            ItemListElement = new[]
            {
                new ListItemSchema
                {
                    Position = 1,
                    Name = "Home",
                    Item = "https://example.com"
                },
                new ListItemSchema
                {
                    Position = 2,
                    Name = "Products",
                    Item = "https://example.com/products"
                }
            }
        };

        // Act
        var json = SerializeSchema(schema);

        // Assert
        Assert.IsTrue(json.Contains("\"@type\":\"BreadcrumbList\""));
        Assert.IsTrue(json.Contains("\"@type\":\"ListItem\""));
        Assert.IsTrue(json.Contains("\"position\":1"));
        Assert.IsTrue(json.Contains("\"name\":\"Home\""));
        Assert.IsTrue(json.Contains("\"position\":2"));
        Assert.IsTrue(json.Contains("\"name\":\"Products\""));
    }

    [TestMethod]
    public void ImageObjectSchema_WithUrl_SerializesCorrectly()
    {
        // Arrange
        var schema = new ImageObjectSchema
        {
            Url = "https://example.com/image.jpg",
            Width = 800,
            Height = 600
        };

        // Act
        var json = SerializeSchema(schema);

        // Assert
        Assert.IsTrue(json.Contains("\"url\":\"https://example.com/image.jpg\""));
        Assert.IsTrue(json.Contains("\"width\":800"));
        Assert.IsTrue(json.Contains("\"height\":600"));
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
