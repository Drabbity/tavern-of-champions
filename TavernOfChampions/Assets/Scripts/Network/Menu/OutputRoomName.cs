using Photon.Pun;
using TMPro;
using UnityEngine;

namespace TavernOfChampions.Network.Menu
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class OutputRoomName : MonoBehaviourPunCallbacks
    {
        [SerializeField] private string _preRoomCodeText = "";

        private TextMeshProUGUI _tmpro;

        public void Start()
        {
            if(PhotonNetwork.InRoom)
                OnJoinedRoom();
        }

        public override void OnJoinedRoom()
        {
            if(!_tmpro) _tmpro = GetComponent<TextMeshProUGUI>();

            _tmpro.text = _preRoomCodeText + " " + PhotonNetwork.CurrentRoom.Name;
        }
    }
}
