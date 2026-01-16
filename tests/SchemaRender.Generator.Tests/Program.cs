using SchemaRender;
using SchemaRender.Generator.Tests;
using SchemaRender.Schemas;

// Create a recipe schema using the generated class
var recipe = new TestRecipeSchema
{
    Name = "Best Lasagna Ever",
    Description = "A delicious homemade lasagna recipe",
    CookTime = TimeSpan.FromMinutes(45),
    PrepTime = TimeSpan.FromMinutes(30),
    Author = new PersonSchema { Name = "John Doe" },
    Ingredients = ["pasta sheets", "ricotta cheese", "mozzarella", "tomato sauce", "ground beef"]
};

// Verify it implements ISchema
ISchema schema = recipe;

// Render to string
var json = SchemaRenderer.RenderSchemaToString(schema);

Console.WriteLine("Generated JSON-LD:");
Console.WriteLine(json);
Console.WriteLine();

// Test with schema context
var context = new SchemaContext();
context.Add(recipe);

Console.WriteLine("Full render output:");
Console.WriteLine(SchemaRenderer.RenderToString(context));
Console.WriteLine();
Console.WriteLine("=".PadRight(80, '='));
Console.WriteLine();

// Test LocalBusiness schema with nested address and geo
var business = new LocalBusinessSchema
{
    Name = "Joe's Pizza",
    Description = "Best pizza in town",
    Url = "https://joespizza.example.com",
    Telephone = "+1-555-123-4567",
    Email = "info@joespizza.example.com",
    PriceRange = "$$",
    Address = new PostalAddressSchema
    {
        StreetAddress = "123 Main St",
        AddressLocality = "New York",
        AddressRegion = "NY",
        PostalCode = "10001",
        AddressCountry = "US"
    },
    Geo = new GeoCoordinatesSchema
    {
        Latitude = 40.7128,
        Longitude = -74.0060
    },
    OpeningHoursSpecification = ["Mo-Fr 11:00-22:00", "Sa-Su 12:00-23:00"],
    ServesCuisine = "Italian",
    SameAs = ["https://facebook.com/joespizza", "https://twitter.com/joespizza"]
};

context.Add(business);

Console.WriteLine("LocalBusiness JSON-LD:");
Console.WriteLine(SchemaRenderer.RenderSchemaToString(business));
