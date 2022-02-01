using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WebSocketSharp;

public class PlayFabAccountManager : MonoBehaviour
{
    [SerializeField] private Text _titleLabel;

    private const string _nicknameKey = "Player-nickname";
    private const string _passwordKey = "Player-password";
    
    private void Start()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccountSuccess, OnFailure);
    }

    public void ForgetAccount()
    {
        if (PlayerPrefs.HasKey(_nicknameKey) && PlayerPrefs.HasKey(_passwordKey))
        {
            PlayerPrefs.DeleteKey(_nicknameKey);
            PlayerPrefs.DeleteKey(_passwordKey);
            SceneManager.LoadScene("Launcher");
        }
        else
        {
            Debug.Log("You cannot forget a guest account");
            SceneManager.LoadScene("Launcher");
        }
    }

    private void OnGetAccountSuccess(GetAccountInfoResult result)
    {
        var nickname = result.AccountInfo.Username;

        if (nickname.IsNullOrEmpty())
            nickname = "Guest";
        
        _titleLabel.text = $"Welcome back, {nickname}\nYour PlayFab ID: {result.AccountInfo.PlayFabId}\nYour registration date: {result.AccountInfo.Created}";
    }

    private void OnFailure(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError($"Something went wrong: {errorMessage}");
    }
}
