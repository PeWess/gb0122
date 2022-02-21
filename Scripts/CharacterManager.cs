using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    private const string _characterStoreID = "characters_store";
    private const string _virtualCurrency = "FL";
    private string _inputFieldText;

    [SerializeField] private GameObject _plusPanel;
    [SerializeField] private GameObject _characterPanel;
    [SerializeField] private GameObject _plus1;
    [SerializeField] private GameObject _character1;
    
    [SerializeField] private Text _characterName1;
    [SerializeField] private Text _characterName2;
    [SerializeField] private Text _characterInfo1;
    [SerializeField] private Text _characterInfo2;

    private void Start()
    {
        UpdateCharacters();
    }

    public void OnNameChanged(string newName)
    {
        _inputFieldText = newName;
    }

    public void OnCreateButtonClicked()
    {
        if (string.IsNullOrEmpty(_inputFieldText))
        {
            Debug.LogError("Input field should not be empty");
            return;
        }

        PlayFabClientAPI.GetStoreItems(new GetStoreItemsRequest
        {
            StoreId = _characterStoreID
        }, result => { HandleStoreResult(result.Store); }, Debug.LogError);
    }

    private void HandleStoreResult(List<StoreItem> items)
    {
        foreach (var item in items)
        {
            Debug.Log(item.ItemId);
            PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
            {
                ItemId = item.ItemId,
                Price = (int) item.VirtualCurrencyPrices[_virtualCurrency],
                VirtualCurrency = _virtualCurrency,
            }, result =>
            {
                Debug.Log($"Item {result.Items[0].ItemId} was purchased");
                GetCharacter(result.Items[0].ItemId);
            }, Debug.LogError);
        }
    }

    private void GetCharacter(string itemId)
    {
        PlayFabClientAPI.GrantCharacterToUser(new GrantCharacterToUserRequest
        {
            ItemId = itemId,
            CharacterName = _inputFieldText,
        }, result =>
        {
            UpdateCharacterStatistics(result.CharacterId);
        }, Debug.LogError);
    }

    private void UpdateCharacterStatistics(string characterId)
    {
        PlayFabClientAPI.UpdateCharacterStatistics(new UpdateCharacterStatisticsRequest
        {
            CharacterId = characterId,
            CharacterStatistics = new Dictionary<string, int>
            {
                {"Level", 1},
                {"Exp", 0},
                {"Health", 100},
                {"Mana", 100},
                {"Stamina", 100}
            }
        }, result =>
        {
            UpdateCharacters();
        }, Debug.LogError);
    }

    private void UpdateCharacters()
    {
        PlayFabClientAPI.GetAllUsersCharacters(new ListUsersCharactersRequest(),
            result =>
            {
                for (int i = 0; i != 2 && i != result.Characters.Count; ++i)
                {
                    var name = result.Characters[i].CharacterName;
                    PlayFabClientAPI.GetCharacterStatistics(new GetCharacterStatisticsRequest
                    {
                        CharacterId = result.Characters[i].CharacterId,
                    }, res =>
                    {
                        _plus1.SetActive(false);
                        _character1.SetActive(true);
                        _characterName1.text = name;
                        _characterInfo1.text = "Level: " + res.CharacterStatistics["Level"].ToString() +
                                               "\nHealth: " + res.CharacterStatistics["Health"].ToString() +
                                               "\nStamina: " + res.CharacterStatistics["Stamina"].ToString() +
                                               "\nMana: " + res.CharacterStatistics["Mana"].ToString();
                    }, Debug.LogError);
                }
                _characterPanel.SetActive(false);
                _plusPanel.SetActive(true);
            }, Debug.LogError);
    }
}