public class Dice
{
    Random random = new Random();
    public int NumberOfDice { get; set; }
    public int SidesPerDice { get; set; }
    public int Modifier { get; set; }
    public int DiceValue { get; set; }
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




