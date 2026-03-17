namespace Lab08;

public class Maelstrom : Monster
{
    public Maelstrom()
    {
        Name = "maelstrom";
        Health = 6;
        ArmorClass = 15;
        Weapon = new Weapon("wind blast", 1, 6);
        Inventory.Add(Weapon);
    }

    public override string SenseMessage => "you feel violent winds swirling nearby";

    public override void TakeTurn(Game game)
    {
        Random rand = new Random();
        if (rand.Next(2) == 0)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("the maelstrom hurls you to another room");
            game.TeleportPlayer();
        }
        else
        {
            game.MonsterAttacksPlayer(this);
        }
    }
}