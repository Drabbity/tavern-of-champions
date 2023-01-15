using Photon.Pun;
using UnityEngine;

public class StartGame_TMP : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _startGameButton;

    public void Awake()
        => OnJoinedRoom();

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
            _startGameButton.SetActive(true);
        else
            _startGameButton.SetActive(false);
    }

    public void StartGame()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel(1);
        }
    }
}
