using System;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayFabAccount : MonoBehaviour
{
    [SerializeField] private Text _welcomeMessage;
    [SerializeField] private Text _createErrorLabel;
    [SerializeField] private Text _signInErrorLabel;

    private string _username;
    private string _mail;
    private string _password;
    
    private const string _authKey = "Player-unique-id";
    private const string _nicknameKey = "Player-nickname";
    private const string _passwordKey = "Player-password";

    private void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            PlayFabSettings.staticSettings.TitleId = "BADF2";

        if (!PlayerPrefs.HasKey(_nicknameKey) && !PlayerPrefs.HasKey(_passwordKey))
        {
            var needCreation = !PlayerPrefs.HasKey(_authKey);
            Debug.Log($"Need creation: {needCreation}");
            var id = PlayerPrefs.GetString(_authKey, Guid.NewGuid().ToString());
            Debug.Log($"ID: {id}");
            var request = new LoginWithCustomIDRequest {CustomId = id, CreateAccount = needCreation};
            PlayFabClientAPI.LoginWithCustomID(request, result => { PlayerPrefs.SetString(_authKey, id); }, OnFailure);
            _welcomeMessage.text = "Welcome, Guest!";
        }
        else
        {
            PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest()
            {
                Username = PlayerPrefs.GetString(_nicknameKey),
                Password = PlayerPrefs.GetString(_passwordKey)
            }, result =>
            {
                Debug.Log($"Remembered account: {PlayerPrefs.GetString(_nicknameKey)}");
                _welcomeMessage.text = $"Welcome, {PlayerPrefs.GetString(_nicknameKey)}!";
            }, OnFailure);
        }
    }

    public void UpdateUsername(string username)
    {
        _username = username;
    }
    
    public void UpdateMail(string mail)
    {
        _mail = mail;
    }
    
    public void UpdatePassword(string password)
    {
        _password = password;
    }

    public void CreateAccount()
    {
        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
        {
            Username = _username,
            Email = _mail,
            Password = _password,
            RequireBothUsernameAndEmail = true
        }, OnRegisterSuccess, OnFailure);
    }

    public void SignIn()
    {
        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest()
        {
            Username = _username,
            Password = _password
        }, OnSignInSuccess, OnFailure);
    }

    public void PlayWithRememberedAccount()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void RememberUser(string username, string password)
    {
        PlayerPrefs.SetString(_nicknameKey, username);
        PlayerPrefs.SetString(_passwordKey, password);
    }
    
    public void Back()
    {
        _createErrorLabel.text = " ";
        _signInErrorLabel.text = " ";
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        RememberUser(_username, _password);
        Debug.Log($"Successful registration: {_username}");
        SceneManager.LoadScene("Lobby");
    }
    
    private void OnSignInSuccess(LoginResult result)
    {
        RememberUser(_username, _password);
        Debug.Log($"Successful sign in: {_username}");
        SceneManager.LoadScene("Lobby");
    }

    private void OnFailure(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.Log($"Something went wrong: {errorMessage}");
        _createErrorLabel.text = errorMessage;
        _signInErrorLabel.text = errorMessage;
    }
}
