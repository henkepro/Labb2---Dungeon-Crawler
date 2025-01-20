using MongoDB.Bson;
using MongoDB.Driver;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;

public class LevelData
{
    private static List<LevelElement> _elements = new List<LevelElement>();
    public static List<LevelElement> Elements { get { return _elements; } }

    private static Dictionary<int, Inventory> _inventory = new Dictionary<int, Inventory>();
    public static Dictionary<int, Inventory> Inventory { get { return _inventory; } }
    public bool mapLoaded = false;

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
                    if(mapLoaded == false)
                    {
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

    public void LoadSavedData()
    {
        var connectionString = "mongodb://localhost:27017";

        var client = new MongoClient(connectionString);

        var database = client.GetDatabase("HenrikVu");

        var collection = database.GetCollection<BsonDocument>("saveFile");

        var documents = collection.Find(new BsonDocument()).ToList();

        int _inventoryslot = 0;

        foreach(var document in documents)
        {
            var characterData = document["Character"];
            char character = characterData.AsString[0];
            switch(character)
            {
                case '#':
                    Wall wall = new Wall();
                    LoadElementDataToList(wall, document);
                    break;
                case '+':
                    Gold gold = new Gold();
                    LoadElementDataToList(gold, document);
                    break;
                case '@':
                    Player player = new Player();
                    LoadElementDataToList(player, document);
                    break;
                case 's':
                    Snake snake = new Snake();
                    LoadElementDataToList(snake, document);
                    break;
                case 'r':
                    Rat rat = new Rat();
                    LoadElementDataToList(rat, document);
                    break;
                case 'x':
                    Inventory inventoryslot = new Inventory();
                    LoadElementDataToList(inventoryslot, document);
                    _inventory.Add(_inventoryslot, inventoryslot);
                    _inventoryslot++;
                    break;
                case '_':
                    InventoryStructure inventory__ = new InventoryStructure();
                    LoadElementDataToList(inventory__, document);
                    break;
                case '|':
                    InventoryStructure inventory_l = new InventoryStructure();
                    LoadElementDataToList(inventory_l, document);
                    break;
                case 'T':
                    Equipment excalibur = new Equipment();
                    excalibur.Excalibur();
                    LoadElementDataToList(excalibur, document);
                    break;
                case 'G':
                    Equipment godSword = new Equipment();
                    godSword.GodSword();
                    LoadElementDataToList(godSword, document);
                    break;
            }
        }

        void LoadElementDataToList(LevelElement element, BsonDocument document)
        {
            element.Id = document["_id"].AsObjectId;
            element.Character = document["Character"].AsString[0];
            element.Color = (ConsoleColor)document["Color"].AsInt32;
            element.Position_X = document["Position_X"].AsInt32;
            element.Position_Y = document["Position_Y"].AsInt32;
            if(element is Wall wall)
            {
                wall.drawOnLoad = document["IsDrawn"].ToBoolean();
            }
            else if(element is Gold gold)
            {
                gold.drawOnLoad = document["IsDrawn"].ToBoolean();
            }
            else if(element is Player player)
            {
                player.Turn = document["Turn"].AsInt32 - 1;
                player.Health = document["Health"].AsInt32;
                player.Gold = document["Gold"].AsInt32;
                player.Name = document["Name"].AsString;
                var equipmentBsonArray = document["Equipment"].AsBsonArray;

                foreach(var equipmentBson in equipmentBsonArray)
                {
                    var equipment = new Equipment
                    {
                        name = equipmentBson["Name"].AsString,
                        AttackModifier = equipmentBson["AttackModifier"].AsInt32,
                        DefenseModifier = equipmentBson["DefenseModifier"].AsInt32,
                        Color = (ConsoleColor)equipmentBson["Color"].AsInt32,
                        Character = equipmentBson["Character"].AsString[0]
                    };
                    player.Equipment.Push(equipment);
                }
            }
            else if(element is Enemy enemy)
            {
                enemy.Health = document["Health"].AsInt32;
                enemy.Name = document["Name"].AsString;
            }

            _elements.Add(element);
        }
    }
}




