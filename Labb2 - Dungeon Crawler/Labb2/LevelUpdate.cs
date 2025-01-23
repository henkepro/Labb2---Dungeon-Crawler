using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Xml.Linq;
class LevelUpdate
{
    private Player Player { get; set; }
    private LevelData LevelData { get; } = new LevelData();
    public void LevelStart()
    {
        Console.WriteLine("Welcome player. Press Y to continue from save or any button to start new run.");
        KeyInfo.Input = Console.ReadKey(true);
        Console.SetCursorPosition(0, 0);
        Console.WriteLine($"{" ", 80}");
        var connectionString = "mongodb://localhost:27017";
        using var client = new MongoClient(connectionString);
        var database = client.GetDatabase("HenrikVu");
        var collectionExists = database.ListCollectionNames().ToList().Contains("saveFile");

        if(KeyInfo.Input.Key == ConsoleKey.Y && collectionExists)
        {
            LevelData.LoadSavedData();
            LevelData.mapLoaded = true;
        } 
        else
        {
            database.DropCollection("saveFile");
            LevelData.Load("Level1.txt");
            LevelData.Load("Inventory.txt");
        }
        ElementExtract();
        LoadUserInterface();
        ElementUpdate();
    }
    private void ElementExtract()
    {
        Player = LevelData.Elements.OfType<Player>().First();
        foreach(LevelElement element in LevelData.Elements)
        {
            element.Player = Player;
            switch (element)
            {
                case Player player:
                    player.SetCharacterData("Henrik", player.Health, ConsoleColor.Gray, new Dice(2, 6, 2), 
                        new Dice(2, 6, 0), LevelData.Inventory, player.Equipment);
                    player.Draw();
                    Player = player;
                    break;
                case Snake snake:
                    snake.SetCharacterData("snake", snake.Health, ConsoleColor.Green, new Dice(3,4,2), new Dice(1,8,5));
                    snake.StatusCheck();
                    break;
                case Rat rat:
                    rat.SetCharacterData("rat", rat.Health, ConsoleColor.Red, new Dice(1, 6, 3), new Dice(1, 6, 1));
                    rat.StatusCheck();
                    break;
                case Wall wall:
                    wall.SetCharacterData(null, 0, ConsoleColor.Gray);
                    wall.StatusCheck();
                    break;
                case Gold gold:
                    gold.SetCharacterData(null, 0, ConsoleColor.DarkYellow, RandyRandom.number.Next(1, 20));
                    gold.StatusCheck();
                    break;
                case InventoryStructure inventory:
                    inventory.SetCharacterData(ConsoleColor.Gray);
                    inventory.StatusCheck();
                    break;
                case Inventory inventory:
                    inventory.SetCharacterData(ConsoleColor.Gray);
                    inventory.StatusCheck();
                    break;
                case Equipment sword:
                    sword.SetCharacterData(ConsoleColor.Magenta);
                    sword.StatusCheck();
                    break;
            }
        }
        if(Player.Equipment.Count > 0)
        {
            for(int i = 0; i < Player.Equipment.Count; i++)
            {
                Creature.LootItem(Player, Player.Equipment.ElementAt(i));
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
    private void ElementUpdate()
    {
        List<LevelElement> elementsToSave = new List<LevelElement>();

        while(Player.Health > 0)
        {
            KeyInfo.Input = Console.ReadKey(true);
            if(KeyInfo.Input.Key == ConsoleKey.H)
            {
                Player.Turn -= 1;
            }
            ClearInterface();
            Player.Update();
            foreach(LevelElement element in LevelData.Elements)
            {
                element.Player = Player;
                switch(element)
                {
                    case Rat rat:
                        rat.Update();
                        rat.CheckCollision();
                        rat.StatusCheck();
                        break;

                    case Snake snake:
                        snake.Update();
                        snake.CheckCollision();
                        snake.StatusCheck();

                        break;
                    case Wall wall:
                        wall.StatusCheck();
                        break;

                    case Gold gold:
                        gold.StatusCheck();
                        break;
                }
                if(KeyInfo.Input.Key == ConsoleKey.H)
                {
                    elementsToSave.Add(element);
                }
            }
            foreach(LevelElement destroyedData in DeleteObjects.List)
            {
                LevelData.Elements.Remove(destroyedData);
            }
            DeleteObjects.TrackList = DeleteObjects.List;
            DeleteObjects.ClearCache();

            if(KeyInfo.Input.Key == ConsoleKey.H)
            {
                var connectionString = "mongodb://localhost:27017";

                using var client = new MongoClient(connectionString);

                var database = client.GetDatabase("HenrikVu");
                database.DropCollection("saveFile");
                var collection = database.GetCollection<BsonDocument>("saveFile");

                if(DeleteObjects.TrackList.Count > 0)
                {
                    foreach(LevelElement destroyedData in DeleteObjects.TrackList)
                    {
                        var filterId2 = Builders<BsonDocument>.Filter.Eq("_id", destroyedData.Id);
                        collection.DeleteOne(filterId2);
                    }
                    DeleteObjects.ClearCache2();
                }

                SaveToDatabase(elementsToSave);
                elementsToSave.Clear();;
                Console.SetCursorPosition(0, 22); Console.WriteLine("Saved. Press G to exit");
                var exit = Console.ReadKey();
                if(exit.Key == ConsoleKey.G){ break; } 
                else
                {
                    Console.SetCursorPosition(0, 22); Console.WriteLine("                                         ");
                    Console.SetCursorPosition(0, 22); Console.WriteLine("Press H to save");
                }
            }
        }
        Player.Remove();
        Console.SetCursorPosition(20,20);
        Console.WriteLine();

        void ClearInterface()
        {
            Console.SetCursorPosition(0, 0);
            Console.Write(new string(' ', (Console.WindowWidth * 3)));
        }
    }
    private void LoadUserInterface()
    {
        Console.CursorVisible = false;
        Console.SetCursorPosition(0, 0);
        Console.Write($"Name: {Player.Name}   -   Health: {Player.Health}/{Player.MaxHealth}   -   Turn:  {Player.Turn+1}   Gold: {Player.Gold}   ");
        Console.SetCursorPosition(64, 8);
        Console.Write($"Inventory(1-9)(F1-F9)");
        Console.SetCursorPosition(64, 14); Console.WriteLine($"Equipments"); 
        Console.SetCursorPosition(64, 15); Console.WriteLine($"Wep:"); 
        Console.SetCursorPosition(64, 16); Console.WriteLine($"OffH:"); 
        Console.SetCursorPosition(64, 17); Console.WriteLine($"Arm:"); 
        Console.SetCursorPosition(64, 18); Console.WriteLine($"Helm:"); 
        Console.SetCursorPosition(64, 19); Console.WriteLine($"Glove:"); 
        Console.SetCursorPosition(64, 20); Console.WriteLine($"Boots:");
        Console.SetCursorPosition(0, 22); Console.WriteLine("Press H to save");
    }

    private void SaveToDatabase(List<LevelElement> elements)
    {
        var connectionString = "mongodb://localhost:27017";

        using var client = new MongoClient(connectionString);

        var database = client.GetDatabase("HenrikVu");
        var collection = database.GetCollection<BsonDocument>("saveFile");

        foreach(var element in elements)
        {
            var newElement = new BsonDocument{
                { "_id", ObjectId.GenerateNewId() },
                { "Character", element.Character.ToString() },
                { "Position_X", element.Position_X },
                { "Position_Y", element.Position_Y },
                { "Color", element.Color }
            };

            if(element is Wall wall)
            {
                newElement.Add("IsDrawn", wall.IsDrawn);
            } 
            else if(element is Gold gold)
            {
                newElement.Add("IsDrawn", gold._isDrawn);
            }
            else if(element is Player player)
            {
                var equipmentBsonArray = new BsonArray();
                foreach(var equipment in player.Equipment)
                {
                    var equipmentBson = new BsonDocument
                    {
                        { "Name", equipment.name },
                        { "AttackModifier", equipment.AttackModifier },
                        { "DefenseModifier", equipment.DefenseModifier },
                        { "Color", equipment.Color },
                        { "Character", equipment.Character.ToString() }
                    };
                    equipmentBsonArray.Add(equipmentBson);
                }

                newElement.Add("Turn", player.Turn);
                newElement.Add("Health", player.Health);
                newElement.Add("Gold", player.Gold);
                newElement.Add("Name", player.Name);
                newElement.Add("Equipment", equipmentBsonArray);
            }
            else if(element is Enemy enemy)
            {
                newElement.Add("Health", enemy.Health);
                newElement.Add("Name", enemy.Name);
            }

            collection.InsertOne(newElement);
        }
    }
}

