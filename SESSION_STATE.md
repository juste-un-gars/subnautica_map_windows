# SESSION_STATE.md

**Last Updated:** 2026-01-20
**Current Session:** SESSION_004_heading_fix

---

## Current Status

**Status:** SESSION 004 COMPLÈTE
**Next Action:** Développer l'app Android companion

---

## Quick Resume

Pour reprendre : `"continue"` ou `"let's continue"`

---

## Recent Sessions

### SESSION_004: Heading Fix (2026-01-20)
**Goal:** Corriger le heading qui retourne 0 au début d'une nouvelle partie
**Status:** COMPLÈTE

**Problème:** Le heading (direction du joueur) retournait 0 au début d'une nouvelle partie, mais fonctionnait dans une partie avancée.

**Cause:** `player.transform.eulerAngles.y` ne fournit pas la bonne rotation au début du jeu (probablement lié à l'initialisation du joueur dans le lifepod).

**Solution:** Utiliser la rotation de la caméra (`MainCameraControl.main`) au lieu du transform du joueur, avec fallback sur `Camera.main` puis `player.transform`.

**Fichiers modifiés:**
- `MapAPI/GameDataCollector.cs` (lignes 100-116)

**Commits:**
- `c417f48` - Session 004: Fix heading returning 0 at game start

**Releases:**
- v1.0.2: https://github.com/juste-un-gars/subnautica_map_windows/releases/tag/v1.0.2

---

### SESSION_003: README Documentation (2026-01-20)
**Goal:** Créer un README.md en anglais pour le projet
**Status:** COMPLÈTE

**Réalisé:**
- Création de README.md avec documentation complète en anglais
- Instructions d'installation pour BepInEx
- Documentation des endpoints API avec exemples JSON
- Guide de configuration réseau pour accès LAN
- Section troubleshooting
- Instructions de build

**Commits:**
- `18f3ccc` - Add English README with installation and API documentation

---

### SESSION_002: Beacon Fix (2026-01-19)
**Goal:** Corriger la collecte des beacons (tableau vide dans l'API)
**Status:** COMPLÈTE

**Problème:** L'API retournait `"Beacons": []` même avec des balises placées

**Cause:** La méthode `CollectBeacons()` utilisait la réflexion pour accéder à `PingManager.pings` (champ privé) qui ne fonctionnait pas correctement.

**Solution:** Réécriture de `CollectBeacons()` avec une approche directe :
- Utilisation de `FindObjectsOfType<Beacon>()` pour les balises joueur
- Utilisation de `FindObjectsOfType<PingInstance>()` pour les signaux (lifepods, story)
- Récupération correcte des labels via `beacon.beaconLabel?.GetLabel()`

**Fichiers modifiés:**
- `MapAPI/GameDataCollector.cs` (lignes 134-202)

**Commits:**
- `00d1dd1` - Session 002: Fix beacon collection and add API documentation

**Releases:**
- v1.0.1: https://github.com/juste-un-gars/subnautica_map_windows/releases/tag/v1.0.1

---

### SESSION_001: Project Setup (2025-01-19)
**Goal:** Créer la structure complète du mod MapAPI
**Status:** COMPLÈTE

**Completed:**
- [x] CLAUDE.md créé et configuré
- [x] Git initialisé et connecté au repo distant
- [x] Structure projet MapAPI créée
- [x] MapAPI.csproj avec références BepInEx/Unity/Subnautica
- [x] Models créés (GameState, PlayerInfo, BeaconInfo, VehicleInfo, TimeInfo, Vector3Info)
- [x] Plugin.cs (point d'entrée BepInEx)
- [x] GameDataCollector.cs (collecte données jeu)
- [x] HttpServer/MapHttpServer.cs (serveur EmbedIO)
- [x] HttpServer/ApiController.cs (endpoints API)
- [x] Tester dans Subnautica (SUCCÈS - ping et state fonctionnels)
- [x] Release v1.0.0 créée

---

## Project Structure

```
E:\SubnauticaMap\
├── CLAUDE.md
├── PROJET.md
├── SESSION_STATE.md
├── README.md              # Documentation en anglais (GitHub)
├── CONNECTION.md          # Doc API pour app Android
├── release/
│   ├── MapAPI_v1.0.0.zip
│   └── MapAPI_v1.0.1.zip  # Latest
└── MapAPI/
    ├── MapAPI.csproj
    ├── Plugin.cs
    ├── GameDataCollector.cs
    ├── Models/
    │   ├── GameState.cs
    │   ├── PlayerInfo.cs
    │   ├── BeaconInfo.cs
    │   ├── VehicleInfo.cs
    │   ├── TimeInfo.cs
    │   └── Vector3Info.cs
    ├── HttpServer/
    │   ├── MapHttpServer.cs
    │   └── ApiController.cs
    └── bin/Release/net472/  # DLLs compilées
        ├── MapAPI.dll
        ├── EmbedIO.dll
        ├── Swan.Lite.dll
        └── System.ValueTuple.dll
```

---

## Configuration

- **Subnautica Path:** `D:\STEAM\steamapps\common\Subnautica\`
- **Plugin Name:** MapAPI
- **Server Port:** 63030
- **API Endpoints:** `/api/state`, `/api/ping`
- **Git Remote:** https://github.com/juste-un-gars/subnautica_map_windows

---

## Next Steps (Prochaine Session)

1. Développer l'app Android companion
2. Intégrer la carte de Subnautica
3. Afficher la position du joueur en temps réel
4. Afficher les beacons sur la carte
