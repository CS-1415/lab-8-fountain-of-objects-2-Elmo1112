namespace Lab08.Tests;

using NUnit.Framework;

[TestFixture]
public class WeaponTests
{
    [Test]
    public void RollDamage_IsWithinRange()
    {
        Weapon weapon = new Weapon("Test Sword", 2, 6);
        for (int i = 0; i < 100; i++)
        {
            int dmg = weapon.RollDamage();
            Assert.That(dmg, Is.InRange(2, 6));
        }
    }
}

[TestFixture]
public class PlayerTests
{
    [Test]
    public void Player_StartsAlive()
    {
        Player player = new Player();
        Assert.That(player.IsAlive, Is.True);
    }

    [Test]
    public void Player_TakeDamage_ReducesHealth()
    {
        Player player = new Player();
        int startingHp = player.Health;
        player.TakeDamage(5);
        Assert.That(player.Health, Is.EqualTo(startingHp - 5));
    }

    [Test]
    public void Player_HealthNeverGoesBelowZero()
    {
        Player player = new Player();
        player.TakeDamage(9999);
        Assert.That(player.Health, Is.EqualTo(0));
        Assert.That(player.IsAlive, Is.False);
    }

    [Test]
    public void Player_PicksUpBetterWeapon()
    {
        Player player = new Player();
        Weapon betterWeapon = new Weapon("Great Sword", 3, 12);
        player.PickUpItem(betterWeapon);
        Assert.That(player.Weapon.Name, Is.EqualTo("Great Sword"));
    }

    [Test]
    public void Player_DoesNotDowngradeWeapon()
    {
        Player player = new Player();
        string originalWeapon = player.Weapon.Name;
        Weapon weakWeapon = new Weapon("Stick", 1, 2);
        player.PickUpItem(weakWeapon);
        Assert.That(player.Weapon.Name, Is.EqualTo(originalWeapon));
    }
}

[TestFixture]
[TestFixture]
public class MonsterTests
{
    [Test]
    public void Goblin_StartsWithCorrectStats()
    {
        Goblin goblin = new Goblin();
        Assert.That(goblin.Health, Is.EqualTo(8));
        Assert.That(goblin.ArmorClass, Is.EqualTo(11));
        Assert.That(goblin.IsAlive, Is.True);
    }

    [Test]
    public void Troll_HasMoreHealthThanGoblin()
    {
        Troll troll = new Troll();
        Goblin goblin = new Goblin();
        Assert.That(troll.Health, Is.GreaterThan(goblin.Health));
    }

    [Test]
    public void Monster_HasItemsInInventory()
    {
        Goblin goblin = new Goblin();
        Assert.That(goblin.Inventory.Count, Is.GreaterThan(0));
    }
}