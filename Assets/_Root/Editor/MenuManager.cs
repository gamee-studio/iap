using Pancake.Iap;
using UnityEditor;

namespace Pancake.Editor
{
    public static class MenuManager
    {
        [MenuItem("Tools/Pancake/IAP %w", false, 1)]
        public static void MenuOpenSettings()
        {
            // Load settings object or create a new one if it doesn't exist.
            var instance = IAPSetting.LoadSetting();
            if (instance == null) IAPCreator.CreateSettingsAsset();

            SettingsWindow.ShowWindow();
        }
    }
}