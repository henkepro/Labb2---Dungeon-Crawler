using System.Reflection.Emit;
using System.Xml.Linq;

class LevelUpdate
{
    public Player Player { get; set; }
    public LevelData LevelData { get; } = new LevelData();
    public List<LevelElement> deleteObjectList = new List<LevelElement>();
    public void LevelStart()
    {
        FileRead("Level1.txt");
        FileRead("Inventory.txt");
        ElementExtract();
        LoadUserInterface();
        ElementUpdate();
    }
    void ElementExtract()
    {
        Player = LevelData.Elements.OfType<Player>().First();
        foreach(LevelElement element in LevelData.Elements)
        {
            element.Player = Player;
            switch (element)
            {
                case Player player:
                    player.SetCharacterData("Henrik", 100, ConsoleColor.Gray, new Dice(2, 6, 2), new Dice(2, 6, 0), LevelData.Inventory);
                    player.Draw();
                    break;
                case Snake snake:
                    snake.SetCharacterData("snake", 25, ConsoleColor.Green, new Dice(3,4,2), new Dice(1,8,5));
                    snake.StatusCheck(null);
                    break;
                case Rat rat:
                    rat.SetCharacterData("rat", 10, ConsoleColor.Red, new Dice(1, 6, 3), new Dice(1, 6, 1));
                    rat.StatusCheck(null);
                    break;
                case Wall wall:
                    wall.SetCharacterData(null, 0, ConsoleColor.Gray);
                    wall.StatusCheck(null);
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
    void ElementUpdate()
    {
        while(Player.Health > 0)
        {
            ConsoleKeyInfo keyinfo = Console.ReadKey(true);
            ClearInterface();
            Player.Update(LevelData.Elements, keyinfo);
            foreach(LevelElement element in LevelData.Elements)
            {
                element.Player = Player;
                switch(element)
                {
                    case Rat rat:
                        rat.Update();
                        rat.CheckCollision(LevelData.Elements);
                        rat.StatusCheck(deleteObjectList);
                        break;

                    case Snake snake:
                        snake.Update();
                        snake.CheckCollision(LevelData.Elements);
                        snake.StatusCheck(deleteObjectList);

                        break;
                    case Wall wall:
                        wall.StatusCheck(deleteObjectList);
                        break;
                }
            }
            foreach(LevelElement destroyedData in deleteObjectList)
            {
                LevelData.Elements.Remove(destroyedData);
            }
        }
        Player.Remove();
        Console.SetCursorPosition(20,20);
        Console.WriteLine();
    }
    void FileRead(string file)
    {
        using(StreamReader readMap = new StreamReader(@$"..\..\..\Labb2\Misc\{file}"))
        {
            string levelMap = null;
            while((levelMap = readMap.ReadLine()) != null)
            {
                if(levelMap != null)
                {
                    LevelData.Load(levelMap);
                }
            }
        }
        LevelData._position_X = 0;
        LevelData._position_Y = 3;
    }
    void LoadUserInterface()
    {
        Console.CursorVisible = false;
        Console.SetCursorPosition(0, 0);
        Console.Write($"Name: {Player.Name}   -   Health: {Player.Health}/{Player.MaxHealth}   -   Turn: {Player.Turn}   ");
        Console.SetCursorPosition(68, 8);
        Console.Write($"Inventory(1-9)");
    }
    void ClearInterface()
    {
        Console.SetCursorPosition(0, 1);
        Console.Write(new string(' ', (Console.WindowWidth * 2)));
        Console.SetCursorPosition(63, 11);
        Console.Write(new string(' ', (Console.WindowWidth - 63)));
    }
}