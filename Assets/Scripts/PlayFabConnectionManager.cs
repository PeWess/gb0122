using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class PlayFabConnectionManager : MonoBehaviour
{
    private static Text _playFabResultTxt;

    private void Start()
    {
        _playFabResultTxt = gameObject.GetComponent<Text>();
    }

    public static void OnSuccess(LoginResult result)
    {
        _playFabResultTxt.text = "Success";
        _playFabResultTxt.color = Color.green;
    }
    
    public static void OnFailure(PlayFabError error)
    {
        _playFabResultTxt.text = "Failed";
        _playFabResultTxt.color = Color.red;
        
        var errorMessage = error.GenerateErrorReport();
        Debug.Log($"Something went wrong: {errorMessage}");
    }
}
