using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class PlayFabButtonManager : MonoBehaviour
{
    private Button _logInBtn;
    
    private string _customID = "GeekBrainsLesson_3";
    private string _titleId = "BADF2";

    private void Start()
    {
        _logInBtn = gameObject.GetComponent<Button>();
        _logInBtn.onClick.AddListener(OnLogInClick);
    }

    private void OnLogInClick()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            PlayFabSettings.staticSettings.TitleId = _titleId;
        
        var request = new LoginWithCustomIDRequest{ CustomId = _customID, CreateAccount = true };
        PlayFabClientAPI.LoginWithCustomID(request, PlayFabConnectionManager.OnSuccess, PlayFabConnectionManager.OnFailure);
    }
}
