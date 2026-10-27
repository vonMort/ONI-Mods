# ONI Mods

Mods for Oxygen Not Included. This repository contains source code, runtime
resources, and player-facing descriptions. Installable mod packages are distributed through
[GitHub Releases](https://github.com/vonMort/ONI-Mods/releases).

| Mod | Description |
|---|---|
| [Compact Smart Battery](CompactSmartBattery/README.md) | A 1x2 wall-mounted Smart Battery with half the base capacity |

Each mod's README contains its player-facing description and compatibility details.

## Installation

Download the ZIP for your mod from GitHub Releases and extract the contained
mod directory into ONI's `mods/Local/` directory. You can open the mod directory
from the game's Mods screen. Enable the mod and restart ONI.

For updates, replace the existing local mod directory with the extracted one.
Do not enable both a local and a Steam Workshop copy of the same mod.

Technical documentation, editable artwork, and release tools are maintained
in a separate development repository.

## AI transparency

Development is assisted by a multi-model AI system running locally on an
NVIDIA DGX Spark, supplemented by remote Mistral services. Specialized models
support coding, reasoning, and local knowledge retrieval.
