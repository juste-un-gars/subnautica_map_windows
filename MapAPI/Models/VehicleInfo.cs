/**
 * @file VehicleInfo.cs
 * @description DTO for vehicle information
 * @created 2025-01-19
 */

namespace MapAPI.Models
{
    /// <summary>
    /// Represents a player vehicle (Seamoth, Cyclops, Prawn)
    /// </summary>
    public class VehicleInfo
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Vehicle type: "Seamoth", "Cyclops", "Exosuit"
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Custom or default vehicle name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// World position
        /// </summary>
        public Vector3Info Position { get; set; }
    }
}
