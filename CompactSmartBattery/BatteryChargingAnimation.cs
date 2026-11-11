using HarmonyLib;
using KSerialization;

namespace CompactSmartBattery;

[SerializationConfig(MemberSerialization.OptIn)]
public sealed class BatteryChargingAnimation : KMonoBehaviour
{
    private Operational operational;
    private KBatchedAnimController controller;

    private bool charging;
    private string currentAnimation;

    protected override void OnSpawn()
    {
        base.OnSpawn();
        operational = GetComponent<Operational>();
        controller = GetComponent<KBatchedAnimController>();
        Subscribe((int)GameHashes.OperationalChanged, OnOperationalChanged);
        RefreshAnimation();
    }

    internal void SetCharging(bool value)
    {
        charging = value;
        RefreshAnimation();
    }

    private void OnOperationalChanged(object data)
    {
        if (!operational.IsOperational)
            charging = false;
        RefreshAnimation();
    }

    private void RefreshAnimation()
    {
        string animation = !operational.IsOperational ? "off" : charging ? "working_loop" : "on";
        if (animation == currentAnimation)
            return;

        currentAnimation = animation;
        controller.Play(animation, charging && operational.IsOperational
            ? KAnim.PlayMode.Loop : KAnim.PlayMode.Once);
    }
}

[HarmonyPatch(typeof(BatterySmart), nameof(BatterySmart.EnergySim200ms))]
internal static class BatterySmart_ChargingAnimation_Patch
{
    private static void Prefix(BatterySmart __instance)
    {
        BatteryChargingAnimation animation = __instance.GetComponent<BatteryChargingAnimation>();
        if (animation == null)
            return;

        // Sample the completed energy tick before vanilla resets its counters.
        animation.SetCharging(__instance.WattsUsed > 0f
            && __instance.PercentFull > __instance.PreviousPercentFull);
    }
}
