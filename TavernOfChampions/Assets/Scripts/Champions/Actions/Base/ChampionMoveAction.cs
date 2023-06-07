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

        [PunRPC]
        protected override void Execute_RPC(Vector2Int tile)
        {
            base.Execute_RPC(tile);

            _moves--;
            _gridManager.MoveChampion(_championController.CurrentPosition, tile);
        }

        protected bool CanMove()
        {
            return _moves > 0 &&
                (_championController.CanAttackInMove || !_championController.UsedAction || _championController.UsedAction == this);
        }
    }
}
