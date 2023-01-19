using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using TavernOfChampions.UI.Menu;
using TavernOfChampions.Logging;

namespace TavernOfChampions.Network.Menu
{
    class RoomCreate : MonoBehaviour
    {
        [SerializeField] private MenuSwitcher _menuSwitcher;

        private const int _MAX_PLAYERS = 5;

        public void OnClick_CreateRoom()
        {
            var roomName = CreateRandomRoomName();
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = _MAX_PLAYERS;

            PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
            GameLogger.Instance.Info($"Room created with Name: {roomName}", LoggerType.NETWORK, this);

            _menuSwitcher.SwitchMenu(MenuType.ROOM_MENU);
        }

        private string CreateRandomRoomName()
        {
            return Random.Range(0, 100000).ToString();
        }
    }
}
