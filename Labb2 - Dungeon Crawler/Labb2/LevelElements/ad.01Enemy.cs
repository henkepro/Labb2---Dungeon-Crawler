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
    public virtual void StatusCheck()
    {
        if(Health > 0 && SquareDistanceTo(Player) <= 25)
        {
            Draw();
        }
        else if(Health <= 0)
        {
            DeleteObjects.List.Add(this);
        }
    }
    public void CheckCollision()
    {
        foreach(LevelElement element in LevelData.Elements)
        {
            bool positionX = Position_X == element.Position_X;
            bool positionY = Position_Y == element.Position_Y;
            bool positionCollide = positionX && positionY;
            if(positionCollide && element != this)
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
                LoadPosition(element);
            }
        }
    }
    public void SetCharacterData(string name, int maxHealth, ConsoleColor color, Dice attackDice, Dice defenseDice)
    {
        AttackDice = attackDice;
        DefenseDice = defenseDice;
        Name = name;
        MaxHealth = maxHealth;
        Health = maxHealth;
        Color = color;
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
    protected int SquareDistanceTo(Player player)
    {
        int DeltaXsquared = (int)Math.Pow(Position_X - player.Position_X, 2);
        int DeltaYsquared = (int)Math.Pow(Position_Y - player.Position_Y, 2);
        int squareDistance = DeltaXsquared + DeltaYsquared;
        return squareDistance;
    }
    public abstract void Update();
}
