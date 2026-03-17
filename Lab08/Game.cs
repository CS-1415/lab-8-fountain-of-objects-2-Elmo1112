namespace Lab08;

public class Game
{
    private int _size;
    private Room[,] _map;
    private Player _player;
    private bool _fountainEnabled = false;
    private bool _gameOver = false;

    public bool FountainEnabled => _fountainEnabled;
    public int Size => _size;

    public Game(int size = 6)
    {
        _size = size;
        _player = new Player();
        _map = new Room[size, size];
        InitializeMap();
    }

    private void InitializeMap()
    {
        for (int r = 0; r < _size; r++)
            for (int c = 0; c < _size; c++)
                _map[r, c] = new Room();

        _map[0, 0].IsEntrance = true;
        _map[0, 2].IsFountainRoom = true;

        Random rand = new Random();

        for (int r = 0; r < _size; r++)
        {
            for (int c = 0; c < _size; c++)
            {
                if (_map[r, c].IsEntrance || _map[r, c].IsFountainRoom) continue;

                int roll = rand.Next(100);

                if (roll < 15)
                {
                    _map[r, c].HasPit = true;
                }
                else if (roll < 55)
                {
                    _map[r, c].Monster = MakeMonster(rand);
                }
            }
        }
    }

    private Monster MakeMonster(Random rand)
    {
        int roll = rand.Next(4);
        if (roll == 0) return new Goblin();
        if (roll == 1) return new Skeleton();
        if (roll == 2) return new Troll();
        return new Maelstrom();
    }

    public void Run()
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("=== The Fountain of Objects II ===");
        Console.WriteLine("Find the Fountain, activate it, and return to the entrance.\n");

        while (!_gameOver)
        {
            Console.WriteLine(new string('-', 50));
            DescribeLocation();
            SenseNearby();
            _player.PrintStatus();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\nWhat do you want to do? ");
            string command = Console.ReadLine().Trim().ToLower();

            HandleCommand(command);
        }
    }

    private void DescribeLocation()
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"\nYou are in room (Row={_player.Row}, Col={_player.Col}).");

        Room room = _map[_player.Row, _player.Col];

        if (room.IsEntrance)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("You see light coming from outside the cavern. This is the entrance.");
        }

        if (room.IsFountainRoom)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            if (_fountainEnabled)
                Console.WriteLine("You hear the rushing waters of the Fountain. It has been reactivated!");
            else
                Console.WriteLine("You hear water dripping. The Fountain of Objects is here!");
        }
    }

    private void SenseNearby()
    {
        int[] dr = { -1, 1, 0, 0 };
        int[] dc = { 0, 0, -1, 1 };

        for (int i = 0; i < 4; i++)
        {
            int r = _player.Row + dr[i];
            int c = _player.Col + dc[i];

            if (r < 0 || r >= _size || c < 0 || c >= _size) continue;

            Room room = _map[r, c];

            if (room.HasPit)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("You feel a cold draft. There is a pit nearby.");
            }

            if (room.Monster != null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(room.Monster.SenseMessage);
            }
        }
    }

    private void HandleCommand(string command)
    {
        if (command == "move north") Move(-1, 0);
        else if (command == "move south") Move(1, 0);
        else if (command == "move east") Move(0, 1);
        else if (command == "move west") Move(0, -1);
        else if (command == "enable fountain") EnableFountain();
        else if (command == "drink potion") _player.UsePotion();
        else if (command == "help") ShowHelp();
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Unknown command. Type 'help' for commands.");
        }
    }

    private void Move(int dr, int dc)
    {
        int newR = _player.Row + dr;
        int newC = _player.Col + dc;

        if (newR < 0 || newR >= _size || newC < 0 || newC >= _size)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You can't move that way.");
            return;
        }

        _player.Row = newR;
        _player.Col = newC;

        CheckRoom();
    }

    private void CheckRoom()
    {
        Room room = _map[_player.Row, _player.Col];

        if (room.HasPit)
        {
            Random rand = new Random();
            int damage = rand.Next(5, 11);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"You fall into a pit and take {damage} damage!");
            _player.TakeDamage(damage);
            room.HasPit = false;
        }

        if (room.Monster != null && room.Monster.IsAlive)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\nA {room.Monster.Name} blocks your path!");
            RunCombat(room.Monster);
            if (!room.Monster.IsAlive)
                room.Monster = null;
        }

        if (_player.Row == 0 && _player.Col == 0 && _fountainEnabled)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\nYou escaped the cavern with the Fountain reactivated!");
            Console.WriteLine("YOU WIN!");
            _gameOver = true;
        }

        if (!_player.IsAlive)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nYou have been defeated. Game over.");
            _gameOver = true;
        }
    }

    private void RunCombat(Monster monster)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\n=== COMBAT: {monster.Name} (HP: {monster.Health}, AC: {monster.ArmorClass}) ===");

        while (_player.IsAlive && monster.IsAlive)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\nYour HP: {_player.Health}/{_player.MaxHealth} | {monster.Name} HP: {monster.Health}");
            Console.WriteLine("Actions: [attack] [drink potion] [flee]");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("> ");
            string action = Console.ReadLine().Trim().ToLower();

            if (action == "attack")
            {
                PlayerAttacksMonster(monster);
            }
            else if (action == "drink potion")
            {
                _player.UsePotion();
            }
            else if (action == "flee")
            {
                Random rand = new Random();
                if (rand.Next(2) == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("You flee!");
                    return;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You failed to flee!");
                }
            }
            else
            {
                Console.WriteLine("Invalid action.");
                continue;
            }

            if (monster.IsAlive)
                monster.TakeTurn(this);
        }

        if (!monster.IsAlive)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nYou defeated the {monster.Name}!");
            foreach (Item item in monster.Inventory)
            {
                Console.WriteLine($"You picked up: {item.Name}");
                _player.PickUpItem(item);
            }
        }
    }

    public void PlayerAttacksMonster(Monster monster)
    {
        int roll = _player.RollAttack();
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($"You attack! (Roll: {roll} vs AC {monster.ArmorClass}) - ");

        if (roll >= monster.ArmorClass)
        {
            int dmg = _player.RollDamage();
            monster.TakeDamage(dmg);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"HIT for {dmg}! {monster.Name} has {monster.Health} HP left.");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("MISS!");
        }
    }

    public void MonsterAttacksPlayer(Monster monster)
    {
        int roll = monster.Attack();
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($"{monster.Name} attacks! (Roll: {roll} vs AC {_player.ArmorClass}) - ");

        if (roll >= _player.ArmorClass)
        {
            int dmg = monster.Weapon.RollDamage();
            _player.TakeDamage(dmg);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"HIT for {dmg}! You have {_player.Health} HP left.");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("MISS!");
        }
    }

    public void TeleportPlayer()
    {
        Random rand = new Random();
        _player.Row = rand.Next(_size);
        _player.Col = rand.Next(_size);
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"You land in room (Row={_player.Row}, Col={_player.Col})!");
        CheckRoom();
    }

    private void EnableFountain()
    {
        if (_map[_player.Row, _player.Col].IsFountainRoom)
        {
            _fountainEnabled = true;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("You activate the Fountain of Objects! Now return to the entrance!");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("There is no fountain here.");
        }
    }

    private void ShowHelp()
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Commands: move north/south/east/west | enable fountain | drink potion | help");
    }

    public Player GetPlayer() => _player;
    public Room GetRoom(int r, int c) => _map[r, c];
}
