using System.Collections.Generic;
using UnityEngine;
using TavernOfChampions.Grid;
using TavernOfChampions.Turn;

namespace TavernOfChampions.Champion.Actions
{
    public class BasicMove : ChampionMoveAction
    {
        public override void Initialize(GridManager gridManager, ChampionController championController, TurnManager turnManager)
        {
            base.Initialize(gridManager, championController, turnManager);

            turnManager.OnMoveEnd += () => { _moves = _movesPerTurn; };
            _moves = _movesPerTurn;
        }

        public override Vector2Int[] GetLegalMoves()
        {
            var movableTiles = new List<Vector2Int>();

            if (_moves > 0 && (_championController.CanAttackInMove || !_championController.UsedAction || _championController.UsedAction == this))
            {
                var currentPosition = _championController.CurrentPosition;

                movableTiles.Add(currentPosition + Vector2Int.up);
                movableTiles.Add(currentPosition + Vector2Int.down);
                movableTiles.Add(currentPosition + Vector2Int.left);
                movableTiles.Add(currentPosition + Vector2Int.right);
            }

            return LegalTileValidation.ValidateChampions(movableTiles, _gridManager).ToArray();
        }
    }
}
