using Photon.Pun;
using Photon.Realtime;
using TavernOfChampions.Data;
using UnityEngine;

namespace TavernOfChampions.Network
{
    public class Connect : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject _loadingPanel;

        void Start()
        {
            _loadingPanel.SetActive(true);
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.GameVersion = PlayerData.GameVersion;
            PhotonNetwork.NickName = PlayerData.Username;
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            print($"Connected to Server as { PhotonNetwork.LocalPlayer.NickName }");

            if (!PhotonNetwork.InLobby)
                PhotonNetwork.JoinLobby();

            _loadingPanel.SetActive(false);
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            print($"Disconnected from Server. Cause: {cause.ToString()}");
        }
    }
}
