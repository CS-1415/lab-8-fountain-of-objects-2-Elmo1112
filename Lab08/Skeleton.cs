namespace Lab08;

public class Skeleton : Monster
{
    public Skeleton()
    {
        Name = "Skeleton";
        Health = 12;
        ArmorClass = 13;
        Weapon = new Weapon("bone club", 1, 6);
        Inventory.Add(Weapon);
    }

    public override string SenseMessage => "You hear ratling of bones nearby";

    public override void TakeTurn(Game game)
    {
        game.MonsterAttacksPlayer(this);
    }
}