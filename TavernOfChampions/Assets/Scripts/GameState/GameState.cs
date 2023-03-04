using UnityEngine;
using UnityEngine.Events;

namespace TavernOfChampions.GameState
{
    public class GameState : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onStateActive;

        public void ExecuteOnStart()
        {
            _onStateActive.Invoke();
        }
    }
}
