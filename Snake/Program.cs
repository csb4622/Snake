namespace Snake;

public static class Program
{
    public static void Main(params string[] args)
    {
        using var game = new SnakeGame();
        game.Run();       
    }
}