using System.Collections.Generic;
using UnityEngine;
using TavernOfChampions.Grid;

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
            var _movableTiles = new List<Vector2Int>();

            if (_moves > 0)
            {
                var currentPosition = _championController.CurrentPosition;

                _movableTiles.Add(currentPosition + Vector2Int.up);
                _movableTiles.Add(currentPosition + Vector2Int.down);
                _movableTiles.Add(currentPosition + Vector2Int.left);
                _movableTiles.Add(currentPosition + Vector2Int.right);
            }

            return LegalTileValidation.ValidateChampions(_movableTiles, _gridManager).ToArray();
        }
    }
}
