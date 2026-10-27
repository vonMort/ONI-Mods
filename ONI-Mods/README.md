# Compact Smart Battery (ONI Mod)

Eine kompakte Smart Battery für Oxygen Not Included, die an eine Rückwand
montiert wird statt auf den Boden.

## Features

- **Rückwandmontage**: Nur an Rückwänden platzierbar (`BuildLocationRule.OnBackWall`,
  wie die aquatischen Pflegestationen). Kein Boden nötig.
- **Kompakt**: 1 Feld breit, 2 Felder hoch, nutzt die Grafik der kleinen
  Vanilla-Batterie (`batterysm_kanim`).
- **Smart-Battery-Verhalten**: Automationsschwellen einstellbar
  (Aktivieren/Deaktivieren), Automation-Ausgang am oberen Feld,
  Ladezustandsanzeige — identische UI wie die Vanilla Smart Battery.
- **Werte**:
  - Kapazität: 10 kJ (50 % der Smart Battery)
  - Verlust: 2/3 J/s (wie Smart Battery)
  - Baukosten: 250 kg raffiniertes Metall
  - Forschung: **Acoustics** (gemeinsam mit der Smart Battery)
  - Sonstige Werte (Hitpoints, Dekor, Wärme, Schmelzpunkt) wie Smart Battery

## Bauen

Benötigte Referenzen im Ordner `API Reference/` (aus der ONI-Installation
`OxygenNotIncluded/OxygenNotIncluded_Data/Managed/`):
- `Assembly-CSharp.dll`
- `Assembly-CSharp-firstpass.dll`
- `0Harmony.dll`

Unity-DLLs (`UnityEngine.CoreModule.dll` etc.) werden automatisch aus dem
Spiel-Ordner bezogen. Steam-Pfad ggf. übergeben:

```bash
dotnet build -p:SteamFolder="C:\Program Files (x86)\Steam"
```

Optional kann der Output direkt in den Mods-Ordner kopiert werden:

```bash
dotnet build -p:ModsFolder="%USERPROFILE%\Documents\Klei\OxygenNotIncluded\mods\Dev"
```

## Installieren

1. `dotnet build` (Release: `dotnet build -c Release`)
2. Im Spiel: Mods → Mod-Ordner öffnen
3. Dort einen Ordner `CompactSmartBattery` anlegen
4. `ONI-Mods.dll`, `mod_info.yaml` und `translations/` hineinkopieren
5. Spiel neu starten, Mod aktivieren

## Struktur

- `CompactSmartBatteryConfig.cs` — Gebäudedefinition (1x2, Rückwand,
  Smart-Battery-Komponenten, 10 kJ, 250 kg)
- `UserMod.cs` — Mod-Einstiegspunkt (`UserMod2`), Harmony-Patches:
  - `Techs.Load` → Gebäude zum Acoustics-Tech hinzufügen
  - `GeneratedBuildings.LoadGeneratedBuildings` → Strings + Plan-Screen
    (Power → Batteries, direkt nach der Smart Battery)
- `translations/de.po` — deutsche Texte (Englisch ist in `UserMod.cs` hinterlegt)

## Technische Notizen

- Die Gebäudekonfiguration wird vom Spiel automatisch per Typ-Scan gefunden
  (`IBuildingConfig`-Implementierung), kein manuelles Prefab-Register nötig.
- Die Threshold-SideScreen bindet sich automatisch über die
  `IActivationRangeTarget`-Schnittstelle der `BatterySmart`-Komponente.
- `mod_info.yaml` mit `APIVersion: 2` und `supportedContent: ALL` —
  funktioniert in Basis-Spiel und Spaced Out! ohne DLC-Pflicht.
