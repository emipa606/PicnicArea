using HarmonyLib;
using RimWorld;
using Verse;

namespace PicnicArea;

[HarmonyPatch(typeof(MemoryThoughtHandler), "TryGainMemory", typeof(Thought_Memory), typeof(Pawn))]
public static class MemoryThoughtHandler_TryGainMemory_Patch
{
    [HarmonyPrefix]
    public static bool Prefix(Thought_Memory newThought, Pawn otherPawn, ref MemoryThoughtHandler __instance)
    {
        if (newThought.def != ThoughtDefOf.AteWithoutTable)
        {
            return true;
        }

        if (!PicnicArea.VerifyPicnicSpot(__instance.pawn))
        {
            return true;
        }

        if (!PicnicArea.VerifyPicnicConditions(__instance.pawn.Map))
        {
            return true;
        }

        return false;
    }
}