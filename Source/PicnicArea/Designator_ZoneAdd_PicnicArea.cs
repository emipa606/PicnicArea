using RimWorld;
using UnityEngine;
using Verse;

namespace PicnicArea
{
    public class Designator_ZoneAdd_PicnicArea : Designator_ZoneAdd
    {
        // ReSharper disable once MemberCanBeProtected.Global Has to be visible to add to game
        public Designator_ZoneAdd_PicnicArea()
        {
            zoneTypeToPlace = typeof(Zone_PicnicArea);
            defaultLabel = "PicnicAreaZone".Translate();
            defaultDesc = "PicnicAreaZoneDesc".Translate();
            icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_PicnicArea");
            hotKey = KeyBindingDefOf.Misc2;
        }


        protected override string NewZoneLabel => "PicnicAreaZone".Translate();

        public override AcceptanceReport CanDesignateCell(IntVec3 c)
        {
            if (!base.CanDesignateCell(c).Accepted)
            {
                return false;
            }

            if (c.GetTerrain(Map).passability == Traversability.Impassable)
            {
                return false;
            }

            if (c.GetTerrain(Map).HasTag("Water"))
            {
                return false;
            }

            if (!c.UsesOutdoorTemperature(Map))
            {
                return false;
            }

            return true;
        }

        protected override Zone MakeNewZone()
        {
            return new Zone_PicnicArea(Find.CurrentMap.zoneManager);
        }
    }
}