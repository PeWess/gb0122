using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class PhotonLoginManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _roomsListPanel;
    [SerializeField] private GameObject _roomPanel;
    [SerializeField] private GameObject _playerNamePrefab;
    [SerializeField] private GameObject _roomBtnPrefab;
    [SerializeField] private GameObject _roomsInfoPanel;

    private Dictionary<string, RoomInfo> _roomList = new Dictionary<string, RoomInfo>();

    private string _roomName = "";
    private TypedLobby _customLobby = new TypedLobby("customLobby", LobbyType.Default);

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        Connect();
        AddRoomListInfo();
    }
    
    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinRandomRoom();
    }

    public void UpdateRoomName(string roomName)
    {
        _roomName = roomName;
    }

    public void OnCreateOrFindingRoom()
    {
        if (_roomName == "")
            PhotonNetwork.JoinRandomRoom();
        PhotonNetwork.JoinOrCreateRoom(_roomName, new RoomOptions(), _customLobby);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"Room creation failed: {message}");
    }
    
    public override void OnCreatedRoom()
    {
        _roomList.Add(_roomName, PhotonNetwork.CurrentRoom);
    }

    public override void OnJoinedRoom()
    {
        var yCoord = 500;
        _roomsListPanel.SetActive(false);
        _roomPanel.SetActive(true);
        _roomPanel.GetComponentInChildren<Text>().text = $"{_roomName}";
        foreach (var p in PhotonNetwork.PlayerList)
        {
            var newPlayer = Instantiate(_playerNamePrefab, _roomPanel.transform);
            newPlayer.transform.localScale = new Vector3(1, 1, 1);
            newPlayer.transform.localPosition = new Vector3(0, yCoord, 0);
            newPlayer.gameObject.GetComponentInChildren<Text>().text = p.UserId;
            yCoord -= 65;
            newPlayer.SetActive(true);
        }
    }
    
    
    private void AddRoomListInfo()
    {
        var yCoord = 500;
        foreach (var room in _roomList)
        {
            var newRoom = Instantiate(_roomBtnPrefab, _roomsInfoPanel.transform);
            newRoom.transform.localScale = new Vector3(1, 1, 1);
            newRoom.transform.localPosition = new Vector3(0, yCoord, 0);
            newRoom.gameObject.GetComponentInChildren<Text>().text = room.Value.Name;
            yCoord += 65; 
            
            newRoom.gameObject.GetComponentInChildren<Button>().onClick.AddListener(delegate { ConnectToRoom(room.Value.Name); });
            newRoom.SetActive(true);
        }
    }
    
    private void ConnectToRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }
}
