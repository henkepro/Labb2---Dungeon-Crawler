﻿using System.Reflection.Emit;
using System.Xml.Linq;

class LevelUpdate
{
    public Player Player { get; set; }
    public LevelData LevelData { get; } = new LevelData();
    public List<LevelElement> deleteObjectList = new List<LevelElement>();
    public void LevelStart()
    {
        ElementExtract();
        LoadUserInterface();
        ElementUpdate();
    }
    void ElementExtract()
    {
        using(StreamReader readMap = new StreamReader(@"..\..\..\Labb2\Misc\Level1.txt"))
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
            switch (element)
            {
                case Player player:
                    player.SetCharacterData('@', "Henrik", 100, ConsoleColor.Gray, new Dice(2, 5, 6), new Dice(2, 6, 0));
                    player.Draw();
                    break;
                case Snake snake:
                    snake.SetCharacterData('s', "snake", 25, ConsoleColor.Green, new Dice(1,8,5), new Dice(3,4,2));
                    snake.StatusCheck(null, Player);
                    break;
                case Rat rat:
                    rat.SetCharacterData('r', "rat", 10, ConsoleColor.Red, new Dice(1, 6, 3), new Dice(1, 6, 1));
                    rat.StatusCheck(null, Player);
                    break;
                case Wall wall:
                    wall.SetCharacterData('#', null, 0, ConsoleColor.Gray);
                    wall.StatusCheck(null, Player);
                    break;
            }
        }
    }
    void ElementUpdate()
    {
        while(true)
        {
            ConsoleKeyInfo keyinfo = Console.ReadKey();
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
                            wall.StatusCheck(deleteObjectList, Player);
                            break;
                    }
                }
            }
            else
            {
                Player.Remove();
                Console.SetCursorPosition(20, 20);
                break;
            }
            foreach(LevelElement destroyedData in deleteObjectList)
            {
                LevelData.Elements.Remove(destroyedData);
            }
        }
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
}