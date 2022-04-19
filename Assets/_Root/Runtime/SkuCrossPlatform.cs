using System;
using UnityEngine;

namespace Pancake.Iap
{
    [Serializable]
    public class SkuCrossPlatform
    {
        [SerializeField] private string defaultId;
        [SerializeField] private bool overrideId;
        [SerializeField] private string androidId;
        [SerializeField] private string iOSId;

        public virtual string Id
        {
            get
            {
                if (!OverrideId) return DefaultId;

#if UNITY_ANDROID
                return AndroidId;
#elif UNITY_IOS
                return IosId;
#else
                return string.Empty;
#endif
            }
        }

        public virtual string IosId => iOSId;

        public virtual string AndroidId => androidId;

        public string DefaultId => defaultId;

        public bool OverrideId => overrideId;

        public SkuCrossPlatform(string androidId, string iOSId)
        {
            this.androidId = androidId;
            this.iOSId = iOSId;
        }
    }
}