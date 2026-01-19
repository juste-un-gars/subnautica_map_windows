/**
 * @file PlayerInfo.cs
 * @description DTO for player information
 * @created 2025-01-19
 */

namespace MapAPI.Models
{
    /// <summary>
    /// Represents player state information
    /// </summary>
    public class PlayerInfo
    {
        /// <summary>
        /// Player world position (x=E/W, y=depth, z=N/S)
        /// </summary>
        public Vector3Info Position { get; set; }

        /// <summary>
        /// Player heading in degrees (0-360)
        /// </summary>
        public float Heading { get; set; }

        /// <summary>
        /// Current depth (positive value)
        /// </summary>
        public float Depth { get; set; }

        /// <summary>
        /// Current biome name
        /// </summary>
        public string Biome { get; set; }
    }
}
