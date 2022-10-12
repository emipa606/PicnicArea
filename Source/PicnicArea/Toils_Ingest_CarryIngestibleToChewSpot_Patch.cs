using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace PicnicArea;

[HarmonyPatch(typeof(Toils_Ingest), "CarryIngestibleToChewSpot", typeof(Pawn), typeof(TargetIndex))]
public static class Toils_Ingest_CarryIngestibleToChewSpot_Patch
{
    [HarmonyPostfix]
    public static void Postfix(Pawn pawn, TargetIndex ingestibleInd, ref Toil __result)
    {
        bool BaseChairValidator(Thing t)
        {
            if (t.def.building == null || !t.def.building.isSittable)
            {
                return false;
            }

            if (t.IsForbidden(pawn))
            {
                return false;
            }

            if (!pawn.CanReserve(t))
            {
                return false;
            }

            if (!t.IsSociallyProper(pawn))
            {
                return false;
            }

            if (t.IsBurning())
            {
                return false;
            }

            if (t.HostileTo(pawn))
            {
                return false;
            }

            var result = false;
            for (var i = 0; i < 4; i++)
            {
                var edifice = (t.Position + GenAdj.CardinalDirections[i]).GetEdifice(t.Map);
                if (edifice == null || edifice.def.surfaceType != SurfaceType.Eat)
                {
                    continue;
                }

                result = true;
                break;
            }

            return result;
        }

        var possibleThing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map,
            ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.OnCell,
            TraverseParms.For(pawn),
            32f, t => BaseChairValidator(t) && t.Position.GetDangerFor(pawn, t.Map) == Danger.None);

        if (possibleThing != null && Rand.Bool)
        {
            return;
        }

        if (!PicnicArea.VerifyPicnicConditions(pawn.Map))
        {
            return;
        }

        var picnicSittable = getPicnicSittable(pawn);
        if (picnicSittable == null)
        {
            PicnicArea.LogMessage("No good spot");
            return;
        }

        var toil = new Toil();
        toil.initAction = delegate
        {
            var actor = toil.actor;
            actor.Reserve(picnicSittable, actor.CurJob);
            actor.Map.pawnDestinationReservationManager.Reserve(actor, actor.CurJob, picnicSittable.Position);
            actor.pather.StartPath(picnicSittable, PathEndMode.OnCell);
        };
        toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
        __result = toil;
    }

    private static Thing getPicnicSittable(Pawn pawn)
    {
        var zonesList = pawn.Map.zoneManager.AllZones.InRandomOrder();
        Thing returnValue = null;
        var currentMinRange = 1000f;
        foreach (var zone in zonesList)
        {
            if (zone is not Zone_PicnicArea picnicArea)
            {
                continue;
            }

            if (picnicArea.cells.Count == 0)
            {
                Log.ErrorOnce("Picnic zone has 0 cells (this should never happen): " + picnicArea, -563287);
                continue;
            }

            if (picnicArea.ContainsStaticFire)
            {
                PicnicArea.LogMessage("Contains fire");
                continue;
            }

            if (pawn.Position.DistanceTo(picnicArea.Position) > currentMinRange)
            {
                PicnicArea.LogMessage($"Range is longer than {currentMinRange}");
                continue;
            }

            currentMinRange = pawn.Position.DistanceTo(picnicArea.Position);
            var sittables = picnicArea.AllContainedThings.Where(thing => Predicate(thing, pawn));

            if (!sittables.Any())
            {
                PicnicArea.LogMessage("No sittables");
                continue;
            }

            returnValue = sittables.RandomElement();
        }

        return returnValue;
    }

    private static bool Predicate(Thing thing, Pawn pawn)
    {
        if (thing.def.building == null || !thing.def.building.isSittable)
        {
            return false;
        }

        if (thing.IsForbidden(pawn))
        {
            return false;
        }

        if (!pawn.CanReserve(thing))
        {
            return false;
        }

        if (thing.Position.GetDangerFor(pawn, thing.Map) > pawn.NormalMaxDanger())
        {
            return false;
        }

        if (!thing.IsSociallyProper(pawn))
        {
            return false;
        }

        if (thing.IsBurning())
        {
            return false;
        }

        return !thing.HostileTo(pawn);
    }
}