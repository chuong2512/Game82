using System.Collections;
using System.Collections.Generic;
using Jackal;
using UnityEngine;

public class PurchasingManager : Singleton<PurchasingManager>
{
    public GameObject cuaHang;
    public MonetizeButton monetizeButton1,monetizeButton2,monetizeButton3;

    public void OnPressDown(int i)
    {
        switch (i)
        {
            case 1:
                IAPManager.OnPurchaseSuccess = () =>
                {
                    UserProgress.Current.Coins += 10;
                    monetizeButton1.UpdateButtons();
                    monetizeButton2.UpdateButtons();
                    monetizeButton3.UpdateButtons();
                };
                IAPManager.Instance.BuyProductID(IAPKey.PACK1);
                break;
            case 2:
                IAPManager.OnPurchaseSuccess = () =>
                {
                    UserProgress.Current.Coins += 50;
                    monetizeButton1.UpdateButtons();
                    monetizeButton2.UpdateButtons();
                    monetizeButton3.UpdateButtons();
                };
                IAPManager.Instance.BuyProductID(IAPKey.PACK2);
                break;
            case 3:
                IAPManager.OnPurchaseSuccess = () =>
                {
                    UserProgress.Current.Coins += 100;
                    monetizeButton1.UpdateButtons();
                    monetizeButton2.UpdateButtons();
                    monetizeButton3.UpdateButtons();
                };
                IAPManager.Instance.BuyProductID(IAPKey.PACK3);
                break;
            case 4:
                IAPManager.OnPurchaseSuccess = () =>
                {
                    UserProgress.Current.Coins += 200;
                    monetizeButton1.UpdateButtons();
                    monetizeButton2.UpdateButtons();
                    monetizeButton3.UpdateButtons();
                };
                IAPManager.Instance.BuyProductID(IAPKey.PACK4);
                break;
        }
    }

    public void Sub(int i)
    {
        GameDataManager.Instance.playerData.SubDiamond(i);
    }

    public void Show()
    {
        cuaHang.SetActive(true);
    }
}