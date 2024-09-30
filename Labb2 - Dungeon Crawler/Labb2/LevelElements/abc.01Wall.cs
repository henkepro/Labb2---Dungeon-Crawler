using System.Numerics;
using System.Xml.Linq;

public class Wall : LevelElement
{
    private bool IsDrawn;
    public void SetCharacterData(char character, string name, int maxHealth, ConsoleColor color)
    {
        Character = character;
        Color = color;
    }
    public void StatusCheck(List<LevelElement> deleteObject, Player player)
    {
        if (SquareDistanceTo(player) <= 25 && !IsDrawn)
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




