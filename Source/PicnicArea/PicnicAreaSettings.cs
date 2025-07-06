using Verse;

namespace PicnicArea;

/// <summary>
///     Definition of the settings for the mod
/// </summary>
internal class PicnicAreaSettings : ModSettings
{
    public bool AnyWeather;
    public IntRange TemperatureRange = new(5, 35);
    public int TimeStart = 8;
    public int TimeStop = 16;
    public bool VerboseLogging;

    /// <summary>
    ///     Saving and loading the values
    /// </summary>
    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref TemperatureRange, "TemperatureRange", new IntRange(10, 20));
        Scribe_Values.Look(ref TimeStart, "TimeStart", 8);
        Scribe_Values.Look(ref TimeStop, "TimeStop", 16);
        Scribe_Values.Look(ref AnyWeather, "AnyWeather");
        Scribe_Values.Look(ref VerboseLogging, "VerboseLogging");
    }
}