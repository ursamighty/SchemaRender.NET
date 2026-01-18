using System.Text.Json;
using SchemaRender.Helpers;

namespace SchemaRender.Schemas;

/// <summary>
/// Schema.org Event structured data.
/// See: https://schema.org/Event
/// </summary>
public sealed class EventSchema : ISchema
{
    /// <summary>
    /// The name of the event.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The start date and time of the event.
    /// </summary>
    public required DateTimeOffset StartDate { get; init; }

    /// <summary>
    /// The end date and time of the event.
    /// </summary>
    public DateTimeOffset? EndDate { get; init; }

    /// <summary>
    /// A description of the event.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// An image representing the event.
    /// </summary>
    public ImageObjectSchema? Image { get; init; }

    /// <summary>
    /// The name of the location where the event takes place.
    /// </summary>
    public string? Location { get; init; }

    /// <summary>
    /// The physical address of the event location.
    /// </summary>
    public PostalAddressSchema? Address { get; init; }

    /// <summary>
    /// The organization or person organizing the event.
    /// </summary>
    public OrganizationSchema? Organizer { get; init; }

    /// <summary>
    /// Offers for attending the event (tickets, etc.).
    /// </summary>
    public IReadOnlyList<OfferSchema>? Offers { get; init; }

    /// <summary>
    /// The status of the event (e.g., "EventScheduled", "EventCancelled", "EventPostponed").
    /// </summary>
    public string? EventStatus { get; init; }

    /// <summary>
    /// The attendance mode of the event (e.g., "OfflineEventAttendanceMode", "OnlineEventAttendanceMode", "MixedEventAttendanceMode").
    /// </summary>
    public string? EventAttendanceMode { get; init; }

    /// <summary>
    /// The URL of the event page.
    /// </summary>
    public string? Url { get; init; }

    /// <summary>
    /// A performer at the event (e.g., a presenter, musician, musical group, etc.).
    /// </summary>
    public string? Performer { get; init; }

    /// <summary>
    /// The aggregate rating for the event.
    /// </summary>
    public AggregateRatingSchema? AggregateRating { get; init; }

    /// <summary>
    /// Indicates whether the event is accessible for free.
    /// </summary>
    public bool? IsAccessibleForFree { get; init; }

    /// <summary>
    /// The maximum number of attendees that can attend the event.
    /// </summary>
    public int? MaximumAttendeeCapacity { get; init; }

    /// <summary>
    /// The typical expected age range for the event (e.g., "18+", "7-12").
    /// </summary>
    public string? TypicalAgeRange { get; init; }

    /// <summary>
    /// A person or organization that supports the event through financial or other contributions.
    /// </summary>
    public string? Sponsor { get; init; }

    /// <summary>
    /// The duration of the event (alternative to specifying both startDate and endDate).
    /// </summary>
    public TimeSpan? Duration { get; init; }

    /// <inheritdoc />
    public void Write(Utf8JsonWriter w)
    {
        w.WriteStartObject();
        w.WriteString("@context", "https://schema.org");
        w.WriteString("@type", "Event");
        w.WriteString("name", Name);
        w.WriteString("startDate", StartDate.ToString("O"));

        if (EndDate is not null)
            w.WriteString("endDate", EndDate.Value.ToString("O"));

        if (Description is not null)
            w.WriteString("description", Description);

        if (Image is { HasValue: true })
        {
            w.WritePropertyName("image");
            Image.Write(w);
        }

        if (Location is not null)
            w.WriteString("location", Location);

        if (Address is { HasValue: true })
        {
            w.WritePropertyName("address");
            Address.Write(w);
        }

        if (Organizer is not null)
        {
            w.WritePropertyName("organizer");
            Organizer.Write(w);
        }

        if (Offers is { Count: > 0 })
        {
            w.WritePropertyName("offers");
            w.WriteStartArray();
            foreach (var offer in Offers)
            {
                if (offer.HasValue)
                    offer.Write(w);
            }
            w.WriteEndArray();
        }

        if (EventStatus is not null)
            w.WriteString("eventStatus", EventStatus);

        if (EventAttendanceMode is not null)
            w.WriteString("eventAttendanceMode", EventAttendanceMode);

        if (Url is not null)
            w.WriteString("url", Url);

        if (Performer is not null)
            w.WriteString("performer", Performer);

        if (AggregateRating is { HasValue: true })
        {
            w.WritePropertyName("aggregateRating");
            AggregateRating.Write(w);
        }

        if (IsAccessibleForFree is not null)
            w.WriteBoolean("isAccessibleForFree", IsAccessibleForFree.Value);

        if (MaximumAttendeeCapacity is not null)
            w.WriteNumber("maximumAttendeeCapacity", MaximumAttendeeCapacity.Value);

        if (TypicalAgeRange is not null)
            w.WriteString("typicalAgeRange", TypicalAgeRange);

        if (Sponsor is not null)
            w.WriteString("sponsor", Sponsor);

        if (Duration is not null)
            w.WriteString("duration", SchemaHelpers.FormatDuration(Duration.Value));

        w.WriteEndObject();
    }
}
