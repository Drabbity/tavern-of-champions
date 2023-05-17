using UnityEngine;
using UnityEngine.UI;

namespace TavernOfChampions.Turn.UI
{
    public class TurnDisplayer : MonoBehaviour
    {
        [SerializeField] private TurnManager _turnManager;
        [SerializeField] private Image _turnBackground;
        [SerializeField] private Color _yourTurnColor;
        [SerializeField] private Color _enemyTurnColor;

        public void Awake()
        {
            _turnManager.OnMoveStart += () => { _turnBackground.color = _yourTurnColor; };
            _turnManager.OnMoveEnd += () => { _turnBackground.color = _enemyTurnColor; };           
        }
    }
}