using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace CompactSmartBattery;

public class CompactSmartBatteryConfig : BatterySmartConfig
{
    public new const string ID = "CompactSmartBattery";

    public override BuildingDef CreateBuildingDef()
    {
        BuildingDef def = base.CreateBuildingDef();
        def.PrefabID = ID;
        def.name = ID;
        def.InitDef();
        def.WidthInCells = 1;
        def.AnimFiles = new[] { BatteryArtwork.GetAnimation() };
        def.MassForTemperatureModification *= 250f / def.Mass[0];
        def.Mass = new[] { 250f };
        def.BuildLocationRule = BuildLocationRule.OnBackWall;
        def.GenerateOffsets();
        def.LogicOutputPorts = new List<LogicPorts.Port>
        {
            LogicPorts.Port.OutputPort(
                BatterySmart.PORT_ID,
                new CellOffset(0, 1),
                STRINGS.BUILDINGS.PREFABS.BATTERYSMART.LOGIC_PORT,
                STRINGS.BUILDINGS.PREFABS.BATTERYSMART.LOGIC_PORT_ACTIVE,
                STRINGS.BUILDINGS.PREFABS.BATTERYSMART.LOGIC_PORT_INACTIVE,
                show_wire_missing_icon: true)
        };
        SoundEventVolumeCache.instance.AddVolume("batterysm_kanim", "Battery_rattle", NOISE_POLLUTION.NOISY.TIER1);
        return def;
    }

    public override void DoPostConfigureComplete(GameObject go)
    {
        base.DoPostConfigureComplete(go);
        // Keep the 10 kJ base capacity; spawn-time multipliers such as Bigger Capacity apply later.
        go.GetComponent<BatterySmart>().capacity *= 0.5f;
        if (BatteryArtwork.UsesCustomAnimation(go.GetComponent<BatterySmart>()))
        {
            // Vanilla animates any active battery, including one that is discharging.
            go.GetComponent<StateMachineController>().cmpdef.defs.RemoveAll(
                def => def is PoweredActiveController.Def);
            go.AddOrGet<BatteryChargingAnimation>();
        }
    }

    public override void DoPostConfigureUnderConstruction(GameObject go)
    {
        base.DoPostConfigureUnderConstruction(go);
        KBatchedAnimController controller = go.GetComponent<KBatchedAnimController>();
        if (controller.AnimFiles[0].name == BatteryArtwork.AnimationName)
        {
            controller.initialAnim = "build";
            controller.defaultAnim = "build";
        }
    }
}