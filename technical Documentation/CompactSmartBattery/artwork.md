# Custom battery artwork: quick guide

The mod includes a prototype body, charge bar, and automation lamp.
The small vanilla battery remains the fallback if the compiled custom assets
are omitted. No C# changes are needed to replace the artwork.

The repository includes the compiled prototype under
`CompactSmartBattery/anim/` and an original resource generator at
`CompactSmartBattery/artwork/generate_prototype.py`.
Run the commands below from the repository root.
Editable SVGs, PNG sprites, Spriter source,
and converter staging output are generated locally, not committed.

## Rebuild the prototype on Linux

Requires Python 3, Inkscape, and a working kanimal-SE converter:

```bash
python3 CompactSmartBattery/artwork/generate_prototype.py
/path/to/kanimal-cli kanim \
  CompactSmartBattery/artwork/generated/compactsmartbattery.scml \
  -o CompactSmartBattery/artwork/converted
```

The generator overwrites `CompactSmartBattery/artwork/source/` and
`CompactSmartBattery/artwork/generated/`.
Back up hand-edited artwork before rerunning it. `source/ui_0.svg` and
`generated/ui_0.png` show the composed prototype.

For modern Linux distributions, the upstream .NET Core 3.1 binary can
fail on ICU/OpenSSL dependencies. Do not downgrade system libraries.
The [modernized fork](https://github.com/xihaoshangdi/kanimal-SE-arm64)
targets .NET 10 and ImageSharp; despite its name it can be published for
Linux x64 with:

```bash
dotnet publish kanimal-cli/kanimal-cli.csproj -c Release \
  -r linux-x64 --self-contained true -o modern-linux
```

Run its `kanimal-cli --help` first. Convert without `-i`: this prototype
uses discrete sprite swaps. Keep all PNG sprites beside the SCML.
Copy the resulting animation/build binaries to the export directory below;
rename the atlas `compactsmartbattery.png` to `compactsmartbattery_0.png`.
The prototype has ten clips, 11 charge frames, two lamp frames, and a
12-frame charging loop with alternating bolts and subtle horizontal vibration.
After conversion, round-trip to SCML to check those counts before replacing
the compiled assets. Verify scale, anchors, and appearance in ONI.

## 1. Draw and assemble

Use an ONI-compatible KAnim exporter (for example a Spriter SCML workflow
with a compatible KAnim conversion tool). ONI does **not** load an SCML,
GIF, or a loose sprite sheet directly. Keep editable sources under
`CompactSmartBattery/artwork/`, outside the packaged `anim/` directory;
export only the compiled files there.

Draw separate transparent PNG parts: casing, mounting bracket, charge
bar/frame, and lamp. Use your own artwork. Local vanilla battery assets
can serve as layout/scale references; do not redistribute extracted game
assets as your source art.

Fit the body into **1 cell wide by 2 cells tall**. Preserve the small
battery's export scale and ground-relative pivot; do not assume one atlas
pixel equals one world unit. Align the bottom of the casing with the bottom
of the lower cell. Power connects at (0,0), automation at (0,1).
Put the mounting bracket on the back of the casing, not underneath it.

Create these **symbols** (case-sensitive):

| Symbol | Purpose |
|---|---|
| `ui` | Complete static icon used by the build menu |
| `meter_target` | Charge-bar anchor, placed in every completed-building body pose |
| `logicmeter_target` | Automation-lamp anchor, placed in every completed-building body pose |
| `meter_fill`, `meter_frame`, `meter_OL` | Recommended vanilla-compatible names for charge display parts |

Anchors must be actual exported sprite symbols, not editor-only bones or
points. Keep them present and enabled in every completed-building body frame.
Use fully transparent marker sprites: previews and construction do not create
the meters that normally hide these symbols. Their transforms must still export.
Meter and lamp animations are rendered separately, in front of the
body: position their parts relative to their respective anchor origins.
Do not bake a second bar/lamp into the casing. Preview the composed result
to catch doubled offsets and incorrect pivots.

## 2. Create the animations

All clips belong to **one** exported KAnim. Names are case-sensitive:

| Animation | Required content |
|---|---|
| `ui` | One frame containing the complete `ui` symbol |
| `place` | Complete placement outline; no visible markers or separate meters |
| `build` | Construction blueprint outline, tinted by the game's construction renderer |
| `off` | Non-operational body pose with both anchors |
| `on` | Operational body pose with both anchors |
| `working_pre` | Short transition into activity, with both anchors |
| `working_loop` | Seamless charging loop, with both anchors moving with the body |
| `working_pst` | Short transition out of activity, with both anchors |
| `meter` | Charge states ordered from completely empty to completely full |
| `logicmeter` | Exactly two frames: frame 0 red/off, frame 1 green/on |

The static body/transition clips can each be one identical frame.
They must still exist. An optional additional `idle` pose is fine,
but does not replace `off` or `on`.

The custom visual controller replaces the vanilla powered-active animation
controller only for this prefab with custom artwork. It selects `working_loop`
only when the completed energy tick received power and gained stored energy.
Discharge, idle, and a green automation signal alone do not trigger it.
This does not change `Operational.IsActive`, heat generation, or automation.

**Charge bar:** use, for example, 11 equally timed exported frames for
empty, 10%, ..., full, with identical casing-relative placement. Include
the frame and fill in the `meter` clip, not just the fill. The vanilla
`MeterController` picks a paused frame from the actual charge fraction;
it does not play this clip as a timed filling animation. Its
`MinMaxStepLerp` reserves the endpoint frames for exactly empty/full, so
intermediate visual steps are approximate, not exact 10% thresholds.
Export at least 3 frames, and make the last frame explicitly full.
Check the compiled frame count: some exporters resample keyframe timing.

**Automation lamp:** frame 0 represents a red output (not requesting
charge), frame 1 a green output (requesting charge). The game selects the
first/last frame from the actual automation output, including threshold
hysteresis; it is **not** simply "green while charging" or a body `on` pose.
Use shape as well as color, e.g. a dark dash versus a bright dot.
This is a graphic indicator, not a Unity light illuminating the room.

Optional `meter_cb` and `logicmeter_cb` clips are selected by vanilla for
non-default color sets. If included, preserve the same frame counts,
ordering, and alignment, using distinguishable colors/shapes.

## 3. Export and install

Export one texture atlas and the paired animation/build binaries:

```text
CompactSmartBattery/
  anim/
    assets/
      compactsmartbattery/
        compactsmartbattery_anim.bytes
        compactsmartbattery_build.bytes
        compactsmartbattery_0.png
```

The final folder name makes ONI register **`compactsmartbattery_kanim`**.
Keep the `anim/assets/<name>/` nesting exactly as shown. Use one RGBA atlas
to avoid texture-order ambiguity; keep its UV mapping consistent with the
build binary. Leave sources, backups, and editor files outside this folder.

Build normally from the repository root:

```bash
dotnet build CompactSmartBattery/CompactSmartBattery.csproj -c Release
```

The output and optional `ModsFolder` deployment include the exported
`anim/` files automatically. An incomplete export fails the build; missing
required clips/symbols in a loaded custom asset produce an explicit game
log error rather than silently reverting to vanilla.

Install the complete package, then fully restart ONI. Custom artwork also
enables the vanilla automation lamp; the fallback-only suppression is
automatically bypassed. The prefab ID, capacity, and save data do not change.

## 4. In-game checklist

- Inspect the menu icon, placement preview, construction, and finished body.
- Check scale, anchors, clipping, and layer order at different zoom levels.
- Observe the charge bar at 0%, partial charge, and 100%.
- Verify bolts/vibration during charging only; discharge and a green signal
  without incoming power must leave the body still.
- Confirm transparent anchors remain invisible in the preview, blueprint,
  completed building, and after save/reload.
- Connect automation wire and cross both configured thresholds: red and
  green must agree with the overlay, including the state between thresholds.
- Check body poses when flooded or missing a back wall, then restore operation.
- Save/reload, and inspect the log for missing animation/symbol messages.

To return to the fallback, remove the custom `anim/assets/compactsmartbattery`
directory from the installed package and rebuild into a clean output folder.
Incremental builds/deployment do not delete previously copied artwork.
