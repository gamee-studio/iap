using System;
using Pancake.Iap;
using UnityEditor;
using UnityEngine;

namespace Pancake.Editor
{
    public class SettingsWindow : EditorWindow
    {
        private Vector2 _scrollPosition;
        private UnityEditor.Editor _editor;

        private void OnGUI()
        {
            if (_editor == null) _editor = UnityEditor.Editor.CreateEditor(IAPSetting.Instance);

            if (_editor == null)
            {
                EditorGUILayout.HelpBox("Coundn't create the settings resources editor.", MessageType.Error);
                return;
            }
            SettingsEditor.callFromEditorWindow = true;
            _editor.DrawHeader();
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(6, 3, 3, 3) });
            _editor.OnInspectorGUI();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            SettingsEditor.callFromEditorWindow = false;
        }

        private static SettingsWindow GetWindow()
        {
            // Get the window and make sure it will be opened in the same panel with inspector window.
            var editorAsm = typeof(UnityEditor.Editor).Assembly;
            var inspWndType = editorAsm.GetType("UnityEditor.InspectorWindow");
            var window = GetWindow<SettingsWindow>(inspWndType);
            window.titleContent = new GUIContent("IAP");

            return window;
        }

        public static void ShowWindow()
        {
            var window = GetWindow();
            if (window == null)
            {
                Debug.LogError("Coundn't open the iap settings window.");
                return;
            }

            window.minSize = new Vector2(280, 0);
            window.Show();
        }

        private void OnEnable()
        {
            Uniform.FoldoutSettings.LoadSetting();
        }

        private void OnDisable()
        {
            Uniform.FoldoutSettings.SaveSetting();
            EditorUtility.ClearProgressBar();
            AssetDatabase.SaveAssets();
        }
    }
}