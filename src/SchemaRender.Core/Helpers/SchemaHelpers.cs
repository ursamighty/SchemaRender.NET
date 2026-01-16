namespace SchemaRender.Helpers;

/// <summary>
/// Shared helper methods for schema serialization.
/// </summary>
internal static class SchemaHelpers
{
    /// <summary>
    /// Formats a TimeSpan as an ISO 8601 duration string (e.g., "PT45M", "PT2H30M").
    /// </summary>
    /// <param name="duration">The duration to format.</param>
    /// <returns>An ISO 8601 duration string.</returns>
    internal static string FormatDuration(TimeSpan duration)
    {
        // ISO 8601 duration format
        if (duration.TotalHours >= 1)
            return $"PT{(int)duration.TotalHours}H{duration.Minutes}M";
        return $"PT{(int)duration.TotalMinutes}M";
    }
}
