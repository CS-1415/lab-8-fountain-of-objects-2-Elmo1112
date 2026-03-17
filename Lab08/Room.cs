namespace Lab08;

public class Room
{
    public bool IsEntrance { get; set; } = false;
    public bool IsFountainRoom { get; set; } = false;
    public bool HasPit { get; set; } = false;
    public Monster? Monster { get; set; } = null;
}