/**
 * @file Plugin.cs
 * @description BepInEx plugin entry point for MapAPI
 * @created 2025-01-19
 */

using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using MapAPI.HttpServer;
using UnityEngine;

namespace MapAPI
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "com.subnautica.mapapi";
        public const string PluginName = "MapAPI";
        public const string PluginVersion = "1.0.0";

        internal static ManualLogSource Log { get; private set; }

        // Configuration
        public static ConfigEntry<int> ServerPort { get; private set; }
        public static ConfigEntry<float> RefreshInterval { get; private set; }
        public static ConfigEntry<bool> EnableServer { get; private set; }

        // Components
        private MapHttpServer _httpServer;
        private GameDataCollector _dataCollector;

        private void Awake()
        {
            Log = Logger;

            // Bind configuration
            ServerPort = Config.Bind(
                "Server",
                "Port",
                63030,
                "HTTP server port"
            );

            RefreshInterval = Config.Bind(
                "Server",
                "RefreshInterval",
                1.0f,
                "Data refresh interval in seconds"
            );

            EnableServer = Config.Bind(
                "Server",
                "Enabled",
                true,
                "Enable HTTP server"
            );

            Log.LogInfo($"{PluginName} v{PluginVersion} loaded");

            // Initialize data collector
            _dataCollector = new GameDataCollector(RefreshInterval.Value);

            // Start HTTP server if enabled
            if (EnableServer.Value)
            {
                StartHttpServer();
            }
        }

        private void StartHttpServer()
        {
            try
            {
                _httpServer = new MapHttpServer(ServerPort.Value, _dataCollector);
                _httpServer.Start();
                Log.LogInfo($"HTTP server started on port {ServerPort.Value}");
                Log.LogInfo($"Access at: http://localhost:{ServerPort.Value}/api/state");
            }
            catch (System.Exception ex)
            {
                Log.LogError($"Failed to start HTTP server: {ex.Message}");
            }
        }

        private void Update()
        {
            // Update game data on main thread
            _dataCollector?.Tick();
        }

        private void OnDestroy()
        {
            // Clean shutdown
            if (_httpServer != null)
            {
                _httpServer.Stop();
                Log.LogInfo("HTTP server stopped");
            }
        }
    }
}
