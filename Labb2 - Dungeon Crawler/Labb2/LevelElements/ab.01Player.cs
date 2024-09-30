using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Player : LevelElement
{
    public int Turn { get; set; }
    public string Name { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public Dice AttackDice { get; set; }
    public Dice DefenseDice { get; set; }
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
        foreach(LevelElement element in levelData)
        {
            if(Position_X == element.Position_X && Position_Y == element.Position_Y && element != this && !CollisionDetected)
            {
                CollisionDetected = true;
            }
            if(CollisionDetected)
            {
                Console.SetCursorPosition(0, 1);
                switch(element)
                {
                    case Rat rat:
                        creature.PlayerAttack(this, rat);
                        creature.EnemyAttack(this, rat);
                        break;

                    case Snake snake:
                        creature.PlayerAttack(this, snake);
                        creature.EnemyAttack(this, snake);
                        break;
                }
                LoadPosition();
            }
        }
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
    public void Movement(ConsoleKeyInfo keyinfo)
    {
        switch(keyinfo.Key)
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
    public override string ToString()
    {
        return $"You (ATK: {AttackDice}) attacked the {Enemy.Name} (DEF: {Enemy.DefenseDice}), ";
    }
}
