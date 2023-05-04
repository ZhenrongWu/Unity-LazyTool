using UnityEditor;
using UnityEngine;

namespace Lazy_Tool.Editor
{
    public abstract class CustomHierarchy
    {
        [MenuItem("GameObject/Create Header", priority = 1)]
        private static void CreateHeader()
        {
            var gameObject = new GameObject("Header")
            {
                tag = "EditorOnly",
                transform =
                {
                    hideFlags = HideFlags.NotEditable | HideFlags.HideInInspector
                }
            };

            Undo.RegisterCreatedObjectUndo(gameObject, "Create Header");

            Selection.activeGameObject = gameObject;
        }
    }
}