namespace PingPong.Foster;

public interface IStatable : IUpdatable
{
    void Startup();
    void UpdateGameState(ref GameState state);
}