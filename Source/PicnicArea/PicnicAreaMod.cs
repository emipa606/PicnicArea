using Mlie;
using UnityEngine;
using Verse;

namespace PicnicArea;

[StaticConstructorOnStartup]
internal class PicnicAreaMod : Mod
{
    /// <summary>
    ///     The instance of the settings to be read by the mod
    /// </summary>
    public static PicnicAreaMod Instance;

    private static string currentVersion;

    /// <summary>
    ///     The private settings
    /// </summary>
    public readonly PicnicAreaSettings Settings;

    /// <summary>
    ///     Cunstructor
    /// </summary>
    /// <param name="content"></param>
    public PicnicAreaMod(ModContentPack content) : base(content)
    {
        Instance = this;
        currentVersion =
            VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
        Settings = GetSettings<PicnicAreaSettings>();
    }

    /// <summary>
    ///     The title for the mod-settings
    /// </summary>
    /// <returns></returns>
    public override string SettingsCategory()
    {
        return "Picnic Area";
    }

    /// <summary>
    ///     The settings-window
    ///     For more info: https://rimworldwiki.com/wiki/Modding_Tutorials/ModSettings
    /// </summary>
    /// <param name="rect"></param>
    public override void DoSettingsWindowContents(Rect rect)
    {
        var listingStandard = new Listing_Standard();
        listingStandard.Begin(rect);
        listingStandard.Gap();
        listingStandard.Label("PicnicAreaTemperatureRange".Translate(), -1,
            "PicnicAreaTemperatureRangeDesc".Translate());
        listingStandard.IntRange(ref Settings.TemperatureRange, -40, 60);
        listingStandard.Label("PicnicAreaTimeRange".Translate(), -1, "PicnicAreaTimeRangeDesc".Translate());

        listingStandard.Label($"{"PicnicAreaStartTime".Translate()}: {Settings.TimeStart}");
        listingStandard.IntAdjuster(ref Settings.TimeStart, 1, -1);
        if (Settings.TimeStart > 23)
        {
            Settings.TimeStart = 0;
        }

        if (Settings.TimeStart < 0)
        {
            Settings.TimeStart = 23;
        }

        if (Settings.TimeStart == Settings.TimeStop)
        {
            Settings.TimeStop++;
        }

        listingStandard.Label($"{"PicnicAreaStopTime".Translate()}: {Settings.TimeStop}");
        listingStandard.IntAdjuster(ref Settings.TimeStop, 1, -1);
        if (Settings.TimeStop > 23)
        {
            Settings.TimeStop = 0;
        }

        if (Settings.TimeStop < 0)
        {
            Settings.TimeStop = 23;
        }

        if (Settings.TimeStart == Settings.TimeStop)
        {
            Settings.TimeStart--;
        }

        listingStandard.CheckboxLabeled("PicnicAreaAllWeather".Translate(), ref Settings.AnyWeather);
        listingStandard.CheckboxLabeled("PicnicAreaVerboseLogging".Translate(), ref Settings.VerboseLogging);

        if (currentVersion != null)
        {
            listingStandard.Gap();
            GUI.contentColor = Color.gray;
            listingStandard.Label("PicnicAreaModVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listingStandard.End();
    }
}