using Photon.Pun;
using Photon.Realtime;
using TavernOfChampions.Champion.Actions;
using TavernOfChampions.Champion.Actions.UI;
using TavernOfChampions.Grid;
using TavernOfChampions.Logging;
using UnityEngine;

namespace TavernOfChampions.Champion
{
    public class ChampionController : MonoBehaviour
    {
        const int _MAX_AVAILABLE_DAMAGE = 1000;
        [field: SerializeField] public Sprite ChampionBanner { get; private set; }
        public ChampionAction CurrentAction
        {
            get { return _currentAction; }
            set
            {
                if(PhotonNetwork.LocalPlayer == Owner && (_turnManager.IsMyTurn || value == null))
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

        public bool HasMoved
        {
            get => _hasMoved;

            set
            {
                if (value)
                    _hasMoved = true;
            }
        }
        private bool _hasMoved = false;

        [SerializeField] private ChampionAction[] _actions;
        [SerializeField] private ChampionOutline _outline;
        [SerializeField] private int _maxHp;
        [SerializeField] private int _maxArmor;
        [SerializeField] private int _maxBlock;

        private GridManager _gridManager;
        private TurnManager _turnManager;
        private GameLogger _logger;
        private int _hp;
        private int _armor;
        private int _block;
        
        public void Initialize(GridManager gridManager, TurnManager turnManager, Vector2Int location, Player owner)
        {
            _gridManager = gridManager;
            _turnManager = turnManager;
            CurrentPosition = location;
            _logger = GameLogger.Instance;
            Owner = owner;
            _hp = _maxHp;
            _armor = _maxArmor;
            _block = _maxBlock;
            
            InitializeActions();
            _turnManager.OnMoveEnd += () => { CurrentAction = null; };
            _turnManager.OnMoveEnd += () => { _block = _maxBlock; };
            _turnManager.OnMoveEnd += () => { _hasMoved = false; };
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

        public void TakeDamage(int damage)
        {
            var damageAfterBlock = damage - _block;

            _logger.Info($"Champions block { gameObject } got damaged by { damage }", LoggerType.CHAMPION, this);
            _block = Mathf.Clamp(_block - damage, 0, _maxBlock);
            damage = Mathf.Clamp(damageAfterBlock, 0, damage);

            var damageAfterArmor = damage - _armor;

            _logger.Info($"Champions armor { gameObject } got damaged by { damage }", LoggerType.CHAMPION, this);
            _armor = Mathf.Clamp(_armor - damage, 0, _maxArmor);
            damage = Mathf.Clamp(damageAfterArmor, 0, damage);

            _logger.Info($"Champions  { gameObject } was damaged by { damage }", LoggerType.CHAMPION, this);
            _hp -= damage;

            if (_hp <= 0)
            {
                gameObject.SetActive(false);
                _logger.Info($"Champions { gameObject } died", LoggerType.CHAMPION, this);
            }
                
        }

        private void InitializeActions()
        {
            foreach (var action in _actions)
            {
                action.Initialize(_gridManager, this, _turnManager);
            }
        }
    }
}
