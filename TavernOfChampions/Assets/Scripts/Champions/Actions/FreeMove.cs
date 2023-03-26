using System.Collections.Generic;
using UnityEngine;
using TavernOfChampions.Grid;

namespace TavernOfChampions.Champion.Actions
{
    public class FreeMove : BasicMove
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
                for (int x = -1; x <= 1; x++)
                    for (int y = -1; y <= 1; y++)
                        if (x != 0 || y != 0)
                            _movableTiles.Add(_championController.CurrentPosition + new Vector2Int(x, y));
            }

            return LegalTileValidation.ValidateChampions(_movableTiles, _gridManager).ToArray();
        }
    }
}
