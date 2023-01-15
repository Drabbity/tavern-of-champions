using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using TavernOfChampions.UI.Menu;

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

            PhotonNetwork.JoinRoom(_inputRoomName.text);
            _menuSwitcher.SwitchMenu(MenuType.ROOM_MENU);

        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            base.OnJoinRoomFailed(returnCode, message);
            print(message);
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            foreach(var room in roomList)
            {
                print(room.Name);
            }
        }
    }
}
