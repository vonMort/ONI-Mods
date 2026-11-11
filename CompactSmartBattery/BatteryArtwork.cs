using System;
using UnityEngine;

namespace CompactSmartBattery;

internal static class BatteryArtwork
{
    internal const string AnimationName = "compactsmartbattery_kanim";

    internal static KAnimFile GetAnimation()
    {
        if (!Assets.TryGetAnim(AnimationName, out KAnimFile animation))
        {
            Debug.Log("[CompactSmartBattery] Custom artwork not installed; using the small vanilla battery.");
            return Assets.GetAnim("batterysm_kanim");
        }

        KAnimFileData data = animation.GetData();
        if (data == null || data.build == null)
            throw new InvalidOperationException(
                "[CompactSmartBattery] Custom artwork has no loaded animation/build data.");

        foreach (string name in new[] { "ui", "place", "build", "off", "on", "working_pre", "working_loop", "working_pst" })
            RequireAnimation(data, name, 1);
        RequireAnimation(data, "meter", 3);
        RequireAnimation(data, "logicmeter", 2);

        foreach (string name in new[] { "ui", "meter_target", "logicmeter_target" })
            if (data.build.GetSymbol(new KAnimHashedString(name)) == null)
                throw new InvalidOperationException(
                    $"[CompactSmartBattery] Custom artwork is missing the '{name}' symbol.");

        return animation;
    }

    internal static bool UsesCustomAnimation(BatterySmart battery)
    {
        KAnimFile[] files = battery.GetComponent<KBatchedAnimController>().AnimFiles;
        return files.Length > 0 && files[0].name == AnimationName;
    }

    private static void RequireAnimation(KAnimFileData data, string name, int minimumFrames)
    {
        for (int i = 0; i < data.animCount; i++)
        {
            KAnim.Anim animation = data.GetAnim(i);
            if (animation.name == name && animation.numFrames >= minimumFrames)
                return;
        }

        throw new InvalidOperationException(
            $"[CompactSmartBattery] Custom artwork requires '{name}' with at least {minimumFrames} frames.");
    }
}