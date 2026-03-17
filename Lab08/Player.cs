namespace Lab08;

public class Player
{
    public int Row { get; set; } = 0;
    public int Col { get; set; } = 0;
    public int Health { get; set; } = 30;
    public int MaxHealth { get; set; } = 30;
    public int ArmorClass { get; set; } = 12;
    public Weapon Weapon { get; set; }
    public List<Item> Inventory { get; set; } = new List<Item>();
    public bool IsAlive => Health > 0;

    public Player()
    {
        Weapon = new Weapon("Short Sword", 1, 6);
        Inventory.Add(Weapon);
        Inventory.Add(new Item("Health Potion"));
        Inventory.Add(new Item("Health Potion"));
    }

    public int RollAttack()
    {
        Random rand = new Random();
        return rand.Next(1, 21);
    }

    public int RollDamage()
    {
        return Weapon.RollDamage();
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;
        if (Health < 0) Health = 0;
    }

    public void PickUpItem(Item item)
    {
        Inventory.Add(item);
        if (item is Weapon w && w.MaxDamage > Weapon.MaxDamage)
        {
            Weapon = w;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"You have {w.Name} (DMG: {w.MinDamage}-{w.MaxDamage})");
        }
    }

    public void UsePotion()
    {
        Item potion = null;
        foreach (Item i in Inventory)
        {
            if (i.Name == "health pottion")
            {
                potion = i;
                break;
            }
        }

        if (potion == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You have no potions");
            return;
        }

        Inventory.Remove(potion);
        Random rand = new Random();
        int heal = rand.Next(6, 11);
        Health += heal;
        if (Health > MaxHealth) Health = MaxHealth;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("You drink a potion and receover {heal} HP. ({Health}/{MaxHealth})");
    }

    public void PrintStatus()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("HP: {Health}/{MaxHealth} | AC: {ArmorClass} | Weapon: {Weapon.Name} ({Weapon.MinDamage}-{Weapon.MaxDamage})");
        int potions = 0;
        foreach (Item i in Inventory)
            if (i.Name == "Health Potion") potions++;
        Console.WriteLine("Potions: {potions} | Items: {Inventory.Count}");
    }
}