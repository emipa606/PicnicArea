using HarmonyLib;
using RimWorld;
using Verse;

namespace PicnicArea;

[HarmonyPatch(typeof(MemoryThoughtHandler), nameof(MemoryThoughtHandler.TryGainMemory), typeof(Thought_Memory),
    typeof(Pawn))]
public static class MemoryThoughtHandler_TryGainMemory
{
    public static bool Prefix(Thought_Memory newThought, ref MemoryThoughtHandler __instance)
    {
        if (newThought.def != ThoughtDefOf.AteWithoutTable)
        {
            return true;
        }

        if (!PicnicArea.VerifyPicnicSpot(__instance.pawn))
        {
            return true;
        }

        return !PicnicArea.VerifyPicnicConditions(__instance.pawn.Map);
    }
}