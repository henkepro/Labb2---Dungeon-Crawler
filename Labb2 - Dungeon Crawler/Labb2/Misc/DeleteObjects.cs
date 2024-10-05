public static class DeleteObjects
{
    public static List<LevelElement> List = new List<LevelElement>();

    public static void ClearCache()
    {
        if(List != null)
        {
            List.Clear();
        }
    }
}
