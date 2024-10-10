using System.Reflection.PortableExecutable;

public class LevelData
{
    private static List<LevelElement> _elements = new List<LevelElement>();
    public static List<LevelElement> Elements { get { return _elements; } }

    private static Dictionary<int, Inventory> _inventory = new Dictionary<int, Inventory>();
    public static Dictionary<int, Inventory> Inventory { get { return _inventory; } }

    public void Load(string fileName)
    {
        using(StreamReader readMap = new StreamReader(@$"..\..\..\Labb2\Misc\.txt\{fileName}"))
        {
            int _inventoryslot = 0;
            int _position_X = 0;
            int _position_Y = 3;
            char _character =  '\0';
            string levelMap = string.Empty;
            while((levelMap = readMap.ReadLine()) != null)
            {
                foreach(char character in levelMap)
                {
                    _character = character;
                    switch(character)
                    {
                        case '#':
                            Wall wall = new Wall();
                            AddElementDataToList(wall);
                            break;
                        case '+':
                            Gold gold = new Gold();
                            AddElementDataToList(gold);
                            break;
                        case '@':
                            Player player = new Player();
                            AddElementDataToList(player);
                            break;
                        case 's':
                            Snake snake = new Snake();
                            AddElementDataToList(snake);
                            break;
                        case 'r':
                            Rat rat = new Rat();
                            AddElementDataToList(rat);
                            break;
                        case 'x':
                            Inventory inventoryslot = new Inventory();
                            AddElementDataToList(inventoryslot);
                            _inventory.Add(_inventoryslot, inventoryslot);
                            _inventoryslot++;
                            break;
                        case '_':
                            InventoryStructure inventory__ = new InventoryStructure();
                            AddElementDataToList(inventory__);
                            break;
                        case '|':
                            InventoryStructure inventory_l = new InventoryStructure();
                            AddElementDataToList(inventory_l);
                            break;
                        case 'T':
                            Equipment excalibur = new Equipment();
                            excalibur.Excalibur();
                            AddElementDataToList(excalibur);
                            break;
                        case 'G':
                            Equipment godSword = new Equipment();
                            godSword.GodSword();
                            AddElementDataToList(godSword);
                            break;
                    }
                    _position_X++;
                }
                _position_X = 0;
                _position_Y++;
            }
            void AddElementDataToList(LevelElement element)
            {
                element.Character = _character;
                element.Position_Y = _position_Y;
                element.Position_X = _position_X;
                _elements.Add(element);
            }
        }
    }
}




