public class Wall : LevelElement
{
    public bool IsDrawn { get; set; }
    public void StatusCheck(LevelElement element, Player player)
    {
        int squareDistance = (int)(Math.Pow(element.Position_X - player.Position_X, 2) + Math.Pow(element.Position_Y - player.Position_Y, 2));

        if (squareDistance <= 25 && IsDrawn == false)
        {
            element.Draw();
            IsDrawn = true;
        }
    }
}




