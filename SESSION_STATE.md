# SESSION_STATE.md

**Last Updated:** 2026-01-19
**Current Session:** SESSION_002_beacon_fix

---

## Current Status

**Status:** SESSION 002 COMPLÈTE
**Next Action:** Créer release GitHub v1.0.1 + développer l'app Android

---

## Quick Resume

Pour reprendre : `"continue"` ou `"let's continue"`

---

## Recent Sessions

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

**Completed:**
- [x] Installer .NET SDK (10.0.101)
- [x] Compiler le projet (`dotnet build -c Release`)
- [x] Tester dans Subnautica (SUCCÈS - ping et state fonctionnels)
- [x] Commit des fichiers créés (0b13751)
- [x] Créer zip release (`release/MapAPI_v1.0.0.zip`)
- [x] Créer CONNECTION.md (doc pour app Android)

**À faire (utilisateur):**
- [ ] Créer release GitHub v1.0.0 avec le zip
- [ ] Commit CONNECTION.md

---

## Project Structure

```
E:\SubnauticaMap\
├── CLAUDE.md
├── PROJET.md
├── SESSION_STATE.md
├── CONNECTION.md          # Doc API pour app Android
├── release/
│   └── MapAPI_v1.0.0.zip  # Release prête pour GitHub
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

## Next Steps (Next Session)

1. Vérifier que `dotnet --version` fonctionne
2. `cd E:\SubnauticaMap\MapAPI && dotnet build -c Release`
3. Le build copiera automatiquement vers `BepInEx\plugins\MapAPI\`
4. Lancer Subnautica et tester `http://localhost:63030/api/ping`
5. Commit et push des fichiers
