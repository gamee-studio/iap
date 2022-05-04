using System;

namespace Pancake.Iap
{
    public static class IAPChain
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <param name="onCompleted"></param>
        /// <returns></returns>
        public static IAPData OnCompleted(this IAPData product, Action onCompleted)
        {
            IAPManager.RegisterCompletedEvent(product.sku.Id, onCompleted);
            return product;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <param name="onError"></param>
        /// <returns></returns>
        public static IAPData OnError(this IAPData product, Action onError)
        {
            IAPManager.RegisterFaildEvent(product.sku.Id, onError);
            return product;
        }
    }
}