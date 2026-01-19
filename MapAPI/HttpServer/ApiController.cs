/**
 * @file ApiController.cs
 * @description Web API controller for game state endpoints
 * @created 2025-01-19
 */

using System.Threading.Tasks;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;

namespace MapAPI.HttpServer
{
    /// <summary>
    /// API controller exposing game state endpoints
    /// </summary>
    public class ApiController : WebApiController
    {
        private readonly GameDataCollector _dataCollector;

        public ApiController(GameDataCollector dataCollector)
        {
            _dataCollector = dataCollector;
        }

        /// <summary>
        /// GET /api/ping - Health check endpoint
        /// </summary>
        [Route(HttpVerbs.Get, "/ping")]
        public object GetPing()
        {
            return new
            {
                status = "ok",
                version = Plugin.PluginVersion
            };
        }

        /// <summary>
        /// GET /api/state - Get current game state
        /// </summary>
        [Route(HttpVerbs.Get, "/state")]
        public object GetState()
        {
            if (!_dataCollector.IsGameReady())
            {
                HttpContext.Response.StatusCode = 503;
                return new
                {
                    error = "Game not ready",
                    message = "Player not loaded or game is in menu"
                };
            }

            var state = _dataCollector.GetCurrentState();
            if (state == null)
            {
                HttpContext.Response.StatusCode = 503;
                return new
                {
                    error = "No data",
                    message = "Game state not yet collected"
                };
            }

            return state;
        }
    }
}
