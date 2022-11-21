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
        var harmony = new Harmony("mlie.PicnicArea");
        var assembly = Assembly.GetExecutingAssembly();
        harmony.PatchAll(assembly);
    }

    public static bool VerifyPicnicSpot(Pawn pawn)
    {
        if (pawn.DevelopmentalStage.Baby() && !pawn.Spawned)
        {
            return true;
        }

        return VerifyPicnicSpot(pawn.Position, pawn.Map);
    }

    public static bool VerifyPicnicSpot(IntVec3 intVec3, Map map)
    {
        var zonesList = map.zoneManager.AllZones;
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

            if (picnicArea.cells.Contains(intVec3))
            {
                return true;
            }
        }

        return false;
    }

    public static bool VerifyPicnicConditions(Pawn pawn)
    {
        if (pawn.DevelopmentalStage.Baby() && !pawn.Spawned)
        {
            return true;
        }

        return VerifyPicnicConditions(pawn.Map);
    }

    public static bool VerifyPicnicConditions(Map map)
    {
        var tempRange = PicnicAreaMod.instance.Settings.TemperatureRange;
        var outsideTemp = map.mapTemperature.OutdoorTemp;
        if (outsideTemp < tempRange.min || outsideTemp > tempRange.max)
        {
            LogMessage("Wrong temp");
            return false;
        }

        var startTime = PicnicAreaMod.instance.Settings.TimeStart;
        var stopTime = PicnicAreaMod.instance.Settings.TimeStop;
        var currentTime = GenLocalDate.HourOfDay(map);
        if (startTime < stopTime && currentTime < startTime || currentTime > stopTime ||
            startTime > stopTime && currentTime < startTime && currentTime > stopTime)
        {
            LogMessage("Wrong time of day");
            return false;
        }

        if (PicnicAreaMod.instance.Settings.AnyWeather)
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
        if (PicnicAreaMod.instance.Settings.VerboseLogging)
        {
            Log.Message($"[PicnicArea]: {message}");
        }
    }
}