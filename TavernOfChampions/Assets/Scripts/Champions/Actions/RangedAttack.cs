using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TavernOfChampions.Grid;
using UnityEngine;

namespace TavernOfChampions.Champion.Actions
{
    public class RangedAttack : ChampionAction
    {
        [SerializeField] private int _attacksPerTurn = 1;
        [SerializeField] private List<int> _radiuses;
        [SerializeField] private int damage = 20;

        private int _attacks;

        private List<Vector2Int> _attackableTiles = new List<Vector2Int>();

        public override void Initialize(GridManager gridManager, ChampionController championController, TurnManager turnManager)
        {
            base.Initialize(gridManager, championController, turnManager);

            turnManager.OnMoveEnd += () => { _attacks = _attacksPerTurn; };
            _attacks = _attacksPerTurn;
        }

        [PunRPC]
        public override void Execute(Player player, Vector2Int tile)
        {
            _gridManager.GetChampion(tile).TakeDamage(damage);
            _attacks--;

            _championController.CurrentAction = this;
        }

        [PunRPC]
        public override Vector2Int[] GetLegalMoves()
        {
            _attackableTiles.Clear();

            if(_attacks > 0)
            {
                foreach (var radius in _radiuses)
                    _attackableTiles.AddRange(TileSelectorPresets.SelectRadius(radius, _championController.CurrentPosition));
            }

            return LegalTileValidation.ValidateChampions(_attackableTiles, _gridManager, _championController.Owner).ToArray();
        }
    }
}

