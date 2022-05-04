using System.Globalization;
using System.IO;
using System.Linq;
using Pancake.Iap;
using UnityEditor;
using UnityEngine;

namespace Pancake.Editor
{
    public class IAPCreator
    {
        private const string PRODUCT_IMPL_PATH = "Assets/_Root/Scripts/Product.cs";

        internal static IAPSetting CreateSettingsAsset()
        {
            // Stop if the asset is already created.
            var instance = IAPSetting.LoadSetting();
            if (instance != null)
            {
                return instance;
            }

            // Create Resources folder if it doesn't exist.
            EnsureFolderExists("Assets/Resources");

            // Now create the asset inside the Resources folder.
            instance = IAPSetting.Instance; // this will create a new instance of the EMSettings scriptable.
            AssetDatabase.CreateAsset(instance, "Assets/Resources/IapSettings.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("Settings was created at Assets/Resources/IapSettings.asset");

            return instance;
        }

        /// <summary>
        /// Creates the folder if it doesn't exist.
        /// </summary>
        /// <param name="path">Path - the slashes will be corrected.</param>
        private static void EnsureFolderExists(string path)
        {
            path = SlashesToPlatformSeparator(path);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// Replaces / in file path to be the os specific separator.
        /// </summary>
        /// <returns>The path.</returns>
        /// <param name="path">Path with correct separators.</param>
        public static string SlashesToPlatformSeparator(string path) { return path.Replace("/", Path.DirectorySeparatorChar.ToString()); }

        public static void GenerateImplProduct()
        {
            EnsureFolderExists("Assets/_Root/Scripts");

            var str = "namespace Pancake.Iap\n{";
            str += "\n\tpublic static class Product\n\t{";

            var skus = IAPSetting.SkusData;
            for (int i = 0; i < skus.Count; i++)
            {
                var itemName = skus[i].sku.Id.Split('.').Last();
                str += $"\n\t\tpublic static IAPData Purchase{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(itemName)}()";
                str += "\n\t\t{";
                str += $"\n\t\t\treturn IAPManager.Purchase(IAPSetting.SkusData[{i}]);";
                str += "\n\t\t}";
                str += "\n";
            }

            str += "\n\t}";
            str += "\n}";

            var writer = new StreamWriter(PRODUCT_IMPL_PATH, false);
            writer.Write(str);
            writer.Close();
            AssetDatabase.ImportAsset(PRODUCT_IMPL_PATH);
        }
    }
}