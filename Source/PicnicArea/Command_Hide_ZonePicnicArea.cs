using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace PicnicArea
{
    public class Command_Hide_ZonePicnicArea : Command_Hide
    {
        public Command_Hide_ZonePicnicArea(IHideable hideable)
            : base(hideable)
        {
        }

        protected override IEnumerable<FloatMenuOption> GetOptions()
        {
            return GetHideOptions();
        }

        public static IEnumerable<FloatMenuOption> GetHideOptions()
        {
            yield return new FloatMenuOption("ShowAllZones".Translate(), delegate
            {
                ToggleAll(hidden: false);
            });
            yield return new FloatMenuOption("HideAllZones".Translate(), delegate
            {
                ToggleAll(hidden: true);
            });
            foreach (FloatMenuOption item in ZoneTypeOptions<Zone_PicnicArea>("PicnicAreaZone".Translate(), ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_PicnicArea")))
            {
                yield return item;
            }
        }
    }
}