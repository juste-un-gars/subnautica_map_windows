# Session 001: Project Setup

## Date: 2025-01-19
## Duration: ~30 min
## Goal: Créer la structure complète du mod MapAPI pour Subnautica

---

## Completed Tasks

- [x] Lecture et compréhension de PROJET.md
- [x] Création de CLAUDE.md basé sur le template
- [x] Vérification configuration Git (branch main, remote origin)
- [x] Création structure dossiers MapAPI/
- [x] MapAPI.csproj avec références:
  - BepInEx.dll, 0Harmony.dll
  - UnityEngine.dll, UnityEngine.CoreModule.dll
  - Assembly-CSharp.dll
  - EmbedIO 3.5.2 (NuGet)
- [x] Models créés:
  - Vector3Info.cs
  - PlayerInfo.cs
  - BeaconInfo.cs
  - VehicleInfo.cs
  - TimeInfo.cs
  - GameState.cs
- [x] Plugin.cs - Point d'entrée BepInEx avec configuration
- [x] GameDataCollector.cs - Collecte throttlée des données jeu
- [x] HttpServer/MapHttpServer.cs - Wrapper serveur EmbedIO
- [x] HttpServer/ApiController.cs - Endpoints /api/state et /api/ping

---

## Current Status

**Currently working on:** Compilation du projet
**Blocker:** .NET SDK non installé
**Progress:** Structure complète, en attente de build

---

## Next Steps

1. [ ] Installer .NET SDK 8.0
2. [ ] Redémarrer terminal
3. [ ] `dotnet build -c Release` dans MapAPI/
4. [ ] Tester le mod dans Subnautica
5. [ ] Git commit et push

---

## Technical Decisions

- **EmbedIO:** Choisi pour sa légèreté et compatibilité Mono/Unity
- **Thread-safety:** Lock object dans GameDataCollector pour accès concurrent
- **Throttling:** Collecte limitée à 1x/sec via RefreshInterval
- **Post-build:** Copie automatique vers BepInEx/plugins/MapAPI/

---

## Files Created

### MapAPI/
- `MapAPI.csproj` - Configuration projet avec références
- `Plugin.cs` - Point d'entrée BepInEx
- `GameDataCollector.cs` - Collecte données Unity

### MapAPI/Models/
- `Vector3Info.cs` - DTO position 3D
- `PlayerInfo.cs` - DTO joueur
- `BeaconInfo.cs` - DTO balises
- `VehicleInfo.cs` - DTO véhicules
- `TimeInfo.cs` - DTO cycle jour/nuit
- `GameState.cs` - DTO principal

### MapAPI/HttpServer/
- `MapHttpServer.cs` - Serveur EmbedIO
- `ApiController.cs` - Endpoints REST

---

## Session Summary

Session initiale du projet MapAPI. Toute la structure du mod a été créée :
- Projet C# configuré avec les références Subnautica/BepInEx/Unity
- DTOs pour l'API JSON
- Collecteur de données avec thread-safety
- Serveur HTTP EmbedIO avec endpoints /api/state et /api/ping

En attente de l'installation du .NET SDK pour compiler et tester.

---

## Handoff Notes

- **Critical:** Installer .NET SDK avant de continuer
- **Command:** `dotnet build -c Release` dans le dossier MapAPI
- **Test:** `http://localhost:63030/api/ping` une fois Subnautica lancé
