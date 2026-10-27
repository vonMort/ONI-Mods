using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class CompactSmartBatteryConfig : IBuildingConfig
{
    public const string ID = "CompactSmartBattery";

    public override BuildingDef CreateBuildingDef()
    {
        float[] construction_mass = new float[1] { 250f };
        BuildingDef def = BuildingTemplates.CreateBuildingDef(
            ID,
            1,
            2,
            "batterysm_kanim",
            30,
            60f,
            construction_mass,
            MATERIALS.REFINED_METALS,
            800f,
            BuildLocationRule.OnBackWall,
            TUNING.BUILDINGS.DECOR.PENALTY.TIER2,
            NOISE_POLLUTION.NOISY.TIER1);

        def.ExhaustKilowattsWhenActive = 0f;
        def.SelfHeatKilowattsWhenActive = 0.5f;
        def.ViewMode = OverlayModes.Power.ID;
        def.AudioCategory = "Metal";
        def.RequiresPowerOutput = true;
        def.UseWhitePowerOutputConnectorColour = true;
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
        def.AddSearchTerms(SEARCH_TERMS.POWER);
        def.AddSearchTerms(SEARCH_TERMS.BATTERY);
        SoundEventVolumeCache.instance.AddVolume("batterysm_kanim", "Battery_rattle", NOISE_POLLUTION.NOISY.TIER1);
        return def;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
    {
        go.AddComponent<RequireInputs>();
        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.PowerBuilding, false);
    }

    public override void DoPostConfigureComplete(GameObject go)
    {
        BatterySmart battery = go.AddOrGet<BatterySmart>();
        battery.capacity = 10000f;
        battery.joulesLostPerSecond = 2f / 3f;
        battery.powerSortOrder = 1000;
        go.AddOrGetDef<PoweredActiveController.Def>();
    }
}
