using RimWorld;

namespace PicnicArea
{
    [DefOf]
    public static class PicnicAreaThoughtDefOf
    {
        public static ThoughtDef AteAtPicnicArea;

        static PicnicAreaThoughtDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(ThoughtDefOf));
        }
    }
}