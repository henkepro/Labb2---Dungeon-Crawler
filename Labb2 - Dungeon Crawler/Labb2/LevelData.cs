public class LevelData
{
    private int _position_X;
    private int _position_Y = 3;
    private List<LevelElement> _elements = new List<LevelElement>();
    public List<LevelElement> Elements { get { return _elements; } }
    public void Load(string fileName)
    {
        foreach (char character in fileName)
        {
            switch(character)
            {
                case '#':
                    Wall wall = new Wall();
                    SetPositionXY(wall);
                    _elements.Add(wall);
                    break;
                case '@':
                    Player player = new Player();
                    SetPositionXY(player);
                    _elements.Add(player);
                    break;
                case 's':
                    Snake snake = new Snake();
                    SetPositionXY(snake);
                    _elements.Add(snake);
                    break;
                case 'r':
                    Rat rat = new Rat();
                    SetPositionXY(rat);
                    _elements.Add(rat);
                    break;
            }
            _position_X++;
        }
        _position_X = 0;
        _position_Y++;
    }
    private void SetPositionXY(LevelElement element)
    {
        element.Position_X = _position_X;
        element.Position_Y = _position_Y;
    }
}




