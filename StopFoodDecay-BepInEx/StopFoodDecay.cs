using BepInEx;
using HarmonyLib;

namespace StopFoodDecay_BepInEx
{
    [BepInPlugin("Jeremiah.StopFoodDecay", "Stop Food Decay", "1.0.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        void Awake()
        {
            Logger.LogDebug("StopFoodDecay.Plugin.Awake");
            Harmony harmony = new Harmony("Jeremiah.StopFoodDecay");
            harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());
            Logger.LogDebug("StopFoodDecay.Plugin.Awake: harmony patch applied");
        }
    }

    [HarmonyPatch(typeof(Perishable), nameof(Perishable.ItemParentChanged))]
    class Perishable_ItemParentChanged_Patch
    {
        private static readonly BepInEx.Logging.ManualLogSource LOGGER = BepInEx.Logging.Logger.CreateLogSource("StopFoodDecay.Perishable_ItemParentChanged_Patch");

        [HarmonyPrefix]
        public static void Prefix(Perishable __instance)
        {
            Item item = __instance.Item;
            if (item != null && item.IsFood)
            {
                if (__instance.DepletionRate != 0f)
                {
                    __instance.SetDurabilityDepletion(0f);
                    LOGGER.LogDebug(string.Format("StopFoodDecay::Perishable.ItemParentChanged: {0}, SetDurabilityDepletion set to {1}.", __instance.name, 0f));
                }
                if (!__instance.DontPerishInWorld)
                {
                    __instance.DontPerishInWorld = true;
                    LOGGER.LogDebug(string.Format("StopFoodDecay::Perishable.ItemParentChanged: {0}, DontPerishInWorld is now {1}.", __instance.name, __instance.DontPerishInWorld));
                }
                if (!__instance.DontPerishSkipTime)
                {
                    __instance.DontPerishSkipTime = true;
                    LOGGER.LogDebug(string.Format("StopFoodDecay::Perishable.ItemParentChanged: {0}, DontPerishSkipTime is now {1}.", __instance.name, __instance.DontPerishSkipTime));
                }
            }
        }
    }
}