# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

**Key Documentation Files:**
- **[CLAUDE.md](CLAUDE.md)** - Project overview, objectives, architecture, session management
- **[SESSION_STATE.md](SESSION_STATE.md)** - Current session status and recent work
- **[.claude/REFERENCE.md](.claude/REFERENCE.md)** - Quick reference: URLs, credentials, cmdlets
- **[PROJET.md](PROJET.md)** - Detailed project specifications (FR)

---

## Project Context

**Project Name:** SubnauticaMapCompanion
**Repository:** https://github.com/juste-un-gars/subnautica_map_windows
**Tech Stack:** BepInEx, Nautilus, EmbedIO, Unity/Mono
**Primary Language(s):** C# (.NET Framework 4.7.2)
**Key Dependencies:**
- BepInEx (modding framework)
- Nautilus (Subnautica modding API)
- EmbedIO 3.x (embedded HTTP server)

**Architecture Pattern:** BepInEx Plugin + Embedded HTTP Server
**Development Environment:** Windows, Visual Studio / dotnet CLI

---

## Project Overview

### Objective
Create a **Subnautica mod** that exposes game data via an embedded HTTP server, enabling a future Android app to display the player's position in real-time on a map.

### Architecture
```
┌────────────────────────────────────┐
│         MOD SUBNAUTICA             │
│  (BepInEx Plugin + HTTP Server)    │
│                                    │
│  • Collects data every 1s          │
│  • HTTP server on port 63030       │
│  • Exposes GET /api/state (JSON)   │
└────────────────┬───────────────────┘
                 │ HTTP JSON (polling 1x/sec)
                 │ (local WiFi network)
                 ▼
┌────────────────────────────────────┐
│         ANDROID APP (future)       │
└────────────────────────────────────┘
```

---

## Project Structure

```
SubnauticaMapCompanion/
├── SubnauticaMapCompanion.csproj    # C# project (.NET Framework 4.7.2)
├── Plugin.cs                         # BepInEx entry point [BepInPlugin]
├── GameDataCollector.cs              # Game data collection (Update loop)
├── HttpServer/
│   ├── MapHttpServer.cs              # EmbedIO server start/stop
│   └── ApiController.cs              # Controller for GET /api/state
├── Models/
│   ├── GameState.cs                  # Main DTO
│   ├── PlayerInfo.cs                 # Position, heading, depth, biome
│   ├── BeaconInfo.cs                 # Player beacons
│   ├── VehicleInfo.cs                # Seamoth, Cyclops, Prawn
│   └── TimeInfo.cs                   # Day/night cycle
└── Config/
    └── PluginConfig.cs               # Configuration (port, refresh interval)
```

---

## Build Commands

```bash
# Create project
dotnet new classlib -n SubnauticaMapCompanion -f net472

# Build
dotnet build -c Release

# Deploy (copy to BepInEx plugins folder)
# Copy SubnauticaMapCompanion.dll to:
# $(SUBNAUTICA_PATH)/BepInEx/plugins/SubnauticaMapCompanion/
```

### Environment Variable
Set `SUBNAUTICA_PATH` to your Subnautica installation directory for reference resolution.

---

## API Specification

### Main Endpoint
```
GET http://<PC_IP>:63030/api/state
```

### Response Format
```json
{
  "timestamp": 1705590000,
  "player": {
    "position": { "x": -234.5, "y": -89.2, "z": 156.7 },
    "heading": 145.3,
    "depth": 89.2,
    "biome": "mushroomForest"
  },
  "time": {
    "dayNightValue": 0.35,
    "isDay": true
  },
  "beacons": [...],
  "vehicles": [...]
}
```

### Health Endpoint
```
GET http://<PC_IP>:63030/api/ping
→ { "status": "ok", "version": "1.0.0" }
```

---

## Key Game Classes

```csharp
// Player position
Player.main.transform.position  // Vector3 (x=E/W, y=depth, z=N/S)
Player.main.transform.eulerAngles.y  // heading 0-360°

// Depth
Ocean.GetDepthOf(Player.main.gameObject)

// Biome
LargeWorldStreamer.main.GetBiome(Player.main.transform.position)

// Day/Night
DayNightCycle.main.IsDay()
DayNightCycle.main.timePassedAsFloat

// Beacons
PingManager.pings  // Dictionary<PingType, List<PingInstance>>

// Vehicles
Vehicle[] vehicles = UnityEngine.Object.FindObjectsOfType<Vehicle>();
SubRoot[] cyclops = UnityEngine.Object.FindObjectsOfType<SubRoot>();
```

### Safety Checks
Always verify objects exist before access:
```csharp
if (Player.main == null) return null;
if (LargeWorldStreamer.main == null) return null;
```

---

## Technical Considerations

### Thread Safety
- HTTP server runs on a separate thread
- Game data access (Unity) must be on main thread
- Collect data in `Update()`, store in thread-safe object
- HTTP server reads from thread-safe cache

### Performance
- Throttle collection to 1x/second (not every frame)
- Reuse objects to avoid allocations in Update loop

### Error Handling
- Return empty state or HTTP 503 when `Player.main` is null
- Log errors to BepInEx console

---

## Coordinate System

```
        North (+Z)
           ▲
           │
West  ◄────┼────► East (+X)
(-X)       │
           ▼
        South (-Z)

Depth: Y negative = deeper
       Y = 0 → water surface
       Y = -1400 → Lava Lakes
```

**World dimensions:** ~4000m x 4000m (-2000 to +2000 on X and Z)

---

## File Encoding Standards

- **All files:** UTF-8 with LF (Unix) line endings
- **Timestamps:** ISO 8601 (YYYY-MM-DD HH:mm)
- **Time format:** 24-hour (HH:mm)

---

## Claude Code Session Management

### Quick Start (TL;DR)

**Continue work:** `"continue"` or `"let's continue"`
**New session:** `"new session: Feature Name"` or `"start new session"`

**Claude handles everything automatically** - no need to manage session numbers or files manually.

---

### Session File Structure

**Two-Tier System:**
1. **SESSION_STATE.md** (root) - Overview and index of all sessions
2. **.claude/sessions/SESSION_XXX_[name].md** - Detailed session files

**Naming:** `SESSION_001_project_setup.md` (three digits, 001-999)

**Session Limits (Recommendations):**
- Max tasks: 20-25 per session
- Max files modified: 15-20 per session
- Recommended duration: 2-4 hours

---

### Session Management Rules

#### MANDATORY Actions:
1. Always read CLAUDE.md first for context
2. Always read current session file
3. Update session in real-time as tasks complete
4. Document all code (headers, functions, .EXPLAIN.md)
5. Never lose context between messages
6. Auto-save progress every 10-15 minutes
7. Verify documentation before marking tasks complete

#### When to Create New Session:
- New major feature/module
- Completed session goal
- Different project area
- After long break
- Approaching session limits

---

### Common Commands

**Continue:** "continue", "let's continue", "keep going"
**New session:** "new session: [name]", "start new session"
**Save:** "save progress", "checkpoint"
**Update:** "update session", "update SESSION_STATE.md"
**Document:** "document files", "create EXPLAIN files"
**Audit:** "check documentation", "audit docs"

---

## Documentation Standards

### Required Documentation Elements

#### 1. File Header (All Files)
```csharp
/**
 * @file filename.cs
 * @description Brief file purpose
 * @session SESSION_XXX
 * @created YYYY-MM-DD
 * @author SubnauticaMapCompanion
 */
```

#### 2. Function Documentation
```csharp
/// <summary>
/// Brief function description
/// </summary>
/// <param name="paramName">Parameter description</param>
/// <returns>Return description</returns>
/// <exception cref="Exception">Error conditions</exception>
```

---

## Git Workflow Integration

### Branch Naming
**Format:** `feature/session-XXX-brief-description`
**Examples:** `feature/session-001-http-server`, `bugfix/session-003-null-check`

### Commit Messages
```
Session XXX: [Brief summary]

[Details]

Changes:
- Change 1
- Change 2

Session: SESSION_XXX
```

---

## Validation Checklist (MVP)

- [ ] HTTP server starts when game launches
- [ ] GET /api/state returns valid JSON
- [ ] Player position updates every second
- [ ] Depth and biome are correct
- [ ] Beacon list with positions
- [ ] Vehicle list with positions
- [ ] Server stops cleanly on game exit

### Manual Testing
1. Launch Subnautica with mod
2. Open browser: `http://localhost:63030/api/state`
3. Verify JSON updates
4. Test from another device on same network

---

## References

- Nautilus Documentation: https://subnauticamodding.github.io/Nautilus/
- Subnautica Modding Guide: https://mroshaw.github.io/Subnautica/
- Similar Project (archived): https://github.com/MartinSGill/SubnauticaWatcher
- EmbedIO Documentation: https://github.com/unosquare/embedio

---

**Last Updated:** 2026-01-19
**Version:** 1.0.0
