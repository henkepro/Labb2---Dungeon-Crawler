using System.Xml.Linq;

public abstract class Item : LevelElement
{
    public string name;
    public bool _isEquipped = false;
    public int AttackModifier { get; set; }
    public int DefenseModifier { get; set; }
    public void StatusCheck()
    {
        Draw();
    }
    public void SetCharacterData(ConsoleColor color)
    {
        Color = color;
    }
}

public class Equipment : Item
{
    public void Excalibur()
    {
        name = "Excalibur";
        AttackModifier = 50;
        DefenseModifier = 20;
    }
    public void GodSword()
    {
        name = "GodSword";
        AttackModifier = 20;
        DefenseModifier = 0;
    }
}
