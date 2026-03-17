namespace Lab08;

public abstract class Monster
{
    public string Name { get; set; } = "";
    public int Health { get; set; }
    public int ArmorClass { get; set; }
    public Weapon Weapon { get; set; } = new Weapon("fists", 1, 2);
    public List<Item> Inventory { get; set; } = new List<Item>();
    public bool IsAlive => Health > 0;

    public virtual int Attack()
    {
        Random rand = new Random();
        return rand.Next(1, 21);
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;
        if (Health < 0) Health = 0;
    }

    public abstract void TakeTurn(Game game);

    public abstract string SenseMessage { get; }
}