using FrooxEngine;
using HarmonyLib;
using ResoniteModLoader;

namespace Default_Laser_State
{
    public class Patch : ResoniteMod
    {
        public override string Author => "LeCloutPanda";
        public override string Name => "Default Laser State";
        public override string Version => "0.0.1";

        public static ModConfiguration config;
        [AutoRegisterConfigKey] public static ModConfigurationKey<bool> ENABLED = new ModConfigurationKey<bool>("Default state", "What state will the lasers have", () => true);

        public override void OnEngineInit()
        {
            config = GetConfiguration();
            config.Save(true);

            Harmony harmony = new Harmony("dev.lecloutpanda.defaultlaserstate");
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(InteractionHandler), "OnAwake")]
        class CommonToolPatch
        {
            [HarmonyPostfix]
            static void Postfix(InteractionHandler __instance, Sync<bool> ____laserEnabled)
            {
                __instance.RunInUpdates(3, () =>
                {
                    if (__instance.Slot.ActiveUser != __instance.LocalUser)
                        return;

                    ____laserEnabled.Value = config.GetValue(ENABLED);
                });
            }
        }
    }
}