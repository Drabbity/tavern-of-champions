using Photon.Pun;
using System;
using UnityEngine;

namespace TavernOfChampions.Grid
{
    public class TurnManager : MonoBehaviourPun
    {
        public event Action OnMoveEnd;
        public bool IsMyTurn { get; private set; }

        private void Start()
        {
            IsMyTurn = false;
            SetTurns();
        }

        public void EndTurn()
        {
            if (!IsMyTurn)
                return;

            OnMoveEnd.Invoke();
            IsMyTurn = false;
            base.photonView.RPC("PassTurn", RpcTarget.Others);
        }

        [PunRPC]
        private void PassTurn()
        {
            IsMyTurn = true;
        }

        private void SetTurns()
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            if(UnityEngine.Random.Range(0, 2) == 0)
            {
                base.photonView.RPC("PassTurn", RpcTarget.Others);
            }
            else
            {
                IsMyTurn = true;
            }
        }
    }
}
