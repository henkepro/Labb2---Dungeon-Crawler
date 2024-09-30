public class Dice
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




