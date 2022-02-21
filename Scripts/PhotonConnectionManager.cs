using UnityEngine;
using UnityEngine.UI;

public class PhotonConnectionManager : MonoBehaviour
{
    private static Text _photonResultTxt;

    private void Start()
    {
        _photonResultTxt = gameObject.GetComponent<Text>();
    }

    public static void OnConnection()
    {
        _photonResultTxt.text = "Connected";
        _photonResultTxt.color = Color.green;
    }
    
    public static void OnDisconnection()
    {
        _photonResultTxt.text = "Disconnected";
        _photonResultTxt.color = Color.red;
    }

    public static void OnFailure(string errorText)
    {
        _photonResultTxt.text = errorText;
        _photonResultTxt.color = Color.yellow;
    }
}
