using Snorlax.Iap;
using UnityEditor;
using UnityEngine;

namespace Snorlax.IapEditor
{
    [CustomPropertyDrawer(typeof(SkuCrossPlatform))]
    public class SkuIdEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var defaultId = property.FindPropertyRelative("defaultId");
            var overrideId = property.FindPropertyRelative("overrideId");
            var androidId = property.FindPropertyRelative("androidId");
            var iOSId = property.FindPropertyRelative("iOSId");

            if (!overrideId.boolValue)
            {
                defaultId.stringValue =
                    EditorGUI.TextField(new Rect(position.x, position.y, position.size.x - 95, EditorGUI.GetPropertyHeight(property.FindPropertyRelative("defaultId"))),
                        new GUIContent("Id", "Product Id"),
                        defaultId.stringValue);
            }

            overrideId.boolValue =
                EditorGUI.ToggleLeft(
                    new Rect(position.x + position.size.x - 95, position.y, 95, EditorGUI.GetPropertyHeight(property.FindPropertyRelative("overrideId"))),
                    new GUIContent("   Override", "Override id for specific platform"),
                    overrideId.boolValue);

            if (overrideId.boolValue)
            {
                androidId.stringValue =
                    EditorGUI.TextField(new Rect(position.x, position.y, position.size.x - 95, EditorGUI.GetPropertyHeight(property.FindPropertyRelative("androidId"))),
                        new GUIContent("Android Id", "Product id override for android"),
                        androidId.stringValue);

                iOSId.stringValue =
                    EditorGUI.TextField(
                        new Rect(position.x,
                            position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing,
                            position.size.x - 95,
                            EditorGUI.GetPropertyHeight(property.FindPropertyRelative("iOSId"))),
                        new GUIContent("iOS Id", "Product id override for ios"),
                        iOSId.stringValue);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var totalLine = 1;
            var overrideProperty = property.FindPropertyRelative("overrideId");
            if (overrideProperty.boolValue) totalLine = 2;
            return EditorGUIUtility.singleLineHeight * totalLine + EditorGUIUtility.standardVerticalSpacing * (totalLine - 1);
        }
    }
}