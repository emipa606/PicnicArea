using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace PicnicArea;

[StaticConstructorOnStartup]
public class PicnicArea
{
    static PicnicArea()
    {
        new Harmony("mlie.PicnicArea").PatchAll(Assembly.GetExecutingAssembly());
    }

    public static bool VerifyPicnicSpot(Pawn pawn)
    {
        if (pawn is not { Spawned: true })
        {
            return true;
        }

        return pawn.Map == null || VerifyPicnicSpot(pawn.Position, pawn.Map);
    }

    public static bool VerifyPicnicSpot(IntVec3 intVec3, Map map)
    {
        // ReSharper disable once ForCanBeConvertedToForeach
        for (var index = 0; index < map.zoneManager.AllZones.Count; index++)
        {
            var zone = map.zoneManager.AllZones[index];
            if (zone is not Zone_PicnicArea picnicArea)
            {
                continue;
            }

            if (picnicArea.cells.Count == 0)
            {
                Log.ErrorOnce($"Picnic zone has 0 cells (this should never happen): {picnicArea}", -563287);
                continue;
            }

            if (picnicArea.cells.Contains(intVec3))
            {
                return true;
            }
        }

        return false;
    }

    public static bool VerifyPicnicConditions(Pawn pawn)
    {
        if (pawn is not { Spawned: true })
        {
            return false;
        }

        return pawn.Map != null && VerifyPicnicConditions(pawn.Map);
    }

    public static bool VerifyPicnicConditions(Map map)
    {
        var tempRange = PicnicAreaMod.Instance.Settings.TemperatureRange;
        var outsideTemp = map.mapTemperature.OutdoorTemp;
        if (outsideTemp < tempRange.min || outsideTemp > tempRange.max)
        {
            LogMessage("Wrong temp");
            return false;
        }

        var startTime = PicnicAreaMod.Instance.Settings.TimeStart;
        var stopTime = PicnicAreaMod.Instance.Settings.TimeStop;
        var currentTime = GenLocalDate.HourOfDay(map);
        if (startTime < stopTime && currentTime < startTime || currentTime > stopTime ||
            startTime > stopTime && currentTime < startTime && currentTime > stopTime)
        {
            LogMessage("Wrong time of day");
            return false;
        }

        if (PicnicAreaMod.Instance.Settings.AnyWeather)
        {
            return true;
        }

        var currentWeather = map.weatherManager.curWeather;
        if (currentWeather == null)
        {
            LogMessage("No weather");
            return false;
        }

        if (currentWeather.isBad)
        {
            LogMessage("Weather bad");
            return false;
        }

        // ReSharper disable once InvertIf looks better
        if (currentWeather.rainRate > 0 || currentWeather.snowRate > 0)
        {
            LogMessage("Weather bad");
            return false;
        }

        return true;
    }

    public static void LogMessage(string message)
    {
        if (PicnicAreaMod.Instance.Settings.VerboseLogging)
        {
            Log.Message($"[PicnicArea]: {message}");
        }
    }
}