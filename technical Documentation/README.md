# Technical documentation

Public development and artwork documentation for contributors.
Player-facing mod descriptions live in each mod project's README.
This directory is not included in mod packages.

## Mods

| Mod | Development | Artwork |
|---|---|---|
| Compact Smart Battery | [Build, local installation, and implementation](CompactSmartBattery/README.md) | [Graphics, animation, and converter guide](CompactSmartBattery/artwork.md) |

## Shared solution

Each mod has its own sibling C# project directory, assembly, metadata,
translations, and deployment package. Build all mods from the repository root:

```bash
dotnet build ONI-Mods.sln -c Release
```

Add future mod projects as sibling directories and register them with:

```bash
dotnet sln ONI-Mods.sln add <path-to-project.csproj>
```

Use a unique assembly name, mod static ID, and deployment directory per mod.
See the individual development guides for local game reference paths and deployment.

Primary documentation and source strings are written in English.
Localized player-facing translations live within each mod's project.
