using System;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Snorlax.Iap
{
    [Serializable]
    public class IAPData
    {
#if UNITY_EDITOR
        // ReSharper disable once NotAccessedField.Global
        [HideInInspector] public string name;
#endif
        public SkuCrossPlatform sku;
        public ProductType productType;
    }
}