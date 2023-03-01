using Photon.Pun;
using Photon.Realtime;
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

        public void Initialize(GridManager gridManager, ChampionController championController)
        {
            _gridManager = gridManager;
            _championController = championController;
            GetComponent<PhotonView>().ViewID = _gridManager.ChampionViewId;
        }

        [PunRPC]
        public abstract Vector2Int[] GetLegalMoves();

        public abstract void Execute(Player player, Vector2Int tile);
    }
}

