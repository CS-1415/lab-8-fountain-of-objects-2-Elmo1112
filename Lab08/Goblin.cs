namespace Lab08;

public class Goblin : Monster
{
    public Goblin()
    {
        Name = "Goblin";
        Health = 8;
        ArmorClass = 11;
        Weapon = new Weapon("rusty dagger", 1, 4);
        Inventory.Add(Weapon);
    }

    public override string SenseMessage => "you hear noise in the darkness nearby";

    public override void TakeTurn(Game game)
    {
        game.MonsterAttacksPlayer(this);
    }
}