/**
 * @file GameState.cs
 * @description Main DTO containing all game state data
 * @created 2025-01-19
 */

using System.Collections.Generic;

namespace MapAPI.Models
{
    /// <summary>
    /// Complete game state for API response
    /// </summary>
    public class GameState
    {
        /// <summary>
        /// Unix timestamp of data collection
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// Player information
        /// </summary>
        public PlayerInfo Player { get; set; }

        /// <summary>
        /// Day/night cycle info
        /// </summary>
        public TimeInfo Time { get; set; }

        /// <summary>
        /// All beacons/pings
        /// </summary>
        public List<BeaconInfo> Beacons { get; set; } = new List<BeaconInfo>();

        /// <summary>
        /// All vehicles
        /// </summary>
        public List<VehicleInfo> Vehicles { get; set; } = new List<VehicleInfo>();
    }
}
