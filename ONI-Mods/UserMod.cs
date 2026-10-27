using System.Collections.Generic;
using Database;
using HarmonyLib;
using KMod;
using STRINGS;
using TUNING;
using UnityEngine;

namespace ONI_Mods
{
    public class ModStrings
    {
        public static LocString NAME = "Compact Smart Battery";
        public static LocString DESC = "A slim power accumulator mounted on a back wall.\n\nAutomates power grids by sending a signal based on its charge level.";
        public static LocString EFFECT = "Stores up to 10 kJ of electrical power and outputs an Automation signal based on its stored charge.\n\nMust be built against a back wall.";
    }

    public class Mod : UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            ModUtil.RegisterForTranslation(typeof(ModStrings));
            base.OnLoad(harmony);
        }
    }

    [HarmonyPatch(typeof(Techs), "Load")]
    public static class Techs_Load_Patch
    {
        public static void Postfix()
        {
            Db.Get().Techs.Get("Acoustics").AddUnlockedItemIDs(CompactSmartBatteryConfig.ID);
        }
    }

    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
    public static class GeneratedBuildings_Load_Patch
    {
        public static void Postfix()
        {
            Strings.Add(
                "STRINGS.BUILDINGS.PREFABS.COMPACTSMARTBATTERY.NAME",
                ModStrings.NAME);
            Strings.Add(
                "STRINGS.BUILDINGS.PREFABS.COMPACTSMARTBATTERY.DESC",
                ModStrings.DESC);
            Strings.Add(
                "STRINGS.BUILDINGS.PREFABS.COMPACTSMARTBATTERY.EFFECT",
                ModStrings.EFFECT);

            ModUtil.AddBuildingToPlanScreen(
                "Power",
                CompactSmartBatteryConfig.ID,
                "batteries",
                "BatterySmart",
                ModUtil.BuildingOrdering.After);
        }
    }
}
