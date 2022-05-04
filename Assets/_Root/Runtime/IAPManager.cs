using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Pancake.Iap
{
    public class IAPManager : MonoBehaviour, IStoreListener
    {
        public event Action<PurchaseEventArgs> OnPurchaseSucceedEvent;
        public event Action<string> OnPurchaseFailedEvent;

        private static IAPManager instance;
        private IStoreController _controller;
        private IExtensionProvider _extensions;

        public static IAPManager Instance
        {
            get
            {
                if (instance == null) Init();

                return instance;
            }
        }

        public List<IAPData> Skus { get; set; } = new List<IAPData>();
        public InformationPurchaseResult receiptInfo { get; set; }
        public bool IsInitialized { get; set; }

        public static void Init()
        {
            if (instance != null) return;

            if (Application.isPlaying)
            {
                var obj = new GameObject("IAPManager") { hideFlags = HideFlags.HideAndDontSave };
                instance = obj.AddComponent<IAPManager>();
                instance.Init(IAPSetting.SkusData);
                DontDestroyOnLoad(obj);
            }
        }

        public void Init(IEnumerable<IAPData> skuItems)
        {
            if (this != Instance) return;

            if (IsInitialized) return;
            Skus.Clear();
            Skus.AddRange(skuItems);
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            RequestProductData(builder);
            builder.Configure<IGooglePlayConfiguration>();

            UnityPurchasing.Initialize(this, builder);
            IsInitialized = true;
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            switch (error)
            {
                case InitializationFailureReason.AppNotKnown:
                    Debug.LogError("Is your App correctly uploaded on the relevant publisher console?");
                    break;
                case InitializationFailureReason.PurchasingUnavailable:
                    Debug.LogWarning("Billing disabled!");
                    break;
                case InitializationFailureReason.NoProductsAvailable:
                    Debug.LogWarning("No products available for purchase!");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(error), error, null);
            }
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
#if !UNITY_EDITOR
            receiptInfo = purchaseEvent.purchasedProduct.hasReceipt ? GetIapInformationPurchase(purchaseEvent.purchasedProduct.receipt) : null;
#endif

            PurchaseVerified(purchaseEvent);
            return PurchaseProcessingResult.Complete;
        }

        public void PurchaseProduct(string productId)
        {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            _controller?.InitiatePurchase(productId);
#endif
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) { OnPurchaseFailedEvent?.Invoke(failureReason.ToString()); }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            // Purchasing has succeeded initializing. Collect our Purchasing references.
            // Overall Purchasing system, configured with products for this application.
            _controller = controller;
            _extensions = extensions;
            foreach (var product in _controller.products.all)
            {
                _controller.ConfirmPendingPurchase(product);
            }
        }

        private InformationPurchaseResult GetIapInformationPurchase(string json)
        {
            if (string.IsNullOrEmpty(json)) return null;

#if UNITY_ANDROID
            string jsonNode = JSON.Parse(json)["Payload"].Value;
            string jsonData = JSON.Parse(jsonNode)["json"].Value;
            long purchaseTime = JSON.Parse(jsonData)["purchaseTime"].AsLong;
            const string device = "ANDROID";
            string productId = JSON.Parse(jsonData)["productId"].Value;
            var iapType = GetIapType(productId);
            string transactionId = JSON.Parse(json)["TransactionID"].Value;
            int purchaseState = JSON.Parse(jsonData)["purchaseState"].AsInt;
            string purchaseToken = JSON.Parse(jsonData)["purchaseToken"].Value;
            string signature = JSON.Parse(jsonNode)["signature"].Value;
            return new InformationPurchaseResult(device,
                iapType.ToString(),
                transactionId,
                productId,
                purchaseState,
                purchaseTime,
                purchaseToken,
                signature,
                SystemInfo.deviceUniqueIdentifier);
#elif UNITY_IOS
            string jsonNode = JSON.Parse(json)["receipt"].Value;
            long purchaseTime = JSON.Parse(jsonNode)["receipt_creation_date_ms"].AsLong;
            const string device = "IOS";
            string productId = JSON.Parse(jsonNode)["in_app"][0]["product_id"].Value;
            var iapType = GetIapType(productId);
            string transactionId = JSON.Parse(jsonNode)["in_app"][0]["transaction_id"].Value;
            int purchaseState = JSON.Parse(json)["status"].AsInt;
            const string purchaseToken = "";
            const string signature = "";
            return new InformationPurchaseResult(device,
                iapType.ToString(),
                transactionId,
                productId,
                purchaseState,
                purchaseTime,
                purchaseToken,
                signature,
                SystemInfo.deviceUniqueIdentifier);
#endif
        }

        private void PurchaseVerified(PurchaseEventArgs e) { OnPurchaseSucceedEvent?.Invoke(e); }

        private void RequestProductData(ConfigurationBuilder builder)
        {
            foreach (var p in Skus)
            {
                builder.AddProduct(p.sku.Id, p.productType);
            }
        }

        private ProductType GetIapType(string sku)
        {
            foreach (var item in Skus)
            {
                if (item.sku.Id.Equals(sku)) return item.productType;
            }

            return ProductType.Consumable;
        }

        private void HandlePurchaseFaild(string str) { OnPurchaseFailedEvent?.Invoke(str); }

        private void HandlePurchaseSucceed(PurchaseEventArgs e) { OnPurchaseSucceedEvent?.Invoke(e); }
    }
}