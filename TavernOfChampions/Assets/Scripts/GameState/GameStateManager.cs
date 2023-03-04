using UnityEngine;

namespace TavernOfChampions.GameState
{
    public class GameStateManager : MonoBehaviour
    {
        [SerializeField] private GameState[] _gameStates;

        private int _currentGameState = 0;

        public void Start()
        {
            SwitchState();
        }

        public void SwitchState()
        {
            _gameStates[_currentGameState++].ExecuteOnStart();
        }
    }
}
