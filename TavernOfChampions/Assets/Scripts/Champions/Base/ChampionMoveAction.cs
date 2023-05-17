using Photon.Pun;
using TavernOfChampions.Grid;
using TavernOfChampions.Turn;
using UnityEngine;

namespace TavernOfChampions.Champion.Actions
{
    public abstract class ChampionMoveAction : ChampionAction
    {
        [SerializeField] protected int _movesPerTurn = 5;

        protected int _moves;

        public override void Initialize(GridManager gridManager, ChampionController championController, TurnManager turnManager)
        {
            base.Initialize(gridManager, championController, turnManager);

            turnManager.OnMoveEnd += () => { _moves = _movesPerTurn; };
            _moves = _movesPerTurn;
        }

        public override void Execute(Vector2Int tile)
        {
            _moves--;

            photonView.RPC("Execute_RPC", RpcTarget.All, tile);
            base.Execute(tile);
        }

        [PunRPC]
        protected virtual void Execute_RPC(Vector2Int tile)
        {
            _gridManager.MoveChampion(_championController.CurrentPosition, tile);
        }
    }
}
