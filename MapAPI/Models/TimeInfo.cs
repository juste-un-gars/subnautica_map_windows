/**
 * @file TimeInfo.cs
 * @description DTO for day/night cycle information
 * @created 2025-01-19
 */

namespace MapAPI.Models
{
    /// <summary>
    /// Represents the current time/day-night state
    /// </summary>
    public class TimeInfo
    {
        /// <summary>
        /// Time passed as float value
        /// </summary>
        public float DayNightValue { get; set; }

        /// <summary>
        /// True if currently daytime
        /// </summary>
        public bool IsDay { get; set; }
    }
}
