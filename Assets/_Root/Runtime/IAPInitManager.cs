using UnityEngine;

namespace Pancake.Iap
{
    public static class IAPInitManager
    {
        private static bool isInitialized;

        public static void Init()
        {
            if (isInitialized) return;

            if (Application.isPlaying)
            {
                IAPManager.Init();
                isInitialized = true;
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AutoInitalize()
        {
            if (IAPSetting.RuntimeAutoInitialize) Init();
        }
    }
}