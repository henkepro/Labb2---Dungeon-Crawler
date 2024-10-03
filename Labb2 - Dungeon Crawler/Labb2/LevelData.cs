public class LevelData
{
    public int _inventoryslot;
    public int _position_X;
    public int _position_Y = 3;
    private List<LevelElement> _elements = new List<LevelElement>();
    public List<LevelElement> Elements { get { return _elements; } }

    private Dictionary<int, Inventory> _inventory = new Dictionary<int, Inventory>();
    public Dictionary<int, Inventory> Inventory { get { return _inventory; } }
    public void Load(string fileName)
    {
        foreach (char character in fileName)
        {
            switch(character)
            {
                case '#':
                    Wall wall = new Wall();
                    wall.Character = character;
                    SetPositionXY(wall);
                    _elements.Add(wall);
                    break;
                case '@':
                    Player player = new Player();
                    player.Character = character;
                    SetPositionXY(player);
                    _elements.Add(player);
                    break;
                case 's':
                    Snake snake = new Snake();
                    snake.Character = character;
                    SetPositionXY(snake);
                    _elements.Add(snake);
                    break;
                case 'r':
                    Rat rat = new Rat();
                    rat.Character = character;
                    SetPositionXY(rat);
                    _elements.Add(rat);
                    break;
                case 'x':
                    Inventory inventoryslot = new Inventory();
                    inventoryslot.Character = character;
                    SetPositionXY(inventoryslot);
                    _elements.Add(inventoryslot);
                    _inventory.Add(_inventoryslot, inventoryslot);
                    _inventoryslot++;
                    break;
                case '_':
                    InventoryStructure inventory__ = new InventoryStructure();
                    inventory__.Character = character;
                    SetPositionXY(inventory__);
                    _elements.Add(inventory__);
                    break;
                case '|':
                    InventoryStructure inventory_l = new InventoryStructure();
                    inventory_l.Character = character;
                    SetPositionXY(inventory_l);
                    _elements.Add(inventory_l);
                    break;
                case 'T':
                    Equipment excalibur = new Equipment();
                    excalibur.Excalibur();
                    excalibur.Character = character;
                    excalibur.name = "Excalibur";
                    SetPositionXY(excalibur);
                    _elements.Add(excalibur);
                    break;
                case 'G':
                    Equipment godSword = new Equipment();
                    godSword.GodSword();
                    godSword.Character = character;
                    godSword.name = "GodSword";
                    SetPositionXY(godSword);
                    _elements.Add(godSword);
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




