using System;
using System.Data;
using System.Xml.Linq;

LevelData levelData = new LevelData();
ReadLevelFile readLevelFile = new ReadLevelFile();

readLevelFile.ExtractData(levelData);
Player player = levelData.Elements.OfType<Player>().Single();
ElementExtract(levelData.Elements);
ElementUpdate(levelData.Elements, player);
Console.SetCursorPosition(0, 0);
Console.Write($"Name: {player.Name}   -   Health: {player.Health}/{player.MaxHealth}   -   Turn: {player.Turn}   ");
void ElementExtract(List<LevelElement> levelData)
{
    foreach (LevelElement element in levelData)
    {
        Console.SetCursorPosition(element.Position_X, element.Position_Y);
        int squareDistance = (int)(Math.Pow(element.Position_X - player.Position_X, 2) + Math.Pow(element.Position_Y - player.Position_Y, 2));
        switch (element)
        {
            case Player player:
                player.CharacterData('@', "Henrik", 100, ConsoleColor.Gray);
                player.Draw();
                break;
            case Snake snake:
                snake.CharacterData('s', "snake", 25, ConsoleColor.Green);
                snake.StatusCheck(null, player);
                break;
            case Rat rat:
                rat.CharacterData('r', "rat", 10, ConsoleColor.Red);
                rat.StatusCheck(null, player);
                break;
            case Wall wall:
                wall.CharacterData('#', ConsoleColor.Gray);
                wall.RangeCheck(element, player);
                break;
        }
    }
}
void ElementUpdate(List<LevelElement> levelData, Player player)
{
    List<LevelElement> deleteObjectList = new List<LevelElement>();
    while (true)
    {
        ConsoleKeyInfo keyinfo = Console.ReadKey(true);
        Console.SetCursorPosition(0, 1);
        Console.Write(new string(' ', (Console.WindowWidth * 2)));
        player.Update(levelData, keyinfo);
        if (player.Health > 0)
        {
            foreach (LevelElement element in levelData)
            {
                switch (element)
                {
                    case Rat rat:
                        rat.Update();
                        rat.CheckCollision(levelData);
                        rat.StatusCheck(deleteObjectList, player);
                        break;

                    case Snake snake:
                        snake.Update();
                        snake.Movement(levelData, player);
                        snake.CheckCollision(levelData);
                        snake.StatusCheck(deleteObjectList, player);

                        break;
                    case Wall wall:
                        wall.RangeCheck(element, player);
                        break;
                }
            }
        } 
        else
        {
            Console.SetCursorPosition(20,20);
            break;
        }

        foreach (var destroyedData in deleteObjectList)
        {
            levelData.Remove(destroyedData);
        }
    }
}
class LevelData
{
    private int _position_X;
    private int _position_Y = 3;
    private List<LevelElement> _elements = new List<LevelElement>();
    public List<LevelElement> Elements { get { return _elements; } }
    public void Load(string filename)
    {
        foreach (char s in filename)
        {
            if (s == '#')
            {
                LevelElement element = new Wall();
                Wall wall = new Wall();
                wall.Position_X = _position_X;
                wall.Position_Y = _position_Y;
                _elements.Add(wall);
            }
            else if (s == '@')
            {
                LevelElement element = new Player();
                Player player = (Player)element;
                player.Position_X = _position_X;
                player.Position_Y = _position_Y;
                player.AttackDice = new Dice(2, 5, 6);
                player.DefenseDice = new Dice(2, 6, 0);
                _elements.Add(player);
            }
            else if (s == 's')
            {
                LevelElement element = new Snake();
                Snake snake = (Snake)element;
                snake.Position_X = _position_X;
                snake.Position_Y = _position_Y;
                snake.AttackDice = new Dice(3, 4, 2);
                snake.DefenseDice = new Dice(1, 8, 5);
                _elements.Add(snake);
            }
            else if (s == 'r')
            {
                LevelElement element = new Rat();
                Rat rat = (Rat)element;          
                rat.Position_X = _position_X;
                rat.Position_Y = _position_Y;
                rat.AttackDice = new Dice(1, 6, 3);
                rat.DefenseDice = new Dice(1, 6, 1);
                _elements.Add(rat);
            }
            _position_X++;
        }
        _position_X = 0;
        _position_Y++;
    }
}
abstract class LevelElement
{
    protected Random random = new Random();
    public char Character { get; set; }
    public int Position_X { get; set; }
    public int Position_Y { get; set; }
    public ConsoleColor Color { get; set; }
    public virtual void CharacterData(char character, ConsoleColor color)
    {
        Character = character;
        Color = color;
    }
    public void Draw()
    {
        Console.ForegroundColor = Color;
        Console.SetCursorPosition(Position_X, Position_Y);
        Console.Write(Character);
    }
    public void Remove()
    {
        Console.SetCursorPosition(Position_X, Position_Y);
        Console.Write(' ');
    }
}
class Player : LevelElement
{
    private int prevPosition_X;
    private int prevPosition_Y;
    public int Turn { get; set; }
    public string Name { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public Dice AttackDice { get; set; }
    public Dice DefenseDice { get; set; }
    public Enemy Enemy { get; set; }
    public bool CollisionDetected { get; set; }
    public virtual void PlayerAttack(List<LevelElement> levelData, Player player, Enemy enemy)
    {
        if (player.Health > 0)
        {
            Enemy = enemy;
            Console.ForegroundColor = ConsoleColor.Yellow;
            int attackResult = player.AttackDice.Throw() - enemy.DefenseDice.Throw();
            if (attackResult > 0)
            {
                enemy.Health -= attackResult;
            }
            if (enemy.Health < 0)
            {
                Console.Write($"{this}killing it instantly.");
            }
            else
            {
                switch (attackResult)
                {
                    case <= 0:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write($"{this}but did not manage to make any damage.");

                        break;
                    case < 3:
                        Console.Write($"{this}slightly wounding it.");

                        break;
                    case < 7:
                        Console.Write($"{this}moderately wounding it.");

                        break;
                    case >= 7:
                        Console.Write($"{this}severely wounding it.");

                        break;
                }
            }
            Console.WriteLine();
        }
    }
    public override string ToString()
    {
        return $"You (ATK: {AttackDice}) attacked the {Enemy.Name} (DEF: {Enemy.DefenseDice}), ";
    }
    public void CharacterData(char character, string name, int maxHealth, ConsoleColor color)
    {
        Name = name;
        Health = maxHealth;
        MaxHealth = maxHealth;
        Character = character;
        Color = color;
        Console.SetCursorPosition(0, 0);
        Console.Write($"Name: {Name}   -   Health: {Health}/{MaxHealth}   -   Turn: {Turn}   ");
    }
    public void SavePosition()
    {
        prevPosition_X = Position_X;
        prevPosition_Y = Position_Y;
        CollisionDetected = false;
    }
    public void LoadPosition()
    {
        Position_X = prevPosition_X;
        Position_Y = prevPosition_Y;
        CollisionDetected = false;
    }
    public void Update(List<LevelElement> levelData, ConsoleKeyInfo keyinfo)
    {
        Remove();
        SavePosition();
        Movement(keyinfo);
        CheckCollision(levelData);
        Draw();
        Turn++;
        Console.SetCursorPosition(0, 0);
        Console.Write($"Name: {Name}   -   Health: {Health}/{MaxHealth}   -   Turn: {Turn}   ");
    }
    public void CheckCollision(List<LevelElement> levelData)
    {
        foreach (LevelElement element in levelData)
        {
            if (Position_X == element.Position_X && Position_Y == element.Position_Y && element != this && !CollisionDetected)
            {
                CollisionDetected = true;
            }
            if (CollisionDetected)
            {
                Console.SetCursorPosition(0, 1);
                switch (element)
                    {
                        case Rat:
                        Rat rat = (Rat)element;
                        PlayerAttack(levelData, this, rat);
                        rat.EnemyAttack(levelData, this, rat);
                        break;

                        case Snake:
                        Snake snake = (Snake)element;
                        PlayerAttack(levelData, this, snake);
                        snake.EnemyAttack(levelData, this, snake);
                        break;
                    }
                LoadPosition();
            }
        }
    }
    public void Movement(ConsoleKeyInfo keyinfo)
    {
        switch (keyinfo.Key)
        {
            case ConsoleKey.LeftArrow:
            Position_X--;
                break;
            case ConsoleKey.RightArrow:
            Position_X++;
                break;
            case ConsoleKey.UpArrow:
            Position_Y--;
                break;
            case ConsoleKey.DownArrow:
            Position_Y++;
                break;
        }
    }
}
abstract class Enemy : LevelElement
{
    protected enemyPattern _pattern;
    protected int prevPosition_X;
    protected int prevPosition_Y;
    public string Name { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public Dice AttackDice { get; set; }
    public Dice DefenseDice { get; set; }
    public Player PlayerOne { get; set; }
    public bool CollisionDetected { get; set; }
    public enemyPattern Pattern { get; set; }
    public virtual void StatusCheck(List<LevelElement> element, Player player)
    {
        PlayerOne = player;
        int squareDistance = (int)(Math.Pow(Position_X - player.Position_X, 2) + Math.Pow(Position_Y - player.Position_Y, 2));
        if (Health > 0 && squareDistance <= 25)
        {
            Draw();
        }
        else if (Health <= 0)
        {
            element.Add(this);
        }
    }
    public void CharacterData(char character, string name, int maxHealth, ConsoleColor color)
    {
        Character = character;
        Name = name;
        MaxHealth = maxHealth;
        Health = maxHealth;
        Color = color;
    }
    public virtual void EnemyAttack(List<LevelElement> levelData, Player player, Enemy enemy)
    {
        if (enemy.Health > 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            int attackResult = enemy.AttackDice.Throw() - player.DefenseDice.Throw();
            if (attackResult > 0)
            {
                player.Health -= attackResult;
            }
            if (player.Health <= 0)
            {
                Console.Write($"{this}killing you instantly. (GAME OVER)");
                player.Health = 0;
            }
            else 
            { 
                switch (attackResult)
                {
                    case <= 0:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write($"{this}but did not manage to make any damage.");

                        break;
                    case < 3:
                        Console.Write($"{this}slightly wounding you.");

                        break;
                    case < 7:
                        Console.Write($"{this}moderately wounding you.");

                        break;
                    case >= 7:
                        Console.Write($"{this}severly wounding you.");

                        break;
                }
            }
            Console.WriteLine();
        }
    }
    public void SavePosition()
    {
        prevPosition_X = Position_X;
        prevPosition_Y = Position_Y;
        CollisionDetected = false;
    }
    public void LoadPosition()
    {
        Position_X = prevPosition_X;
        Position_Y = prevPosition_Y;
        CollisionDetected = false;
    }
    public virtual void Movement()
    {
        Pattern = (enemyPattern)random.Next(0, 4);
        switch (Pattern)
        {
            case enemyPattern.up:
                Position_Y--;
                break;
            case enemyPattern.down:
                Position_Y++;
                break;
            case enemyPattern.left:
                Position_X--;
                break;
            case enemyPattern.right:
                Position_X++;
                break;
            default:
                break;
        }
    }
    public void CheckCollision(List<LevelElement> levelData)
    {
        foreach (LevelElement element in levelData)
        {
            if (Position_X == element.Position_X && Position_Y == element.Position_Y && element != this && !CollisionDetected)
            {
                CollisionDetected = true;
            }
            if (CollisionDetected)
            {
                Console.SetCursorPosition(0, 1);
                switch (element)
                {
                    case Player:
                        EnemyAttack(levelData, PlayerOne, this);
                        PlayerOne.PlayerAttack(levelData, PlayerOne, this);
                        break;
                    default:
                        break;
                }
                LoadPosition();
            }
        }
    }
    public override string ToString()
    {
        return $"The {Name} (ATK: {AttackDice}) attacked you (DEF: {PlayerOne.DefenseDice}), ";
    }
    public abstract void Update();
}
class Rat : Enemy
{
    public override void Update()
    {
        SavePosition();
        Remove();
        Movement();
    }
}
class Snake : Enemy
{
    public void Movement(List<LevelElement> levelData, Player player)
    {
        foreach (LevelElement element in levelData)
        {
            if (!CollisionDetected)
            {
                if ((Math.Abs(Position_X - player.Position_X) == 1 && Math.Abs(Position_Y - player.Position_Y) == 1)
                    || (Math.Abs(Position_X - player.Position_X) == 1 && Math.Abs(Position_Y - player.Position_Y) == 0))
                {
                    if (Position_X > player.Position_X)
                    {
                        Position_X++;
                    }
                    else
                    {
                        Position_X--;
                    }
                }
                else if ((Math.Abs(Position_X - player.Position_X) == 1 && Math.Abs(Position_Y - player.Position_Y) == 1)
                    || (Math.Abs(Position_X - player.Position_X) == 0 && Math.Abs(Position_Y - player.Position_Y) == 1))
                {
                    if (Position_Y > player.Position_Y)
                    {
                        Position_Y++;
                    }
                    else
                    {
                        Position_Y--;
                    }
                }
            }
        }
    }
    public override void Update()
    {
        SavePosition();
        Remove();
    }
}
class Dice
{
    Random random = new Random();
    int NumberOfDice { get; set; }
    int SidesPerDice { get; set; }
    int Modifier { get; set; }
    int DiceValue { get; set; }
    public Dice(int numberOfDice, int sidesPerDice, int modifier)
    {
        NumberOfDice = numberOfDice;
        SidesPerDice = sidesPerDice;
        Modifier = modifier;
    }
    public int Throw()
    {
        DiceValue = 0;
        for (int i = 0; i < NumberOfDice; i++)
        {
            DiceValue += random.Next(1, SidesPerDice + 1);
        }
        return DiceValue += Modifier;
    }
    public override string ToString()
    {
        return $"{NumberOfDice}d{SidesPerDice}+{Modifier} => {DiceValue}";
    }
}
class Wall : LevelElement
{
    public bool IsDrawn { get; set; }
    public void RangeCheck(LevelElement element, Player player)
    {
        int squareDistance = (int)(Math.Pow(element.Position_X - player.Position_X, 2) + Math.Pow(element.Position_Y - player.Position_Y, 2));
        if (squareDistance <= 25 && IsDrawn == false)
        {
            element.Draw();
            IsDrawn = true;
        }
    }
}
class ReadLevelFile
{
    public void ExtractData(LevelData levelData)
    {
        Console.CursorVisible = false;
        using (StreamReader readMap = new StreamReader("Level1.txt"))
        {
            string levelMap = null;
            while ((levelMap = readMap.ReadLine()) != null)
            {
                if (levelMap != null)
                {
                    levelData.Load(levelMap);
                }
            }
            
        }
    }
}
public enum enemyPattern { left, right, up, down };




