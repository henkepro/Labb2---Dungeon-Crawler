using System;

public class CreatureAttack
{
    public void PlayerAttack(Player player, Enemy enemy)
    {
        player.Enemy = enemy;
        if(player.Health > 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            int attackResult = player.AttackDice.Throw() - enemy.DefenseDice.Throw();
            if(attackResult > 0)
            {
                enemy.Health -= attackResult;
            }
            if(enemy.Health < 0)
            {
                Console.Write($"{player}killing it instantly.");
            }
            else
            {
                switch(attackResult)
                {
                    case <= 0:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write($"{player}but did not manage to make any damage.");

                        break;
                    case < 3:
                        Console.Write($"{player}slightly wounding it.");

                        break;
                    case < 7:
                        Console.Write($"{player}moderately wounding it.");

                        break;
                    case >= 7:
                        Console.Write($"{player}severely wounding it.");

                        break;
                }
            }
            Console.WriteLine();
        }
    }
    public void EnemyAttack(Player player, Enemy enemy)
    {
        enemy.Player = player;
        if (enemy.Health > 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            int attackResult = enemy.AttackDice.Throw() - player.DefenseDice.Throw();
            if(attackResult > 0)
            {
                player.Health -= attackResult;
            }
            if(player.Health <= 0)
            {
                Console.Write($"{enemy}killing you instantly. (GAME OVER)");
                player.Health = 0;
            }
            else
            {
                switch(attackResult)
                {
                    case <= 0:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write($"{enemy}but did not manage to make any damage.");

                        break;
                    case < 3:
                        Console.Write($"{enemy}slightly wounding you.");

                        break;
                    case < 7:
                        Console.Write($"{enemy}moderately wounding you.");

                        break;
                    case >= 7:
                        Console.Write($"{enemy}severly wounding you.");

                        break;
                }
            }
            Console.WriteLine();
        }
    }
}




