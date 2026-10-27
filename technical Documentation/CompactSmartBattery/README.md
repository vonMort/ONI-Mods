# Compact Smart Battery: technical documentation

A 1x2 wall-mounted Smart Battery for Oxygen Not Included. Uses original
prototype artwork, the Smart Battery's automation controls, and a
10 kJ base capacity (half the vanilla Smart Battery).

## Features

- Requires a back wall behind both occupied cells, but no floor.
- Costs 250 kg of refined metal and unlocks with the Smart Battery.
- Power connection in the bottom cell; automation output in the top cell.
- Inherits the Smart Battery's charge thresholds, hysteresis, settings
  copying, save support, heat, charge loss, decor, and other attributes.
- Uses the game's localized Smart Battery description and effect text,
  including Power links and green/red automation signal formatting.
- Includes a charge bar and red/green automation lamp. The small vanilla
  battery remains the fallback if the custom artwork is omitted.
- Supports the base game and Spaced Out! without requiring a DLC.

## Capacity and Bigger Capacity

The base capacity is initialized through `BatterySmartConfig` and then
halved: 20 kJ becomes 10 kJ in the unmodified game.

[Bigger Capacity](https://steamcommunity.com/sharedfiles/filedetails/?id=1720247784)
applies its `BatteryMultiplier` to battery instances during `Battery.OnSpawn`.
That setting also applies to this mod, without an extra multiplier or a
dependency on Bigger Capacity.

| BatteryMultiplier | Compact Smart Battery | Smart Battery |
|---|---|---|
| 1 (or Bigger Capacity disabled) | 10 kJ | 20 kJ |
| 2 | 20 kJ | 40 kJ |
| 4 | 40 kJ | 80 kJ |

Seeing 20 kJ with `BatteryMultiplier: 2` is expected, not a failure to halve
the base capacity. Set that option to 1 if you want the unmodified capacities.
The description intentionally does not hard-code a capacity: the game's
battery details display the actual capacity, including other mods' changes.

## Build

For an installable ZIP and optional GitHub release draft, use the
[local release guide](../releases.md).

Requires a .NET SDK, .NET Framework 4.8 reference assemblies, and an ONI
installation at build U59-744825 or newer. Game DLLs are referenced locally,
not bundled.

From the repository root:

```bash
dotnet build CompactSmartBattery/CompactSmartBattery.csproj -c Release
```

Default Steam roots are `~/.local/share/Steam` on Linux and
`C:\Program Files (x86)\Steam` on Windows. Override `SteamFolder` if needed,
or pass the full Managed directory:

```bash
dotnet build CompactSmartBattery/CompactSmartBattery.csproj -c Release \
  -p:GameFolder="/path/to/OxygenNotIncluded/OxygenNotIncluded_Data/Managed"
```

`GameAssemblies` optionally overrides only the ONI/Harmony reference
directory; Unity is still loaded from `GameFolder`.

The package is written to `CompactSmartBattery/bin/Release/`:

```text
CompactSmartBattery.dll
mod.yaml
mod_info.yaml
translations/de.po
anim/assets/compactsmartbattery/
```

## Install or upgrade

Open the mod directory from ONI's Mods screen. Copy the package files into
a `Local/CompactSmartBattery` directory, then restart ONI and enable the mod.
To upgrade an existing installation, replace its package rather than
enabling a second copy.

**When upgrading from the former ONI-Mods project, remove `ONI-Mods.dll`
from the installed mod directory and replace it with `CompactSmartBattery.dll`.**
Do not leave both DLLs installed. The existing building ID
`CompactSmartBattery` and mod static ID `ONI_Mods.CompactSmartBattery` are
preserved for existing installations and saves.

Optional deployment copies the complete package into
`$(ModsFolder)/CompactSmartBattery`. Pass the desired parent directory:

```bash
dotnet build CompactSmartBattery/CompactSmartBattery.csproj -c Release \
  -p:ModsFolder="$HOME/.config/unity3d/Klei/Oxygen Not Included/mods/Dev"
```

Deployment does not remove legacy DLLs or disable other installed copies.

## Localization and implementation

English is the source language. `translations/de.po` translates the mod's
name and back-wall requirement. The remaining building text and automation
tooltips come directly from the localized game strings.

`CompactSmartBatteryConfig` inherits `BatterySmartConfig`, changing only
the footprint, artwork, mass, mounting rule, automation port location, and
base capacity. Narrow Harmony patches register strings before prefab
creation, add the building to the Smart Battery's research and Power menu
without duplicates, load translations, and skip the separate logic meter
only when this prefab uses the fallback artwork. With custom artwork, a dedicated
visual controller replaces the vanilla powered-active animation controller.
It animates charging only after a tick with incoming power and a net increase
in stored energy, without changing battery operation or automation.

## Custom artwork

See the [artwork quick guide](artwork.md) for sprite assembly,
required animations, charge-bar frames, automation-lamp states, and export
paths. The included `compactsmartbattery_kanim` asset is selected automatically;
without it the mod keeps its vanilla fallback. Exported `.bytes` and `.png`
files under `anim/` are included in build output and deployment, while
editable files under `artwork/` are not shipped.

For in-game acceptance, compare capacities with Bigger Capacity disabled
and enabled; inspect the localized links and automation colors; exercise
both charge thresholds; remove and restore the back wall; and save/reload.
