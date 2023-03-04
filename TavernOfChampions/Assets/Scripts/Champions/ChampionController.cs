using Photon.Pun;
using Photon.Realtime;
using TavernOfChampions.Champion.Actions;
using TavernOfChampions.Champion.Actions.UI;
using TavernOfChampions.Grid;
using UnityEngine;

namespace TavernOfChampions.Champion
{
    public class ChampionController : MonoBehaviour
    {
        [field: SerializeField] public Sprite ChampionBanner { get; private set; }
        public ChampionAction CurrentAction
        {
            get { return _currentAction; }
            set
            {
                if(_turnManager.IsMyTurn || value == null)
                {
                    _currentAction = value;

                    if (_currentAction != null)
                        _gridManager.GridVisualizer.HighlightTiles(_currentAction.GetLegalMoves());
                    else
                        _gridManager.GridVisualizer.ClearHighlights();
                }
            }
        }
        private ChampionAction _currentAction;

        public Player Owner { get; private set; }

        public Vector2Int CurrentPosition { get; set; }

        [SerializeField] private ChampionAction[] _actions;
        [SerializeField] private ChampionOutline _outline;

        private GridManager _gridManager;
        private TurnManager _turnManager;
        
        public void Initialize(GridManager gridManager, TurnManager turnManager, Vector2Int location, Player owner)
        {
            _gridManager = gridManager;
            _turnManager = turnManager;
            CurrentPosition = location;
            Owner = owner;
            
            InitializeActions();
            _turnManager.OnMoveEnd += () => { _currentAction = null; };
            _outline.ChangeOutlineColor(owner != PhotonNetwork.LocalPlayer);
        }

        public void Select()
        {
            ActionCardsList.Instance.ClearList();
            if(Owner == PhotonNetwork.LocalPlayer)
            {
                ActionCardsList.Instance.PopulateList(_actions, this);
            }
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
