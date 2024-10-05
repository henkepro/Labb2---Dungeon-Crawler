public class Snake : Enemy
{
    public override void Update()
    {
        SavePosition();
        Remove();
        Movement();
    }
    public void Movement()
    {
        if (!CollisionDetected && SquareDistanceTo(Player) <= 2)
        {
            if (Position_X > Player.Position_X)
            {
                Position_X++;
            }
            else if (Position_X < Player.Position_X)
            {
                Position_X--;
            }
            else if(Position_Y > Player.Position_Y)
            {
                Position_Y++;
            }
            else if(Position_Y < Player.Position_Y)
            {
                Position_Y--;
            }
        }
    }
}




