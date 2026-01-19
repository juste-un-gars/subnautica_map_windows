/**
 * @file BeaconInfo.cs
 * @description DTO for beacon/ping information
 * @created 2025-01-19
 */

namespace MapAPI.Models
{
    /// <summary>
    /// Represents a beacon/ping marker
    /// </summary>
    public class BeaconInfo
    {
        /// <summary>
        /// Unique identifier for the beacon
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Display label
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// World position
        /// </summary>
        public Vector3Info Position { get; set; }

        /// <summary>
        /// Color index (0-7)
        /// </summary>
        public int ColorIndex { get; set; }

        /// <summary>
        /// Whether beacon is visible on HUD
        /// </summary>
        public bool Visible { get; set; }
    }
}
