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

        [SerializeField] private ChampionAction[] _actions;
        [SerializeField] private ChampionOutline _outline;
        [SerializeField] private int _maxHp;
        [SerializeField] private int _maxArmor;
        [SerializeField] private int _block;

        private GridManager _gridManager;
        private TurnManager _turnManager;
        private GameLogger _logger;
        private int _hp;
        private int _armor;
        
        public void Initialize(GridManager gridManager, TurnManager turnManager, Vector2Int location, Player owner)
        {
            _gridManager = gridManager;
            _turnManager = turnManager;
            CurrentPosition = location;
            _logger = GameLogger.Instance;
            Owner = owner;
            _hp = _maxHp;
            _armor = _maxArmor;
            
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

        public void TakeDamage(int damage)
        {
            _logger.Info($"Champion { gameObject } block absorbed { Mathf.Clamp(damage, 0, _block) } damage", LoggerType.CHAMPION, this);
            damage = Mathf.Clamp(damage - _block, 0, _MAX_AVAILABLE_DAMAGE);

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
