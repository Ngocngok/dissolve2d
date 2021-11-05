#if PAYMENT_ENABLE
using System;
using System.Collections.Generic;
using UnityEngine.Purchasing;

namespace RocketTeam.Sdk.Services.Interfaces
{
    public interface IPaymentManager : IG2Service
    {
        /// <summary>
        /// Register listener for IAP events
        /// </summary>
        void RegisterListener(Action onInitializeSuccess, Action<InitializationFailureReason> onInitializeFail,
            Action<Product> onPurchaseSuccess, Action<string, PurchaseFailureReason, string> onPurchaseFail);

        /// <summary>
        /// Initialize IAP with specified product list
        /// </summary>
        void Initialize();

        /// <summary>
        /// Buy a product with specified productId
        /// </summary>
        void PurchaseProduct(string productId, string payload = "", long userId = 0);

        /// <summary>
        /// Restore a previous product, iOS only
        /// </summary>
        void RestorePurchases();

        /// <summary>
        /// Get a product with specified productId
        /// </summary>
        Product GetProduct(string productId);

        /// <summary>
        /// Get all products
        /// </summary>
        Product[] GetAllProducts();

        /// <summary>
        /// Check init state of payment module
        /// </summary>
        bool IsInitialized();

        void Init();
    }
}
#endif