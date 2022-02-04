using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class PhotonRoomList : MonoBehaviour, ILobbyCallbacks
{
    [SerializeField] private GameObject _roomBtnPrefab;
    [SerializeField] private GameObject _roomsInfoPanel;

    private Dictionary<string, RoomInfo> _roomList = new Dictionary<string, RoomInfo>();
    private List<GameObject> _roomButtons = new List<GameObject>();

    public void OnJoinedLobby()
    {
        _roomList.Clear();
        ClearRoomsInfo();
    }

    public void OnLeftLobby()
    {
        _roomList.Clear();
        ClearRoomsInfo();
    }

    public void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateRoomsList(roomList);
        AddRoomListInfo();
    }

    public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
        Debug.Log("Lobby statistics updated");
    }

    private void UpdateRoomsList(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            RoomInfo info = roomList[i];

            if (info.RemovedFromList)
                _roomList.Remove(info.Name);

            else
                _roomList[info.Name] = info;
        }
    }

    private void ClearRoomsInfo()
    {
        _roomButtons.Clear();
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
        }
    }

    private void ConnectToRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }
}