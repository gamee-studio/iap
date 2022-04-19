using System.Collections.Generic;
using UnityEngine;

namespace Pancake.Iap
{
    public class IAPSetting : ScriptableObject
    {
        private static IAPSetting instance;

        public static IAPSetting Instance
        {
            get
            {
                if (instance != null) return instance;

                instance = LoadSetting();

                if (instance == null)
                {
#if !UNITY_EDITOR
                        Debug.LogError("IAP settings not found! Please go to menu Tools > Snorlax > IAP to setup the plugin.");
#endif
                    instance = CreateInstance<IAPSetting>(); // Create a dummy scriptable object for temporary use.
                }

                return instance;
            }
        }

        #region member

        [SerializeField] private bool runtimeAutoInitialize = true;
        [SerializeField] private List<IAPData> skusData = new List<IAPData>();

        #endregion

        #region properties

        public static bool RuntimeAutoInitialize => Instance.runtimeAutoInitialize;

        public static List<IAPData> SkusData => Instance.skusData;

        #endregion

        #region api

        public static IAPSetting LoadSetting() { return Resources.Load<IAPSetting>("IapSettings"); }

        #endregion
    }
}