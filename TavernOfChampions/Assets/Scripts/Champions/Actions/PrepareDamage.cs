using Photon.Pun;
using System.Collections.Generic;
using TavernOfChampions.Champion.Actions.UI;
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

        private ActionStatus _actionStatus = null;

        public override void Initialize(GridManager gridManager, ChampionController championController, TurnManager turnManager)
        {
            base.Initialize(gridManager, championController, turnManager);
            _allowedActions.Add(this);

            _championController.OnActionUsed += CheckPreparation;
        }

        [PunRPC]
        protected override void Execute_RPC(Vector2Int tile)
        {
            if (!CanUseAction())
                return;

            _championController.UsedAction = this;
            _championController.CurrentAction = null;

            _basicAttack.DamageMultiplier++;

            if (!_actionStatus)
                _actionStatus = _championController.ActionStatusList.CreateActionStatus(_actionCardSprite, _basicAttack.DamageMultiplier - 1);
            else
                _actionStatus.SetCount(_basicAttack.DamageMultiplier - 1);
        }

        public override Vector2Int[] GetLegalMoves()
        {
            if (CanUseAction())
            {
                Execute(Vector2Int.zero);
                return new Vector2Int[0];
            }               
            else
                return new Vector2Int[0];
        }

        private void CheckPreparation()
        {
            if (_championController.UsedAction && !_allowedActions.Contains(_championController.UsedAction))
            {
                _basicAttack.DamageMultiplier = 1;
                if (_actionStatus)
                {
                    _championController.ActionStatusList.DestroyActionStatus(_actionStatus);
                    _actionStatus = null;
                }
            }
        }
        private bool CanUseAction()
            => !_championController.UsedAction && _basicAttack.DamageMultiplier < _maxPreparations + 1;
    }
}