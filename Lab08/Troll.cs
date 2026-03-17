namespace Lab08;

public class Troll : Monster
{
    public Troll()
    {
        Name = "Troll";
        Health = 20;
        ArmorClass = 14;
        Weapon = new Weapon("Giant Club", 2, 8);
        Inventory.Add(Weapon);
    }

    public override string SenseMessage => "You smell something and hear heavy footsteps.";

    public override void TakeTurn(Game game)
    {
        game.MonsterAttacksPlayer(this);
        if (IsAlive) game.MonsterAttacksPlayer(this);
    }
}