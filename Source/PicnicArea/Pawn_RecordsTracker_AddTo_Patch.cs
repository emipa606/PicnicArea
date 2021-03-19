using HarmonyLib;
using RimWorld;

namespace PicnicArea
{
    [HarmonyPatch(typeof(Pawn_RecordsTracker), "AddTo", typeof(RecordDef), typeof(float))]
    public static class Pawn_RecordsTracker_AddTo_Patch
    {
        [HarmonyPrefix]
        public static void Postfix(RecordDef def, float value, ref Pawn_RecordsTracker __instance)
        {
            if (def != RecordDefOf.NutritionEaten)
            {
                return;
            }

            if (!PicnicArea.VerifyPicnicSpot(__instance.pawn))
            {
                return;
            }

            if (!PicnicArea.VerifyPicnicConditions(__instance.pawn.Map))
            {
                return;
            }

            __instance.pawn.needs.mood?.thoughts.memories.TryGainMemory(PicnicAreaThoughtDefOf.AteAtPicnicArea);
        }
    }
}