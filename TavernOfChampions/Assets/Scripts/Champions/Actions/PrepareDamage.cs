using System.Collections.Generic;
using TavernOfChampions.Grid;
using TavernOfChampions.Turn;
using UnityEngine;

namespace TavernOfChampions.Champion.Actions
{
    public class PrepareDamage : ChampionAction
    {
        [SerializeField] private List<ChampionAction> _allowedActions = new List<ChampionAction>();
        [SerializeField] private BasicAttack _basicAttack;
        [SerializeField] private int _maxPreparations;

        public override void Initialize(GridManager gridManager, ChampionController championController, TurnManager turnManager)
        {
            base.Initialize(gridManager, championController, turnManager);
            _allowedActions.Add(this);
            turnManager.OnMoveEnd += () =>
            {
                if (_championController.UsedAction && !_allowedActions.Contains(_championController.UsedAction))
                {
                    _basicAttack.DamageMultiplier = 1;
                }
            };
        }

        public override void Execute(Vector2Int tile)
        {
            base.Execute(tile);
            _basicAttack.DamageMultiplier++;
        }

        public override Vector2Int[] GetLegalMoves()
        {
            if (!_championController.UsedAction && _basicAttack.DamageMultiplier < _maxPreparations + 1)
                return new Vector2Int[] { _championController.CurrentPosition };
            else
                return new Vector2Int[0];
        }
    }
}