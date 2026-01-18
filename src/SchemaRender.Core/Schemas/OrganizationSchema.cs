using System.Text.Json;

namespace SchemaRender.Schemas;

/// <summary>
/// Schema.org Organization structured data.
/// See: https://schema.org/Organization
/// </summary>
public sealed class OrganizationSchema : ISchema
{
    /// <summary>
    /// The name of the organization.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The URL of the organization's website.
    /// </summary>
    public string? Url { get; init; }

    /// <summary>
    /// The organization's logo.
    /// </summary>
    public ImageObjectSchema? Logo { get; init; }

    /// <summary>
    /// A description of the organization.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// The email address of the organization.
    /// </summary>
    public string? Email { get; init; }

    /// <summary>
    /// The telephone number of the organization.
    /// </summary>
    public string? Telephone { get; init; }

    /// <summary>
    /// The postal address of the organization.
    /// </summary>
    public PostalAddressSchema? Address { get; init; }

    /// <summary>
    /// Social media profile URLs.
    /// </summary>
    public IReadOnlyList<string>? SameAs { get; init; }

    /// <summary>
    /// The official name of the organization (e.g., the registered company name).
    /// </summary>
    public string? LegalName { get; init; }

    /// <summary>
    /// The person or organization that founded this organization.
    /// </summary>
    public string? Founder { get; init; }

    /// <summary>
    /// The date the organization was founded.
    /// </summary>
    public DateTimeOffset? FoundingDate { get; init; }

    /// <summary>
    /// The number of employees in the organization.
    /// </summary>
    public int? NumberOfEmployees { get; init; }

    /// <summary>
    /// The Tax / Fiscal ID of the organization.
    /// </summary>
    public string? TaxID { get; init; }

    /// <summary>
    /// An identifier for the organization (e.g., DUNS number, LEI code).
    /// </summary>
    public string? Identifier { get; init; }

    /// <summary>
    /// The larger organization that this organization is a part of.
    /// </summary>
    public string? ParentOrganization { get; init; }

    /// <summary>
    /// The geographic area where the organization provides service.
    /// </summary>
    public string? AreaServed { get; init; }

    /// <inheritdoc />
    public void Write(Utf8JsonWriter w)
    {
        w.WriteStartObject();
        w.WriteString("@context", "https://schema.org");
        w.WriteString("@type", "Organization");
        w.WriteString("name", Name);

        if (Url is not null)
            w.WriteString("url", Url);

        if (Logo is { HasValue: true })
        {
            w.WritePropertyName("logo");
            Logo.Write(w);
        }

        if (Description is not null)
            w.WriteString("description", Description);

        if (Email is not null)
            w.WriteString("email", Email);

        if (Telephone is not null)
            w.WriteString("telephone", Telephone);

        if (Address is { HasValue: true })
        {
            w.WritePropertyName("address");
            Address.Write(w);
        }

        if (SameAs is { Count: > 0 })
        {
            w.WritePropertyName("sameAs");
            w.WriteStartArray();
            foreach (var url in SameAs)
                w.WriteStringValue(url);
            w.WriteEndArray();
        }

        if (LegalName is not null)
            w.WriteString("legalName", LegalName);

        if (Founder is not null)
            w.WriteString("founder", Founder);

        if (FoundingDate is not null)
            w.WriteString("foundingDate", FoundingDate.Value.ToString("O"));

        if (NumberOfEmployees is not null)
            w.WriteNumber("numberOfEmployees", NumberOfEmployees.Value);

        if (TaxID is not null)
            w.WriteString("taxID", TaxID);

        if (Identifier is not null)
            w.WriteString("identifier", Identifier);

        if (ParentOrganization is not null)
            w.WriteString("parentOrganization", ParentOrganization);

        if (AreaServed is not null)
            w.WriteString("areaServed", AreaServed);

        w.WriteEndObject();
    }
}
