using System.Xml.Linq;

public class Rat : Enemy
{
    private Enums RatPattern { get; set; }
    public void Movement()
    {
        RatPattern = (Enums)random.Next(0, 4);
        switch(RatPattern)
        {
            case Enums.up:
                Position_Y--;
                break;
            case Enums.down:
                Position_Y++;
                break;
            case Enums.left:
                Position_X--;
                break;
            case Enums.right:
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




