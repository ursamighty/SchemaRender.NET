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
    OpeningHours = ["Mo-Fr 11:00-22:00", "Sa-Su 12:00-23:00"],
    ServesCuisine = "Italian",
    SameAs = ["https://facebook.com/joespizza", "https://twitter.com/joespizza"]
};

context.Add(business);

Console.WriteLine("LocalBusiness JSON-LD:");
Console.WriteLine(SchemaRenderer.RenderSchemaToString(business));
Console.WriteLine();
Console.WriteLine("=".PadRight(80, '='));
Console.WriteLine();

// Test Phase 1 schemas: ImageObjectSchema, AggregateRatingSchema, OfferSchema, WebSiteSchema
var website = new WebSiteSchema
{
    Name = "Example Website",
    Url = "https://example.com",
    Description = "An example website for testing"
};

Console.WriteLine("WebSite JSON-LD:");
Console.WriteLine(SchemaRenderer.RenderSchemaToString(website));
Console.WriteLine();
Console.WriteLine("=".PadRight(80, '='));
Console.WriteLine();

// Test Product with all nested schemas
var product = new ProductSchema
{
    Name = "Smart Watch Pro",
    Description = "Advanced fitness tracker and smart watch",
    Image = new ImageObjectSchema
    {
        Url = "https://example.com/watch.jpg",
        Width = 800,
        Height = 600,
        Caption = "Smart Watch Pro in black"
    },
    Brand = "TechCorp",
    Sku = "SW-PRO-001",
    Gtin = "12345678901234",
    AggregateRating = new AggregateRatingSchema
    {
        RatingValue = 4.5,
        RatingCount = 127,
        BestRating = 5,
        WorstRating = 1
    },
    Offers = new[]
    {
        new OfferSchema
        {
            Price = 299.99m,
            PriceCurrency = "USD",
            Availability = "https://schema.org/InStock",
            Url = "https://example.com/buy/watch"
        }
    },
    Review = new[]
    {
        new ReviewSchema
        {
            ReviewBody = "Great watch with excellent battery life!",
            ReviewRating = 5,
            Author = "Jane Smith",
            DatePublished = DateTimeOffset.UtcNow.AddDays(-7)
        }
    }
};

Console.WriteLine("Product with nested schemas JSON-LD:");
Console.WriteLine(SchemaRenderer.RenderSchemaToString(product));
Console.WriteLine();
Console.WriteLine("=".PadRight(80, '='));
Console.WriteLine();

// Test VideoObject
var video = new VideoObjectSchema
{
    Name = "How to Use Smart Watch",
    ContentUrl = "https://example.com/videos/tutorial.mp4",
    Description = "Complete tutorial on using the Smart Watch Pro",
    ThumbnailUrl = "https://example.com/videos/thumb.jpg",
    UploadDate = DateTimeOffset.UtcNow.AddDays(-30),
    Duration = TimeSpan.FromMinutes(12)
};

Console.WriteLine("VideoObject JSON-LD:");
Console.WriteLine(SchemaRenderer.RenderSchemaToString(video));
Console.WriteLine();
Console.WriteLine("=".PadRight(80, '='));
Console.WriteLine();

// Test Event
var eventSchema = new EventSchema
{
    Name = "Tech Conference 2024",
    StartDate = new DateTimeOffset(2024, 6, 15, 9, 0, 0, TimeSpan.Zero),
    EndDate = new DateTimeOffset(2024, 6, 17, 18, 0, 0, TimeSpan.Zero),
    Description = "Annual technology conference",
    Image = new ImageObjectSchema
    {
        Url = "https://example.com/conference.jpg",
        Width = 1200,
        Height = 630
    },
    Location = "Convention Center",
    Address = new PostalAddressSchema
    {
        StreetAddress = "456 Tech Ave",
        AddressLocality = "San Francisco",
        AddressRegion = "CA",
        PostalCode = "94105",
        AddressCountry = "US"
    },
    Organizer = new OrganizationSchema
    {
        Name = "Tech Events Inc",
        Url = "https://techevents.example.com"
    },
    EventStatus = "EventScheduled",
    EventAttendanceMode = "OfflineEventAttendanceMode"
};

Console.WriteLine("Event JSON-LD:");
Console.WriteLine(SchemaRenderer.RenderSchemaToString(eventSchema));
Console.WriteLine();
Console.WriteLine("=".PadRight(80, '='));
Console.WriteLine();

// Test BlogPosting
var blogPost = new BlogPostingSchema
{
    Headline = "Introduction to Schema.org Markup",
    Description = "Learn how to implement structured data",
    Image = new ImageObjectSchema
    {
        Url = "https://example.com/blog/intro.jpg",
        Width = 1200,
        Height = 630
    },
    DatePublished = DateTimeOffset.UtcNow.AddDays(-5),
    DateModified = DateTimeOffset.UtcNow.AddDays(-1),
    Author = new PersonSchema
    {
        Name = "Alice Developer",
        Url = "https://example.com/authors/alice"
    },
    Publisher = new OrganizationSchema
    {
        Name = "Tech Blog",
        Logo = new ImageObjectSchema
        {
            Url = "https://example.com/logo.png",
            Width = 200,
            Height = 60
        }
    },
    ArticleSection = "Tutorials",
    WordCount = 1250
};

Console.WriteLine("BlogPosting JSON-LD:");
Console.WriteLine(SchemaRenderer.RenderSchemaToString(blogPost));
Console.WriteLine();
Console.WriteLine("=".PadRight(80, '='));
Console.WriteLine();

// Test FAQPage
var faqPage = new FAQPageSchema
{
    Name = "Product FAQ",
    Description = "Frequently asked questions about our products",
    MainEntity = new[]
    {
        new QuestionSchema
        {
            Name = "What is the warranty period?",
            AcceptedAnswer = new[]
            {
                new AnswerSchema
                {
                    Text = "All products come with a 2-year manufacturer warranty.",
                    DateCreated = DateTimeOffset.UtcNow.AddMonths(-1)
                }
            }
        },
        new QuestionSchema
        {
            Name = "Do you ship internationally?",
            AcceptedAnswer = new[]
            {
                new AnswerSchema
                {
                    Text = "Yes, we ship to over 50 countries worldwide."
                }
            }
        }
    }
};

Console.WriteLine("FAQPage JSON-LD:");
Console.WriteLine(SchemaRenderer.RenderSchemaToString(faqPage));
Console.WriteLine();
Console.WriteLine("=".PadRight(80, '='));
Console.WriteLine();

// Test BreadcrumbList
var breadcrumbs = new BreadcrumbListSchema
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
        },
        new ListItemSchema
        {
            Position = 3,
            Name = "Smart Watches",
            Item = "https://example.com/products/watches"
        }
    }
};

Console.WriteLine("BreadcrumbList JSON-LD:");
Console.WriteLine(SchemaRenderer.RenderSchemaToString(breadcrumbs));
Console.WriteLine();
Console.WriteLine("=".PadRight(80, '='));
Console.WriteLine();

// Test HowTo
var howTo = new HowToSchema
{
    Name = "How to Set Up Your Smart Watch",
    Description = "Step-by-step guide for setting up your new smart watch",
    Image = new ImageObjectSchema
    {
        Url = "https://example.com/howto/setup.jpg",
        Width = 800,
        Height = 600
    },
    TotalTime = TimeSpan.FromMinutes(15),
    PrepTime = TimeSpan.FromMinutes(5),
    Step = new[]
    {
        new HowToStepSchema
        {
            Text = "Charge the watch for at least 2 hours before first use",
            Name = "Initial Charging"
        },
        new HowToStepSchema
        {
            Text = "Download the companion app from your phone's app store",
            Name = "Install App"
        },
        new HowToStepSchema
        {
            Text = "Power on the watch and follow the pairing instructions",
            Name = "Pair Device"
        }
    },
    Supply = new[] { "Smart Watch", "Charging cable", "Smartphone" },
    Tool = new[] { "Smartphone with Bluetooth" }
};

Console.WriteLine("HowTo JSON-LD:");
Console.WriteLine(SchemaRenderer.RenderSchemaToString(howTo));
Console.WriteLine();
Console.WriteLine("=".PadRight(80, '='));
Console.WriteLine();

Console.WriteLine("All tests completed successfully!");
