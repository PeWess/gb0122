using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class CatalogManager : MonoBehaviour
{
    [SerializeField] private Text _catalogLabel;
    private readonly Dictionary<string, CatalogItem> _catalog = new Dictionary<string, CatalogItem>();

    private void Start()
    {
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), OnGetCatalogSuccess, OnFailure);
    }

    private void OnGetCatalogSuccess(GetCatalogItemsResult result)
    {
        HandleCatalog(result.Catalog);
        ShowCatalogItems();
        Debug.Log("Catalog was loaded successfully");
    }

    private void OnFailure(PlayFabError error)
    {
        var errorMessage = error.ErrorMessage;
        Debug.Log($"Something went wrong: {errorMessage}");
    }

    private void HandleCatalog(List<CatalogItem> catalog)
    {
        foreach (var item in catalog)
        {
            _catalog.Add(item.ItemId, item);
            Debug.Log($"Catalog item {item.ItemId} was added successfully!");
        }
    }

    public void ShowCatalogItems()
    {
        foreach (var item in _catalog)
        {
            if(item.Value.VirtualCurrencyPrices.ContainsKey("FL")) 
                _catalogLabel.text += $"{item.Value.DisplayName}. Price: {item.Value.VirtualCurrencyPrices["FL"]} FL\n";
        }
    }
}
