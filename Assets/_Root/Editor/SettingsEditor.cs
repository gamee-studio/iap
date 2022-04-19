using System;
using System.Collections.Generic;
using System.IO;
using Pancake.Iap;
using UnityEditor;
using UnityEngine;

namespace Pancake.Editor
{
    [CustomEditor(typeof(IAPSetting))]
    internal class SettingsEditor : UnityEditor.Editor
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

            Uniform.DrawUppercaseSection("IAP_SETTING",
                "SETTING",
                () =>
                {
                    EditorGUILayout.PropertyField(_skusData.property, _skusData.content);
                    foreach (var iapData in IAPSetting.SkusData)
                    {
                        iapData.name = iapData.sku.Id;
                    }

                    EditorGUILayout.Space();

                    if (GUILayout.Button("Generate Script", GUILayout.Height(EditorGUIUtility.singleLineHeight * 1.3f)))
                    {
                        IAPCreator.GenerateImplProduct();
                    }
                });

            EditorGUI.EndDisabledGroup();
            serializedObject.ApplyModifiedProperties();
        }
    }
}