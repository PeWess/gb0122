using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PhotonButtonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button _logInBtn;
    [SerializeField] private Button _logOutBtn;

    private string _gameVersion = "1";

    private void Start()
    {
        _logInBtn.onClick.AddListener(LogIn);
        _logOutBtn.onClick.AddListener(LogOut);
    }

    private void LogIn()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonConnectionManager.OnFailure("You have already been connected");
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = _gameVersion;
            
            PhotonConnectionManager.OnConnection();
        }
    }

    private void LogOut()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
            
            PhotonConnectionManager.OnDisconnection();
        }
        else
        {
            PhotonConnectionManager.OnFailure("You have already been disconnected");
        }
    }
}
