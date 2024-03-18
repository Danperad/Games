// See https://aka.ms/new-console-template for more information

namespace PingPong.FNA;

public static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        using var game = new PingPongGame();
        game.Run();
    }
}