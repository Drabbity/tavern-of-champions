using Photon.Pun;
using TavernOfChampions.Grid;
using UnityEngine;

namespace TavernOfChampions.Champion.Actions
{
    [RequireComponent(typeof(PhotonView))]
    public abstract class ChampionAction : MonoBehaviourPun
    {
        public Sprite ActionCardSprite { get => _actionCardSprite; }
        [SerializeField] private Sprite _actionCardSprite;

        protected GridManager _gridManager;
        protected ChampionController _championController;

        public virtual void Initialize(GridManager gridManager, ChampionController championController, TurnManager turnManager)
        {
            _gridManager = gridManager;
            _championController = championController;
            GetComponent<PhotonView>().ViewID = _gridManager.ChampionViewId;
        }

        public virtual void Execute(Vector2Int tile)
        {
            _championController.CurrentAction = this;
            _championController.UsedAction = this;
        }

        public abstract Vector2Int[] GetLegalMoves();
    }
}

