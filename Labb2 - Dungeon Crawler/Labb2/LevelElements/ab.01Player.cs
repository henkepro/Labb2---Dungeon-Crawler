using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

public class Player : LevelElement
{
    public Dictionary<int, Inventory> Inventory { get; set; } = new Dictionary<int, Inventory>();
    public Dictionary<int, string> Equipped { get; set; } = new Dictionary<int, string>();
    public Stack<Equipment> Equipment { get; set; } = new Stack<Equipment>();
    bool _itemEquipped = false;
    public int Gold { get; set; }
    public int Turn { get; set; }
    public string Name { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public Dice AttackDice { get; set; }
    public Dice DefenseDice { get; set; }
    public void Update()
    {
        SavePosition();
        Remove();
        PlayerInput();
        CheckCollision();
        Draw();
        InterfaceUpdate();
    }
    private void CheckCollision()
    {
        foreach(LevelElement element in LevelData.Elements)
        {
            bool PositionX = Position_X == element.Position_X;
            bool PositionY = Position_Y == element.Position_Y;
            bool PositionCollide = PositionX && PositionY;
            if((PositionCollide && element != this))
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
                        creature.LootItem(this, sword);
                        Equipment.Push(sword);
                        LoadPosition(element);
                        break;
                    case Gold gold:
                        creature.LootGold(this, gold);
                        LoadPosition(element);
                        break;
                }
                LoadPosition(element);
            }
        }
    }
    private void PlayerInput()
    {
       EquipItem(Equipment);
       TrashItem(Equipment);
       Movement();
    }
    private void TrashItem(Stack<Equipment> items)
    {
        bool isNumberFKey = KeyInfo.Input.Key >= ConsoleKey.F1 && KeyInfo.Input.Key <= ConsoleKey.F9;
        if(items.Count() != 0 && isNumberFKey)
        {
            bool removeItem = false;
            foreach(Equipment item in items)
            {
                switch(KeyInfo.Input.Key)
                {
                    case ConsoleKey.F1:
                        int lastIndex = Equipment.Count() -1;
                        creature.inventoryCount = lastIndex;
                        if(item.Position_X == Inventory[lastIndex].Position_X && item._isEquipped != true)
                        {
                            item.Remove();
                            Console.SetCursorPosition(item.Position_X, item.Position_Y);
                            Console.Write("x");
                            removeItem = true;
                            Console.SetCursorPosition(65, 12);
                            Console.Write($"Deleted item {item.name}    ");
                        }
                        break;
                }
            }
            if(removeItem)
            {
                Equipment.Pop();
                removeItem = false;
            }
        }
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
                Console.WriteLine($"Equipped: {item.name}    ATK:{item.AttackModifier} DEF:{item.DefenseModifier}                 ");

            } 
            else
            {
                Console.SetCursorPosition(item.Position_X - 1, item.Position_Y);
                Console.Write(" ");
                item._isEquipped = false;
                AttackDice.Modifier -= item.AttackModifier;
                DefenseDice.Modifier -= item.DefenseModifier;
                Console.SetCursorPosition(65, 11);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"UnEquipped: {item.name}                                                          ");
            }
        }
    }
    private void EquipItem(Stack<Equipment> items)
    {
        bool isNumberKey = KeyInfo.Input.Key >= ConsoleKey.D1 && KeyInfo.Input.Key <= ConsoleKey.D9;
        if(items != null && isNumberKey)
        {
            foreach(Equipment item in items)
            {
                switch(KeyInfo.Input.Key)
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
    private void Movement()
    {
        switch(KeyInfo.Input.Key)
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
        Console.Write($"Name: {Name}   -   Health: {Health}/{MaxHealth}   -   Turn:  {Turn}   Gold: {Gold}");
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
