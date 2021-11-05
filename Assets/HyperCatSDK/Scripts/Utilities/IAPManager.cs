using System;
using System.Collections;
using System.Collections.Generic;
#if !PROTOTYPE
using AppsFlyerSDK;
using Newtonsoft.Json;
using Sigtrap.Relays;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;

#endif

public class IAPManager : Singleton<IAPManager>
#if !PROTOTYPE
    , IStoreListener
#endif
{
#if !PROTOTYPE
    private static IStoreController storeController; // The Unity Purchasing system.
    private static IExtensionProvider storeExtensionProvider; // The store-specific Purchasing subsystems.

    public string GetLocalizedPrice(string productId)
    {
        if (IsInitialized())
        {
            var product = storeController.products.WithID(productId);
            if (product == null)
                return null;
            else
            {
                return product.metadata.localizedPriceString;
            }
        }
        else
        {
            return null;
        }
    }

    private void Start()
    {
        if (storeController == null)
        {
            InitializePurchasing();
        }
    }

    private bool IsInitialized()
    {
        return storeController != null && storeExtensionProvider != null;
    }

    public void InitializePurchasing()
    {
        if (IsInitialized())
        {
            return;
        }

        StartCoroutine(Init());
    }

    private IEnumerator Init()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        foreach (var item in GameConst.listIAP)
        {
            builder.AddProduct(item.Key, item.Value);
        }

        while (!IsInitialized())
        {
            UnityPurchasing.Initialize(this, builder);
            yield return Yielders.Get(10f);
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        HCDebug.Log("IAP >Initialized Success!", HCColor.purple);

        storeController = controller;
        storeExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        HCDebug.Log("IAP >OnInitializeFailed InitializationFailureReason:" + error);
    }


    public void BuyProduct(string productId)
    {
        if (!GameManager.NetworkAvailable)
        {
            PopupNoInternet.Show();
            return;
        }

        if (IsInitialized())
        {
            var product = storeController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                HCDebug.Log(string.Format("IAP >Purchasing product asychronously: '{0}'", product.definition.id));
                storeController.InitiatePurchase(product);
            }
            else
            {
                HCDebug.Log("IAP >BuyProductID: FAILED. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            HCDebug.Log("IAP >BuyProductID FAILED. Not initialized.");
        }
    }

    public void RestorePurchases()
    {
        if (!IsInitialized())
        {
            HCDebug.Log("IAP >RestorePurchases FAIL. Not initialized.");
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            HCDebug.Log("IAP >RestorePurchases started ...");

            var apple = storeExtensionProvider.GetExtension<IAppleExtensions>();

            apple.RestoreTransactions((result) => { HCDebug.Log("IAP >RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore."); });
        }
        else
        {
            HCDebug.Log("IAP >RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    private void OnBuyNoAdsOffer()
    {
        GameManager.Instance.Data.User.PurchasedNoAds = true;
        Database.SaveData();
        EventGlobalManager.Instance.OnPurchaseNoAds.Dispatch();
        HCDebug.Log("IAP >No Ads Offer purchased! Remove all ads now.", HCColor.purple);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        try
        {
            if (string.Equals(args.purchasedProduct.definition.id, GameConst.NO_ADS_ID, StringComparison.Ordinal))
            {
                OnBuyNoAdsOffer();
            }

            var gCurrency = args.purchasedProduct.metadata.isoCurrencyCode;
            var gPrice = ((float) args.purchasedProduct.metadata.localizedPrice * 0.7f).ToString();
            var addparam = new Dictionary<string, string>()
            {
                {AFInAppEvents.CONTENT_ID, args.purchasedProduct.definition.id},
                {AFInAppEvents.REVENUE, gPrice},
                {AFInAppEvents.CURRENCY, gCurrency}
            };

#if UNITY_ANDROID
            var googleReceipt = GooglePurchase.FromJson(args.purchasedProduct.receipt);
            HCDebug.Log("IAP >Receipt: " + googleReceipt.TransactionID + " ", HCColor.yellow);
            AnalyticManager.Instance.AppflyerLogIAPAndroid(googleReceipt.PayloadData.signature, googleReceipt.PayloadData.json, gPrice, gCurrency);
#endif

#if UNITY_IOS
        var appleReceipt = ApplePurchase.FromJson(args.purchasedProduct.receipt);
#endif

            return PurchaseProcessingResult.Complete;
        }
        catch (Exception e)
        {
            HCDebug.Log("IAP >Failed " + e.Message, HCColor.red);
            return PurchaseProcessingResult.Pending;
        }
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        HCDebug.Log(string.Format("IAP >OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId,
            failureReason), HCColor.red);
    }

#endif
}

#if !PROTOTYPE
public class GooglePurchase
{
    public PayloadData PayloadData;

    public string Store;
    public string TransactionID;
    public string Payload;

    public static GooglePurchase FromJson(string json)
    {
        var purchase = JsonUtility.FromJson<GooglePurchase>(json);
        purchase.PayloadData = PayloadData.FromJson(purchase.Payload);
        return purchase;
    }
}

public class ApplePurchase
{
    public string Store;
    public string TransactionID;
    public string Payload;

    public static ApplePurchase FromJson(string json)
    {
        var purchase = JsonUtility.FromJson<ApplePurchase>(json);
        return purchase;
    }
}

public class PayloadData
{
    public JsonData JsonData;

    public string signature;
    public string json;

    public static PayloadData FromJson(string json)
    {
        var payload = JsonUtility.FromJson<PayloadData>(json);
        payload.JsonData = JsonUtility.FromJson<JsonData>(payload.json);
        return payload;
    }
}

public class JsonData
{
    public string orderId;
    public string packageName;
    public string productId;
    public long purchaseTime;
    public int purchaseState;
    public string purchaseToken;
}
#endif