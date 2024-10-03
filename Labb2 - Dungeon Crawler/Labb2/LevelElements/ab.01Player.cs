using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Player : LevelElement
{
    public Dictionary<int, Inventory> Inventory { get; set; } = new Dictionary<int, Inventory>();
    public Stack<Equipment> Equipment { get; set; } = new Stack<Equipment>();
    bool _itemEquipped = false;
    public int addtoModifier;
    public int Turn { get; set; }
    public string Name { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public Dice AttackDice { get; set; }
    public Dice DefenseDice { get; set; }
    public void Update(List<LevelElement> levelData, ConsoleKeyInfo keyinfo)
    {
        SavePosition();
        Remove();
        PlayerInput(keyinfo);
        CheckCollision(levelData);
        Draw();
        InterfaceUpdate();
    }
    private void CheckCollision(List<LevelElement> levelData)
    {
        foreach(LevelElement element in levelData)
        {
            bool positionCollide = (Position_X == element.Position_X) && (Position_Y == element.Position_Y);
            if((positionCollide && element != this))
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
                    case Equipment sword:
                        creature.LootItem(this, sword, Inventory);
                        Equipment.Push(sword);
                        break;
                }
                LoadPosition();
            }
        }
    }
    private void PlayerInput(ConsoleKeyInfo keyinfo)
    {
       EquipItem(keyinfo, Equipment);
       Movement(keyinfo);
    }
    private void IsEquipped(Equipment item, int key)
    {
        if(item.Position_X == Inventory[key].Position_X)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            if(!item._isEquipped)
            {
                Console.SetCursorPosition(item.Position_X - 1, item.Position_Y);
                Console.Write(":");
                item._isEquipped = true;
                AttackDice.Modifier += item.AttackModifier;
                DefenseDice.Modifier += item.DefenseModifier;
                Console.SetCursorPosition(65, 11);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Equipped: {item.name}");
            } 
            else
            {
                Console.SetCursorPosition(item.Position_X - 1, item.Position_Y);
                Console.Write(" ");
                item._isEquipped = false;
                AttackDice.Modifier -= item.AttackModifier;
                DefenseDice.Modifier -= item.DefenseModifier;
                Console.SetCursorPosition(63, 11);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"UnEquipped: {item.name}");
            }
        }
    }
    private void EquipItem(ConsoleKeyInfo keyInfo, Stack<Equipment> items)
    {
        bool isNumberKey = keyInfo.Key >= ConsoleKey.D1 && keyInfo.Key <= ConsoleKey.D9;
        if(items != null && isNumberKey)
        {
            foreach(Equipment item in items)
            {
                switch(keyInfo.Key)
                {
                    case ConsoleKey.D1:
                        IsEquipped(item, 0);
                        break;
                    case ConsoleKey.D2:
                        IsEquipped(item, 1);
                        break;
                    case ConsoleKey.D3:
                        IsEquipped(item, 2);
                        break;
                    case ConsoleKey.D4:
                        IsEquipped(item, 3);
                        break;
                    case ConsoleKey.D5:
                        IsEquipped(item, 4);
                        break;
                    case ConsoleKey.D6:
                        IsEquipped(item, 5);
                        break;
                    case ConsoleKey.D7:
                        IsEquipped(item, 6);
                        break;
                    case ConsoleKey.D8:
                        IsEquipped(item, 7);
                        break;
                    case ConsoleKey.D9:
                        IsEquipped(item, 8);
                        break;
                }
            }
        }
    }
    private void Movement(ConsoleKeyInfo keyinfo)
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
    private void InterfaceUpdate()
    {
        Turn++;
        Console.SetCursorPosition(0, 0);
        Console.Write($"Name: {Name}   -   Health: {Health}/{MaxHealth}   -   Turn: {Turn}   ");
    }
    public void SetCharacterData(string name, int maxHealth, ConsoleColor color, Dice attackDice, Dice defenseDice, Dictionary<int, Inventory> inventory)
    {
            Inventory = inventory;
            AttackDice = attackDice;
            Name= name;
            DefenseDice = defenseDice;
            Health = maxHealth;
            MaxHealth = maxHealth;
            Color = color;
    }
    public override string ToString()
    {
        return $"You (ATK: {AttackDice}) attacked the {Enemy.Name} (DEF: {Enemy.DefenseDice}), ";
    }
}
