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
                    wall.Position_X = _position_X;
                    wall.Position_Y = _position_Y;
                    _elements.Add(wall);
                    break;
                case '@':
                    Player player = new Player();
                    player.Position_X = _position_X;
                    player.Position_Y = _position_Y;
                    player.AttackDice = new Dice(2, 5, 6);
                    player.DefenseDice = new Dice(2, 6, 0);
                    _elements.Add(player);
                    break;
                case 's':
                    Snake snake = new Snake();
                    snake.Position_X = _position_X;
                    snake.Position_Y = _position_Y;
                    snake.AttackDice = new Dice(3, 4, 2);
                    snake.DefenseDice = new Dice(1, 8, 5);
                    _elements.Add(snake);
                    break;
                case 'r':
                    Rat rat = new Rat();
                    rat.Position_X = _position_X;
                    rat.Position_Y = _position_Y;
                    rat.AttackDice = new Dice(1, 6, 3);
                    rat.DefenseDice = new Dice(1, 6, 1);
                    _elements.Add(rat);
                    break;
            }
            _position_X++;
        }
        _position_X = 0;
        _position_Y++;
    }
}




