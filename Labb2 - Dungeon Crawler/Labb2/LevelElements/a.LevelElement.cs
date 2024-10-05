using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public abstract class LevelElement
{
    private int prevPosition_X;
    private int prevPosition_Y;
    public Player Player { get; set; }
    public Enemy Enemy { get; set; }
    public bool CollisionDetected { get; set; }
    public char Character { get; set; }
    public int Position_X { get; set; }
    public int Position_Y { get; set; }
    public ConsoleColor Color { get; set; }
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
    public void SavePosition()
    {
        prevPosition_X = Position_X;
        prevPosition_Y = Position_Y;
    }
    public void LoadPosition(LevelElement element)
    {
        if(element is Equipment || element is Gold && this is Player)
        {
            CollisionDetected = false;
            return;
        }
        Position_X = prevPosition_X;
        Position_Y = prevPosition_Y;
        CollisionDetected = false;
    }
}
