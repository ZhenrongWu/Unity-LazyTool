using System.Linq;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

namespace Lazy_Tool.Editor
{
    [InitializeOnLoad]
    public class Header
    {
        static Header()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
        }

        private static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            Color fontColor       = Color.white;
            Color backgroundColor = Color.gray;

            var instanceIDToObject = EditorUtility.InstanceIDToObject(instanceID);
            if (instanceIDToObject == null) return;

            foreach (var gameObject in GameObject.FindGameObjectsWithTag("EditorOnly"))
            {
                if (gameObject != instanceIDToObject) continue;

                if (Selection.instanceIDs.Contains(instanceID))
                {
                    backgroundColor = new Color32(64, 160, 255, 255);
                }

                Rect rect = new Rect(selectionRect.position, selectionRect.size);
                EditorGUI.DrawRect(selectionRect, backgroundColor);
                EditorGUI.LabelField(rect, instanceIDToObject.name,
                    new GUIStyle
                    {
                        normal    = new GUIStyleState { textColor = fontColor },
                        fontStyle = FontStyle.Bold,
                        alignment = TextAnchor.MiddleCenter
                    }
                );
            }
        }
    }
}

#endif