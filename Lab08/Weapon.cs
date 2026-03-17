namespace Lab08;

public class Weapon : Item
{
    public int MinDamage { get; set; }
    public int MaxDamage { get; set; }

    public Weapon(string name, int min, int max) : base(name)
    {
        MinDamage = min;
        MaxDamage = max;
    }

    public int RollDamage()
    {
        Random rand = new Random();
        return rand.Next(MinDamage, MaxDamage + 1);
    }
}