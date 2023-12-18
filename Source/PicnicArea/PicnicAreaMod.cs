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
    public static PicnicAreaMod instance;

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
        instance = this;
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
        var listing_Standard = new Listing_Standard();
        listing_Standard.Begin(rect);
        listing_Standard.Gap();
        listing_Standard.Label("PicnicAreaTemperatureRange".Translate(), -1,
            "PicnicAreaTemperatureRangeDesc".Translate());
        listing_Standard.IntRange(ref Settings.TemperatureRange, -40, 60);
        listing_Standard.Label("PicnicAreaTimeRange".Translate(), -1, "PicnicAreaTimeRangeDesc".Translate());

        listing_Standard.Label($"{"PicnicAreaStartTime".Translate()}: {Settings.TimeStart}");
        listing_Standard.IntAdjuster(ref Settings.TimeStart, 1, -1);
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

        listing_Standard.Label($"{"PicnicAreaStopTime".Translate()}: {Settings.TimeStop}");
        listing_Standard.IntAdjuster(ref Settings.TimeStop, 1, -1);
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

        listing_Standard.CheckboxLabeled("PicnicAreaAllWeather".Translate(), ref Settings.AnyWeather);
        listing_Standard.CheckboxLabeled("PicnicAreaVerboseLogging".Translate(), ref Settings.VerboseLogging);

        if (currentVersion != null)
        {
            listing_Standard.Gap();
            GUI.contentColor = Color.gray;
            listing_Standard.Label("PicnicAreaModVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listing_Standard.End();
    }
}