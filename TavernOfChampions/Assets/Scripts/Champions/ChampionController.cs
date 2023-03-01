using Photon.Pun;
using TavernOfChampions.Champion.Actions;
using TavernOfChampions.Champion.Actions.UI;
using TavernOfChampions.Grid;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TavernOfChampions.Champion
{
    public class ChampionController : MonoBehaviour
    {
        public ChampionAction CurrentAction
        {
            get { return _currentAction; }
            set
            {
                _currentAction = value;

                if (_currentAction != null)
                    _gridManager.GridVisualizer.HighlightTiles(_currentAction.GetLegalMoves());
                else
                    _gridManager.GridVisualizer.ClearHighlights();
            }
        }
        private ChampionAction _currentAction;

        public Vector2Int CurrentPosition { get; set; }

        [SerializeField] private ChampionAction[] _actions;

        private GridManager _gridManager;
        
        private void Start()
        {
            _gridManager = GridManager.Instance;
            InitializeActions();
        }

        public void Select()
        {
            ActionCardsList.Instance.ClearList();
            ActionCardsList.Instance.PopulateList(_actions, this);
        }

        public void DeSelect()
        {
            ActionCardsList.Instance.ClearList();
        }
        private void InitializeActions()
        {
            foreach (var action in _actions)
            {
                action.Initialize(_gridManager, this);
            }
        }
    }
}
