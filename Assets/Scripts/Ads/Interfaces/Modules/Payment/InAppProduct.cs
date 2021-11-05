#if PAYMENT_ENABLE
using System;
using UnityEngine.Purchasing;

namespace RocketTeam.Sdk.Services.Interfaces
{
    [Serializable]
    public class InAppProduct
    {
        public bool isInited=false;
        public string name;
        public string productId;
        public ProductType productType;

        public string iosAppStoreId;
    }
}
#endif