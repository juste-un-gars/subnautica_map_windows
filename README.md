# MapAPI - Subnautica Companion Mod

A BepInEx mod for Subnautica that exposes game data via an embedded HTTP server, enabling companion apps to display real-time player information on an external map.

## Features

- **Real-time player tracking**: Position, heading, depth, and current biome
- **Beacon list**: All player-placed beacons with labels and positions
- **Vehicle tracking**: Seamoth, Cyclops, and Prawn Suit locations
- **Day/night cycle**: Current time state in the game
- **HTTP API**: Simple JSON endpoints for easy integration

## Requirements

### For Players

- **Subnautica** (Steam version)
- **BepInEx 5.x** installed in Subnautica
- Windows with .NET Framework 4.7.2

### For Developers

- .NET Framework 4.7.2 SDK
- Visual Studio or dotnet CLI

## Installation

1. **Install BepInEx** (if not already installed):
   - Download [BepInEx 5.x](https://github.com/BepInEx/BepInEx/releases) for Unity Mono
   - Extract to your Subnautica folder (`steamapps/common/Subnautica/`)
   - Run the game once to generate BepInEx folders

2. **Install MapAPI**:
   - Download the latest release from [Releases](https://github.com/juste-un-gars/subnautica_map_windows/releases)
   - Extract the `MapAPI` folder to `Subnautica/BepInEx/plugins/`

3. **Final structure**:
   ```
   Subnautica/
   └── BepInEx/
       └── plugins/
           └── MapAPI/
               ├── MapAPI.dll
               ├── EmbedIO.dll
               ├── Swan.Lite.dll
               └── System.ValueTuple.dll
   ```

## Configuration

Configuration file is auto-generated at `BepInEx/config/com.subnautica.mapapi.cfg`:

| Setting | Default | Description |
|---------|---------|-------------|
| Port | 63030 | HTTP server port |
| RefreshInterval | 1.0 | Data collection interval (seconds) |
| Enabled | true | Enable/disable HTTP server |

## API Endpoints

### Base URL
```
http://<PC_IP>:63030
```

### Health Check
```
GET /api/ping
```

Response:
```json
{
  "status": "ok",
  "version": "1.0.0"
}
```

### Game State
```
GET /api/state
```

Response:
```json
{
  "Timestamp": 1768810657,
  "Player": {
    "Position": { "X": "-83.8", "Y": "-4.9", "Z": "-289.9" },
    "Heading": 145.3,
    "Depth": "4.979864",
    "Biome": "safeShallows"
  },
  "Time": {
    "DayNightValue": "39698.25",
    "IsDay": false
  },
  "Beacons": [
    {
      "Id": "beacon_0",
      "Label": "My Base",
      "Position": { "X": "100.0", "Y": "-20.0", "Z": "150.0" },
      "ColorIndex": 0,
      "Visible": true
    }
  ],
  "Vehicles": [
    {
      "Id": "vehicle_0",
      "Type": "SeaMoth",
      "Name": "SeaMoth",
      "Position": { "X": "-107.2", "Y": "-6.7", "Z": "-304.4" }
    }
  ]
}
```

Returns `503 Service Unavailable` when player is not in-game (main menu, loading).

## Coordinate System

```
        North (+Z)
           ^
           |
West  <----+----> East (+X)
(-X)       |
           v
        South (-Z)

Y axis (vertical):
  Y = 0      -> Water surface
  Y = -100   -> 100m depth
  Y = -1400  -> Lava Lakes (deepest point)
```

World dimensions: ~4000m x 4000m (-2000 to +2000 on X and Z axes)

## Network Setup

### Same-Device Access
```
http://localhost:63030/api/state
```

### LAN Access (for companion apps)

1. Find your PC's IP address:
   ```cmd
   ipconfig
   ```
   Look for "IPv4 Address" (e.g., `192.168.1.100`)

2. Allow port through Windows Firewall:
   ```powershell
   # Run as Administrator
   New-NetFirewallRule -DisplayName "Subnautica MapAPI" -Direction Inbound -Port 63030 -Protocol TCP -Action Allow
   ```

3. Access from other devices:
   ```
   http://192.168.1.100:63030/api/state
   ```

## Troubleshooting

| Problem | Solution |
|---------|----------|
| "Connection refused" | Subnautica not running or mod not loaded |
| "Connection timeout" | Firewall blocking port 63030 |
| Empty response / null data | Player is in main menu, not in-game |
| Beacons array empty | Place a beacon in-game and check again |

### Verify mod is loaded

Check the BepInEx console or `BepInEx/LogOutput.log` for:
```
[Info   : MapAPI] MapAPI v1.0.0 loaded
[Info   : MapAPI] HTTP server started on port 63030
```

## Building from Source

```bash
# Clone repository
git clone https://github.com/juste-un-gars/subnautica_map_windows.git
cd subnautica_map_windows/MapAPI

# Set Subnautica path (for references)
set SUBNAUTICA_PATH=D:\STEAM\steamapps\common\Subnautica

# Build
dotnet build -c Release

# Output: bin/Release/net472/
```

## Project Structure

```
MapAPI/
├── Plugin.cs              # BepInEx entry point
├── GameDataCollector.cs   # Game data collection (Unity main thread)
├── HttpServer/
│   ├── MapHttpServer.cs   # EmbedIO server setup
│   └── ApiController.cs   # API endpoints
└── Models/
    ├── GameState.cs       # Main response DTO
    ├── PlayerInfo.cs      # Player data
    ├── BeaconInfo.cs      # Beacon data
    ├── VehicleInfo.cs     # Vehicle data
    ├── TimeInfo.cs        # Day/night cycle
    └── Vector3Info.cs     # 3D coordinates
```

## License

MIT License

## Credits

- [BepInEx](https://github.com/BepInEx/BepInEx) - Modding framework
- [EmbedIO](https://github.com/unosquare/embedio) - Embedded HTTP server
- [Nautilus](https://github.com/SubnauticaModding/Nautilus) - Subnautica modding library

## Version History

- **v1.0.1** - Fixed beacon collection (was returning empty array)
- **v1.0.0** - Initial release with player, vehicles, beacons, and time data
