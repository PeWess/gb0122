using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class PhotonLoginManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _roomsListPanel;
    [SerializeField] private GameObject _roomPanel;
    [SerializeField] private GameObject _playerNamePrefab;
    
    private string _roomName;
    private TypedLobby _customLobby = new TypedLobby("customLobby", LobbyType.Default);

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        if (!PhotonNetwork.IsConnected)
            PhotonNetwork.ConnectUsingSettings();
    }

    public void UpdateRoomName(string roomName)
    {
        _roomName = roomName;
    }

    public void OnCreateOrFindingRoom()
    {
        PhotonNetwork.JoinOrCreateRoom(_roomName, new RoomOptions(), _customLobby);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"Room creation failed: {message}");
    }

    public override void OnJoinedRoom()
    {
        _roomsListPanel.SetActive(false);
        _roomPanel.SetActive(true);
        _roomPanel.GetComponentInChildren<Text>().text = $"{_roomName}";
        foreach (var p in PhotonNetwork.PlayerList)
        {
            var newPlayer = Instantiate(_playerNamePrefab, _roomPanel.transform);
            newPlayer.transform.localScale = new Vector3(1, 1, 1);
            newPlayer.transform.localPosition = new Vector3(0, 500, 0);
            newPlayer.gameObject.GetComponentInChildren<Text>().text = p.UserId;
        }
    }
}
