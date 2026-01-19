/**
 * @file GameDataCollector.cs
 * @description Collects game data from Unity/Subnautica on main thread
 * @created 2025-01-19
 */

using System;
using System.Collections.Generic;
using MapAPI.Models;
using UnityEngine;

namespace MapAPI
{
    /// <summary>
    /// Collects game data at a throttled rate and stores it thread-safely
    /// </summary>
    public class GameDataCollector
    {
        private readonly float _refreshInterval;
        private float _lastUpdateTime;
        private readonly object _lock = new object();
        private GameState _currentState;

        public GameDataCollector(float refreshInterval)
        {
            _refreshInterval = refreshInterval;
            _lastUpdateTime = 0f;
        }

        /// <summary>
        /// Called every frame from Plugin.Update(), throttled internally
        /// </summary>
        public void Tick()
        {
            float currentTime = Time.time;
            if (currentTime - _lastUpdateTime < _refreshInterval)
                return;

            _lastUpdateTime = currentTime;
            CollectData();
        }

        /// <summary>
        /// Get current game state (thread-safe)
        /// </summary>
        public GameState GetCurrentState()
        {
            lock (_lock)
            {
                return _currentState;
            }
        }

        /// <summary>
        /// Check if game data is available
        /// </summary>
        public bool IsGameReady()
        {
            return Player.main != null;
        }

        private void CollectData()
        {
            if (!IsGameReady())
            {
                lock (_lock)
                {
                    _currentState = null;
                }
                return;
            }

            try
            {
                var state = new GameState
                {
                    Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    Player = CollectPlayerInfo(),
                    Time = CollectTimeInfo(),
                    Beacons = CollectBeacons(),
                    Vehicles = CollectVehicles()
                };

                lock (_lock)
                {
                    _currentState = state;
                }
            }
            catch (Exception ex)
            {
                Plugin.Log.LogError($"Error collecting data: {ex.Message}");
            }
        }

        private PlayerInfo CollectPlayerInfo()
        {
            var player = Player.main;
            if (player == null) return null;

            var position = player.transform.position;
            var heading = player.transform.eulerAngles.y;
            var depth = Ocean.GetDepthOf(player.gameObject);

            string biome = "unknown";
            try
            {
                biome = LargeWorld.main?.GetBiome(position) ?? "unknown";
            }
            catch
            {
                biome = "unknown";
            }

            return new PlayerInfo
            {
                Position = new Vector3Info(position),
                Heading = heading,
                Depth = depth,
                Biome = biome
            };
        }

        private TimeInfo CollectTimeInfo()
        {
            if (DayNightCycle.main == null) return null;

            return new TimeInfo
            {
                DayNightValue = DayNightCycle.main.timePassedAsFloat,
                IsDay = DayNightCycle.main.IsDay()
            };
        }

        private List<BeaconInfo> CollectBeacons()
        {
            var beacons = new List<BeaconInfo>();

            try
            {
                // Find all Beacon objects in the world (player-placed beacons)
                var beaconObjects = UnityEngine.Object.FindObjectsOfType<Beacon>();
                int index = 0;

                foreach (var beacon in beaconObjects)
                {
                    if (beacon == null) continue;

                    // Get the label from the beacon
                    string label = beacon.beaconLabel?.GetLabel() ?? "Beacon";

                    // Get color from the ping instance if available
                    int colorIndex = 0;
                    bool visible = true;

                    var pingInstance = beacon.GetComponent<PingInstance>();
                    if (pingInstance != null)
                    {
                        colorIndex = pingInstance.colorIndex;
                        visible = pingInstance.visible;
                    }

                    beacons.Add(new BeaconInfo
                    {
                        Id = $"beacon_{index++}",
                        Label = label,
                        Position = new Vector3Info(beacon.transform.position),
                        ColorIndex = colorIndex,
                        Visible = visible
                    });
                }

                // Also collect Signal pings (story signals, lifepods, etc.)
                var signalPings = UnityEngine.Object.FindObjectsOfType<PingInstance>();
                foreach (var ping in signalPings)
                {
                    if (ping == null || ping.origin == null) continue;

                    // Skip if it's a beacon (already collected) or vehicle
                    if (ping.GetComponent<Beacon>() != null) continue;
                    if (ping.GetComponent<Vehicle>() != null) continue;
                    if (ping.GetComponent<SubRoot>() != null) continue;

                    // Only include visible signals
                    if (!ping.visible) continue;

                    beacons.Add(new BeaconInfo
                    {
                        Id = $"signal_{index++}",
                        Label = ping.GetLabel() ?? "Signal",
                        Position = new Vector3Info(ping.origin.position),
                        ColorIndex = ping.colorIndex,
                        Visible = ping.visible
                    });
                }
            }
            catch (Exception ex)
            {
                Plugin.Log.LogWarning($"Error collecting beacons: {ex.Message}");
            }

            return beacons;
        }

        private List<VehicleInfo> CollectVehicles()
        {
            var vehicles = new List<VehicleInfo>();
            int index = 0;

            try
            {
                // Collect Seamoth and Exosuit (Prawn)
                var vehicleObjects = UnityEngine.Object.FindObjectsOfType<Vehicle>();
                foreach (var vehicle in vehicleObjects)
                {
                    if (vehicle == null) continue;

                    string type = vehicle.GetType().Name;
                    if (type == "Exosuit") type = "Exosuit"; // Prawn Suit

                    vehicles.Add(new VehicleInfo
                    {
                        Id = $"vehicle_{index++}",
                        Type = type,
                        Name = vehicle.name ?? type,
                        Position = new Vector3Info(vehicle.transform.position)
                    });
                }

                // Collect Cyclops (SubRoot, not Vehicle)
                var subRoots = UnityEngine.Object.FindObjectsOfType<SubRoot>();
                foreach (var subRoot in subRoots)
                {
                    if (subRoot == null) continue;

                    // Filter only Cyclops (not player bases)
                    if (!subRoot.isCyclops) continue;

                    vehicles.Add(new VehicleInfo
                    {
                        Id = $"vehicle_{index++}",
                        Type = "Cyclops",
                        Name = subRoot.name ?? "Cyclops",
                        Position = new Vector3Info(subRoot.transform.position)
                    });
                }
            }
            catch (Exception ex)
            {
                Plugin.Log.LogWarning($"Error collecting vehicles: {ex.Message}");
            }

            return vehicles;
        }
    }
}
