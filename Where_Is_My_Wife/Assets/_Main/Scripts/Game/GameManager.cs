using UnityEngine;

namespace WhereIsMyWife.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public GameState GameState { get; private set; } = GameState.Gameplay;
    }

    public enum GameState
    {
        Gameplay,
        UI,
        Pause,
    }
}