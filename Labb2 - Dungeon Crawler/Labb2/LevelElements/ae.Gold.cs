public class Gold : LevelElement
{
    public bool _isDrawn { get; set; } = false;
    public bool drawOnLoad { get; set; } = false;
    public int _gold;
    public void SetCharacterData(string name, int maxHealth, ConsoleColor color, int gold)
    {
        Color = color;
        _gold = gold;
    }
    public void StatusCheck()
    {
        if(SquareDistanceTo(Player) <= 25 && !_isDrawn)
        {
            Draw();
            _isDrawn = true;
        }
        else if(drawOnLoad == true)
        {
            Draw();
            _isDrawn = true;
        }
    }
    private int SquareDistanceTo(Player player)
    {
        int DeltaXsquared = (int)Math.Pow(Position_X - player.Position_X, 2);
        int DeltaYsquared = (int)Math.Pow(Position_Y - player.Position_Y, 2);
        int squareDistance = DeltaXsquared + DeltaYsquared;
        return squareDistance;
    }
}
