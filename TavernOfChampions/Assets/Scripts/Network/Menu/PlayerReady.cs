using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TavernOfChampions.UI.Menu;

namespace TavernOfChampions.Network.Menu
{
    public class PlayerReady : MonoBehaviourPunCallbacks
    {
        [SerializeField] private ReadyButton _buttonImage;

        private bool _isReady = false;

        private List<Player> _readyPlayers = new List<Player>();

        public void OnClick_Ready()
        {
            _isReady = !_isReady;
            _buttonImage.SetButtonStatus(_isReady);

            base.photonView.RPC("SetReadyStatus", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer, _isReady);
        } 

        public override void OnJoinedRoom()
        {
            _isReady = false;
            _readyPlayers.Clear();
            _buttonImage.SetButtonStatus(false);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
            => _readyPlayers.Remove(otherPlayer);

        [PunRPC]
        private void SetReadyStatus(Player player, bool isReady)
        {
            if (isReady)
            {
                _readyPlayers.Add(player);
                if (IsEveryoneReady())
                    StartGame();
            }
            else
                _readyPlayers.Remove(player);
        }

        private void StartGame()
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel(1);
        }

        private bool IsEveryoneReady()
        {
            if (_readyPlayers.Count == 2)
                return true;
            return false;
        }
    }
}
