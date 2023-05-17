using Photon.Pun;
using System;

namespace TavernOfChampions.Turn
{
    public class TurnManager : MonoBehaviourPun
    {
        public event Action OnMoveEnd;
        public event Action OnMoveStart;
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
            photonView.RPC("PassTurn", RpcTarget.Others);
        }

        [PunRPC]
        private void PassTurn()
        {
            IsMyTurn = true;
            OnMoveStart.Invoke();
        }

        private void SetTurns()
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            if (UnityEngine.Random.Range(0, 2) == 0)
            {
                photonView.RPC("PassTurn", RpcTarget.Others);
            }
            else
            {
                IsMyTurn = true;
            }
        }
    }
}
