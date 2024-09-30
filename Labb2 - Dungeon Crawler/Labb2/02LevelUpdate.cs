class LevelUpdate
{
    public Player Player { get; set; }
    public LevelData LevelData { get; } = new LevelData();
    public void LevelStart()
    {
        ElementExtract();
        LoadUserInterface();
        ElementUpdate();
    }
    void LoadUserInterface()
    {
        Console.CursorVisible = false;
        Console.SetCursorPosition(0, 0);
        Console.Write($"Name: {Player.Name}   -   Health: {Player.Health}/{Player.MaxHealth}   -   Turn: {Player.Turn}   ");
    }
    void ClearInterface()
    {
        Console.SetCursorPosition(0, 1);
        Console.Write(new string(' ', (Console.WindowWidth * 2)));
    }
    void ElementExtract()
    {
        using(StreamReader readMap = new StreamReader("Level1.txt"))
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
        Player = LevelData.Elements.OfType<Player>().First();
        foreach(LevelElement element in LevelData.Elements)
        {
            int squareDistance = (int)(Math.Pow(element.Position_X - Player.Position_X, 2) + Math.Pow(element.Position_Y - Player.Position_Y, 2));
            switch(element)
            {
                case Player player:
                    player.CharacterData('@', "Henrik", 100, ConsoleColor.Gray);
                    player.Draw();
                    break;
                case Snake snake:
                    snake.CharacterData('s', "snake", 25, ConsoleColor.Green);
                    snake.StatusCheck(null, Player);
                    break;
                case Rat rat:
                    rat.CharacterData('r', "rat", 10, ConsoleColor.Red);
                    rat.StatusCheck(null, Player);
                    break;
                case Wall wall:
                    wall.CharacterData('#', ConsoleColor.Gray);
                    wall.RangeCheck(element, Player);
                    break;
            }
        }
    }
    void ElementUpdate()
    {
        List<LevelElement> deleteObjectList = new List<LevelElement>();
        while(true)
        {
            ConsoleKeyInfo keyinfo = Console.ReadKey(true);
            ClearInterface();
            Player.Update(LevelData.Elements, keyinfo);
            if(Player.Health > 0)
            {
                foreach(LevelElement element in LevelData.Elements)
                {
                    switch(element)
                    {
                        case Rat rat:
                            rat.Update();
                            rat.CheckCollision(LevelData.Elements);
                            rat.StatusCheck(deleteObjectList, Player);
                            break;

                        case Snake snake:
                            snake.Player = Player;
                            snake.Update();
                            snake.CheckCollision(LevelData.Elements);
                            snake.StatusCheck(deleteObjectList, Player);

                            break;
                        case Wall wall:
                            wall.RangeCheck(element, Player);
                            break;
                    }
                }
            }
            else
            {
                Console.SetCursorPosition(20, 20);
                break;
            }
            foreach(LevelElement destroyedData in deleteObjectList)
            {
                LevelData.Elements.Remove(destroyedData);
            }
        }
    }
}