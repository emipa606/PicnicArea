using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace PicnicArea;

public class Zone_PicnicArea : Zone
{
    private static readonly List<Color> picnicAreaZoneColors = [];

    private static int nextPicnicAreaZoneColorIndex;

    public Zone_PicnicArea(ZoneManager zoneManager) : base("PicnicAreaZone".Translate(), zoneManager)
    {
    }

    public Zone_PicnicArea()
    {
    }

    public override bool IsMultiselectable => false;

    protected override Color NextZoneColor => nextPicnicAreaZoneColor();

    private static IEnumerable<Color> PicnicAreaZoneColors()
    {
        yield return Color.Lerp(Color.yellow, Color.gray, 0.3f);
        yield return Color.Lerp(Color.yellow, Color.gray, 0.4f);
        yield return Color.Lerp(Color.yellow, Color.gray, 0.5f);
        yield return Color.Lerp(Color.yellow, Color.gray, 0.6f);
    }

    private static Color nextPicnicAreaZoneColor()
    {
        picnicAreaZoneColors.Clear();
        foreach (var color in PicnicAreaZoneColors())
        {
            var item = new Color(color.r, color.g, color.b, 0.09f);
            picnicAreaZoneColors.Add(item);
        }

        var result = picnicAreaZoneColors[nextPicnicAreaZoneColorIndex];
        nextPicnicAreaZoneColorIndex++;
        if (nextPicnicAreaZoneColorIndex >= picnicAreaZoneColors.Count)
        {
            nextPicnicAreaZoneColorIndex = 0;
        }

        return result;
    }

    public override IEnumerable<Gizmo> GetZoneAddGizmos()
    {
        yield return DesignatorUtility.FindAllowedDesignator<Designator_ZoneAdd_PicnicArea_Expand>();
    }
}