using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using TavernOfChampions.Grid;
using TavernOfChampions.Turn;
using UnityEngine;

namespace TavernOfChampions.Champion.Actions
{
    public class BasicAttack : ChampionAction
    {
        [SerializeField] private int _attacksPerTurn = 1;
        [SerializeField] private List<int> _radiuses;
        [SerializeField] private int _baseDamage = 20;
        [SerializeField] private string _rolldamageFormula = "";
        [SerializeField] private int _piercingDamage = 0;

        public int RangeMultiplier { get; set; } = 0;
        public int DamageMultiplier { get; set; } = 1;

        private int _attacks;

        public override void Initialize(GridManager gridManager, ChampionController championController, TurnManager turnManager)
        {
            base.Initialize(gridManager, championController, turnManager);

            turnManager.OnMoveEnd += () => { _attacks = _attacksPerTurn; };
            _attacks = _attacksPerTurn;
        }

        public override void Execute(Vector2Int tile)
        {
            photonView.RPC("Execute_RPC", RpcTarget.All, tile, DiceRoller.RollDice(_rolldamageFormula).Sum());
        }

        [PunRPC]
        protected void Execute_RPC(Vector2Int tile, int diceRoll)
        {
            base.Execute_RPC(tile);
            _championController.CurrentAction = this;

            _attacks--;
            var damage = _baseDamage + diceRoll;
            _gridManager.GetChampion(tile).TakeDamage(damage * DamageMultiplier, _piercingDamage);
        }

        public override Vector2Int[] GetLegalMoves()
        {
            var attackableTiles = new List<Vector2Int>();

            if(CanAttack())
            {
                foreach (var radius in _radiuses)
                    attackableTiles.AddRange(TileSelectorPresets.SelectRadius(radius, _championController.CurrentPosition));

                for(int radius = _radiuses.Max(); radius <= _radiuses.Max() + RangeMultiplier; radius++)
                {
                    attackableTiles.AddRange(TileSelectorPresets.SelectRadius(radius, _championController.CurrentPosition));
                }
            }

            return LegalTileValidation.ValidateChampions(attackableTiles, _gridManager, _championController.Owner).ToArray();
        }

        private bool CanAttack()
        {
            return _attacks > 0 &&
                (_championController.CanAttackInMove || !_championController.UsedAction || _championController.UsedAction == this);
        }
    }
}

