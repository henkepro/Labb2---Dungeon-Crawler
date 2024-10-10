using System.Reflection.Emit;
using System.Xml.Linq;
class LevelUpdate
{
    private Player Player { get; set; }
    private LevelData LevelData { get; } = new LevelData();
    public void LevelStart()
    {
        LevelData.Load("Level1.txt");
        LevelData.Load("Inventory.txt");
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
                    player.SetCharacterData("Henrik", 100, ConsoleColor.Gray, new Dice(2, 6, 2), 
                        new Dice(2, 6, 0), LevelData.Inventory);
                    player.Draw();
                    break;
                case Snake snake:
                    snake.SetCharacterData("snake", 25, ConsoleColor.Green, new Dice(3,4,2), new Dice(1,8,5));
                    snake.StatusCheck();
                    break;
                case Rat rat:
                    rat.SetCharacterData("rat", 10, ConsoleColor.Red, new Dice(1, 6, 3), new Dice(1, 6, 1));
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
    }
    private void ElementUpdate()
    {
        while(Player.Health > 0)
        {
            KeyInfo.Input = Console.ReadKey(true);
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
                }
            }
            foreach(LevelElement destroyedData in DeleteObjects.List)
            {
                LevelData.Elements.Remove(destroyedData);
            }
            DeleteObjects.ClearCache();
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
    }
}