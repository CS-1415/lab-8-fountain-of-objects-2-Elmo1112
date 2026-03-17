using Lab08;

Console.WriteLine("choose world size small, medium or large:");
string input = Console.ReadLine().Trim().ToLower();

int size = 6;
if (input == "small") size = 4;
else if (input == "large") size = 8;

Game game = new Game(size);
game.Run();
