using HarmonyLib;
using Verse;

namespace PicnicArea
{
    [HarmonyPatch(typeof(GenGrid), "HasEatSurface", typeof(IntVec3), typeof(Map))]
    public static class GenGrid_HasEatSurface_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(this IntVec3 c, Map map, ref bool __result)
        {
            if (__result)
            {
                return;
            }

            //if (!PicnicArea.VerifyPicnicSpot(c, map))
            //{
            //    return;
            //}

            if (!PicnicArea.VerifyPicnicConditions(map))
            {
                return;
            }

            for (var i = 4; i >= 0; i--)
            {
                var rot = new Rot4(i);
                var intVec = c + rot.FacingCell;
                var possibleChair = intVec.GetFirstThing<Thing>(map);
                if (possibleChair?.def.building == null || !possibleChair.def.building.isSittable)
                {
                    continue;
                }

                if (possibleChair.Rotation != rot.Opposite)
                {
                    continue;
                }

                if (!PicnicArea.VerifyPicnicSpot(intVec, map))
                {
                    continue;
                }

                __result = true;
                return;
            }
        }
    }
}