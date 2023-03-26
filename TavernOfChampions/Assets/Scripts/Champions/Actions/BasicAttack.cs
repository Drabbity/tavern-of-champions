using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using TavernOfChampions.Grid;
using UnityEngine;

namespace TavernOfChampions.Champion.Actions
{
    public class BasicAttack : ChampionAction
    {
        [SerializeField] private int _attacksPerTurn = 1;
        [SerializeField] private List<int> _radiuses;
        [SerializeField] private int _baseDamage = 20;
        [SerializeField] private string _rolldamageFormula = "";

        private int _attacks;

        private readonly List<Vector2Int> _attackableTiles = new List<Vector2Int>();

        public override void Initialize(GridManager gridManager, ChampionController championController, TurnManager turnManager)
        {
            base.Initialize(gridManager, championController, turnManager);

            turnManager.OnMoveEnd += () => { _attacks = _attacksPerTurn; };
            _attacks = _attacksPerTurn;
        }

        public override void Execute(Vector2Int tile)
        {
            var damage = _baseDamage + DiceRoller.RollDice(_rolldamageFormula).Sum();
            _attacks--;

            photonView.RPC("Execute_RPC", RpcTarget.All, tile, damage);

            _championController.CurrentAction = this;
        }

        [PunRPC]
        private void Execute_RPC(Vector2Int tile, int damage)
        {
            _gridManager.GetChampion(tile).TakeDamage(damage);
        }

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

