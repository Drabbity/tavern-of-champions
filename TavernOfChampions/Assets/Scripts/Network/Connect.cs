using Photon.Pun;
using Photon.Realtime;
using TavernOfChampions.Data;
using UnityEngine;
using TavernOfChampions.Logging;

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
            GameLogger.Instance.Info($"Connected to Server as { PhotonNetwork.LocalPlayer.NickName }", LoggerType.NETWORK, this);

            if (!PhotonNetwork.InLobby)
                PhotonNetwork.JoinLobby();

            _loadingPanel.SetActive(false);
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            GameLogger.Instance.Info($"Disconnected from Server. Cause: {cause.ToString()}", LoggerType.NETWORK, this);
        }
    }
}
