using System.Numerics;
using System.Xml.Linq;

public class Wall : LevelElement
{
    private bool IsDrawn;
    public void SetCharacterData(string name, int maxHealth, ConsoleColor color)
    {
        Color = color;
    }
    public void StatusCheck()
    {
        if (SquareDistanceTo(Player) <= 25 && !IsDrawn)
        {
            Draw();
            IsDrawn = true;
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




