using Photon.Pun;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    void Awake()
    {
        PhotonNetwork.Instantiate("Character", gameObject.transform.position, gameObject.transform.rotation);
    }
}
