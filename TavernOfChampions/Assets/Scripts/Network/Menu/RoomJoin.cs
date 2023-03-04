using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using TavernOfChampions.UI.Menu;
using TavernOfChampions.Logging;

namespace TavernOfChampions.Network.Menu
{
    public class RoomJoin : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMP_InputField _inputRoomName;
        [SerializeField] private MenuSwitcher _menuSwitcher;

        public void OnClick_JoinRoom()
        {

            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2;

            GameLogger.Instance.Info($"Joining Room with Name: { _inputRoomName.text }", LoggerType.NETWORK, this);

            PhotonNetwork.JoinRoom(_inputRoomName.text);
            _menuSwitcher.SwitchMenu(MenuType.ROOM_MENU);

        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            base.OnJoinRoomFailed(returnCode, message);

            GameLogger.Instance.Warning($"Couldn't join room: { message }", LoggerType.NETWORK, this);
        }
    }
}
