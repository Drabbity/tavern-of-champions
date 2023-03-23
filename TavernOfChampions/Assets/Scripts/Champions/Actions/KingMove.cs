using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TavernOfChampions.Grid;

namespace TavernOfChampions.Champion.Actions
{
    public class KingMove : ChampionAction
    {
        [SerializeField] private int _movesPerTurn = 5;

        private int _moves;

        private List<Vector2Int> _movableTiles = new List<Vector2Int>();

        public override void Initialize(GridManager gridManager, ChampionController championController, TurnManager turnManager)
        {
            base.Initialize(gridManager, championController, turnManager);

            turnManager.OnMoveEnd += () => { _moves = _movesPerTurn; };
            _moves = _movesPerTurn;
        }

        [PunRPC]
        public override void Execute(Player player, Vector2Int tile)
        {
            _gridManager.MoveChampion(_championController.CurrentPosition, tile);
            _moves--;

            _championController.CurrentAction = this;
        }

        [PunRPC]
        public override Vector2Int[] GetLegalMoves()
        {
            _movableTiles.Clear();

            if(_moves > 0)
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
