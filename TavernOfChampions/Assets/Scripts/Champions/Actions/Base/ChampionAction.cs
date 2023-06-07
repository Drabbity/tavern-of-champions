using Photon.Pun;
using System.Linq;
using TavernOfChampions.Grid;
using TavernOfChampions.Turn;
using UnityEngine;

namespace TavernOfChampions.Champion.Actions
{
    [RequireComponent(typeof(PhotonView))]
    public abstract class ChampionAction : MonoBehaviourPun
    {
        public Sprite ActionCardSymbolSprite { get => _actionCardSprite; }
        [SerializeField] protected Sprite _actionCardSprite;

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
            photonView.RPC("Execute_RPC", RpcTarget.All, tile);
        }

        [PunRPC]
        protected virtual void Execute_RPC(Vector2Int tile)
        {
            if (!GetLegalMoves().ToList().Contains(tile))
                return;

            _championController.UsedAction = this;
            _championController.CurrentAction = null;
        }

        public abstract Vector2Int[] GetLegalMoves();
    }
}

