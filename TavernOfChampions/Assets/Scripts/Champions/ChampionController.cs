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
        [SerializeField] private Transform _hpBar;

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

        public void TakeDamage(int damage, int piercingDamage)
        {
            damage = BlockDamage(damage);
            damage = ArmorDamage(damage);
            HPDamage(damage, piercingDamage);

            var hpBarScale = _hpBar.transform.localScale;
            hpBarScale.x = (float)_hp / _maxHp;
            _hpBar.transform.localScale = hpBarScale;

            if(_hp <= 0)
            {
                _logger.Info($"Champion { gameObject } was killed!", LoggerType.CHAMPION, this);
                gameObject.SetActive(false);
            }
        }

        private int BlockDamage(int damage)
        {
            var damageAbsorbed = Mathf.Clamp(damage, 0, _block);
            var damageLeft = Mathf.Clamp(damage - _block, 0, damage);

            _block = Mathf.Clamp(_block - damage, 0, _maxBlock);
            _logger.Info($"Champions block { gameObject } absorbed { damageAbsorbed } damage. Block left: { _block }", LoggerType.CHAMPION, this);
            
            return damageLeft;
        }

        private int ArmorDamage(int damage)
        {
            var damageAbsorbed = Mathf.Clamp(damage, 0, _armor);
            var damageLeft = Mathf.Clamp(damage - _armor, 0, damage);

            _armor = Mathf.Clamp(_armor - damage, 0, _maxArmor);
            _logger.Info($"Champions armor { gameObject } absorbed { damageAbsorbed } damage. Armor left: { _armor }", LoggerType.CHAMPION, this);

            return damageLeft;
        }

        private void HPDamage(int damage, int piercingDamage)
        {
            var damageAbsorbed = Mathf.Clamp(damage, 0, _hp);

            damage = damage < piercingDamage ? piercingDamage : damage;

            _hp = Mathf.Clamp(_hp - damage, 0, _maxHp);
            _logger.Info($"Champions health { gameObject } damaged by { damageAbsorbed }. Health left: { _hp }", LoggerType.CHAMPION, this);
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
