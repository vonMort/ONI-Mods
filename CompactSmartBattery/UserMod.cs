using System;
using System.IO;
using Database;
using HarmonyLib;
using KMod;
using STRINGS;

namespace CompactSmartBattery
{
    public class ModStrings
    {
        public static LocString NAME = "Compact Smart Battery";

        public static LocString BACKWALL =
            "Must be built against a back wall. Does not require a floor.";
    }

    public class Mod : UserMod2
    {
    }

    [HarmonyPatch(typeof(Localization), nameof(Localization.Initialize))]
    public static class Localization_Initialize_Patch
    {
        public static void Postfix()
        {
            ModUtil.RegisterForTranslation(typeof(ModStrings));
            string language = Localization.GetLocale()?.Code;
            if (!string.IsNullOrEmpty(language))
            {
                string file = Path.Combine(
                    Path.GetDirectoryName(typeof(Mod).Assembly.Location),
                    "translations", language + ".po");
                if (File.Exists(file))
                    Localization.OverloadStrings(Localization.LoadStringsFile(file, false));
            }
        }
    }

    [HarmonyPatch(typeof(Techs), "Load")]
    public static class Techs_Load_Patch
    {
        public static void Postfix()
        {
            Tech tech = Db.Get().Techs.resources.Find(item => item.unlockedItemIDs.Contains(BatterySmartConfig.ID));
            if (tech == null)
                throw new InvalidOperationException("Cannot find the Smart Battery research unlock.");
            if (!tech.unlockedItemIDs.Contains(CompactSmartBatteryConfig.ID))
                tech.AddUnlockedItemIDs(CompactSmartBatteryConfig.ID);
        }
    }

    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
    public static class GeneratedBuildings_Load_Patch
    {
        public static void Prefix()
        {
            Strings.Add(
                "STRINGS.BUILDINGS.PREFABS.COMPACTSMARTBATTERY.NAME",
                UI.FormatAsLink(ModStrings.NAME, CompactSmartBatteryConfig.ID));
            Strings.Add(
                "STRINGS.BUILDINGS.PREFABS.COMPACTSMARTBATTERY.DESC",
                STRINGS.BUILDINGS.PREFABS.BATTERYSMART.DESC);
            Strings.Add(
                "STRINGS.BUILDINGS.PREFABS.COMPACTSMARTBATTERY.EFFECT",
                STRINGS.BUILDINGS.PREFABS.BATTERYSMART.EFFECT + "\n\n" + ModStrings.BACKWALL);

            int powerIndex = TUNING.BUILDINGS.PLANORDER.FindIndex(item => item.category == new HashedString("Power"));
            if (powerIndex < 0)
                throw new InvalidOperationException("Cannot find the Power build menu.");
            var power = TUNING.BUILDINGS.PLANORDER[powerIndex];
            if (!power.buildingAndSubcategoryData.Exists(item => item.Key == CompactSmartBatteryConfig.ID))
            {
                ModUtil.AddBuildingToPlanScreen(
                    "Power",
                    CompactSmartBatteryConfig.ID,
                    "batteries",
                    BatterySmartConfig.ID,
                    ModUtil.BuildingOrdering.After);
            }
        }
    }

    [HarmonyPatch(typeof(BatterySmart), "CreateLogicMeter")]
    public static class BatterySmart_CreateLogicMeter_Patch
    {
        public static bool Prefix(BatterySmart __instance)
        {
            // Only the fallback skin lacks the symbols used by the vanilla automation lamp.
            return __instance.GetComponent<KPrefabID>().PrefabTag != new Tag(CompactSmartBatteryConfig.ID)
                   || BatteryArtwork.UsesCustomAnimation(__instance);
        }
    }
}