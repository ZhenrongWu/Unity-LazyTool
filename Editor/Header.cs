using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

namespace Lazy_Tool.Editor
{
    [InitializeOnLoad]
    public class Header : MonoBehaviour
    {
        static Header()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
        }

        private static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            Color fontColor       = Color.white;
            Color backgroundColor = Color.gray;

            var gameObject = EditorUtility.InstanceIDToObject(instanceID);
            if (gameObject == null) return;

            if (!gameObject.GameObject().CompareTag("EditorOnly")) return;

            if (Selection.instanceIDs.Contains(instanceID))
            {
                backgroundColor = new Color32(64, 160, 255, 255);
            }

            Rect rect = new Rect(selectionRect.position, selectionRect.size);
            EditorGUI.DrawRect(selectionRect, backgroundColor);
            EditorGUI.LabelField(rect, gameObject.name,
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

#endif