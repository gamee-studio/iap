using System;

namespace Pancake.Iap
{
    [Serializable]
    public class InformationPurchaseResult
    {
        public string device;
        public string iapType;
        public string transactionId;
        public string productId;
        public int purchaseState;
        public long purchaseTime;
        public string purchaseToken;
        public string signature;
        public string deviceId;

        public InformationPurchaseResult(
            string device,
            string iapType,
            string transactionId,
            string productId,
            int purchaseState,
            long purchaseTime,
            string purchaseToken,
            string signature,
            string deviceId)
        {
            this.device = device;
            this.iapType = iapType;
            this.transactionId = transactionId;
            this.productId = productId;
            this.purchaseState = purchaseState;
            this.purchaseTime = purchaseTime;
            this.purchaseToken = purchaseToken;
            this.signature = signature;
            this.deviceId = deviceId;
        }
    }
}