using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class Enemy : LevelElement
{
    public string Name { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public Dice AttackDice;
    public Dice DefenseDice;
    public virtual void StatusCheck(List<LevelElement> element, Player player)
    {
        int squareDistance = (int)(Math.Pow(Position_X - player.Position_X, 2) + Math.Pow(Position_Y - player.Position_Y, 2));
        if(Health > 0 && squareDistance <= 25)
        {
            Draw();
        }
        else if(Health <= 0)
        {
            element.Add(this);
        }
    }
    public override void CharacterData(char character, string name, int maxHealth, ConsoleColor color)
    {
        Character = character;
        Name = name;
        MaxHealth = maxHealth;
        Health = maxHealth;
        Color = color;
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
                switch(element)
                {
                    case Player player:
                        ClearInterface();
                        creature.EnemyAttack(player, this);
                        creature.PlayerAttack(player, this);
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
        return $"The {Name} (ATK: {AttackDice}) attacked you (DEF: {Player.DefenseDice}), ";
    }
    public void ClearInterface()
    {
        Console.SetCursorPosition(0, 1);
        Console.Write(new string(' ', (Console.WindowWidth * 2)));
        Console.SetCursorPosition(0, 1);
    }
    public abstract void Update();
}
