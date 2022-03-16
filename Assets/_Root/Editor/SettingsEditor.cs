using System;
using System.Collections.Generic;
using System.IO;
using Snorlax.Iap;
using UnityEditor;
using UnityEngine;

namespace Snorlax.IapEditor
{
    [CustomEditor(typeof(IAPSetting))]
    internal class SettingsEditor : Editor
    {
        private class Property
        {
            public SerializedProperty property;
            public GUIContent content;

            public Property(SerializedProperty property, GUIContent content)
            {
                this.property = property;
                this.content = content;
            }

            public Property(GUIContent content) { this.content = content; }
        }

        private SerializedProperty _autoInitializeProperty;
        private Property _skusData;
        public static bool callFromEditorWindow = false;

        private void Init()
        {
            _autoInitializeProperty = serializedObject.FindProperty("runtimeAutoInitialize");
            _skusData = new Property(serializedObject.FindProperty("skusData"), new GUIContent("Skus", "Sku container"));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            Init();

            if (!callFromEditorWindow)
            {
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox(
                    "This ScriptableObject holds all the settings of Ads. Please go to menu Tools > Snorlax > IAP or click the button below to edit it.",
                    MessageType.Info);
                if (GUILayout.Button("Edit")) SettingsWindow.ShowWindow();
                return;
            }

            EditorGUI.BeginDisabledGroup(EditorApplication.isCompiling);
            
            EditorGUILayout.HelpBox("Product id should look like : com.appname.itemid\ncom.eldenring.doublesoul", MessageType.Info);

            DrawUppercaseSection("IAP_SETTING", "SETTING",
                () =>
                {
                    EditorGUILayout.PropertyField(_skusData.property, _skusData.content);
                    foreach (var iapData in IAPSetting.SkusData)
                    {
                        iapData.name = iapData.sku.Id;
                    }
                });

            EditorGUI.EndDisabledGroup();
            serializedObject.ApplyModifiedProperties();
        }

        #region gui

        private static readonly Dictionary<string, bool> UppercaseSectionsFoldoutStates = new Dictionary<string, bool>();
        private static readonly Dictionary<string, GUIStyle> CustomStyles = new Dictionary<string, GUIStyle>();
        private static GUISkin skin;
        private const string SKIN_PATH = "Assets/_Root/GUISkins/";
        private const string UPM_SKIN_PATH = "Packages/com.gamee.iap/GUISkins/";
        private static GUIStyle uppercaseSectionHeaderExpand;
        private static GUIStyle uppercaseSectionHeaderCollapse;
        private static Texture2D chevronUp;
        private static Texture2D chevronDown;
        private const int CHEVRON_ICON_WIDTH = 10;
        private const int CHEVRON_ICON_RIGHT_MARGIN = 5;

        public static GUIStyle UppercaseSectionHeaderExpand { get { return uppercaseSectionHeaderExpand ??= GetCustomStyle("Uppercase Section Header"); } }

        public static GUIStyle UppercaseSectionHeaderCollapse
        {
            get { return uppercaseSectionHeaderCollapse ??= new GUIStyle(GetCustomStyle("Uppercase Section Header")) { normal = new GUIStyleState() }; }
        }

        public static GUIStyle GetCustomStyle(string styleName)
        {
            if (CustomStyles.ContainsKey(styleName)) return CustomStyles[styleName];

            if (Skin != null)
            {
                var style = Skin.FindStyle(styleName);

                if (style == null) Debug.LogError("Couldn't find style " + styleName);
                else CustomStyles.Add(styleName, style);

                return style;
            }

            return null;
        }

        public static GUISkin Skin
        {
            get
            {
                if (skin != null) return skin;

                const string upmPath = UPM_SKIN_PATH + "Dark.guiskin";
                string path = !File.Exists(Path.GetFullPath(upmPath)) ? SKIN_PATH + "Dark.guiskin" : upmPath;
                skin = AssetDatabase.LoadAssetAtPath(path, typeof(GUISkin)) as GUISkin;

                if (skin == null) Debug.LogError("Couldn't load the GUISkin at " + path);

                return skin;
            }
        }

        public static Texture2D ChevronDown
        {
            get
            {
                if (chevronDown != null) return chevronDown;
                const string upmPath = UPM_SKIN_PATH + "Icons/icon-chevron-down-dark.psd";
                string path = !File.Exists(Path.GetFullPath(upmPath)) ? SKIN_PATH + "Icons/icon-chevron-down-dark.psd" : upmPath;
                chevronDown = AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D)) as Texture2D;
                return chevronDown;
            }
        }

        public static Texture2D ChevronUp
        {
            get
            {
                if (chevronUp != null) return chevronUp;
                const string upmPath = UPM_SKIN_PATH + "Icons/icon-chevron-up-dark.psd";
                string path = !File.Exists(Path.GetFullPath(upmPath)) ? SKIN_PATH + "Icons/icon-chevron-up-dark.psd" : upmPath;
                chevronUp = AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D)) as Texture2D;

                return chevronUp;
            }
        }

        private Texture2D GetChevronIcon(bool foldout) { return foldout ? ChevronUp : ChevronDown; }

        private void DrawUppercaseSection(string key, string sectionName, Action drawer, Texture2D sectionIcon = null, bool defaultFoldout = true)
        {
            if (!UppercaseSectionsFoldoutStates.ContainsKey(key))
                UppercaseSectionsFoldoutStates.Add(key, defaultFoldout);

            bool foldout = UppercaseSectionsFoldoutStates[key];

            EditorGUILayout.BeginVertical(GetCustomStyle("Uppercase Section Box"), GUILayout.MinHeight(foldout ? 30 : 0));

            EditorGUILayout.BeginHorizontal(foldout ? UppercaseSectionHeaderExpand : UppercaseSectionHeaderCollapse);

            // Header label (and button).
            if (GUILayout.Button(sectionName, GetCustomStyle("Uppercase Section Header Label")))
                UppercaseSectionsFoldoutStates[key] = !UppercaseSectionsFoldoutStates[key];

            // The expand/collapse icon.
            var buttonRect = GUILayoutUtility.GetLastRect();
            var iconRect = new Rect(buttonRect.x + buttonRect.width - CHEVRON_ICON_WIDTH - CHEVRON_ICON_RIGHT_MARGIN,
                buttonRect.y,
                CHEVRON_ICON_WIDTH,
                buttonRect.height);
            GUI.Label(iconRect, GetChevronIcon(foldout), GetCustomStyle("Uppercase Section Header Chevron"));

            EditorGUILayout.EndHorizontal();

            // Draw the section content.
            if (foldout) GUILayout.Space(5);

            if (foldout && drawer != null) drawer();

            EditorGUILayout.EndVertical();
        }

        #endregion
    }
}