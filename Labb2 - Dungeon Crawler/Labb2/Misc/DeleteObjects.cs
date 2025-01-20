public static class DeleteObjects
{
    public static List<LevelElement> List = new List<LevelElement>();
    public static List<LevelElement> TrackList = new List<LevelElement>();

    public static void ClearCache()
    {
        if(List != null)
        {
            List.Clear();
        }
    }
    public static void ClearCache2()
    {
        if(TrackList != null)
        {
            TrackList.Clear();
        }
    }
}
