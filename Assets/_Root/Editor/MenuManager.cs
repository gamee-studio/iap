using Snorlax.Iap;
using UnityEditor;

namespace Snorlax.IapEditor
{
    public static class MenuManager
    {
        [MenuItem("Tools/Snorlax/IAP %w", false, 1)]
        public static void MenuOpenSettings()
        {
            // Load settings object or create a new one if it doesn't exist.
            var instance = IAPSetting.LoadSetting();
            if (instance == null) IAPCreator.CreateSettingsAsset();

            SettingsWindow.ShowWindow();
        }
    }
}