using System.Xml.Linq;

public class Rat : Enemy
{
    private RatMove RatPattern { get; set; }
    public void Movement()
    {
        RatPattern = (RatMove)random.Next(0, 4);
        switch(RatPattern)
        {
            case RatMove.up:
                Position_Y--;
                break;
            case RatMove.down:
                Position_Y++;
                break;
            case RatMove.left:
                Position_X--;
                break;
            case RatMove.right:
                Position_X++;
                break;
            default:
                break;
        }
    }
    public override void Update()
    {
        SavePosition();
        Remove();
        Movement();
    }
}




