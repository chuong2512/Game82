using System;
using UnityEngine;
using UnityEngine.UI;

public class MonetizeButton : MonoBehaviour
{
    const int AdsReward = 25;

    [SerializeField] Button buy;
    [SerializeField] Button getCoins;

    PriceLabel priceLabel;

    string Item { get; set; }
    Price Price { get; set; }

    bool isWatchAdsClicked;
    bool isGetCoinsClicked;

    public event Action PurchaseComplete = delegate { };

    int PurchaseProgress
    {
        get => UserProgress.Current.GetItemPurchaseProgress(Item);
        set => UserProgress.Current.SetItemPurchaseProgress(Item, value);
    }

    public void SetPrice(string item, Price price)
    {
        Item = item;
        Price = price;

        UpdateButtons();

        if (price.value <= UserProgress.Current.GetItemPurchaseProgress(item))
            PurchaseComplete.Invoke();
    }

    public void OnUnityAdsReady(string placementId)
    {
        UpdateButtons();
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.LogError(message);
    }

    public void OnUnityAdsDidStart(string placementId)
    {
    }

    public void UpdateButtons()
    {
        buy.gameObject.SetActive(false);
        getCoins.gameObject.SetActive(false);

        switch (Price.type)
        {
            case PriceType.Coins when UserProgress.Current.Coins >= Price.value:
                buy.gameObject.SetActive(true);
                break;
            case PriceType.Coins:
                getCoins.gameObject.SetActive(true);
                break;
        }
    }

    void OnBuyClick()
    {
        UserProgress.Current.Coins -= Price.value;
        PurchaseComplete.Invoke();
    }


    void Awake()
    {
        buy.onClick.AddListener(OnBuyClick);
        getCoins.onClick.AddListener(PurchasingManager.Instance.Show);
    }

    void OnEnable()
    {
        UpdateButtons();
    }
}