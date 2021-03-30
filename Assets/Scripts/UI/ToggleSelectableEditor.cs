#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    /// <summary>
    /// An extension to <see cref="ToggleEditor"/> that adds two fields required for <see cref="ToggleSelectable"/>.
    /// </summary>
    [CustomEditor(typeof(ToggleSelectable), true)]
    [CanEditMultipleObjects]
    public class ToggleSelectableEditor : ToggleEditor {
        private SerializedProperty levelSelectionScrollProperty;
        private SerializedProperty levelSelectionContentRectProperty;
        
        protected override void OnEnable() {
            base.OnEnable();

            levelSelectionScrollProperty = serializedObject.FindProperty("levelSelectionScroll");
            levelSelectionContentRectProperty = serializedObject.FindProperty("levelSelectionContentRect");
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
            var toggleSelectable = serializedObject.targetObject as ToggleSelectable;
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(levelSelectionScrollProperty);
            EditorGUILayout.PropertyField(levelSelectionContentRectProperty);

            EditorGUI.BeginChangeCheck();
            if (EditorGUI.EndChangeCheck()) {
                var scrollRect = levelSelectionScrollProperty.objectReferenceValue as ScrollRect;
                var contentRect = levelSelectionContentRectProperty.objectReferenceValue as RectTransform;

                if (!(toggleSelectable is null)) {
                    toggleSelectable.levelSelectionScroll = scrollRect;
                    toggleSelectable.levelSelectionContentRect = contentRect;
                }
            }
            
            EditorGUILayout.Space();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif