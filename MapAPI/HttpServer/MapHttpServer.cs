/**
 * @file MapHttpServer.cs
 * @description EmbedIO HTTP server wrapper for MapAPI
 * @created 2025-01-19
 */

using System;
using EmbedIO;
using EmbedIO.WebApi;
using Swan.Logging;

namespace MapAPI.HttpServer
{
    /// <summary>
    /// Manages the embedded HTTP server lifecycle
    /// </summary>
    public class MapHttpServer
    {
        private readonly int _port;
        private readonly GameDataCollector _dataCollector;
        private WebServer _server;

        public MapHttpServer(int port, GameDataCollector dataCollector)
        {
            _port = port;
            _dataCollector = dataCollector;

            // Disable Swan logging (EmbedIO's logger)
            Logger.UnregisterLogger<ConsoleLogger>();
        }

        /// <summary>
        /// Start the HTTP server on a background thread
        /// </summary>
        public void Start()
        {
            var url = $"http://*:{_port}/";

            _server = new WebServer(o => o
                    .WithUrlPrefix(url)
                    .WithMode(HttpListenerMode.EmbedIO))
                .WithCors()
                .WithWebApi("/api", m => m
                    .WithController(() => new ApiController(_dataCollector)));

            // Run on background thread
            _server.RunAsync();
        }

        /// <summary>
        /// Stop the HTTP server
        /// </summary>
        public void Stop()
        {
            try
            {
                _server?.Dispose();
                _server = null;
            }
            catch (Exception ex)
            {
                Plugin.Log.LogWarning($"Error stopping server: {ex.Message}");
            }
        }
    }
}
