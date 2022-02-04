using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class RoomSettings : MonoBehaviour
{
    [SerializeField] private Toggle _isForFriends;
    [SerializeField] private Toggle _isClosed;

    public void HideServer()
    {
        PhotonNetwork.CurrentRoom.IsVisible = !_isForFriends.isOn;
    }

    public void CloseRoom()
    {
        PhotonNetwork.CurrentRoom.IsOpen = !_isClosed.isOn;
    }
}
